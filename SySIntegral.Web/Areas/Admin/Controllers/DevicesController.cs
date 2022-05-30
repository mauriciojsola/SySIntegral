using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Application.Mvc.ViewFeatures;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Entities.Devices;
using SySIntegral.Core.Repositories.Assets;
using SySIntegral.Core.Repositories.Devices;
using SySIntegral.Core.Repositories.Organizations;
using SySIntegral.Web.Areas.Admin.Models.Devices;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    [Route("Admin/[Controller]")]
    [Area("Admin")]
    [Authorize(Roles = "Administrador,Administrador de Organización")]
    public class DevicesController : SySIntegralBaseController
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(IOrganizationRepository organizationRepository, IAssetRepository assetRepository, IDeviceRepository deviceRepository, ILogger<DevicesController> logger)
        {
            _logger = logger;
            _organizationRepository = organizationRepository;
            _assetRepository = assetRepository;
            _deviceRepository = deviceRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            var devices = IsLimitedByOrganization
                ? _deviceRepository.GetAll().Where(x => x.Asset.Organization.Id == OrganizationId).Include(x => x.Asset).ThenInclude(x => x.Organization)
                    .OrderBy(x => x.Asset.Organization.Name).ThenBy(x => x.Asset.Name).ThenBy(x => x.Description).ToList()
                : _deviceRepository.GetAll().Include(x => x.Asset).ThenInclude(x => x.Organization)
                    .OrderBy(x => x.Asset.Organization.Name).ThenBy(x => x.Asset.Name).ThenBy(x => x.Description).ToList();

            return View(devices);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            LoadErrors();
            var model = TempData.Get<CreateDeviceViewModel>(TempDataKey) ?? new CreateDeviceViewModel();
            InitModel(model);
            if (model.UniqueId.IsNullOrWhiteSpace())
                model.UniqueId = Guid.NewGuid().ToString();

            return View(model);
        }

        [Route("{id}/Edit")]
        public IActionResult Edit(int id)
        {
            LoadErrors();
            var model = TempData.Get<CreateDeviceViewModel>(TempDataKey) ?? new CreateDeviceViewModel();
            InitModel(model);

            var device = _deviceRepository.GetById(id);

            if (device != null)
            {
                if (IsLimitedByOrganization && device.Asset.Organization.Id != OrganizationId)
                    return Unauthorized("Usted no está autorizado a editar éste Dispositivo");

                model.Id = device.Id;
                model.Description = device.Description.Trim();
                model.UniqueId = device.UniqueId;
                model.SelectedAssetId = device.Asset.Id;
                model.SelectedOrganizationId = device.Asset.Organization.Id;
                model.Assets = _assetRepository.GetByOrganization(model.SelectedOrganizationId).OrderBy(x => x.Name).ToList();

                return View(model);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Update(CreateDeviceViewModel model)
        {
            var errors = ValidateModel(model);
            if (errors.Any())
            {
                TempData.Set<CreateDeviceViewModel>(TempDataKey, model);
                return model.Id > 0 ? RedirectToAction("Edit") : RedirectToAction("Create");
            }

            try
            {
                var device = model.Id > 0
                    ? _deviceRepository.GetById(model.Id)
                    : new Device();

                if (device != null)
                {
                    device.Description = model.Description.Trim();
                    device.UniqueId = model.UniqueId;
                    device.Asset = _assetRepository.GetById(model.SelectedAssetId);

                    if (model.Id > 0)
                    {
                        _deviceRepository.Update(device);
                    }
                    else
                    {
                        _deviceRepository.Insert(device);
                    }
                }
                else
                {
                    AddErrors(new List<string> { "Dispositivo no encontrado" });
                    TempData.Set<CreateDeviceViewModel>(TempDataKey, model);
                    return model.Id > 0 ? RedirectToAction("Edit") : RedirectToAction("Create");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("No se puede actualizar el Dispositivo", ex);
                AddErrors(new List<string> { "No se puede actualizar el Dispositivo" });
                TempData.Set<CreateDeviceViewModel>(TempDataKey, model);
                return model.Id > 0 ? RedirectToAction("Edit") : RedirectToAction("Create");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("GetAssets")]
        public IActionResult GetAssetsPerOrganization(int organizationId)
        {
            var model = new CreateDeviceViewModel { SelectedOrganizationId = organizationId };
            model.Assets = _assetRepository.GetByOrganization(model.SelectedOrganizationId).OrderBy(x => x.Name).ToList();
            return PartialView("_AssetsDropdownPartial", model);
        }

        private void InitModel(CreateDeviceViewModel model)
        {
            model.Organizations = IsLimitedByOrganization
                ? _organizationRepository.GetAll().Where(x => x.Id == OrganizationId).OrderBy(x => x.Name).ToList()
                : _organizationRepository.GetAll().OrderBy(x => x.Name).ToList();

            if (IsLimitedByOrganization)
                model.SelectedOrganizationId = OrganizationId;
            model.Assets = _assetRepository.GetByOrganization(model.SelectedOrganizationId).OrderBy(x => x.Name).ToList();

            model.IsLimitedByOrganization = IsLimitedByOrganization;
        }

        private List<string> ValidateModel(CreateDeviceViewModel model)
        {
            var errors = new List<string>();

            if (IsLimitedByOrganization && model.SelectedOrganizationId != OrganizationId)
                errors.Add("Usted no está autorizado a editar éste Dispositivo");

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                errors.Add("La Descripción del dispositivo es requerida");
            }

            var device = _deviceRepository.GetByDescription(model.Description, model.SelectedOrganizationId);
            if (device != null && device.Id != model.Id)
            {
                errors.Add("Un Dispositivo con el mismo nombre ya existe");
            }

            device = _deviceRepository.GetByUniqueID(model.UniqueId);
            if (device != null && device.Id != model.Id)
            {
                errors.Add("Un Dispositivo con el mismo ID ya existe");
            }

            AddErrors(errors);
            return errors;
        }

        [HttpPost]
        [Route("{id}/Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                var device = _deviceRepository.GetById(id);

                if (device != null)
                {
                    if (IsLimitedByOrganization && device.Asset.Organization.Id != OrganizationId)
                        return Unauthorized("Usted no está autorizado a eliminar éste Dispositivo");

                    _deviceRepository.Delete(device);
                }
                else
                    AddErrors("Dispositivo no encontrada");

            }
            catch (Exception ex)
            {
                _logger.LogError("No se puede borrar el Dispositivo", ex);
                AddErrors("No se puede borrar el Dispositivo");
            }
            return RedirectToAction("Index");
        }

    }
}

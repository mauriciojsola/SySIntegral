using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Entities.Organizations;
using SySIntegral.Core.Entities.Users;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.Assets;
using SySIntegral.Core.Repositories.Organizations;
using SySIntegral.Web.Areas.Admin.Models.Assets;
using SySIntegral.Web.Areas.Admin.Models.Organizations;
using SySIntegral.Web.Areas.Admin.Models.Users;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    [Route("Admin/[Controller]")]
    [Area("Admin")]
    [Authorize]
    public class AssetsController : SySIntegralBaseController
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ILogger<AssetsController> _logger;

        public AssetsController(IAssetRepository assetRepository,IOrganizationRepository organizationRepository, ILogger<AssetsController> logger)
        {
            _logger = logger;
            _assetRepository = assetRepository;
            _organizationRepository = organizationRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            var assets = IsLimitedByOrganization
                    ? _assetRepository.GetAll().Where(x => x.Organization.Id == OrganizationId).Include(x => x.Organization).Include(x => x.Devices).ToList()
                    : _assetRepository.GetAll().Include(x => x.Organization).Include(x => x.Devices).ToList();

            return View(assets);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            var model = new CreateAssetViewModel();
            InitModel(model);
            return View(model);
        }

        [Route("{id}/Edit")]
        public IActionResult Edit(int id)
        {
            var model = new CreateAssetViewModel();
            InitModel(model);

            var asset = _assetRepository.GetById(id);
            
            if (asset != null)
            {
                if (IsLimitedByOrganization && asset.Organization.Id != OrganizationId)
                    return Unauthorized("Usted no está autorizado a editar ésta Instalación");

                model.Id = asset.Id;
                model.Name = asset.Name.Trim();
                model.SelectedOrganizationId = asset.Organization.Id;

                return View(model);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Update(CreateAssetViewModel model)
        {
            var errors = ValidateModel(model);
            if (errors.Any())
            {
                ModelState.Clear();
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                InitModel(model);
                return View("Edit", model);
            }

            try
            {
                var asset = model.Id > 0
                    ? _assetRepository.GetById(model.Id)
                    : new Asset();

                if (asset != null)
                {
                    asset.Name = model.Name.Trim();
                    asset.Organization = _organizationRepository.GetById(model.SelectedOrganizationId);

                    if (model.Id > 0)
                    {
                        _assetRepository.Update(asset);
                    }
                    else
                    {
                        _assetRepository.Insert(asset);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Instalación no encontrada");
                    InitModel(model);
                    return View("Edit", model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("No se puede actualizar la Instalación", ex);
                InitModel(model);
                return View("Edit", model);
            }

            return RedirectToAction("Index");
        }

        private void InitModel(CreateAssetViewModel model)
        {
            model.Organizations = IsLimitedByOrganization
                ? _organizationRepository.GetAll().Where(x => x.Id == OrganizationId).OrderBy(x => x.Name).ToList()
                : _organizationRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        private List<string> ValidateModel(CreateAssetViewModel model)
        {
            var errors = new List<string>();

            if (IsLimitedByOrganization && model.SelectedOrganizationId != OrganizationId)
                errors.Add("Usted no está autorizado a editar esa Instalación");

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                errors.Add("El nombre es requerido");
            }

            var asset = _assetRepository.GetByName(model.Name, model.SelectedOrganizationId);
            if (asset != null && asset.Id != model.Id)
            {
                errors.Add("Una Instalación con el mismo nombre ya existe");
            }

            return errors;
        }

        [HttpPost]
        [Route("{id}/Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                var asset = _assetRepository.GetById(id);

                if (asset != null)
                {
                    if (IsLimitedByOrganization && asset.Organization.Id != OrganizationId)
                        return Unauthorized("Usted no está autorizado a eliminar ésta Instalación");

                    _assetRepository.Delete(asset);
                }
                else
                    ModelState.AddModelError("", "Instalación no encontrada");
                
            }
            catch (Exception ex)
            {
                _logger.LogError("No se puede borrar la Instalación", ex);
            }
            return RedirectToAction("Index");
        }

        private void Errors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

    }
}

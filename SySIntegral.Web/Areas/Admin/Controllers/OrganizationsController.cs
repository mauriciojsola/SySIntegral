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
using SySIntegral.Core.Entities.Organizations;
using SySIntegral.Core.Entities.Users;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.Organizations;
using SySIntegral.Web.Areas.Admin.Models.Organizations;
using SySIntegral.Web.Areas.Admin.Models.Users;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    [Route("Admin/[Controller]")]
    [Area("Admin")]
    [Authorize]
    public class OrganizationsController : Controller
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ILogger<OrganizationsController> _logger;

        public OrganizationsController(IOrganizationRepository organizationRepository, ILogger<OrganizationsController> logger)
        {
            _logger = logger;
            _organizationRepository = organizationRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            var organizations = _organizationRepository.GetAll().Include(x => x.Assets).ToList();
            return View(organizations);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            InitModel();
            return View(new CreateOrganizationViewModel());
        }

        [Route("Edit")]
        public IActionResult Edit(int id)
        {
            InitModel();

            var organization = _organizationRepository.GetById(id);

            if (organization != null)
            {
                return View(new CreateOrganizationViewModel
                {
                    Id = organization.Id,
                    Name = organization.Name
                });
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Update(CreateOrganizationViewModel model)
        {
            var errors = ValidateModel(model);
            if (errors.Any())
            {
                ModelState.Clear();
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                InitModel();
                return View("Edit", model);
            }

            try
            {
                var org = model.Id > 0
                    ? _organizationRepository.GetById(model.Id)
                    : new Organization();

                if (org != null)
                {
                    org.Name = model.Name;

                    if (model.Id > 0)
                    {
                        _organizationRepository.Update(org);
                    }
                    else
                    {
                        _organizationRepository.Insert(org);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Organización no encontrada");
                    InitModel();
                    return View("Edit", model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("No se puede actualizar al Organización", ex);
                InitModel();
                return View("Edit", model);
            }

            return RedirectToAction("Index");
        }

        private void InitModel()
        {

        }

        private List<string> ValidateModel(CreateOrganizationViewModel model)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                errors.Add("El nombre es requerido");
            }

            var org = _organizationRepository.GetByName(model.Name);
            if (org != null && org.Id != model.Id)
            {
                errors.Add("Una Organización con el mismo nombre ya existe");
            }

            return errors;
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                var org = _organizationRepository.GetById(id);

                if (org != null)
                {
                    _organizationRepository.Delete(org);

                }
                else
                    ModelState.AddModelError("", "Organización no encontrada");
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("No se puede borrar la Organización", ex);
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

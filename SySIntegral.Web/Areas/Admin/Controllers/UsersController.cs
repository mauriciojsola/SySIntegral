using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SySIntegral.Core.Entities.Organizations;
using SySIntegral.Core.Entities.Users;
using SySIntegral.Core.Repositories;
using SySIntegral.Web.Areas.Admin.Models.Users;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    [Route("Admin/[Controller]")]
    [Area("Admin")]
    [Authorize]
    public class UsersController : SySIntegralBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IRepository<Organization> _organizationRepository;

        public UsersController(UserManager<ApplicationUser> userManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            RoleManager<IdentityRole> roleManager,
            IRepository<Organization> organizationRepository
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _organizationRepository = organizationRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            var users = GetUsers();
            return View(users);
        }

        private IList<UserDisplayViewModel> GetUsers()
        {
            var result = new List<UserDisplayViewModel>();
            var users = IsLimitedByOrganization ? _userManager.Users.Where(x => x.Organization.Id == OrganizationId) : _userManager.Users;
            users = users.Include(x => x.Organization).OrderBy(x => x.Organization.Name).ThenBy(x => x.LastName).ThenBy(x => x.FirstName);

            foreach (var user in users.ToList())
            {
                var roles = _userManager.GetRolesAsync(user).Result;

                result.Add(new UserDisplayViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Organization = user.Organization.Name,
                    Roles = roles
                });
            }

            return result;
        }

        [Route("Create")]
        public IActionResult Create()
        {
            InitModel();
            return View(new CreateUserViewModel());
        }

        [Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            InitModel();

            var user = await _userManager.FindByIdAsync(id);
            var roles = _roleManager.Roles.ToList();
            var userRole = "";
            foreach (var role in roles)
            {
                var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
                if (isInRole)
                {
                    userRole += role.Id;
                }
            }

            if (user != null)
            {
                return View(new CreateUserViewModel
                {
                    Email = user.Email,
                    Password = "",
                    Id = user.Id,
                    OrganizationId = user.OrganizationId,
                    RoleId = userRole,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(CreateUserViewModel model)
        {
            var errors = ValidateModel(model);
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                InitModel();
                return View("Edit", model);
            }

            var user = !string.IsNullOrWhiteSpace(model.Id)
                ? await _userManager.FindByIdAsync(model.Id)
            : new ApplicationUser();

            var org = _organizationRepository.GetById(model.OrganizationId);
            var role = _roleManager.FindByIdAsync(model.RoleId).Result;

            if (user != null)
            {
                user.OrganizationId = model.OrganizationId;
                user.Organization = org;
                user.UserName = model.Email;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                if (!string.IsNullOrEmpty(model.Password))
                    user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

                var result = !string.IsNullOrWhiteSpace(model.Id)
                    ? await _userManager.UpdateAsync(user)
                    : await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!_userManager.IsInRoleAsync(user, role.Name).Result)
                    {
                        ClearUserRoles(user);
                        var r = await _userManager.AddToRoleAsync(user, role.Name);
                        if (r.Succeeded)
                            return RedirectToAction("Index");
                        else
                        {
                            Errors(r);
                            InitModel();
                            return View("Edit", model);
                        }
                    }
                }
                else
                {
                    Errors(result);
                    InitModel();
                    return View("Edit", model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Usuario no encontrado");
                InitModel();
                return View("Edit", model);
            }


            return RedirectToAction("Index");
        }

        private void ClearUserRoles(ApplicationUser user)
        {
            var roles = _roleManager.Roles.ToList();
            foreach (var role in roles)
            {
                if (_userManager.IsInRoleAsync(user, role.Name).Result)
                {
                    var res = _userManager.RemoveFromRoleAsync(user, role.Name).Result;
                }
            }
        }

        private void InitModel()
        {
            ViewData["roles"] = _roleManager.Roles.ToList();
            ViewData["organizations"] = IsLimitedByOrganization
                    ? _organizationRepository.GetAll().Where(x => x.Id == OrganizationId).OrderBy(x => x.Name).ToList()
                    : _organizationRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        private List<string> ValidateModel(CreateUserViewModel model)
        {
            var errors = new List<string>();
            if (model.OrganizationId <= 0)
                errors.Add("La organización es requerida");

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                errors.Add("El email es requerido");
            }

            if (string.IsNullOrWhiteSpace(model.RoleId))
            {
                errors.Add("El rol es requerido");
            }

            if (string.IsNullOrWhiteSpace(model.Id))
            {
                // New
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    errors.Add("La contraseña es requerida");
                }
                if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
                {
                    errors.Add("La confirmación de contraseña es requerida");
                }

                if ((!string.IsNullOrWhiteSpace(model.Password) || !string.IsNullOrWhiteSpace(model.ConfirmPassword))
                    && model.Password != model.ConfirmPassword)
                {
                    errors.Add("La contraseña y su confirmación no coinciden");
                }
            }
            else
            {
                // Edit
                if ((!string.IsNullOrWhiteSpace(model.Password) || !string.IsNullOrWhiteSpace(model.ConfirmPassword))
                    && model.Password != model.ConfirmPassword)
                {
                    errors.Add("La contraseña y su confirmación no coinciden");
                }
            }

            return errors;
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", _userManager.Users);
        }

        private void Errors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        //[HttpPost]
        //[Route("Create")]
        //public async Task<IActionResult> Create(IdentityRole role)
        //{
        //    await roleManager.CreateAsync(role);
        //    return RedirectToAction("Index");
        //}
    }
}

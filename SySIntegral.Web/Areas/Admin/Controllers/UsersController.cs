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
    public class UsersController : Controller
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
            var users = _userManager.Users.Include(x => x.Organization).OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
            return View(users);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            ViewData["roles"] = _roleManager.Roles.ToList();
            ViewData["organizations"] = _organizationRepository.GetAll().ToList();
            return View(new CreateUserViewModel());
        }

        [Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
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
                ViewData["roles"] = roles;
                ViewData["organizations"] = _organizationRepository.GetAll().ToList();
                return View(new CreateUserViewModel
                {
                    Email = user.Email,
                    Password = "",
                    Id = user.Id,
                    OrganizationId = user.OrganizationId,
                    UserRole = userRole
                });
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(CreateUserViewModel model)
        {
            var user = !string.IsNullOrWhiteSpace(model.Id)
                ? await _userManager.FindByIdAsync(model.Id)
            : new ApplicationUser();

            var org = _organizationRepository.Get(model.OrganizationId);
            var role = _roleManager.FindByIdAsync(model.UserRole).Result;

            if (user != null)
            {
                user.OrganizationId = model.OrganizationId;
                user.Organization = org;
                user.UserName = model.Email;

                if (!string.IsNullOrEmpty(model.Email))
                    user.Email = model.Email;
                else
                    ModelState.AddModelError("", "El Email es requerido");

                if (!string.IsNullOrEmpty(model.Password))
                    user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                else
                    ModelState.AddModelError("", "La contraseña es requerida");

                if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password))
                {
                    var result = !string.IsNullOrWhiteSpace(model.Id) ? await _userManager.UpdateAsync(user) : await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var r = await _userManager.AddToRoleAsync(user, role.Name);
                        if (result.Succeeded)
                            return RedirectToAction("Index");
                        else
                            Errors(result);
                    }
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "Usuario no encontrado");
            return View("Edit", user);
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

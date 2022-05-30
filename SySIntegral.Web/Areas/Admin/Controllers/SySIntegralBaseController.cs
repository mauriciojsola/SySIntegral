using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SySIntegral.Core.Application.Mvc.ViewFeatures;
using SySIntegral.Core.Entities.Users;
using SySIntegral.Core.Infrastructure.Auth;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    public class SySIntegralBaseController : Controller
    {
        // https://www.johndaniel.com/basecontroller-with-dependency-injection/
        // https://github.com/flavien/quickwire

        //private UserManager<ApplicationUser> _userManager;
        private ICurrentUser _currentUser;

        protected const string TempDataKey = "TempDataKey";
        protected const string TempDataErrorsKey = "TempDataErrorsKey";

        //public UserManager<ApplicationUser> UserManager => _userManager ?? (_userManager = HttpContext.RequestServices.GetService<UserManager<ApplicationUser>>());
        public ICurrentUser CurrentUser => _currentUser ??= HttpContext.RequestServices.GetService<ICurrentUser>();

        protected bool IsLimitedByOrganization => CurrentUser.IsLimitedByOrganization();
        protected int OrganizationId => CurrentUser.GetOrganizationId();

        
        protected void LoadErrors()
        {
            var errors = TempData.Get<List<string>>(TempDataErrorsKey);
            if (errors != null && errors.Any())
            {
                foreach (var error in errors)
                    ModelState.AddModelError("", error);
            }

        }

        protected void AddErrors(string error)
        {
            AddErrors(new List<string> { error });
        }

        protected void AddErrors(List<string> errors)
        {
            TempData.Set<List<string>>(TempDataErrorsKey, errors);
        }
    }
}

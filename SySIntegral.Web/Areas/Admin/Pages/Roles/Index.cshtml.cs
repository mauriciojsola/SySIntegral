//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace SySIntegral.Web.Areas.Admin.Pages.Roles
//{
//    [Authorize]
//    public class IndexModel : PageModel
//    {
//        private readonly RoleManager<IdentityRole> _roleManager;

//        public IndexModel(RoleManager<IdentityRole> roleManager)
//        {
//            _roleManager = roleManager;
//        }

//        public IList<IdentityRole> Roles { get; set; }

//        public IActionResult OnGet()
//        {
//            Roles = _roleManager.Roles.ToList();
//            return Page();
//        }



//    }
//}

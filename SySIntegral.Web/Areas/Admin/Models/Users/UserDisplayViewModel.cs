using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SySIntegral.Core.Entities.Organizations;

namespace SySIntegral.Web.Areas.Admin.Models.Users
{
    public class UserDisplayViewModel
    {
        public UserDisplayViewModel()
        {
            Roles = new List<string>();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<string> Roles { get; set; }
        public string Organization { get; set; }
    }
}

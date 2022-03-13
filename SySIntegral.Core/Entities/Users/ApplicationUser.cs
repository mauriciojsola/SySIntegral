using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using SySIntegral.Core.Entities.Organizations;

namespace SySIntegral.Core.Entities.Users
{
    public class ApplicationUser : IdentityUser
    {
        public Organization Organization { get; set; }

        public string OrganizationId { get; set; }

        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }
    }
}

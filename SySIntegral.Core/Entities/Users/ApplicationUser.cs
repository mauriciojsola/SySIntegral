using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace SySIntegral.Core.Entities.Users
{
    public class ApplicationUser : IdentityUser
    {
        public string OrganizationId { get; set; }

        [PersonalData]   
        public string FirstName { get; set; }
 
        [PersonalData]  
        public string LastName { get; set; }
    }
}

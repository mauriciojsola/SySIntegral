using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Core.Infrastructure.Auth
{
    public class SySClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public SySClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var claims = new List<Claim>();

            var userWitOrg = UserManager.Users.Include(x => x.Organization).FirstOrDefault(x => x.Id == user.Id);
            if (userWitOrg?.Organization != null)
                claims.Add(new Claim(SySClaims.OrganizationId, userWitOrg.Organization.Id.ToString()));

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            ((ClaimsIdentity)principal.Identity).AddClaims(claims);

            return principal;
        }
    }
}

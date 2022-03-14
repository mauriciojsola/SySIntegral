using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
            if (user.Organization != null)
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(SySClaims.OrganizationId, user.Organization.Id.ToString())
                });
            }
            return principal;
        }
    }
}

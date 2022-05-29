using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SySIntegral.Core.Infrastructure.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmail(this ClaimsPrincipal principal)
            => principal.FindFirstValue(ClaimTypes.Email);

        public static int GetOrganizationId(this ClaimsPrincipal principal)
            => Convert.ToInt32(principal.FindFirstValue(SySClaims.OrganizationId) ?? "0") ;

        public static string GetFullName(this ClaimsPrincipal principal)
            => principal?.FindFirst(SySClaims.Fullname)?.Value;

        public static string GetFirstName(this ClaimsPrincipal principal)
            => principal?.FindFirst(ClaimTypes.Name)?.Value;

        public static string GetSurname(this ClaimsPrincipal principal)
            => principal?.FindFirst(ClaimTypes.Surname)?.Value;

        public static string GetPhoneNumber(this ClaimsPrincipal principal)
            => principal.FindFirstValue(ClaimTypes.MobilePhone);

        public static string GetUserId(this ClaimsPrincipal principal)
            => principal.FindFirstValue(ClaimTypes.NameIdentifier);

        private static string FindFirstValue(this ClaimsPrincipal principal, string claimType) =>
            principal is null
                ? throw new ArgumentNullException(nameof(principal))
                : principal.FindFirst(claimType)?.Value;
    }
}

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SySIntegral.Core.Entities.Users
{
    public interface ICurrentUser
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserEmail();
        int GetOrganizationId();
        string GetOrganizationName();
        bool IsAuthenticated();
        bool IsInRole(string role);
        bool IsLimitedByOrganization();
        IEnumerable<Claim> GetUserClaims();
    }
}

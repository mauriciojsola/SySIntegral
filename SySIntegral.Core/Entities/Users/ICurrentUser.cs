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
        string GetOrganizationId();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetUserClaims();
    }
}

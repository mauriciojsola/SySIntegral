using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using SySIntegral.Core.Entities.Roles;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Core.Infrastructure.Auth
{
    public class CurrentUser : ICurrentUser, ICurrentUserInitializer
    {
        private ClaimsPrincipal _user;

        public string Name => _user?.Identity?.Name;

        private Guid _userId = Guid.Empty;

        public Guid GetUserId() =>
            IsAuthenticated()
                ? Guid.Parse(_user?.GetUserId() ?? Guid.Empty.ToString())
                : _userId;

        public string GetUserEmail() =>
            IsAuthenticated()
                ? _user!.GetEmail()
                : string.Empty;

        public bool IsAuthenticated() =>
            _user?.Identity?.IsAuthenticated is true;

        public bool IsInRole(string role) =>
            _user?.IsInRole(role) is true;

        public bool IsLimitedByOrganization() => !IsInRole(SySRoles.Administrator);

        public IEnumerable<Claim> GetUserClaims() =>
            _user?.Claims;

        public int GetOrganizationId() =>
            IsAuthenticated() && _user != null ? _user.GetOrganizationId() : 0;

        public string GetOrganizationName() =>
            IsAuthenticated() && _user != null ? _user.GetOrganizationName() : string.Empty;

        public void SetCurrentUser(ClaimsPrincipal user)
        {
            if (_user != null)
            {
                throw new Exception("Method reserved for in-scope initialization");
            }

            _user = user;
        }

        public void SetCurrentUserId(string userId)
        {
            if (_userId != Guid.Empty)
            {
                throw new Exception("Method reserved for in-scope initialization");
            }

            if (!string.IsNullOrEmpty(userId))
            {
                _userId = Guid.Parse(userId);
            }
        }
    }
}

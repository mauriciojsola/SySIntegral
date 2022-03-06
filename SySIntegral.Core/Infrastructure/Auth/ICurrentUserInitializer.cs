using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SySIntegral.Core.Infrastructure.Auth
{
    public interface ICurrentUserInitializer
    {
        void SetCurrentUser(ClaimsPrincipal user);
        void SetCurrentUserId(string userId);
    }
}

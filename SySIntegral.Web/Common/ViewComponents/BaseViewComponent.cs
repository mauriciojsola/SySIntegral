using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SySIntegral.Core.Entities.Users;
using Microsoft.Extensions.DependencyInjection;

namespace SySIntegral.Web.Common.ViewComponents
{
    public abstract class BaseViewComponent : ViewComponent
    {
        private ICurrentUser _currentUser;
        protected ICurrentUser CurrentUser => _currentUser ??= HttpContext.RequestServices.GetService<ICurrentUser>();

        protected bool IsLimitedByOrganization => CurrentUser.IsLimitedByOrganization();
        protected int OrganizationId => CurrentUser.GetOrganizationId();

        protected BaseViewComponent()
        {
        }

    }
}

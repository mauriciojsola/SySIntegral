using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Repositories.Assets;

namespace SySIntegral.Web.Areas.Admin.Views.Organizations
{
    public class AssetsListViewComponent : ViewComponent
    {
        private readonly IAssetRepository _assetRepository;

        public AssetsListViewComponent(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int organizationId)
        {
            var items = await GetAssetsAsync(organizationId);
            return View(items);
        }

        private Task<List<Asset>> GetAssetsAsync(int organizationId)
        {
            return _assetRepository.GetAll().Where(x => x.Organization.Id == organizationId)
                .Include(x => x.Organization).Include(x => x.Devices).ToListAsync();
        }
    }
}

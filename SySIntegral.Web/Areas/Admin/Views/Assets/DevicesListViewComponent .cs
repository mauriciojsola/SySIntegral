using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Entities.Devices;
using SySIntegral.Core.Repositories.Assets;
using SySIntegral.Core.Repositories.Devices;

namespace SySIntegral.Web.Areas.Admin.Views.Assets
{
    public class DevicesListViewComponent  : ViewComponent
    {
        private readonly IDeviceRepository _deviceRepository;

        public DevicesListViewComponent (IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int assetId)
        {
            var items = await GetDevicesAsync(assetId);
            return View(items);
        }

        private Task<List<Device>> GetDevicesAsync(int assetId)
        {
            return _deviceRepository.GetAll().Where(x => x.Asset.Id == assetId)
                .Include(x => x.Asset).ToListAsync();
        }
    }
}

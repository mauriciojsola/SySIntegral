using System;
using System.Collections.Generic;
using System.Text;
using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Entities.Devices;
using SySIntegral.Core.Entities.Organizations;

namespace SySIntegral.Core.Repositories.Devices
{
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(ApplicationDbContext context) : base(context)
        {

        }
    }

    public interface IDeviceRepository : IRepository<Device>
    {
    }
}

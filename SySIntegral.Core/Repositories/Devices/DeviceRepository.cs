using System;
using System.Collections.Generic;
using System.Linq;
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

        public Device GetByDescription(string description, int organizationId)
        {
            return GetAll().FirstOrDefault(x => x.Description == description && x.Asset.Organization.Id == organizationId); 
        }

        public Device GetByUniqueID(string uniqueId)
        {
            return GetAll().FirstOrDefault(x => x.UniqueId == uniqueId); 
        }
    }

    public interface IDeviceRepository : IRepository<Device>
    {
        Device GetByDescription(string description, int organizationId);
        Device GetByUniqueID(string uniqueId);
    }
}

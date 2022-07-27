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
    public class InputDeviceRepository : GenericRepository<InputDevice>, IInputDeviceRepository
    {
        public InputDeviceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public InputDevice GetByDescription(string description, int organizationId)
        {
            return GetAll().FirstOrDefault(x => x.Description == description && x.Asset.Organization.Id == organizationId); 
        }

        public InputDevice GetByUniqueID(string uniqueId)
        {
            return GetAll().FirstOrDefault(x => x.UniqueId == uniqueId); 
        }
    }

    public interface IInputDeviceRepository : IRepository<InputDevice>
    {
        InputDevice GetByDescription(string description, int organizationId);
        InputDevice GetByUniqueID(string uniqueId);
    }
}

using System.Linq;
using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.Devices;

namespace SySIntegral.Core.Repositories.CheckPoints
{
    public class CheckPointRepository : GenericRepository<CheckPoint>, ICheckPointRepository
    {
        public CheckPointRepository(ApplicationDbContext context) : base(context)
        {
        }

        public CheckPoint GetByInputDevice(int inputDeviceId)
        {
            return GetAll().FirstOrDefault(x => x.InputDevice != null && x.InputDevice.Id == inputDeviceId);
        }
    }

    public interface ICheckPointRepository : IRepository<CheckPoint>
    {
        CheckPoint GetByInputDevice(int inputDeviceId);
    }
}

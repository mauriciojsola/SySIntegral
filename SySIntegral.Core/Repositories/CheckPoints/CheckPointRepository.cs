using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.Devices;

namespace SySIntegral.Core.Repositories.CheckPoints
{
    public class CheckPointRepository : GenericRepository<CheckPoint>, ICheckPointRepository
    {
        public CheckPointRepository(ApplicationDbContext context) : base(context)
        {
        }
        
    }

    public interface ICheckPointRepository : IRepository<CheckPoint>
    {
        
    }
}

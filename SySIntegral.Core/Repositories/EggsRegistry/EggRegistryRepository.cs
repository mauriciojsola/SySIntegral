using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.EggsRegistry;

namespace SySIntegral.Core.Repositories.EggsRegistry
{
    public class EggRegistryRepository : GenericRepository<EggRegistry>, IEggRegistryRepository
    {
        public EggRegistryRepository(ApplicationDbContext context) : base(context)
        {

        }
    }

    public interface IEggRegistryRepository : IRepository<EggRegistry>
    {
    }
}

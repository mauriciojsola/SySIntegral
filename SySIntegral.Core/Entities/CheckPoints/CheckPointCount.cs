using SySIntegral.Core.Entities.EggsRegistry;

namespace SySIntegral.Core.Entities.CheckPoints
{
    public class CheckPointCount
    {
        public virtual int CheckPointId { get; set; }
        public virtual CheckPoint CheckPoint { get; set; }

        public virtual int EggRegistryId { get; set; }
        public virtual EggRegistry Registry { get; set; }

    }
}

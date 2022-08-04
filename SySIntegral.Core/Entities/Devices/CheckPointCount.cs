using System;
using System.Collections.Generic;
using System.Text;
using SySIntegral.Core.Entities.EggsRegistry;

namespace SySIntegral.Core.Entities.Devices
{
    public class CheckPointCount
    {
        public virtual int CheckPointId { get; set; }
        public virtual CheckPoint CheckPoint { get; set; }

        public virtual int EggRegistryId { get; set; }
        public virtual EggRegistry Registry { get; set; }

    }
}

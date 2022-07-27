using System;
using System.Collections.Generic;
using System.Text;
using SySIntegral.Core.Entities.EggsRegistry;

namespace SySIntegral.Core.Entities.Devices
{
    public class CounterDeviceCount
    {
        public int CounterDeviceId { get; set; }
        public CounterDevice CounterDevice { get; set; }

        public int EggRegistryId { get; set; }
        public EggRegistry Registry { get; set; }

    }
}

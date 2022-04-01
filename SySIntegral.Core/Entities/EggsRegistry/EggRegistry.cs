using System;
using System.Collections.Generic;
using System.Text;

namespace SySIntegral.Core.Entities.EggsRegistry
{
    public class EggRegistry: BaseEntity
    {
        public string DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public int WhiteEggsCount { get; set; }
        public int ColorEggsCount { get; set; }
    }
}

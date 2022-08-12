using System;
using System.Collections.Generic;
using System.Text;
using SySIntegral.Core.Entities.Devices;

namespace SySIntegral.Core.Entities.EggsRegistry
{
    public class EggRegistry: BaseEntity
    {
        public EggRegistry()
        {
        }

        public int InputDeviceId { get; set; }
        public virtual InputDevice Device { get; set; }

        //public string OldDeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public int WhiteEggsCount { get; set; }
        public int ColorEggsCount { get; set; }
        public DateTime? ReadTimestamp { get; set; }
        public DateTime? ExportTimestamp { get; set; }

    }
}

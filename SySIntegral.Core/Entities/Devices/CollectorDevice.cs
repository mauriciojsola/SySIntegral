using System;
using System.Collections.Generic;
using System.Text;

namespace SySIntegral.Core.Entities.Devices
{
    public class CollectorDevice : Device
    {
        public CollectorDevice()
        {
            DeviceType = DeviceType.Collector;
        }
    }
}

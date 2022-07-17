using System;
using System.Collections.Generic;
using System.Text;

namespace SySIntegral.Core.Entities.Devices
{
    public class CounterDevice : Device
    {
        public CounterDevice()
        {
            DeviceType = DeviceType.Counter;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SySIntegral.Core.Entities.Devices
{
    public class LineDevice : Device
    {
        public LineDevice()
        {
            DeviceType = DeviceType.Line;
        }
    }

}

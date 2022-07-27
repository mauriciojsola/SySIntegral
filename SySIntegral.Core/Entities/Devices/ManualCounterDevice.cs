using System;
using System.Collections.Generic;
using System.Text;

namespace SySIntegral.Core.Entities.Devices
{
    public class ManualCounterDevice : CounterDevice
    {
        public ManualCounterDevice()
        {
            CounterDeviceType = CounterDeviceType.Manual;
        }

        public override void AddChildren(CounterDevice children)
        {
            throw new NotImplementedException();
        }

        public override void RemoveChildren(CounterDevice children)
        {
            throw new NotImplementedException();
        }

        public override int Count()
        {
            throw new NotImplementedException();
        }
    }
}

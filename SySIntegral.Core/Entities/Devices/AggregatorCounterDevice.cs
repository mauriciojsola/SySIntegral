using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SySIntegral.Core.Entities.Devices
{
    public class AggregatorCounterDevice : CounterDevice
    {
        public AggregatorCounterDevice()
        {
            CounterDeviceType = CounterDeviceType.Aggregator;
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
            return Children.Sum(counter => counter.Count());
        }
    }
}

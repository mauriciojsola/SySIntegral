using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SySIntegral.Core.Entities.Devices
{
    public class LineCounterDevice : CounterDevice
    {
        public LineCounterDevice()
        {
            CounterDeviceType = CounterDeviceType.Line;
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
            return Children.Any() ? Children.OrderBy(x => x.SortOrder).First().Count() : 0;
        }
    }

}

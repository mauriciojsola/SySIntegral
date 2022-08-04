using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SySIntegral.Core.Entities.Devices
{
    public class AggregatorCheckPoint : CheckPoint
    {
        public AggregatorCheckPoint()
        {
            CheckPointType = CheckPointType.Aggregator;
        }

        public override void AddChildren(CheckPoint children)
        {
            throw new NotImplementedException();
        }

        public override void RemoveChildren(CheckPoint children)
        {
            throw new NotImplementedException();
        }

        public override int Count()
        {
            return Children.Sum(counter => counter.Count());
        }
    }
}

using System;
using System.Linq;

namespace SySIntegral.Core.Entities.CheckPoints
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

using System;

namespace SySIntegral.Core.Entities.CheckPoints
{
    public class LineCheckPoint : CheckPoint
    {
        public LineCheckPoint()
        {
            CheckPointType = CheckPointType.Line;
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
            return 0; //Children.Any() ? Children.OrderBy(x => x.SortOrder).First().Count() : 0;
        }
    }

}

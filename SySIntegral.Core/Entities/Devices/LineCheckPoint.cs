using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SySIntegral.Core.Entities.Devices
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

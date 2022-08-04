using System;
using System.Collections.Generic;
using System.Text;

namespace SySIntegral.Core.Entities.Devices
{
    public class SimpleCheckPoint : CheckPoint
    {
        public SimpleCheckPoint()
        {
            CheckPointType = CheckPointType.Simple;
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
            throw new NotImplementedException();
        }
    }
}

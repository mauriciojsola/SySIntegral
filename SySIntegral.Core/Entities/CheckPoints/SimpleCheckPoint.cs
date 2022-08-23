using System;

namespace SySIntegral.Core.Entities.CheckPoints
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

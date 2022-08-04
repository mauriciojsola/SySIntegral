using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using SySIntegral.Core.Entities.Assets;

namespace SySIntegral.Core.Entities.Devices
{
    public abstract class CheckPoint : BaseEntity, ICheckPoint
    {
        protected CheckPoint()
        {
            Children = new List<CheckPoint>();
            Countings = new List<CheckPointCount>();
        }

        public virtual CheckPointType CheckPointType { get; protected set; }

        public virtual string Description { get; set; }
        public virtual int AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public virtual int? ParentId { get; set; }
        public virtual CheckPoint Parent { get; set; }

        //[NotMapped]
        public virtual ICollection<CheckPoint> Children { get; protected set; }
        //[NotMapped]
        //public int SortOrder { get; set; }

        public virtual int? InputDeviceId { get; set; }
        public virtual InputDevice InputDevice { get; set; }

        //[NotMapped]
        public virtual ICollection<CheckPointCount> Countings { get; set; }

        public abstract void AddChildren(CheckPoint children);
        public abstract void RemoveChildren(CheckPoint children);
        public abstract int Count();

        //{
        //    return Children.Any() ? Children.OrderBy(x => x.SortOrder).FirstOrDefault().Count() : 0;
        //}
    }

    public interface ICheckPoint
    {
        //public CounterType CounterType { get; }
        //public Counter Parent { get; set; }
        //public IList<Counter> Children { get; }
        //public int SortOrder { get; set; }


        //public void AddChildren(Counter children);
        //public void RemoveChildren(Counter children);
        public int Count();
    }

    public enum CheckPointType
    {
        Aggregator = 1,
        Line = 2,
        Simple = 3
    }
}

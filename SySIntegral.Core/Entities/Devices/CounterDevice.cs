using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using SySIntegral.Core.Entities.Assets;

namespace SySIntegral.Core.Entities.Devices
{
    public abstract class CounterDevice : BaseEntity, ICounterDevice
    {
        protected CounterDevice()
        {
            Children = new List<CounterDevice>();
        }
        
        public CounterDeviceType CounterDeviceType { get; protected set; }
        [NotMapped]
        public CounterDevice Parent { get; set; }
        [NotMapped]
        public IList<CounterDevice> Children { get; protected set; }
        [NotMapped]
        public int SortOrder { get; set; }

        public int DeviceId { get; set; }
        public virtual InputDevice Device { get; set; }


        public abstract void AddChildren(CounterDevice children);
        public abstract void RemoveChildren(CounterDevice children);
        public abstract int Count();
        
        //{
        //    return Children.Any() ? Children.OrderBy(x => x.SortOrder).FirstOrDefault().Count() : 0;
        //}
    }

    public interface ICounterDevice
    {
        //public CounterType CounterType { get; }
        //public Counter Parent { get; set; }
        //public IList<Counter> Children { get; }
        //public int SortOrder { get; set; }


        //public void AddChildren(Counter children);
        //public void RemoveChildren(Counter children);
        public int Count();
    }

    public enum CounterDeviceType
    {
        Aggregator = 1,
        Line = 2,
        Manual = 3
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using SySIntegral.Core.Entities.Assets;

namespace SySIntegral.Core.Entities.Devices
{
    public abstract class Device : BaseEntity, IDevice
    {
        protected Device()
        {
            Children = new List<Device>();
        }

        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }
        public string Description { get; set; }
        public string UniqueId { get; set; }

        public DeviceType DeviceType { get; protected set; }
        [NotMapped]
        public Device Parent { get; set; }
        [NotMapped]
        public IList<Device> Children { get; set; }
        [NotMapped]
        public int SortOrder { get; set; }

        public virtual int Counter()
        {
            return Children.Any() ? Children.OrderBy(x => x.SortOrder).FirstOrDefault().Counter() : 0;
        }
    }

    public interface IDevice
    {
        public DeviceType DeviceType { get; }
        public Device Parent { get; set; }
        public IList<Device> Children { get; set; }
        public int SortOrder { get; set; }

        public int Counter();
    }

    public enum DeviceType
    {
        Collector = 1,
        Line = 2,
        Counter = 3
    }
}

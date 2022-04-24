using System;
using System.Collections.Generic;
using System.Text;
using SySIntegral.Core.Entities.Assets;

namespace SySIntegral.Core.Entities.Devices
{
    public class Device : BaseEntity
    {
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public string Description { get; set; }
        public string UniqueId { get; set; }
    }
}

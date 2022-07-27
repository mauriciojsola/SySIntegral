using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using SySIntegral.Core.Entities.Assets;

namespace SySIntegral.Core.Entities.Devices
{
    public class InputDevice : BaseEntity
    {
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }
        public string Description { get; set; }
        public string UniqueId { get; set; }
    }
}

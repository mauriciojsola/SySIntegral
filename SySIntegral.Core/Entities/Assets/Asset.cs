using System;
using System.Collections.Generic;
using System.Text;
using SySIntegral.Core.Entities.Devices;
using SySIntegral.Core.Entities.Organizations;

namespace SySIntegral.Core.Entities.Assets
{
    public class Asset : BaseEntity
    {
        public Asset()
        {
            Devices = new List<InputDevice>();
        }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public string Name { get; set; }
        public virtual ICollection<InputDevice> Devices { get; set; }
    }
}

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
            Devices = new List<Device>();
        }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string Name { get; set; }
        public ICollection<Device> Devices { get; set; }
    }
}

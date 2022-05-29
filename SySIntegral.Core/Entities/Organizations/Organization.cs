using System;
using System.Collections.Generic;
using System.Text;
using SySIntegral.Core.Entities.Assets;

namespace SySIntegral.Core.Entities.Organizations
{
    public class Organization : BaseEntity
    {
        public Organization()
        {
            Assets = new List<Asset>();
        }

        public string Name { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
    }
}

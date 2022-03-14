using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SySIntegral.Core.Entities
{
    public class BaseEntity : IIdentifiable
    {
        public int Id { get; set; }
    }

    public interface IIdentifiable
    {
        int Id { get; }
    }
}

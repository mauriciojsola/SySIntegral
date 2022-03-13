using System;
using System.Collections.Generic;
using System.Text;

namespace SySIntegral.Core.Entities
{
    public class EntityBase<T> : IEntity<T> where T : IEquatable<T>
    {
        public T Id { get; set; }
    }

    public interface IEntity
    {
    }

    public interface IEntity<T> : IEntity where T : IEquatable<T>
    {
        public T Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Domain.Abstractions
{
    public class Entity
    {
        public Guid Id { get; private set; }

        protected Entity(Guid id)
        {
            Id = id;
        }

        protected Entity() { }

        public override bool Equals(object? obj) => obj is Entity entity && entity.Id == Id;

        public override int GetHashCode() => Id.GetHashCode();

    }
}

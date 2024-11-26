using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Domain.Abstractions
{
    public class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _events = new();

        protected AggregateRoot(Guid id) : base(id)
        {

        }

        protected AggregateRoot() { }

        public void RaiseDomainEvent(IDomainEvent domainEvent) => _events.Add(domainEvent);

        public void ClearDomainEvents() => _events.Clear();

        public IReadOnlyList<IDomainEvent> GetDomainEvent() => _events.ToList();
    }
}

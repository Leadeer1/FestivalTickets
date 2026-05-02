using System;
using System.Collections.Generic;

namespace DDD.SharedKernel.DomainModelLayer.Implementations
{

    public abstract class Entity
    {
        public long Id { get; protected set; }

        private List<IDomainEvent> _domainEvents;
        public IEnumerable<IDomainEvent> DomainEvents
        {
            get { return _domainEvents.AsReadOnly(); }
        }

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void RemoveAllDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public Entity(long id)
        {
            this.Id = id;
            this._domainEvents = new List<IDomainEvent>();
        }

        public Entity()
        {
            this.Id = new Random().Next(0, int.MaxValue);
        }
            
    }
}

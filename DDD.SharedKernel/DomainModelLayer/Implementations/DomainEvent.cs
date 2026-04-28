using System;

namespace DDD.SharedKernel.DomainModelLayer.Implementations
{
    public abstract class DomainEvent : IDomainEvent
    {
        public long Created { get; protected set; }

        public DomainEvent()
        {
            Created = DateTime.Now.Ticks;
        }
    }
}

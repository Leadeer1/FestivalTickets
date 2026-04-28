using DDD.SharedKernel.DomainModelLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.SharedKernel.ApplicationLayer
{
    public interface IEventHandler
    { }

    public interface IEventHandler<TEvent> : IEventHandler 
        where TEvent : IDomainEvent
    {
        void Handle(TEvent eventData);
    }
}

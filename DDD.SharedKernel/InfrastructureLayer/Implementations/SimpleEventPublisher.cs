using DDD.SharedKernel.ApplicationLayer;
using DDD.SharedKernel.DomainModelLayer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.SharedKernel.InfrastructureLayer.Implementations
{
    public class SimpleEventPublisher : IDomainEventPublisher
    {
        protected IServiceProvider _serviceProvider;

        public SimpleEventPublisher(IServiceProvider servicePrivider)
        {
            _serviceProvider = servicePrivider;
        }

        public void Publish<T>(T domainEvent) 
            where T : IDomainEvent
        {
            var _eventHandlers = _serviceProvider.GetServices<IEventHandler<T>>(); 
            foreach (var handler in _eventHandlers)
            {
                handler.Handle(domainEvent);
            }
        }
    }

    
}

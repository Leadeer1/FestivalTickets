using System;
using DDD.FestivalTickets.Core.DomainModelLayer.Events;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.SharedKernel.ApplicationLayer;

namespace DDD.FestivalTickets.Core.ApplicationLayer.DomainEventHandlers
{
    public class SendConfirmationWhenTicketPurchasedHandler : IEventHandler<TicketPurchasedDomainEvent>
    {
        private readonly INotificationService _notificationService;
        
        public SendConfirmationWhenTicketPurchasedHandler(
            INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }
        public void Handle(TicketPurchasedDomainEvent eventData)
        {
            if (eventData == null)
                throw new ArgumentNullException(nameof(eventData));

            _notificationService.SendPurchaseConfirmation(
                eventData.CustomerId,
                eventData.TicketId,
                eventData.FinalPrice
            );
        }
    }
    
}

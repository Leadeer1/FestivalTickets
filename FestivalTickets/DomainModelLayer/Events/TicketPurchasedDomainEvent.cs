using System;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.SharedKernel.DomainModelLayer;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Events
{
    /// <summary>
    /// Zdarzenie domenowe publikowane przez agregat Ticket
    /// w momencie pomyślnego zakupu biletu (wywołanie Ticket.Purchase()).
    ///
    /// Subskrybent: SendConfirmationWhenTicketPurchasedHandler
    /// (wysyła potwierdzenie zakupu przez INotificationService)
    /// </summary>
    public class TicketPurchasedDomainEvent : IDomainEvent
    {
        public long Created    { get; }
        public long     TicketId   { get; }
        public long     CustomerId { get; }
        public long     ZoneId     { get; }
        public Money    FinalPrice { get; }

        public TicketPurchasedDomainEvent(Ticket ticket)
        {
            if (ticket == null) throw new ArgumentNullException(nameof(ticket));

            Created    = DateTime.UtcNow.Ticks;
            TicketId   = ticket.Id;
            CustomerId = ticket.CustomerId;
            ZoneId     = ticket.ZoneId;
            FinalPrice = ticket.FinalPrice;
        }
    }
}

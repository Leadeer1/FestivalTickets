using DDD.SharedKernel.DomainModelLayer;
using System;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Events
{
    /// <summary>
    /// Zdarzenie domenowe publikowane przez agregat Customer
    /// w momencie rejestracji nowego uczestnika festiwalu.
    ///
    /// Subskrybent: SendWelcomeEmailWhenCustomerRegisteredHandler
    /// (może wysłać email powitalny przez INotificationService)
    /// </summary>
    public class CustomerRegisteredDomainEvent : IDomainEvent
    {
        public DateTime Created    { get; }
        public long     CustomerId { get; }
        public string   FullName   { get; }
        public string   Email      { get; }

        public CustomerRegisteredDomainEvent(long customerId, string fullName, string email)
        {
            Created    = DateTime.UtcNow;
            CustomerId = customerId;
            FullName   = fullName;
            Email      = email;
        }
    }
}

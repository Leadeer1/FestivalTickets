using System;
using DDD.FestivalTickets.Core.DomainModelLayer.Events;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using DDD.SharedKernel.DomainModelLayer;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Models
{
    public enum CustomerStatus
    {
        Active = 0,
        Banned = 1
    }

    /// <summary>
    /// AGREGAT: Uczestnik festiwalu.
    /// Odpowiednik Player z EscapeRoom.
    ///
    /// Tabela: Customers
    ///   Id                   (PK)
    ///   FirstName
    ///   LastName
    ///   Email_Value          (VO Email spłaszczone przez OwnsOne)
    ///   CustomerType_Value   (VO CustomerType spłaszczone przez OwnsOne)
    ///   Status
    ///
    /// Logika biznesowa:
    ///   Ban()      – blokuje klienta, nie może kupować biletów
    ///   Activate() – odblokowuje klienta
    /// </summary>
    public class Customer : Entity, IAggregateRoot
    {
        public string         FirstName    { get; protected set; }
        public string         LastName     { get; protected set; }
        public Email          Email        { get; protected set; }   // VO
        public CustomerType   CustomerType { get; protected set; }   // VO – decyduje o rabacie
        public CustomerStatus Status       { get; protected set; }

        // wymagany przez EF Core
        protected Customer() { }

        public Customer(long id, string firstName, string lastName,
                        string email, CustomerType customerType)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Imię nie może być puste.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Nazwisko nie może być puste.");

            FirstName    = firstName;
            LastName     = lastName;
            Email        = new Email(email);    // Email waliduje format w konstruktorze
            CustomerType = customerType ?? throw new ArgumentNullException(nameof(customerType));
            Status       = CustomerStatus.Active;

            // zdarzenie domenowe – handler może wysłać email powitalny
            AddDomainEvent(new CustomerRegisteredDomainEvent(Id, FullName, email));
        }

        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Blokuje klienta. Zablokowany klient nie może kupować biletów –
        /// walidacja w TicketAvailabilityService.
        /// </summary>
        public void Ban()
        {
            if (Status == CustomerStatus.Banned)
                throw new InvalidOperationException($"Klient {FullName} jest już zablokowany.");
            Status = CustomerStatus.Banned;
        }

        public void Activate()
        {
            if (Status == CustomerStatus.Active)
                throw new InvalidOperationException($"Klient {FullName} jest już aktywny.");
            Status = CustomerStatus.Active;
        }
    }
}

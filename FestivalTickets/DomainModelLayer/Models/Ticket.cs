using System;
using System.Collections.Generic;
using DDD.FestivalTickets.Core.DomainModelLayer.Events;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using DDD.SharedKernel.DomainModelLayer;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Models
{
    public enum TicketStatus
    {
        Active    = 0,   // zakupiony, ważny – można wejść na strefę
        Used      = 1,   // zeskanowany przy bramce – uczestnik wszedł
        Cancelled = 2    // anulowany przed wydarzeniem
    }

    /// <summary>
    /// AGREGAT: Bilet na festiwal.
    /// Odpowiednik Visit z EscapeRoom – najważniejsza klasa w domenie.
    ///
    /// Tabela: Tickets
    ///   Id                    (PK)
    ///   ZoneId                (FK do Zones – relacja przez Id, nie referencja!)
    ///   CustomerId            (FK do Customers – relacja przez Id, nie referencja!)
    ///   EventDay_Date         (VO EventDay spłaszczone przez OwnsOne)
    ///   EventDay_DayName      (VO EventDay spłaszczone przez OwnsOne)
    ///   FinalPrice_Amount     (VO Money spłaszczone przez OwnsOne)
    ///   FinalPrice_Currency   (VO Money spłaszczone przez OwnsOne)
    ///   PurchasedAt
    ///   Status
    ///
    /// Tabela: TicketValidations – powiązana przez TicketId (HasMany w TicketConfiguration)
    ///
    /// Logika biznesowa (odpowiednik StopVisit() z EscapeRoom):
    ///   RegisterPolicy() – wstrzykuje politykę rabatową przed Purchase()
    ///   Purchase()       – oblicza FinalPrice, publikuje zdarzenie
    ///   Cancel()         – anuluje bilet
    ///   MarkAsUsed()     – skanuje bilet przy bramce, dodaje TicketValidation
    /// </summary>
    public class Ticket : Entity, IAggregateRoot
    {
        // relacje między agregatami PRZEZ ID – zasada DDD, nie przez referencje
        public long         ZoneId     { get; protected set; }
        public long         CustomerId { get; protected set; }

        public EventDay     EventDay   { get; protected set; }   // VO
        public Money        FinalPrice { get; protected set; }   // VO – uzupełniane w Purchase()
        public DateTime     PurchasedAt{ get; protected set; }
        public TicketStatus Status     { get; protected set; }

        // encja wewnątrz agregatu Ticket – historia skanowań przy bramce
        private readonly List<TicketValidation> _validations = new List<TicketValidation>();
        public IEnumerable<TicketValidation> Validations => _validations.AsReadOnly();

        // polityka wstrzyknięta przez TicketFactory przed wywołaniem Purchase()
        // analogicznie jak _discountPolicy w Visit z EscapeRoom
        private IDiscountPolicy _discountPolicy;

        // wymagany przez EF Core
        protected Ticket() { }

        public Ticket(long id, long zoneId, long customerId, EventDay eventDay)
            : base(id)
        {
            ZoneId     = zoneId;
            CustomerId = customerId;
            EventDay   = eventDay ?? throw new ArgumentNullException(nameof(eventDay));
            FinalPrice = Money.Zero;
            Status     = TicketStatus.Active;
        }

        /// <summary>
        /// Rejestruje politykę rabatową.
        /// Analogia: RegisterPolicy() z Visit w EscapeRoom.
        /// Wywołać PRZED Purchase().
        /// </summary>
        public void RegisterPolicy(IDiscountPolicy policy)
        {
            _discountPolicy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        /// <summary>
        /// GŁÓWNA METODA BIZNESOWA – odpowiednik StopVisit() z EscapeRoom.
        ///
        /// Oblicza cenę finalną:
        ///   1. Pobiera rabat od wstrzykniętej polityki
        ///   2. Odejmuje rabat od ceny bazowej (nigdy poniżej zera)
        ///   3. Zapisuje datę zakupu
        ///   4. Publikuje TicketPurchasedDomainEvent
        ///
        /// Wywoływana przez PurchaseTicketCommandHandler po:
        ///   - sprawdzeniu dostępności (TicketAvailabilityService)
        ///   - zarejestrowaniu polityki (RegisterPolicy)
        /// </summary>
        public void Purchase(Money basePrice, DateTime purchasedAt)
        {
            if (basePrice == null || basePrice < Money.Zero)
                throw new ArgumentException("Cena bazowa nie może być ujemna.");

            PurchasedAt = purchasedAt;

            if (_discountPolicy != null)
            {
                Money discount = _discountPolicy.CalculateDiscount(basePrice, purchasedAt, EventDay);
                FinalPrice = (discount >= basePrice) ? Money.Zero : basePrice - discount;
            }
            else
            {
                FinalPrice = basePrice;
            }

            // zdarzenie domenowe – handler wyśle potwierdzenie przez INotificationService
            AddDomainEvent(new TicketPurchasedDomainEvent(this));
        }

        /// <summary>
        /// Anuluje bilet. Tylko aktywny bilet można anulować.
        /// </summary>
        public void Cancel()
        {
            if (Status == TicketStatus.Used)
                throw new InvalidOperationException(
                    "Nie można anulować biletu który został już wykorzystany.");
            if (Status == TicketStatus.Cancelled)
                throw new InvalidOperationException("Bilet jest już anulowany.");

            Status = TicketStatus.Cancelled;
        }

        /// <summary>
        /// Oznacza bilet jako użyty przy bramce wejściowej.
        /// Zapisuje skanowanie jako TicketValidation (encja w agregacie).
        /// Uniemożliwia ponowne wejście tym samym biletem.
        /// </summary>
        public void MarkAsUsed(long validationId, string gateName)
        {
            if (Status != TicketStatus.Active)
                throw new InvalidOperationException(
                    "Bilet nie jest aktywny – nie można go zeskanować.");
            if (_validations.Count > 0)
                throw new InvalidOperationException(
                    "Bilet został już zeskanowany – próba ponownego wejścia.");

            _validations.Add(new TicketValidation(validationId, Id, gateName, DateTime.UtcNow));
            Status = TicketStatus.Used;
        }
    }
}

using System;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Factories
{
    /// <summary>
    /// Fabryka agregatów Ticket.
    /// Odpowiednik VisitFactory z EscapeRoom.
    ///
    /// Odpowiedzialność:
    ///   1. Tworzenie nowego Ticket z poprawnymi danymi
    ///   2. Rejestracja polityki rabatowej (RegisterPolicy)
    ///   3. Wywołanie Purchase() z ceną bazową strefy
    ///
    /// CommandHandler wywołuje fabrykę zamiast tworzyć Ticket bezpośrednio –
    /// gwarantuje to że każdy nowy bilet przejdzie przez tę samą logikę inicjalizacji.
    /// </summary>
    public class TicketFactory
    {
        private readonly DiscountPolicyFactory _policyFactory;

        public TicketFactory(DiscountPolicyFactory policyFactory)
        {
            _policyFactory = policyFactory
                ?? throw new ArgumentNullException(nameof(policyFactory));
        }

        /// <summary>
        /// Tworzy i inicjalizuje nowy bilet.
        ///
        /// Kolejność operacji (krytyczna):
        ///   1. new Ticket(...)        – stwórz agregat ze statusem Active
        ///   2. RegisterPolicy(...)    – wstrzyknij politykę PRZED Purchase
        ///   3. Purchase(...)          – oblicz FinalPrice, opublikuj zdarzenie
        /// </summary>
        /// <param name="ticketId">Id wygenerowane przez repozytorium lub sekwencję</param>
        /// <param name="zone">Strefa – potrzebna do ceny bazowej i ZoneId</param>
        /// <param name="customer">Klient – potrzebny do wyboru polityki i CustomerId</param>
        /// <param name="eventDay">Dzień festiwalu na który kupowany jest bilet</param>
        /// <returns>Gotowy agregat Ticket z uzupełnionym FinalPrice i zdarzeniem domenowym</returns>
        public Ticket Create(long ticketId, Zone zone, Customer customer, EventDay eventDay)
        {
            if (zone     == null) throw new ArgumentNullException(nameof(zone));
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (eventDay == null) throw new ArgumentNullException(nameof(eventDay));

            // krok 1 – stwórz agregat
            var ticket = new Ticket(ticketId, zone.Id, customer.Id, eventDay);

            // krok 2 – wybierz i zarejestruj politykę rabatową
            var policy = _policyFactory.Create(customer);
            ticket.RegisterPolicy(policy);

            // krok 3 – oblicz cenę (Purchase zawiera główną logikę biznesową)
            ticket.Purchase(zone.BaseTicketPrice, DateTime.UtcNow);

            return ticket;
        }
    }
}

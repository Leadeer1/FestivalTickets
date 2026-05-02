using System.Collections.Generic;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using DDD.SharedKernel.InfrastructureLayer;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Interfaces
{
    /// <summary>
    /// Repozytorium agregatu Ticket.
    /// Metody specyficzne dla domeny używane przez TicketAvailabilityService
    /// oraz CommandHandler przy zakupie/anulowaniu biletów.
    /// </summary>
    public interface ITicketRepository : IRepository<Ticket>
    {
        /// <summary>
        /// Zwraca wszystkie aktywne bilety danego klienta.
        /// </summary>
        IList<Ticket> GetByCustomer(long customerId);

        /// <summary>
        /// Zwraca wszystkie bilety na konkretną strefę w konkretnym dniu.
        /// Używane przez TicketAvailabilityService do sprawdzenia zapełnienia.
        /// </summary>
        IList<Ticket> GetByZoneAndDay(long zoneId, EventDay eventDay);

        /// <summary>
        /// Zwraca liczbę aktywnych (nieanulowanych) biletów na strefę w danym dniu.
        /// Optymalizacja – COUNT zamiast SELECT * gdy potrzebna tylko liczba.
        /// </summary>
        int CountActiveByZoneAndDay(long zoneId, EventDay eventDay);
    }
}

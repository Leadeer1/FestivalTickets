using System;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Services
{
    /// <summary>
    /// Domain Service – serwis domenowy sprawdzający dostępność biletów.
    /// Odpowiednik AddCommentService z EscapeRoom.
    ///
    /// Domain Service jest potrzebny gdy logika biznesowa OBEJMUJE KILKA AGREGATÓW
    /// i nie należy naturalnie do żadnego z nich.
    ///
    /// Tutaj: sprawdzamy Zone (pojemność, status) i Customer (status) jednocześnie.
    /// Ani Zone, ani Customer nie mają dostępu do repozytoriów – dlatego serwis domenowy.
    ///
    /// Używany przez PurchaseTicketCommandHandler PRZED wywołaniem TicketFactory.Create().
    /// </summary>
    public class TicketAvailabilityService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketAvailabilityService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository
                ?? throw new ArgumentNullException(nameof(ticketRepository));
        }

        /// <summary>
        /// Weryfikuje czy można sprzedać bilet na daną strefę w danym dniu.
        ///
        /// Sprawdza kolejno:
        ///   1. Czy klient nie jest zablokowany
        ///   2. Czy strefa jest otwarta (nie zamknięta, nie wyprzedana)
        ///   3. Czy liczba sprzedanych biletów nie przekroczyła pojemności
        ///   4. Czy klient nie kupił już biletu na tę strefę w tym dniu
        ///
        /// Rzuca DomainException (lub InvalidOperationException) gdy warunek nie jest spełniony.
        /// CommandHandler łapie wyjątek i zwraca błąd do UI.
        /// </summary>
        public void CheckAvailability(Zone zone, Customer customer, EventDay eventDay)
        {
            if (zone     == null) throw new ArgumentNullException(nameof(zone));
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (eventDay == null) throw new ArgumentNullException(nameof(eventDay));

            // 1. Klient aktywny?
            if (customer.Status == CustomerStatus.Banned)
                throw new InvalidOperationException(
                    $"Klient '{customer.FullName}' jest zablokowany i nie może kupować biletów.");

            // 2. Strefa otwarta?
            if (zone.Status == ZoneStatus.Closed)
                throw new InvalidOperationException(
                    $"Strefa '{zone.Name}' jest zamknięta.");

            if (zone.Status == ZoneStatus.SoldOut)
                throw new InvalidOperationException(
                    $"Strefa '{zone.Name}' jest wyprzedana na {eventDay}.");

            // 3. Czy jest jeszcze miejsce?
            int soldCount = _ticketRepository.CountActiveByZoneAndDay(zone.Id, eventDay);
            if (soldCount >= zone.Capacity)
                throw new InvalidOperationException(
                    $"Brak wolnych miejsc w strefie '{zone.Name}' na {eventDay}. " +
                    $"Sprzedano {soldCount}/{zone.Capacity} biletów.");

            // 4. Klient nie kupił już biletu na tę strefę tego dnia?
            var existingTickets = _ticketRepository.GetByZoneAndDay(zone.Id, eventDay);
            foreach (var t in existingTickets)
            {
                if (t.CustomerId == customer.Id && t.Status == TicketStatus.Active)
                    throw new InvalidOperationException(
                        $"Klient '{customer.FullName}' posiada już aktywny bilet " +
                        $"na strefę '{zone.Name}' w dniu {eventDay}.");
            }
        }
    }
}

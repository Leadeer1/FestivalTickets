using System;
using System.Collections.Generic;
using System.Linq;
using DDD.SharedKernel.DomainModelLayer;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Models
{
    public enum ZoneStatus
    {
        Open    = 0,
        SoldOut = 1,
        Closed  = 2
    }

    /// <summary>
    /// AGREGAT: Strefa/scena festiwalu.
    /// Odpowiednik Room z EscapeRoom.
    ///
    /// Tabela: Zones
    ///   Id                        (PK)
    ///   Name
    ///   Capacity
    ///   Status
    ///   BaseTicketPrice_Amount    (VO Money spłaszczone przez OwnsOne)
    ///   BaseTicketPrice_Currency  (VO Money spłaszczone przez OwnsOne)
    ///
    /// Tabela: Concerts – powiązana przez ZoneId (HasMany w ZoneConfiguration)
    ///
    /// Logika biznesowa:
    ///   AddConcert()    – dodaje koncert do strefy, waliduje stan
    ///   Close()         – zamknięcie strefy (np. zła pogoda)
    ///   Reopen()        – ponowne otwarcie
    ///   MarkAsSoldOut() – wywoływane gdy CountActiveByZoneAndDay >= Capacity
    /// </summary>
    public class Zone : Entity, IAggregateRoot
    {
        public string     Name            { get; protected set; }
        public int        Capacity        { get; protected set; }   // maks. biletów na jeden wieczór
        public ZoneStatus Status          { get; protected set; }
        public Money      BaseTicketPrice { get; protected set; }   // VO – cena przed rabatem

        // encje wewnątrz agregatu – analogia: Scores w Room
        private readonly List<Concert> _concerts = new List<Concert>();
        public IEnumerable<Concert> Concerts => _concerts.AsReadOnly();

        // wymagany przez EF Core
        protected Zone() { }

        public Zone(long id, string name, int capacity, Money baseTicketPrice)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nazwa strefy nie może być pusta.");
            if (capacity <= 0)
                throw new ArgumentException("Pojemność strefy musi być większa od zera.");
            if (baseTicketPrice == null || baseTicketPrice < Money.Zero)
                throw new ArgumentException("Cena bazowa biletu nie może być ujemna.");

            Name            = name.Trim();
            Capacity        = capacity;
            BaseTicketPrice = baseTicketPrice;
            Status          = ZoneStatus.Open;
        }

        /// <summary>
        /// Dodaje koncert do programu strefy.
        /// Odpowiednik UpdateScore() z Room.
        /// Waliduje: strefa musi być otwarta, artysta nie może już grać tego dnia w tej strefie.
        /// </summary>
        public void AddConcert(Concert concert)
        {
            if (concert == null)
                throw new ArgumentNullException(nameof(concert));
            if (Status == ZoneStatus.Closed)
                throw new InvalidOperationException(
                    $"Strefa '{Name}' jest zamknięta – nie można dodawać koncertów.");

            bool duplicateArtistOnSameDay = _concerts.Any(c =>
                c.ArtistName.Equals(concert.ArtistName, StringComparison.OrdinalIgnoreCase) &&
                c.EventDay   == concert.EventDay);

            if (duplicateArtistOnSameDay)
                throw new InvalidOperationException(
                    $"Artysta '{concert.ArtistName}' gra już w strefie '{Name}' w dniu {concert.EventDay}.");

            _concerts.Add(concert);
        }

        /// <summary>
        /// Zamknięcie strefy (np. zła pogoda, awaria techniczna).
        /// </summary>
        public void Close()
        {
            if (Status == ZoneStatus.Closed)
                throw new InvalidOperationException($"Strefa '{Name}' jest już zamknięta.");
            Status = ZoneStatus.Closed;
        }

        /// <summary>
        /// Ponowne otwarcie zamkniętej strefy.
        /// </summary>
        public void Reopen()
        {
            if (Status != ZoneStatus.Closed)
                throw new InvalidOperationException($"Strefa '{Name}' nie jest zamknięta.");
            Status = ZoneStatus.Open;
        }

        /// <summary>
        /// Oznaczenie strefy jako wyprzedanej.
        /// Wywoływane przez CommandHandler gdy liczba sprzedanych biletów >= Capacity.
        /// </summary>
        public void MarkAsSoldOut()
        {
            if (Status != ZoneStatus.Open)
                throw new InvalidOperationException(
                    $"Strefa '{Name}' nie jest otwarta – nie można oznaczyć jako wyprzedanej.");
            Status = ZoneStatus.SoldOut;
        }
    }
}

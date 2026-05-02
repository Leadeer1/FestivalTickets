using System;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Models
{
    /// <summary>
    /// ENCJA wewnątrz agregatu Zone.
    /// Odpowiednik Score (encji wewnątrz Room) z EscapeRoom – ale jako pełna encja z Id,
    /// bo koncert ma własną tożsamość: konkretny artysta, czas, scena.
    ///
    /// Tabela: Concerts
    ///   Id                   (PK)
    ///   ArtistName
    ///   Genre
    ///   StartTime
    ///   EventDay_Date        (VO EventDay spłaszczone przez OwnsOne)
    ///   EventDay_DayName     (VO EventDay spłaszczone przez OwnsOne)
    ///   ZoneId               (FK do Zones – relacja przez Id, nie przez referencję!)
    ///
    /// Ważne: Concert NIE jest agregatem. Dostęp tylko przez Zone.AddConcert().
    /// Nie ma własnego repozytorium – zarządzany przez ZoneRepository.
    /// </summary>
    public class Concert : Entity
    {
        public string   ArtistName { get; protected set; }
        public string   Genre      { get; protected set; }
        public DateTime StartTime  { get; protected set; }
        public EventDay EventDay   { get; protected set; }   // VO
        public long     ZoneId     { get; protected set; }   // FK – nie referencja do Zone!

        // wymagany przez EF Core
        protected Concert() { }

        public Concert(long id, string artistName, string genre,
                       DateTime startTime, EventDay eventDay, long zoneId)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(artistName))
                throw new ArgumentException("Nazwa artysty nie może być pusta.");
            if (startTime == default)
                throw new ArgumentException("Czas rozpoczęcia koncertu nie może być pusty.");

            ArtistName = artistName.Trim();
            Genre      = string.IsNullOrWhiteSpace(genre) ? "Inne" : genre.Trim();
            StartTime  = startTime;
            EventDay   = eventDay ?? throw new ArgumentNullException(nameof(eventDay));
            ZoneId     = zoneId;
        }

        public override string ToString()
            => $"{ArtistName} | {Genre} | {StartTime:HH:mm} | {EventDay}";
    }
}

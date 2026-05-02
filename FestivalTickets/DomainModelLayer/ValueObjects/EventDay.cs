using System;
using System.Collections.Generic;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects
{
    /// <summary>
    /// Value Object reprezentujący konkretny dzień festiwalu.
    /// Przykład: Date = 2025-05-14, DayName = "Środa Juwenaliowa"
    ///
    /// Mapowanie EF (OwnsOne w ZoneConfiguration i TicketConfiguration):
    ///   EventDay_Date    (DateTime)
    ///   EventDay_DayName (string)
    /// </summary>
    public class EventDay : ValueObject
    {
        public DateTime Date    { get; protected set; }
        public string   DayName { get; protected set; }

        // wymagany przez EF Core
        protected EventDay() { }

        public EventDay(DateTime date, string dayName)
        {
            if (string.IsNullOrWhiteSpace(dayName))
                throw new ArgumentException("Nazwa dnia festiwalu nie może być pusta.");

            Date    = date.Date; // tylko data, bez czasu
            DayName = dayName.Trim();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Date;
            yield return DayName.ToLowerInvariant();
        }

        public override string ToString() => $"{DayName} ({Date:dd.MM.yyyy})";
    }
}

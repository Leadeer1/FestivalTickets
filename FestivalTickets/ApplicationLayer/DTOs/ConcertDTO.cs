using System;

namespace DDD.FestivalTickets.Core.ApplicationLayer.DTOs
{
    public class ConcertDTO
    {
        public long Id { get; set; }

        public string ArtistName { get; set; }
        public string Genre { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EventDate { get; set; }
        public string EventDayName { get; set; }

        public long ZoneId { get; set; }
    }
}

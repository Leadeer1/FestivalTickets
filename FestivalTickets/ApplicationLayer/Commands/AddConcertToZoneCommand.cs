using System;

namespace DDD.FestivalTickets.Core.ApplicationLayer.Commands
{
    public class AddConcertToZoneCommand
    {
        public long ZoneId { get; set; }
        public long ConcertId { get; set; }
        public string ArtistName { get; set; }
        public string Genre { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EventDate { get; set; }
        public string EventDayName { get; set; }
    }
}

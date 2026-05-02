using System;

namespace DDD.FestivalTickets.Core.ApplicationLayer.Commands
{
    public class PurchaseTicketCommand
    {
        public long TicketId { get; set; }
        public long ZoneId { get; set; }
        public long CustomerId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventDayName { get; set; }
        
        
    }
}

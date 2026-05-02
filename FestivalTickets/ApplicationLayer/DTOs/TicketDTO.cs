using System;

namespace DDD.FestivalTickets.Core.ApplicationLayer.DTOs
{
    public class TicketDTO
    {
        public long Id { get; set; }
        public long ZoneId { get; set; }
        public long CustomerId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventDayName { get; set; }
        public decimal FinalPriceAmount { get; set; }
        public string FinalPriceCurrency { get; set; }
        public DateTime PurchasedAt { get; set; }
        public string Status { get; set; }
    }
}

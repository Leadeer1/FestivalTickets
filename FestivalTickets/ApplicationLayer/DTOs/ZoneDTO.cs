using System.Collections.Generic;

namespace DDD.FestivalTickets.Core.ApplicationLayer.DTOs
{
    public class ZoneDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public decimal BaseTicketPriceAmount { get; set; }
        public string BaseTicketPriceCurrency { get; set; }
        public IList<ConcertDTO> Concerts { get; set; }
    }
}

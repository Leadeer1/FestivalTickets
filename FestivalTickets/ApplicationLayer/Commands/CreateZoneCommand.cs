namespace DDD.FestivalTickets.Core.ApplicationLayer.Commands
{
    public class CreateZoneCommand
    {
        public long     ZoneId  { get; set; }
        public string   ZoneName  { get; set; }
        public int Capacity { get; set; }
        public decimal  BaseTicketPrice  { get; set; }
        public string TicketCurrency { get; set; }
    }
}

using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;

namespace DDD.FestivalTickets.Core.ApplicationLayer.Commands
{
    public class RegisterCustomerCommand
    {
        public long CustomerId { get; set; }
        public string         FirstName    { get; set; }
        public string         LastName     { get; set; }
        public string          Email        { get; set; }   // VO
        public CustomerTypeValue   CustomerTypeValue { get; set; }   // VO – decyduje o rabacie
    }
}

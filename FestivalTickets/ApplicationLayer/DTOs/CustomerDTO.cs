namespace DDD.FestivalTickets.Core.ApplicationLayer.DTOs
{
    public class CustomerDTO
    {
        public long Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CustomerType { get; set; }
        public string Status { get; set; }
    }
}

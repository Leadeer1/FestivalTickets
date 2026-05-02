using System.Linq;
using DDD.FestivalTickets.Core.ApplicationLayer.DTOs;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;

namespace DDD.FestivalTickets.Core.ApplicationLayer.Mappers
{
    public class Mapper
    {
        public ConcertDTO Map(Concert concert)
        {
            return new ConcertDTO
            {
                Id = concert.Id,
                ArtistName = concert.ArtistName,
                Genre = concert.Genre,
                StartTime = concert.StartTime,
                EventDate = concert.EventDay.Date,
                EventDayName = concert.EventDay.DayName,
                ZoneId = concert.ZoneId
            };
        }
        public CustomerDTO Map(Customer customer)
        {
            return new CustomerDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email.ToString(),
                CustomerType = customer.CustomerType.ToString(),
                Status = customer.Status.ToString()
            };
        }

        public TicketDTO Map(Ticket ticket)
        {
            return new TicketDTO
            {
                Id = ticket.Id,
                ZoneId = ticket.ZoneId,
                CustomerId = ticket.CustomerId,
                EventDate = ticket.EventDay.Date,
                EventDayName = ticket.EventDay.DayName,
                FinalPriceAmount = ticket.FinalPrice.Amount,
                FinalPriceCurrency = ticket.FinalPrice.Currency,
                PurchasedAt = ticket.PurchasedAt,
                Status = ticket.Status.ToString()
            };
        }

        public ZoneDTO Map(Zone zone)
        {
            return new ZoneDTO
            {
                Id = zone.Id,
                Name = zone.Name,
                Capacity = zone.Capacity,
                Status = zone.Status.ToString(),
                BaseTicketPriceAmount = zone.BaseTicketPrice.Amount,
                BaseTicketPriceCurrency = zone.BaseTicketPrice.Currency,
                Concerts = zone.Concerts.Select(Map).ToList()
            };
        }
    }
    
}

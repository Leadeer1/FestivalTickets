using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DDD.FestivalTickets.Core.InfrastructureLayer.EF
{
    internal class TicketRepository : ITicketRepository
    {
        private readonly FestivalDbContext _db;

        public TicketRepository(FestivalDbContext db)
        {
            _db = db;
        }

        public Ticket Get(long id)
            => _db.Tickets
                  .Include(t => t.Validations)
                  .FirstOrDefault(t => t.Id == id);

        public IList<Ticket> GetAll()
            => _db.Tickets.ToList();

        public IList<Ticket> Find(Expression<Func<Ticket, bool>> expression)
            => _db.Tickets.Where(expression).ToList();

        public void Insert(Ticket entity)
            => _db.Tickets.Add(entity);

        public void Delete(Ticket entity)
            => _db.Tickets.Remove(entity);

        public IList<Ticket> GetByCustomer(long customerId)
            => _db.Tickets
                  .Where(t => t.CustomerId == customerId && t.Status == TicketStatus.Active)
                  .ToList();

        public IList<Ticket> GetByZoneAndDay(long zoneId, EventDay eventDay)
            => _db.Tickets
                  .Where(t => t.ZoneId == zoneId &&
                              t.EventDay.Date == eventDay.Date)
                  .ToList();

        public int CountActiveByZoneAndDay(long zoneId, EventDay eventDay)
            => _db.Tickets
                  .Count(t => t.ZoneId == zoneId &&
                              t.EventDay.Date == eventDay.Date &&
                              t.Status == TicketStatus.Active);
    }
}

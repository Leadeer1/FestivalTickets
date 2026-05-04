using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DDD.FestivalTickets.Core.InfrastructureLayer.EF
{
    internal class ZoneRepository : IZoneRepository
    {
        private readonly FestivalDbContext _db;

        public ZoneRepository(FestivalDbContext db)
        {
            _db = db;
        }

        public Zone Get(long id)
            => _db.Zones.FirstOrDefault(z => z.Id == id);

        public IList<Zone> GetAll()
            => _db.Zones.ToList();

        public IList<Zone> Find(Expression<Func<Zone, bool>> expression)
            => _db.Zones.Where(expression).ToList();

        public void Insert(Zone entity)
            => _db.Zones.Add(entity);

        public void Delete(Zone entity)
            => _db.Zones.Remove(entity);

        public Zone GetWithConcerts(long zoneId)
            => _db.Zones
                  .Include(z => z.Concerts)
                  .FirstOrDefault(z => z.Id == zoneId);

        public IList<Zone> GetAllWithConcerts()
            => _db.Zones
                  .Include(z => z.Concerts)
                  .ToList();
    }
}

using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.SharedKernel.DomainModelLayer;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.InfrastructureLayer.EF
{
    public class FestivalUnitOfWork : IFestivalUnitOfWork
    {
        private readonly FestivalDbContext         _db;
        private readonly IDomainEventPublisher     _publisher;

        public IZoneRepository     Zones     { get; }
        public ITicketRepository   Tickets   { get; }
        public ICustomerRepository Customers { get; }

        public FestivalUnitOfWork(FestivalDbContext db, IDomainEventPublisher publisher)
        {
            _db        = db;
            _publisher = publisher;

            Zones     = new ZoneRepository(db);
            Tickets   = new TicketRepository(db);
            Customers = new CustomerRepository(db);
        }

        /// <summary>
        /// Zapisuje zmiany do bazy i publikuje wszystkie zdarzenia domenowe
        /// zebrane na agregatach (Entity.DomainEvents).
        /// </summary>
        public void Commit()
        {
            // 1. Zbierz zdarzenia domenowe ze wszystkich zmodyfikowanych encji
            var domainEvents = _db.ChangeTracker
                .Entries<DDD.SharedKernel.DomainModelLayer.Implementations.Entity>()
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            // 2. Wyczyść zdarzenia przed SaveChanges (żeby nie zduplikować przy retry)
            foreach (var entry in _db.ChangeTracker
                .Entries<DDD.SharedKernel.DomainModelLayer.Implementations.Entity>())
            {
                entry.Entity.RemoveAllDomainEvents();
            }

            // 3. Zapisz do bazy
            _db.SaveChanges();

            // 4. Opublikuj zdarzenia przez SimpleEventPublisher
            foreach (var domainEvent in domainEvents)
            {
                // Użyj refleksji żeby wywołać Publish<T> z właściwym typem T
                var publishMethod = _publisher.GetType()
                    .GetMethod(nameof(IDomainEventPublisher.Publish))!
                    .MakeGenericMethod(domainEvent.GetType());

                publishMethod.Invoke(_publisher, new object[] { domainEvent });
            }
        }

        public void RejectChanges()
        {
            foreach (var entry in _db.ChangeTracker.Entries())
            {
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}

using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.FestivalTickets.Core.InfrastructureLayer.EF.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace DDD.FestivalTickets.Core.InfrastructureLayer.EF
{
    public class FestivalDbContext : DbContext
    {
        public DbSet<Customer>          Customers          => Set<Customer>();
        public DbSet<Zone>              Zones              => Set<Zone>();
        public DbSet<Ticket>            Tickets            => Set<Ticket>();
        public DbSet<Concert>           Concerts           => Set<Concert>();
        public DbSet<TicketValidation>  TicketValidations  => Set<TicketValidation>();

        public FestivalDbContext(DbContextOptions<FestivalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ZoneConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new ConcertConfiguration());
            modelBuilder.ApplyConfiguration(new TicketValidationConfiguration());
        }
    }
}

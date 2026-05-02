using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.FestivalTickets.Core.InfrastructureLayer.EF.EntityConfiguration
{
    internal class ZoneConfiguration : IEntityTypeConfiguration<Zone>
    {
        public void Configure(EntityTypeBuilder<Zone> builder)
        {
            builder.ToTable("Zones");

            builder.HasKey(z => z.Id);

            builder.Property(z => z.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(z => z.Capacity)
                   .IsRequired();

            builder.Property(z => z.Status)
                   .IsRequired()
                   .HasConversion<int>();

            // VO Money (BaseTicketPrice) spłaszczone przez OwnsOne
            // → kolumny: BaseTicketPrice_Amount, BaseTicketPrice_Currency
            builder.OwnsOne(z => z.BaseTicketPrice, price =>
            {
                price.Property(p => p.Amount)
                     .HasColumnName("BaseTicketPrice_Amount")
                     .IsRequired()
                     .HasColumnType("decimal(18,2)");

                price.Property(p => p.Currency)
                     .HasColumnName("BaseTicketPrice_Currency")
                     .IsRequired()
                     .HasMaxLength(10);
            });

            // Relacja: Zone 1 → N Concerts (encja wewnątrz agregatu)
            builder.HasMany(z => z.Concerts)
                   .WithOne()
                   .HasForeignKey(c => c.ZoneId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

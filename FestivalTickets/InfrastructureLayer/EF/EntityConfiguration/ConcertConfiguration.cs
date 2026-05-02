using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.FestivalTickets.Core.InfrastructureLayer.EF.EntityConfiguration
{
    internal class ConcertConfiguration : IEntityTypeConfiguration<Concert>
    {
        public void Configure(EntityTypeBuilder<Concert> builder)
        {
            builder.ToTable("Concerts");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ArtistName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Genre)
                   .HasMaxLength(100);

            builder.Property(c => c.StartTime)
                   .IsRequired();

            builder.Property(c => c.ZoneId)
                   .IsRequired();

            // VO EventDay spłaszczone przez OwnsOne
            // → kolumny: EventDay_Date, EventDay_DayName
            builder.OwnsOne(c => c.EventDay, ed =>
            {
                ed.Property(e => e.Date)
                  .HasColumnName("EventDay_Date")
                  .IsRequired();

                ed.Property(e => e.DayName)
                  .HasColumnName("EventDay_DayName")
                  .IsRequired()
                  .HasMaxLength(100);
            });
        }
    }
}

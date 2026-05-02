using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.FestivalTickets.Core.InfrastructureLayer.EF.EntityConfiguration
{
    internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.ZoneId).IsRequired();
            builder.Property(t => t.CustomerId).IsRequired();

            builder.Property(t => t.PurchasedAt).IsRequired();

            builder.Property(t => t.Status)
                   .IsRequired()
                   .HasConversion<int>();

            // VO EventDay spłaszczone → EventDay_Date, EventDay_DayName
            builder.OwnsOne(t => t.EventDay, ed =>
            {
                ed.Property(e => e.Date)
                  .HasColumnName("EventDay_Date")
                  .IsRequired();

                ed.Property(e => e.DayName)
                  .HasColumnName("EventDay_DayName")
                  .IsRequired()
                  .HasMaxLength(100);
            });

            // VO Money (FinalPrice) spłaszczone → FinalPrice_Amount, FinalPrice_Currency
            builder.OwnsOne(t => t.FinalPrice, price =>
            {
                price.Property(p => p.Amount)
                     .HasColumnName("FinalPrice_Amount")
                     .IsRequired()
                     .HasColumnType("decimal(18,2)");

                price.Property(p => p.Currency)
                     .HasColumnName("FinalPrice_Currency")
                     .IsRequired()
                     .HasMaxLength(10);
            });

            // Relacja: Ticket 1 → N TicketValidations (encja wewnątrz agregatu)
            builder.HasMany(t => t.Validations)
                   .WithOne()
                   .HasForeignKey(v => v.TicketId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

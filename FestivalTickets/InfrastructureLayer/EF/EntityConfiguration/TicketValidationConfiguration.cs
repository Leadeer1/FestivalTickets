using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.FestivalTickets.Core.InfrastructureLayer.EF.EntityConfiguration
{
    internal class TicketValidationConfiguration : IEntityTypeConfiguration<TicketValidation>
    {
        public void Configure(EntityTypeBuilder<TicketValidation> builder)
        {
            builder.ToTable("TicketValidations");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.TicketId).IsRequired();

            builder.Property(v => v.GateName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(v => v.ScannedAt).IsRequired();
        }
    }
}

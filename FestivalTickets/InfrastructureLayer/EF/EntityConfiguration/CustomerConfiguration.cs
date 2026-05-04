using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.FestivalTickets.Core.InfrastructureLayer.EF.EntityConfiguration
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();

            builder.Property(c => c.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            // VO Email spłaszczone przez OwnsOne → kolumna Email_Value
            builder.OwnsOne(c => c.Email, email =>
            {
                email.Property(e => e.Value)
                     .HasColumnName("Email_Value")
                     .IsRequired()
                     .HasMaxLength(200);

                email.HasIndex(e => e.Value)
                     .IsUnique();
            });

            builder.OwnsOne(c => c.CustomerType, ct =>
            {
                ct.Property(x => x.Value)
                  .HasColumnName("CustomerType_Value")
                  .IsRequired()
                  .HasConversion<int>();
            });

            builder.Property(c => c.Status)
                   .IsRequired()
                   .HasConversion<int>();
        }
    }
}

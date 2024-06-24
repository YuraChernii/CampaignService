using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CampaignDatabase.EntityConfigurations
{
    internal class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Gender)
                .IsRequired();

            builder.Property(c => c.Age)
                .IsRequired();

            builder.Property(c => c.City)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Deposit)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(c => c.IsNewCustomer)
                .IsRequired();
        }
    }
}

using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CampaignDatabase.EntityConfigurations
{
    internal class CampaignTemplateEntityConfiguration : IEntityTypeConfiguration<CampaignTemplate>
    {
        public void Configure(EntityTypeBuilder<CampaignTemplate> builder)
        {
            builder.ToTable("CampaignTemplates");

            builder.HasKey(c => c.Id);

            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);

            builder.Property(e => e.Content).IsRequired().HasColumnType("nvarchar(max)");

            builder.HasMany(e => e.Campaigns)
                  .WithOne(c => c.Template)
                  .HasForeignKey(c => c.TemplateId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

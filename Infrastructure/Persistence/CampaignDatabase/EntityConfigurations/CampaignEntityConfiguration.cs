using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CampaignDatabase.EntityConfigurations
{
    internal class CampaignEntityConfiguration : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable("Campaigns");

            builder.HasKey(c => c.Id);

            builder.HasMany(e => e.ScheduledCampaigns)
                  .WithOne(c => c.Campaign)
                  .HasForeignKey(c => c.CampaignId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

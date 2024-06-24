using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CampaignDatabase.EntityConfigurations
{
    internal class ScheduledCampaignEntityConfiguration : IEntityTypeConfiguration<ScheduledCampaign>
    {
        public void Configure(EntityTypeBuilder<ScheduledCampaign> builder)
        {
            builder.ToTable("ScheduledCampaigns");

            builder.HasKey(c => c.Id);
        }
    }
}

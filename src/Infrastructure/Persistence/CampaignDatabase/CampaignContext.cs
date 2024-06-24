using Core.Entities;
using Infrastructure.Persistence.CampaignDatabase.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.CampaignDatabase
{
    public class CampaignContext : DbContext
    {
        public CampaignContext(DbContextOptions<CampaignContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<ScheduledCampaign> ScheduledCampaigns { get; set; }
        public DbSet<CampaignTemplate> CampaignTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CustomerEntityConfiguration());
            builder.ApplyConfiguration(new CampaignEntityConfiguration());
            builder.ApplyConfiguration(new ScheduledCampaignEntityConfiguration());
            builder.ApplyConfiguration(new CampaignTemplateEntityConfiguration());
        }
    }
}
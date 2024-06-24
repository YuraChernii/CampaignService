using Core.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.CampaignDatabase.Repositories
{
    public class CampaignRepository(CampaignContext context) : ICampaignRepository
    {
        public async Task<Guid> CreateCampaign(Campaign campaign)
        {
            context.Add(campaign);
            await context.SaveChangesAsync();

            return campaign.Id;
        }

        public async Task<IEnumerable<Campaign>> GetAllCampaigns() =>
            await context.Campaigns.ToListAsync();

        public async Task<Campaign?> GetCampaign(Guid id) =>
            await context.Campaigns.FindAsync(id);

        public async Task SaveChangesAsync() =>
            await context.SaveChangesAsync();
    }
}
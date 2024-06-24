using Core.Entities;
using Core.Repositories;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.CampaignDatabase.Repositories
{
    public class ScheduledCampaignRepository(CampaignContext context) : IScheduledCampaignRepository
    {
        public async Task<Guid> CreateScheduledCampaign(ScheduledCampaign scheduledCampaign)
        {
            context.Add(scheduledCampaign);
            await context.SaveChangesAsync();

            return scheduledCampaign.Id;
        }

        public async Task<IEnumerable<ScheduledCampaign>> GetWithHigherPriority(int priority, DateTime date) =>
            await context.ScheduledCampaigns
                .Include(x => x.Campaign)
                .Where(x => x.Campaign.Priority < priority && x.Campaign.SendTime.Date == date.Date)
                .ToListAsync();

        public async Task<ScheduledCampaign?> GetScheduledCampaign(Guid id, bool includeCampaignTemplate = false) =>
            await context.ScheduledCampaigns
                .Include(x => x.Campaign)
                .IncludeIf(includeCampaignTemplate, x => x.Campaign.Template)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task SaveChangesAsync() =>
            await context.SaveChangesAsync();
    }
}
using Core.Entities;

namespace Core.Repositories
{
    public interface IScheduledCampaignRepository
    {
        public Task<Guid> CreateScheduledCampaign(ScheduledCampaign campaign);
        public Task<IEnumerable<ScheduledCampaign>> GetWithHigherPriority(int priority, DateTime date);
        public Task<ScheduledCampaign?> GetScheduledCampaign(Guid id, bool includeCampaignTemplate = false);
        public Task SaveChangesAsync();
    }
}

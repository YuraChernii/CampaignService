using Core.Entities;

namespace Core.Repositories
{
    public interface ICampaignRepository
    {
        public Task<Guid> CreateCampaign(Campaign campaign);
        public Task<IEnumerable<Campaign>> GetAllCampaigns();
        public Task<Campaign?> GetCampaign(Guid id);
        public Task SaveChangesAsync();
    }
}

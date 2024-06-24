using Core.Entities;
using Core.Enums;

namespace Core.Repositories
{
    public interface ICustomerRepository
    {
        public Task<IEnumerable<Customer>> GetAllCustomers(CampaignCondition campaignCondition);
        public Task UpdateLastCampaignSentTime(int id, DateTime dateTime);
    }
}

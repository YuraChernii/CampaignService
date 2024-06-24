using Core.Entities;
using Core.Enums;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.CampaignDatabase.Repositories
{
    public class CustomerRepository(CampaignContext context) : ICustomerRepository
    {
        public async Task<IEnumerable<Customer>> GetAllCustomers(CampaignCondition campaignCondition) =>
            await context.Customers.Where(GetConditionExpression(campaignCondition)).ToListAsync();

        public async Task UpdateLastCampaignSentTime(int id, DateTime dateTime)=>
            await context.Customers
                .Where(h => h.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(h => h.LastCampaignSentTime, dateTime)
                );

        private Expression<Func<Customer, bool>> GetConditionExpression(CampaignCondition condition) =>
            condition switch
            {
                CampaignCondition.Male => c => c.Gender == Gender.Male,
                CampaignCondition.AgeAbove45 => c => c.Age > 45,
                CampaignCondition.CityNewYork => c => c.City == "New York",
                CampaignCondition.DepositAbove100 => c => c.Deposit > 100,
                CampaignCondition.IsNewCustomer => c => c.IsNewCustomer,
                _ => c => true,
            };
    }
}
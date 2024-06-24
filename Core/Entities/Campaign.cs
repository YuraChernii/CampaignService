using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities
{
    public class Campaign(Guid templateId, CampaignCondition condition, DateTime sendTime, int priority) : BaseEntity<Guid>
    {
        public Guid TemplateId { get; private set; } = templateId;
        public CampaignCondition Condition { get; private set; } = condition;
        public DateTime SendTime { get; private set; } = sendTime;
        public int Priority { get; private set; } = priority;
        public CampaignTemplate Template { get; private set; }
        public ICollection<ScheduledCampaign> ScheduledCampaigns { get; private set; }

        public bool DoesCustomerMatchCondition(Customer customer) =>
            Condition switch
            {
                CampaignCondition.Male => customer.Gender == Gender.Male,
                CampaignCondition.AgeAbove45 => customer.Age > 45,
                CampaignCondition.CityNewYork => customer.City == "New York",
                CampaignCondition.DepositAbove100 => customer.Deposit > 100,
                CampaignCondition.IsNewCustomer => customer.IsNewCustomer,
                _ => false,
            };
    }
}
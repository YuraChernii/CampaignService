using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities
{
    public class Campaign(Guid templateId, CampaignCondition condition, DateTime sendTime, int priority, Guid id = default, CampaignTemplate template = null, ICollection<ScheduledCampaign> scheduledCampaigns = null) 
        : BaseEntity<Guid>(id)
    {
        public Guid TemplateId { get; private set; } = templateId;
        public CampaignCondition Condition { get; private set; } = condition;
        public DateTime SendTime { get; private set; } = sendTime;
        public int Priority { get; private set; } = priority;
        public CampaignTemplate Template { get; private set; } = template;
        public ICollection<ScheduledCampaign> ScheduledCampaigns { get; private set; } = scheduledCampaigns;

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
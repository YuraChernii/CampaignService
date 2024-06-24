using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities
{
    public class Campaign : BaseEntity<Guid>
    {
        private Campaign() { }

        public Campaign(CampaignCondition condition, DateTime sendTime, int priority, Guid id = default, Guid templateId = default, CampaignTemplate template = null, ICollection<ScheduledCampaign> scheduledCampaigns = null)
        {
            Id = id;
            Condition = condition;
            SendTime = sendTime;
            Priority = priority;
            TemplateId = templateId;
            Template = template;
            ScheduledCampaigns = scheduledCampaigns;
        }

        public Guid TemplateId { get; private set; }
        public CampaignCondition Condition { get; private set; }
        public DateTime SendTime { get; private set; }
        public int Priority { get; private set; }
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
using Core.Entities.Base;

namespace Core.Entities
{
    public class ScheduledCampaign : BaseEntity<Guid>
    {
        private ScheduledCampaign() { }

        public ScheduledCampaign(Guid id = default, Guid campaignId = default, Campaign campaign = null)
        {
            Id = id;
            CampaignId = campaignId;
            Campaign = campaign;
        }

        public Guid CampaignId { get; private set; }
        public Campaign Campaign { get; private set; }
    }
}
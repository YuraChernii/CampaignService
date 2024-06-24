using Core.Entities.Base;

namespace Core.Entities
{
    public class ScheduledCampaign(Guid id = default, Guid campaignId = default, Campaign campaign = null) : BaseEntity<Guid>(id)
    {
        public Guid CampaignId { get; private set; } = campaignId;
        public Campaign Campaign { get; private set; } = campaign;
    }
}
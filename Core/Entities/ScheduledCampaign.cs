using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities
{
    public class ScheduledCampaign(Guid campaignId): BaseEntity<Guid>
    {
        public Guid CampaignId { get; private set; } = campaignId;
        public Campaign Campaign { get; private set; }
    }
}

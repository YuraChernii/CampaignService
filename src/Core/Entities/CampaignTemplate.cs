using Core.Entities.Base;

namespace Core.Entities
{
    public class CampaignTemplate(string name, string content, Guid id = default, ICollection<Campaign> campaigns = null) : BaseEntity<Guid>(id)
    {
        public string Name { get; private set; } = name;
        public string Content { get; private set; } = content;
        public ICollection<Campaign> Campaigns { get; private set; } = campaigns;
    }
}

using Core.Entities.Base;

namespace Core.Entities
{
    public class CampaignTemplate(string name, string content) : BaseEntity<Guid>
    {
        public string Name { get; private set; } = name;
        public string Content { get; private set; } = content;
        public ICollection<Campaign> Campaigns { get; private set; }
    }
}

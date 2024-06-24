using Core.Entities.Base;

namespace Core.Entities
{
    public class CampaignTemplate : BaseEntity<Guid>
    {
        private CampaignTemplate() { }

        public CampaignTemplate(string name, string content, Guid id = default, ICollection<Campaign> campaigns = null)
        {
            Id = id;
            Name = name;
            Content = content;
            Campaigns = campaigns;
        }

        public string Name { get; private set; }
        public string Content { get; private set; }
        public ICollection<Campaign> Campaigns { get; private set; }
    }
}

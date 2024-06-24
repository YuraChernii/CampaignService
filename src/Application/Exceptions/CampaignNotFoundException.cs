namespace Application.Exceptions
{
    public class CampaignNotFoundException : Exception
    {
        public CampaignNotFoundException(Guid campaignId)
            : base($"Campaign with ID '{campaignId}' was not found.")
        {
        }

        public CampaignNotFoundException(string message) : base(message)
        {
        }

        public CampaignNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

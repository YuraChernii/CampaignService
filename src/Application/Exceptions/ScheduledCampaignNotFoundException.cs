namespace Application.Exceptions
{
    public class ScheduledCampaignNotFoundException : Exception
    {
        public ScheduledCampaignNotFoundException(Guid scheduledCampaignId)
            : base($"Scheduled campaign with ID '{scheduledCampaignId}' was not found.")
        {
        }

        public ScheduledCampaignNotFoundException(string message) : base(message)
        {
        }

        public ScheduledCampaignNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

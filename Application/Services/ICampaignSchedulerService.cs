namespace Application.Services
{
    public interface ICampaignSchedulerService
    {
        Task ScheduleCampaignAsync(ScheduleCampaignParameters parameters);
    }

    public class ScheduleCampaignParameters
    {
        public Guid CampaignId { get; set; }
    }
}

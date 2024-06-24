using Core.Entities;

namespace Application.Services
{
    public interface ICampaignSenderService
    {
        Task SendCampaignToCustomerAsync(Campaign campaign, Customer customer, DateTime sendTime);
    }
}

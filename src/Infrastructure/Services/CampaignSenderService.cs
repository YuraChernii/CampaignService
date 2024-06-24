using Application.Services;
using Core.Entities;

namespace Infrastructure.Services
{
    internal class CampaignSenderService : ICampaignSenderService
    {
        private static readonly object fileLock = new();

        public async Task SendCampaignToCustomerAsync(Campaign campaign, Customer customer, DateTime sendTime)
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CampaignSends");
            Directory.CreateDirectory(directory);
            string fileName = Path.Combine(directory, $"sends_{sendTime:yyyyMMdd}.txt");

            lock (fileLock)
            {
                using StreamWriter writer = new(fileName, true);
                writer.WriteLine($"Send Campaign to Customer {customer.Id}:");
                writer.WriteLine($"Date: {sendTime}");
                writer.WriteLine($"Campaign Template: {campaign.Template}");
                writer.WriteLine($"Priority: {campaign.Priority}");
            }

            await Task.Delay(1800_000);
        }
    }
}

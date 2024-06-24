using Application.Exceptions;
using Application.Services;
using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Core.Repositories;
using Infrastructure.Services;
using Moq;
using Quartz;

namespace Campaigns
{
    public class CampaignSchedulerServiceTests
    {
        private readonly Mock<ICampaignRepository> _mockCampaignRepository = new();
        private readonly Mock<IScheduledCampaignRepository> _mockScheduledCampaignRepository = new();
        private readonly Mock<ISchedulerFactory> _mockSchedulerFactory = new();
        private readonly Mock<ITransactionService> _mockTransactionService = new();
        private readonly Mock<IScheduler> _mockScheduler = new();
        private readonly ICampaignSchedulerService _campaignSchedulerService;

        public CampaignSchedulerServiceTests()
        {
            _mockSchedulerFactory.Setup(f => f.GetScheduler(default)).ReturnsAsync(_mockScheduler.Object);

            _campaignSchedulerService = new CampaignSchedulerService(
                _mockCampaignRepository.Object,
                _mockScheduledCampaignRepository.Object,
                _mockSchedulerFactory.Object,
                _mockTransactionService.Object
            );
        }

        [Fact]
        public async Task ScheduleCampaignAsync_ValidCampaign_SchedulesJob()
        {
            Guid campaignId = Guid.NewGuid();
            Campaign campaign = new(CampaignCondition.AgeAbove45, DateTime.UtcNow.AddHours(1), 1);
            ScheduleCampaignParameters scheduleCampaignParameters = new() { CampaignId = campaignId };
            _mockCampaignRepository.Setup(r => r.GetCampaign(campaignId)).ReturnsAsync(campaign);
            _mockScheduledCampaignRepository.Setup(r => r.CreateScheduledCampaign(It.IsAny<ScheduledCampaign>()))
                                             .ReturnsAsync(Guid.NewGuid());
            _mockTransactionService.Setup(t => t.ExecuteAsync(It.IsAny<Func<Task>>())).Returns<Func<Task>>(func => func());

            await _campaignSchedulerService.ScheduleCampaignAsync(scheduleCampaignParameters);

            _mockScheduler.Verify(s => s.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ITrigger>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ScheduleCampaignAsync_InvalidCampaign_ThrowsCampaignNotFoundException()
        {
            Guid campaignId = Guid.NewGuid();
            ScheduleCampaignParameters scheduleCampaignParameters = new() { CampaignId = campaignId };
            _mockCampaignRepository.Setup(r => r.GetCampaign(campaignId)).ReturnsAsync((Campaign)null);

            await Assert.ThrowsAsync<CampaignNotFoundException>(() => _campaignSchedulerService.ScheduleCampaignAsync(scheduleCampaignParameters));
        }

        [Fact]
        public async Task ScheduleCampaignAsync_CampaignWithPastSendTime_ThrowsInvalidSendTimeException()
        {
            Guid campaignId = Guid.NewGuid();
            Campaign campaign = new(CampaignCondition.AgeAbove45, DateTime.UtcNow.AddHours(-1), 1);
            ScheduleCampaignParameters scheduleCampaignParameters = new() { CampaignId = campaignId };
            _mockCampaignRepository.Setup(r => r.GetCampaign(campaignId)).ReturnsAsync(campaign);
            _mockTransactionService.Setup(t => t.ExecuteAsync(It.IsAny<Func<Task>>())).Returns<Func<Task>>(func => func());

            await Assert.ThrowsAsync<InvalidSendTimeException>(() => _campaignSchedulerService.ScheduleCampaignAsync(scheduleCampaignParameters));
        }
    }
}
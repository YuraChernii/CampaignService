using Application.Commands.ScheduleCampaign;
using Application.Commands.ScheduleCampaigns;
using Application.Services;
using Moq;

namespace Campaigns
{

    public class ScheduleCampaignsCommandHandlerTests
    {
        private readonly Mock<ICampaignSchedulerService> _mockCampaignSchedulerService = new();
        private readonly ScheduleCampaignsCommandHandler _handler;

        public ScheduleCampaignsCommandHandlerTests()
        {
            _handler = new(_mockCampaignSchedulerService.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_SchedulesAllCampaigns()
        {
            List<Guid> campaignIds = [Guid.NewGuid(), Guid.NewGuid()];
            ScheduleCampaignsCommand command = new()
            {
                CampaignIds = campaignIds
            };
            _mockCampaignSchedulerService
                .Setup(s => s.ScheduleCampaignAsync(It.IsAny<ScheduleCampaignParameters>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            foreach (Guid campaignId in campaignIds)
            {
                _mockCampaignSchedulerService.Verify(s => s.ScheduleCampaignAsync(
                    It.Is<ScheduleCampaignParameters>(p => p.CampaignId == campaignId)), Times.Once);
            }
        }

        [Fact]
        public async Task Handle_NullCommand_ThrowsArgumentNullException() =>
            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, CancellationToken.None));


        [Fact]
        public async Task Handle_EmptyCampaignIds_DoesNotCallSchedulerService()
        {
            ScheduleCampaignsCommand command = new()
            {
                CampaignIds = []
            };

            await _handler.Handle(command, CancellationToken.None);

            _mockCampaignSchedulerService.Verify(s => s.ScheduleCampaignAsync(It.IsAny<ScheduleCampaignParameters>()), Times.Never);
        }

        [Fact]
        public async Task Handle_SchedulerServiceThrowsException_ThrowsException()
        {
            List<Guid> campaignIds = [Guid.NewGuid()];
            ScheduleCampaignsCommand command = new()
            {
                CampaignIds = campaignIds
            };
            _mockCampaignSchedulerService
                .Setup(s => s.ScheduleCampaignAsync(It.IsAny<ScheduleCampaignParameters>()))
                .ThrowsAsync(new Exception("Scheduler service failure"));

            Exception exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Scheduler service failure", exception.Message);
        }
    }
}

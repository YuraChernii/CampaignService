using Application.Commands.ScheduleCampaign;
using Application.Services;
using AutoMapper;
using Moq;

namespace Campaigns
{
    public class ScheduleCampaignCommandHandlerTests
    {
        private readonly Mock<ICampaignSchedulerService> _mockCampaignSchedulerService = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly ScheduleCampaignCommandHandler _handler;

        public ScheduleCampaignCommandHandlerTests()
        {
            _handler = new(_mockCampaignSchedulerService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_SchedulesCampaign()
        {
            Guid campaignId = Guid.NewGuid();
            ScheduleCampaignCommand command = new()
            {
                CampaignId = campaignId
            };
            ScheduleCampaignParameters scheduleCampaignParameters = new()
            {
                CampaignId = command.CampaignId
            };
            _mockMapper.Setup(m => m.Map<ScheduleCampaignParameters>(command)).Returns(scheduleCampaignParameters);
            _mockCampaignSchedulerService.Setup(s => s.ScheduleCampaignAsync(scheduleCampaignParameters)).Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            _mockMapper.Verify(m => m.Map<ScheduleCampaignParameters>(command), Times.Once);
            _mockCampaignSchedulerService.Verify(s => s.ScheduleCampaignAsync(scheduleCampaignParameters), Times.Once);
        }

        [Fact]
        public async Task Handle_NullCommand_ThrowsArgumentNullException() =>
            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, CancellationToken.None));

        [Fact]
        public async Task Handle_SchedulerServiceThrowsException_ThrowsException()
        {
            Guid campaignId = Guid.NewGuid();
            ScheduleCampaignCommand command = new()
            {
                CampaignId = campaignId
            };
            ScheduleCampaignParameters scheduleCampaignParameters = new()
            {
                CampaignId = command.CampaignId
            };
            _mockMapper.Setup(m => m.Map<ScheduleCampaignParameters>(command)).Returns(scheduleCampaignParameters);
            _mockCampaignSchedulerService.Setup(s => s.ScheduleCampaignAsync(scheduleCampaignParameters)).ThrowsAsync(new Exception("Scheduler service failure"));

            Exception exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Scheduler service failure", exception.Message);
        }
    }
}

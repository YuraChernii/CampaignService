using Application.Commands.CreateCampaign;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.Repositories;
using Moq;

namespace Campaigns
{

    public class CreateCampaignCommandHandlerTests
    {
        private readonly Mock<ICampaignRepository> _mockCampaignRepository = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly CreateCampaignCommandHandler _handler;

        public CreateCampaignCommandHandlerTests()
        {
            _handler = new(_mockCampaignRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsGuid()
        {
            DateTime now = DateTime.UtcNow;
            Guid campaignId = Guid.NewGuid();
            CreateCampaignCommand command = new()
            {
                Condition = CampaignCondition.AgeAbove45,
                Priority = 1,
                SendTime = DateTime.UtcNow,
                TemplateId = Guid.NewGuid()
            };
            Campaign campaign = new(CampaignCondition.AgeAbove45, now, 1, campaignId);
            _mockMapper.Setup(m => m.Map<Campaign>(command)).Returns(campaign);
            _mockCampaignRepository.Setup(r => r.CreateCampaign(campaign)).ReturnsAsync(campaign.Id);

            Guid result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(campaign.Id, result);
            _mockMapper.Verify(m => m.Map<Campaign>(command), Times.Once);
            _mockCampaignRepository.Verify(r => r.CreateCampaign(campaign), Times.Once);
        }

        [Fact]
        public async Task Handle_NullCommand_ThrowsArgumentNullException() =>
            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, CancellationToken.None));


        [Fact]
        public async Task Handle_CampaignRepositoryThrowsException_ThrowsException()
        {
            DateTime now = DateTime.UtcNow;
            CreateCampaignCommand command = new ()
            {
                Condition = CampaignCondition.AgeAbove45,
                Priority = 1,
                SendTime = now,
                TemplateId = Guid.NewGuid()
            };
            Campaign campaign = new(CampaignCondition.AgeAbove45, now, 1, Guid.NewGuid());
            _mockMapper.Setup(m => m.Map<Campaign>(command)).Returns(campaign);
            _mockCampaignRepository.Setup(r => r.CreateCampaign(campaign)).ThrowsAsync(new Exception("Repository failure"));

            Exception exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Repository failure", exception.Message);
        }
    }
}

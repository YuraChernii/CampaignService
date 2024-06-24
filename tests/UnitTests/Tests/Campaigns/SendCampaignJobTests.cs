using Application.Commands.CreateCampaign;
using Application.Commands.ScheduleCampaign;
using Application.Services;
using Core.Entities;
using Core.Enums;
using Core.Repositories;
using Infrastructure.Jobs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Quartz;

namespace Campaigns
{
    public class SendCampaignJobTests
    {
        private readonly Mock<IScheduledCampaignRepository> _mockScheduledCampaignRepository = new();
        private readonly Mock<ICustomerRepository> _mockCustomerRepository = new();
        private readonly Mock<IServiceScopeFactory> _mockServiceScopeFactory = new();
        private readonly Mock<IMediator> _mockMediator = new();
        private readonly Mock<IServiceScope> _mockServiceScope = new();
        private readonly Mock<ICampaignSenderService> _mockCampaignSenderService = new();
        private readonly SendCampaignJob _sendCampaignJob;

        public SendCampaignJobTests()
        {
            _mockServiceScopeFactory.Setup(f => f.CreateScope()).Returns(_mockServiceScope.Object);
            _mockServiceScope.Setup(s => s.ServiceProvider.GetService(typeof(ICustomerRepository)))
                             .Returns(_mockCustomerRepository.Object);

            _sendCampaignJob = new SendCampaignJob(
                _mockScheduledCampaignRepository.Object,
                _mockCustomerRepository.Object,
                _mockServiceScopeFactory.Object,
                _mockMediator.Object,
                _mockCampaignSenderService.Object
            );
        }

        [Fact]
        public async Task Execute_ValidScheduledCampaign_SendsCampaigns()
        {
            Guid scheduledCampaignId = Guid.NewGuid();
            Campaign campaign = new(CampaignCondition.AgeAbove45, DateTime.UtcNow, 1);
            ScheduledCampaign scheduledCampaign = new(campaign: campaign);
            List<Customer> customers = [new(Gender.Male, 46, "Lviv", 1200, true, 1, DateTime.UtcNow.AddDays(-1))];
            _mockScheduledCampaignRepository.Setup(r => r.GetScheduledCampaign(scheduledCampaignId, true))
                                            .ReturnsAsync(scheduledCampaign);
            _mockScheduledCampaignRepository.Setup(r => r.GetWithHigherPriority(campaign.Priority, campaign.SendTime))
                                            .ReturnsAsync(Enumerable.Empty<ScheduledCampaign>());
            _mockCustomerRepository.Setup(r => r.GetAllCustomers(CampaignCondition.AgeAbove45)).ReturnsAsync(customers);
            IJobExecutionContext jobExecutionContext = GetJobExecutionContext(scheduledCampaignId);

            await _sendCampaignJob.Execute(jobExecutionContext);

            _mockCustomerRepository.Verify(r => r.UpdateLastCampaignSentTime(customers[0].Id, campaign.SendTime), Times.Once);
        }

        [Fact]
        public async Task Execute_NullScheduledCampaign_ThrowsNotImplementedException()
        {
            Guid scheduledCampaignId = Guid.NewGuid();
            _mockScheduledCampaignRepository.Setup(r => r.GetScheduledCampaign(scheduledCampaignId, true))
                                            .ReturnsAsync((ScheduledCampaign)null);
            IJobExecutionContext jobExecutionContext = GetJobExecutionContext(scheduledCampaignId);

            await Assert.ThrowsAsync<NotImplementedException>(() => _sendCampaignJob.Execute(jobExecutionContext));
        }

        [Fact]
        public async Task Execute_CustomerWithHigherPriorityCampaign_SkipsCustomer()
        {
            Guid scheduledCampaignId = Guid.NewGuid();
            Campaign campaign = new(CampaignCondition.AgeAbove45, DateTime.UtcNow, 1);
            ScheduledCampaign scheduledCampaign = new(campaign: campaign);
            Campaign higherPriorityCampaign = new(CampaignCondition.AgeAbove45, DateTime.UtcNow, 2);
            ScheduledCampaign higherPriorityScheduledCampaign = new(campaign: higherPriorityCampaign);
            List<Customer> customers = [new(Gender.Male, 46, "Lviv", 1200, true, 1, DateTime.UtcNow.AddDays(-1))];
            _mockScheduledCampaignRepository.Setup(r => r.GetScheduledCampaign(scheduledCampaignId, true))
                                            .ReturnsAsync(scheduledCampaign);
            _mockScheduledCampaignRepository.Setup(r => r.GetWithHigherPriority(campaign.Priority, campaign.SendTime))
                                            .ReturnsAsync(new List<ScheduledCampaign> { higherPriorityScheduledCampaign });
            _mockCustomerRepository.Setup(r => r.GetAllCustomers(campaign.Condition)).ReturnsAsync(customers);
            IJobExecutionContext jobExecutionContext = GetJobExecutionContext(scheduledCampaignId);

            await _sendCampaignJob.Execute(jobExecutionContext);

            _mockCustomerRepository.Verify(r => r.UpdateLastCampaignSentTime(It.IsAny<int>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task Execute_CustomerAlreadyReceivedCampaign_SchedulesAgain()
        {
            Guid scheduledCampaignId = Guid.NewGuid();
            Campaign campaign = new(CampaignCondition.AgeAbove45, DateTime.UtcNow, 1);
            ScheduledCampaign scheduledCampaign = new(campaign: campaign);
            List<Customer> customers = [new(Gender.Male, 46, "Lviv", 1200, true, 1, DateTime.UtcNow)];
            _mockScheduledCampaignRepository.Setup(r => r.GetScheduledCampaign(scheduledCampaignId, true))
                                            .ReturnsAsync(scheduledCampaign);
            _mockScheduledCampaignRepository.Setup(r => r.GetWithHigherPriority(campaign.Priority, campaign.SendTime))
                                            .ReturnsAsync(Enumerable.Empty<ScheduledCampaign>());
            _mockCustomerRepository.Setup(r => r.GetAllCustomers(campaign.Condition)).ReturnsAsync(customers);
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateCampaignCommand>(), default)).ReturnsAsync(Guid.NewGuid());
            IJobExecutionContext jobExecutionContext = GetJobExecutionContext(scheduledCampaignId);

            await _sendCampaignJob.Execute(jobExecutionContext);

            _mockMediator.Verify(m => m.Send(It.IsAny<CreateCampaignCommand>(), default), Times.Once);
            _mockMediator.Verify(m => m.Send(It.IsAny<ScheduleCampaignCommand>(), default), Times.Once);
        }

        private IJobExecutionContext GetJobExecutionContext(Guid identity)
        {
            IJobDetail jobDetail = JobBuilder.Create<SendCampaignJob>().WithIdentity(identity.ToString()).Build();
            IJobExecutionContext jobExecutionContext = Mock.Of<IJobExecutionContext>(x => x.JobDetail == jobDetail);

            return jobExecutionContext;
        }
    }
}

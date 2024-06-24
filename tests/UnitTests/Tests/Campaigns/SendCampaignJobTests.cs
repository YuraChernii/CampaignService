using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Application.Commands.CreateCampaign;
using Application.Commands.ScheduleCampaign;
using Core.Entities;
using Core.Enums;
using Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Quartz;
using Xunit;

namespace Infrastructure.Jobs.Tests
{
    public class SendCampaignJobTests
    {
        private readonly Mock<IScheduledCampaignRepository> _mockScheduledCampaignRepository = new();
        private readonly Mock<ICustomerRepository> _mockCustomerRepository = new();
        private readonly Mock<IServiceScopeFactory> _mockServiceScopeFactory = new();
        private readonly Mock<IMediator> _mockMediator = new();
        private readonly Mock<IServiceScope> _mockServiceScope = new();
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
                _mockMediator.Object
            );
        }

        [Fact]
        public async Task Execute_ValidScheduledCampaign_SendsCampaigns()
        {
            // Arrange
            Guid scheduledCampaignId = Guid.NewGuid();
            Guid campaignId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Campaign campaign = new(templateId, CampaignCondition.AgeAbove45, DateTime.UtcNow, 1, campaignId);
            ScheduledCampaign scheduledCampaign = new(campaign.Id, scheduledCampaignId, campaign);
            List<Customer> customers = [new(Gender.Male, 46, "Lviv", 1200, true, 1, DateTime.UtcNow.AddDays(-1))];

            _mockScheduledCampaignRepository.Setup(r => r.GetScheduledCampaign(scheduledCampaignId, true))
                                            .ReturnsAsync(scheduledCampaign);
            _mockScheduledCampaignRepository.Setup(r => r.GetWithHigherPriority(campaign.Priority, campaign.SendTime))
                                            .ReturnsAsync(Enumerable.Empty<ScheduledCampaign>());
            _mockCustomerRepository.Setup(r => r.GetAllCustomers(CampaignCondition.AgeAbove45)).ReturnsAsync(customers);

            IJobDetail jobDetail = JobBuilder.Create<SendCampaignJob>().WithIdentity(scheduledCampaignId.ToString()).Build();
            IJobExecutionContext jobExecutionContext = Mock.Of<IJobExecutionContext>(x => x.JobDetail == jobDetail);

            // Act
            await _sendCampaignJob.Execute(jobExecutionContext);

            // Assert
            _mockCustomerRepository.Verify(r => r.UpdateLastCampaignSentTime(customers[0].Id, campaign.SendTime), Times.Once);
        }

        [Fact]
        public async Task Execute_NullScheduledCampaign_ThrowsNotImplementedException()
        {
            // Arrange
            var scheduledCampaignId = Guid.NewGuid();
            _mockScheduledCampaignRepository.Setup(r => r.GetScheduledCampaign(scheduledCampaignId, true))
                                            .ReturnsAsync((ScheduledCampaign)null);

            var jobDetail = JobBuilder.Create<SendCampaignJob>().WithIdentity(scheduledCampaignId.ToString()).Build();
            var jobExecutionContext = Mock.Of<IJobExecutionContext>(x => x.JobDetail == jobDetail);

            // Act & Assert
            await Assert.ThrowsAsync<NotImplementedException>(() => _sendCampaignJob.Execute(jobExecutionContext));
        }

        /*[Fact]
        public async Task Execute_CustomerWithHigherPriorityCampaign_SkipsCustomer()
        {
            // Arrange
            var scheduledCampaignId = Guid.NewGuid();
            var campaign = new Campaign { Condition = "Condition", Priority = 1, SendTime = DateTime.UtcNow, TemplateId = 1 };
            var scheduledCampaign = new ScheduledCampaign { Campaign = campaign };
            var higherPriorityCampaign = new Campaign { Condition = "Condition", Priority = 2 };
            var higherPriorityScheduledCampaign = new ScheduledCampaign { Campaign = higherPriorityCampaign };
            var customers = new List<Customer> { new Customer { Id = Guid.NewGuid(), LastCampaignSentTime = DateTime.UtcNow.AddDays(-1) } };

            _mockScheduledCampaignRepository.Setup(r => r.GetScheduledCampaign(scheduledCampaignId, true))
                                            .ReturnsAsync(scheduledCampaign);
            _mockScheduledCampaignRepository.Setup(r => r.GetWithHigherPriority(campaign.Priority, campaign.SendTime))
                                            .ReturnsAsync(new List<ScheduledCampaign> { higherPriorityScheduledCampaign });
            _mockCustomerRepository.Setup(r => r.GetAllCustomers(campaign.Condition)).ReturnsAsync(customers);

            var jobDetail = JobBuilder.Create<SendCampaignJob>().WithIdentity(scheduledCampaignId.ToString()).Build();
            var jobExecutionContext = Mock.Of<IJobExecutionContext>(x => x.JobDetail == jobDetail);

            // Act
            await _sendCampaignJob.Execute(jobExecutionContext);

            // Assert
            _mockCustomerRepository.Verify(r => r.UpdateLastCampaignSentTime(It.IsAny<Guid>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task Execute_CustomerAlreadyReceivedCampaign_SchedulesAgain()
        {
            // Arrange
            var scheduledCampaignId = Guid.NewGuid();
            var campaign = new Campaign { Condition = "Condition", Priority = 1, SendTime = DateTime.UtcNow, TemplateId = 1 };
            var scheduledCampaign = new ScheduledCampaign { Campaign = campaign };
            var customers = new List<Customer> { new Customer { Id = Guid.NewGuid(), LastCampaignSentTime = DateTime.UtcNow.Date } };

            _mockScheduledCampaignRepository.Setup(r => r.GetScheduledCampaign(scheduledCampaignId, true))
                                            .ReturnsAsync(scheduledCampaign);
            _mockScheduledCampaignRepository.Setup(r => r.GetWithHigherPriority(campaign.Priority, campaign.SendTime))
                                            .ReturnsAsync(Enumerable.Empty<ScheduledCampaign>());
            _mockCustomerRepository.Setup(r => r.GetAllCustomers(campaign.Condition)).ReturnsAsync(customers);
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateCampaignCommand>(), default))
            .ReturnsAsync(Guid.NewGuid());

            var jobDetail = JobBuilder.Create<SendCampaignJob>().WithIdentity(scheduledCampaignId.ToString()).Build();
            var jobExecutionContext = Mock.Of<IJobExecutionContext>(x => x.JobDetail == jobDetail);

            // Act
            await _sendCampaignJob.Execute(jobExecutionContext);

            // Assert
            _mockMediator.Verify(m => m.Send(It.IsAny<CreateCampaignCommand>(), default), Times.Once);
            _mockMediator.Verify(m => m.Send(It.IsAny<ScheduleCampaignCommand>(), default), Times.Once);
        }

        [Fact]
        public async Task SendCampaignToCustomerAsync_LocksAndWritesToFile()
        {
            // Arrange
            var campaign = new Campaign { Template = "Template", Priority = 1 };
            var customer = new Customer { Id = Guid.NewGuid() };
            var sendTime = DateTime.UtcNow;

            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CampaignSends");
            var fileName = Path.Combine(directory, $"sends_{sendTime:yyyyMMdd}.txt");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Act
            await _sendCampaignJob.SendCampaignToCustomerAsync(campaign, customer, sendTime);

            // Assert
            Assert.True(File.Exists(fileName));
            var fileContent = await File.ReadAllTextAsync(fileName);
            Assert.Contains($"Send Campaign to Customer{customer.Id}:", fileContent);
            Assert.Contains($"Date: {sendTime}", fileContent);
            Assert.Contains($"Campaign Template: {campaign.Template}", fileContent);
            Assert.Contains($"Priority: {campaign.Priority}", fileContent);
        }*/
    }
}

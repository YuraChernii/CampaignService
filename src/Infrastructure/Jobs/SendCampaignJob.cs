using Application.Commands.CreateCampaign;
using Application.Commands.ScheduleCampaign;
using Application.Exceptions;
using Application.Services;
using Core.Entities;
using Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure.Jobs
{
    public class SendCampaignJob(
        IScheduledCampaignRepository scheduledCampaignRepository,
        ICustomerRepository customerRepository,
        IServiceScopeFactory serviceScopeFactory,
        IMediator mediator,
        ICampaignSenderService campaignSenderService)
        : IJob
    {
        private static readonly object fileLock = new();

        public async Task Execute(IJobExecutionContext context)
        {
            Guid scheduledCampaignId = Guid.Parse(context.JobDetail.Key.Name);

            ScheduledCampaign scheduledCampaign = await scheduledCampaignRepository.GetScheduledCampaign(scheduledCampaignId, true) 
                ?? throw new ScheduledCampaignNotFoundException(scheduledCampaignId);
            DateTime sendTime = scheduledCampaign.Campaign.SendTime;
            IEnumerable<ScheduledCampaign> scheduledCampaignsWithHigherPriority = await scheduledCampaignRepository.GetWithHigherPriority(scheduledCampaign.Campaign.Priority, sendTime);

            IEnumerable<Customer> customers = await customerRepository.GetAllCustomers(scheduledCampaign.Campaign.Condition);

            List<Task> tasks = [];
            bool shouldScheduleAgain = false;
            foreach (Customer customer in customers)
            {
                if (customer.LastCampaignSentTime.Date == DateTime.UtcNow.Date
                    || scheduledCampaignsWithHigherPriority.Any(x => x.Campaign.DoesCustomerMatchCondition(customer)))
                {
                    shouldScheduleAgain = true;
                    continue;
                }

                tasks.Add(SendCampaignAsync(scheduledCampaign.Campaign, customer, sendTime));
            }

            await Task.WhenAll(tasks);

            if (shouldScheduleAgain)
            {
                Guid campaignId = await mediator.Send(new CreateCampaignCommand()
                {
                    Condition = scheduledCampaign.Campaign.Condition,
                    Priority = scheduledCampaign.Campaign.Priority,
                    SendTime = sendTime.AddDays(1),
                    TemplateId = scheduledCampaign.Campaign.TemplateId,
                });

                await mediator.Send(new ScheduleCampaignCommand()
                {
                    CampaignId = campaignId
                });
            }
        }

        private async Task SendCampaignAsync(Campaign campaign, Customer customer, DateTime sendTime)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            ICustomerRepository repository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

            await repository.UpdateLastCampaignSentTime(customer.Id, sendTime);
            await campaignSenderService.SendCampaignToCustomerAsync(campaign, customer, sendTime);
        }
    }
}

﻿using Application.Exceptions;
using Application.Services;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Infrastructure.Jobs;
using Quartz;

namespace Infrastructure.Services
{
    public class CampaignSchedulerService(
        ICampaignRepository campaignRepository,
        IScheduledCampaignRepository scheduledCampaignRepository,
        ISchedulerFactory schedulerFactory,
        ITransactionService transactionService) 
        : ICampaignSchedulerService
    {
        public async Task ScheduleCampaignAsync(ScheduleCampaignParameters parameters)
        {
            Campaign campaign = await campaignRepository.GetCampaign(parameters.CampaignId) ?? throw new CampaignNotFoundException(parameters.CampaignId);
            
            await transactionService.ExecuteAsync(async () =>
            {
                Guid scheduledCampaignId = await scheduledCampaignRepository.CreateScheduledCampaign(new(campaignId: campaign.Id));

                IScheduler scheduler = await schedulerFactory.GetScheduler();
                IJobDetail job = CreateJob(scheduledCampaignId.ToString());
                ITrigger trigger = CreateTrigger(campaign);
                await scheduler.ScheduleJob(job, trigger);
            });
        }

        private IJobDetail CreateJob(string scheduledCampaignId) =>
            JobBuilder.Create<SendCampaignJob>()
                .WithIdentity(scheduledCampaignId)
                .Build();

        private ITrigger CreateTrigger(Campaign campaign)
        {
            if (campaign.SendTime < DateTime.UtcNow)
            {
                throw new InvalidSendTimeException();
            }

            return TriggerBuilder.Create()
                .WithIdentity(Guid.NewGuid().ToString())
                .StartAt(DateTime.SpecifyKind(campaign.SendTime, DateTimeKind.Utc))
                .WithSimpleSchedule(x => x.WithMisfireHandlingInstructionFireNow())
                .Build();
        }
    }
}

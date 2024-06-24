using Application.Commands.ScheduleCampaigns;
using Application.Services;
using MediatR;

namespace Application.Commands.ScheduleCampaign
{
    internal class ScheduleCampaignsCommandHandler(
        ICampaignSchedulerService campaignSchedulerService) : 
        IRequestHandler<ScheduleCampaignsCommand>
    {
        public async Task Handle(ScheduleCampaignsCommand command, CancellationToken cancellationToken)
        {
            foreach (Guid campaignId in command.CampaignIds)
            {
                await campaignSchedulerService.ScheduleCampaignAsync(new ScheduleCampaignParameters() { CampaignId = campaignId });
            }
        }
    }
}
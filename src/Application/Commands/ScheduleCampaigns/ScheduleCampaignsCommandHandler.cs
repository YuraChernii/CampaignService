using Application.Commands.ScheduleCampaigns;
using Application.Services;
using MediatR;

namespace Application.Commands.ScheduleCampaign
{
    public class ScheduleCampaignsCommandHandler(
        ICampaignSchedulerService campaignSchedulerService) : 
        IRequestHandler<ScheduleCampaignsCommand>
    {
        public async Task Handle(ScheduleCampaignsCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            foreach (Guid campaignId in command.CampaignIds)
            {
                await campaignSchedulerService.ScheduleCampaignAsync(new ScheduleCampaignParameters() { CampaignId = campaignId });
            }
        }
    }
}
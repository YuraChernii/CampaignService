using Application.Services;
using AutoMapper;
using MediatR;

namespace Application.Commands.ScheduleCampaign
{
    public class ScheduleCampaignCommandHandler(ICampaignSchedulerService campaignSchedulerService, IMapper mapper) : IRequestHandler<ScheduleCampaignCommand>
    {
        public async Task Handle(ScheduleCampaignCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            await campaignSchedulerService.ScheduleCampaignAsync(mapper.Map<ScheduleCampaignParameters>(command));
        }
    }
}

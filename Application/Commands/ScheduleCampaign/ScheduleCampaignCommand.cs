using MediatR;

namespace Application.Commands.ScheduleCampaign
{
    public class ScheduleCampaignCommand: IRequest
    {
        public Guid CampaignId { get; set; }
    }
}

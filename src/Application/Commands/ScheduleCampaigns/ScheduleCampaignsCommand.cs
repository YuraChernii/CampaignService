using MediatR;

namespace Application.Commands.ScheduleCampaigns
{
    public class ScheduleCampaignsCommand: IRequest
    {
        public ICollection<Guid> CampaignIds { get; set; }
    }
}
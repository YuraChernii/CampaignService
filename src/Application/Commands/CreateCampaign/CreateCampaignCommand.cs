using Core.Enums;
using MediatR;

namespace Application.Commands.CreateCampaign
{
    public class CreateCampaignCommand: IRequest<Guid>
    {
        public Guid TemplateId { get; set; }
        public CampaignCondition Condition { get; set; }
        public DateTime SendTime { get; set; }
        public int Priority { get; set; }
    }
}

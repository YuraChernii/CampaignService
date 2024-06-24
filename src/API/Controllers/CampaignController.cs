using Application.Commands.ScheduleCampaign;
using Application.Commands.ScheduleCampaigns;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    public class CampaignController(IMediator mediator) : ControllerBase
    {
        [HttpPost("{campaignId}/schedule")]
        [AllowAnonymous]
        public async Task ScheduleCampaign([FromRoute] Guid campaignId) =>
            await mediator.Send(new ScheduleCampaignCommand() { CampaignId = campaignId });

        [HttpPost("schedule")]
        [AllowAnonymous]
        public async Task ScheduleCampaigns([FromBody] ScheduleCampaignsCommand command) =>
            await mediator.Send(command);
    }
}

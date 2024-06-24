using AutoMapper;
using Core.Entities;
using Core.Repositories;
using MediatR;

namespace Application.Commands.CreateCampaign
{
    public class CreateCampaignCommandHandler(
        ICampaignRepository campaignRepository, 
        IMapper mapper) 
        : IRequestHandler<CreateCampaignCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCampaignCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            return await campaignRepository.CreateCampaign(mapper.Map<Campaign>(command));
        }
    }
}
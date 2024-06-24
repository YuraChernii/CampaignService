using AutoMapper;
using Core.Entities;
using Core.Repositories;
using MediatR;

namespace Application.Commands.CreateCampaign
{
    internal class CreateCampaignCommandHandler(
        ICampaignRepository campaignRepository, 
        IMapper mapper) 
        : IRequestHandler<CreateCampaignCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCampaignCommand request, CancellationToken cancellationToken) =>
            await campaignRepository.CreateCampaign(mapper.Map<Campaign>(request));
    }
}

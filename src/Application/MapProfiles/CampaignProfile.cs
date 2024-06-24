using Application.Commands.CreateCampaign;
using Application.Commands.ScheduleCampaign;
using Application.Services;
using AutoMapper;
using Core.Entities;

namespace Application.MapProfiles
{
    internal class CampaignProfile : Profile
    {
        public CampaignProfile()
        {
            CreateMap<ScheduleCampaignCommand, ScheduleCampaignParameters>();
            CreateMap<CreateCampaignCommand, Campaign>();
        }
    }
}

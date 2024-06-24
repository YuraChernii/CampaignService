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
            AllowNullCollections = false;
            ShouldMapProperty = p => true;
            CreateMap<ScheduleCampaignCommand, ScheduleCampaignParameters>();
            CreateMap<CreateCampaignCommand, Campaign>();

            CreateMap<Campaign, Campaign>()
                .ConstructUsing((source, context) => new Campaign(source.Condition, source.SendTime, source.Priority, source.Id, source.TemplateId, source.Template, source.ScheduledCampaigns))
                .ForMember(source => source.Condition, opt => opt.Ignore())
                .ForMember(source => source.SendTime, opt => opt.Ignore())
                .ForMember(source => source.Priority, opt => opt.Ignore())
                .ForMember(source => source.Id, opt => opt.Ignore())
                .ForMember(source => source.TemplateId, opt => opt.Ignore())
                .ForMember(source => source.Template, opt => opt.Ignore())
                .ForMember(source => source.ScheduledCampaigns, opt => opt.Ignore());
        }
    }
}

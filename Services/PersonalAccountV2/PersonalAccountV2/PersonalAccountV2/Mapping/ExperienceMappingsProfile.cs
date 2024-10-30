using AutoMapper;
using BS.Contracts.Event;
using BS.Contracts.Experience;
using PersonalAccountV2.Models.Event;
using PersonalAccountV2.Models.Experience;

namespace PersonalAccountV2.Mapping
{

    public class ExperienceMappingsProfile : Profile
    {
        public ExperienceMappingsProfile()
        {
            CreateMap<ExperienceDto, ExperienceModel>();
        }
    }
}

using AutoMapper;
using BS.Contracts.Accomplishment;
using BS.Contracts.Employee;
using PersonalAccountV2.Models.Accomplishment;
using PersonalAccountV2.Models.Employee;

namespace PersonalAccountV2.Mapping
{

    public class AccomplishmentMappingsProfile : Profile
    {
        public AccomplishmentMappingsProfile()
        {
            CreateMap<AccomplishmentDto, AccomplishmentModel>();
        }
    }
}

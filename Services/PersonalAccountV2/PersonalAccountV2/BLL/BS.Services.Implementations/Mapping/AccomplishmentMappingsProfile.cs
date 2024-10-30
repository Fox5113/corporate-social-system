using AutoMapper;
using BS.Contracts.Accomplishment;
using BS.Contracts.Employee;
using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Implementations.Mapping
{
    public class AccomplishmentMappingsProfile : Profile
    {
        public AccomplishmentMappingsProfile()
        {
            CreateMap<Accomplishment, AccomplishmentDto>();

            CreateMap<AccomplishmentDto, Accomplishment>()
                .ForMember(d => d.CreatedAt, map => map.Ignore())
                .ForMember(d => d.UpdatedAt, map => map.Ignore());
        }
    }
}

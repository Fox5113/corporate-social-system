using AutoMapper;
using BS.Contracts.Communication;
using BS.Contracts.Employee;
using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Implementations.Mapping
{

    public class CommunicationMappingsProfile : Profile
    {
        public CommunicationMappingsProfile()
        {
            CreateMap<Communication, CommunicationDto>();

            CreateMap<CommunicationDto, Communication>()
                .ForMember(d => d.CreatedAt, map => map.Ignore())
                .ForMember(d => d.UpdatedAt, map => map.Ignore());
        }
    }
}

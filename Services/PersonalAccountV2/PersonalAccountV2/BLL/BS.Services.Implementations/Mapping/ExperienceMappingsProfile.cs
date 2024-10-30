using AutoMapper;
using BS.Contracts.Employee;
using BS.Contracts.Experience;
using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Implementations.Mapping
{
    public class ExperienceMappingsProfile : Profile
    {
        public ExperienceMappingsProfile()
        {
            CreateMap<Experience, ExperienceDto>();

            CreateMap<ExperienceDto, Experience>();
        }
    }
}

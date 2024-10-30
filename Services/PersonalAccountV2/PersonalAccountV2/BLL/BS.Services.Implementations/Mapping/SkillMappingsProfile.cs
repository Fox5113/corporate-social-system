using AutoMapper;
using BS.Contracts.Employee;
using BS.Contracts.Skill;
using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Implementations.Mapping
{
  public class SkillMappingsProfile : Profile
    {
        public SkillMappingsProfile()
        {
            CreateMap<Skill, SkillDto>();

            CreateMap<SkillDto, Skill>();
        }
    }
}

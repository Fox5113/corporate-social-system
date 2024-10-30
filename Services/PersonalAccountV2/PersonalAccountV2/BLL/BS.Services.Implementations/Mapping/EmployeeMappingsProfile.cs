using AutoMapper;
using BS.Contracts.Employee;
using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Implementations.Mapping
{
    public class EmployeeMappingsProfile : Profile
    {
        public EmployeeMappingsProfile()
        {
            CreateMap<Employee, EmployeeDto>();

            CreateMap<EmployeeDto, Employee>()
                .ForMember(d => d.SkillsList, map => map.Ignore())
                .ForMember(d => d.EventList, map => map.Ignore())
                .ForMember(d => d.AccomplishmentsList, map => map.Ignore())
                .ForMember(d => d.CommunicationsList, map => map.Ignore())
                .ForMember(d => d.ExperienceList, map => map.Ignore())
                .ForMember(d => d.CreatedAt, map => map.Ignore())
                .ForMember(d => d.UpdatedAt, map => map.Ignore());
        }

        /*
         *         public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        */
    }
}

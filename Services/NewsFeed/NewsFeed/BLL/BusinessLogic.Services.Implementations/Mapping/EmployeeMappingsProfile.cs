using AutoMapper;
using BusinessLogic.Contracts.Employee;
using DataAccess.Entities;

namespace BusinessLogic.Services.Mapping
{
    public class EmployeeMappingsProfile : Profile
    {
        public EmployeeMappingsProfile()
        {
            CreateMap<Employee, EmployeeDto>();
        }
    }
}

using AutoMapper;
using BS.Contracts.Employee;
using BS.Services.Abstractions;
using DA.Entities;
using DA.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        public async Task<Guid> CreateOrUpdate(EmployeeDto employees)
        {
            var employeeItem = _mapper.Map<EmployeeDto, Employee>(employees);
            var id = await _employeeRepository.CreateOrUpdate(employeeItem);
            await _employeeRepository.SaveChangesAsync();
            return id;
        }

        public async Task<ICollection<EmployeeDto>> GetAllEmployee()
        {
            var AllEmployee = await _employeeRepository.GetAllEmployee();
            return _mapper.Map<ICollection<Employee>, ICollection<EmployeeDto>>(AllEmployee);
        }

        public async Task<EmployeeDto> GetByIdAsync(Guid id)
        {
            var Employee = await _employeeRepository.GetAsync(id);
            return _mapper.Map<EmployeeDto>(Employee);
        }
    }
}

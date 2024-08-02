using System;
using System.Collections.Generic;
using DataAccess.Entities;
using BusinessLogic.Abstractions;
using AutoMapper;
using BusinessLogic.Contracts.Employee;
using DataAccess.Repositories.Abstractions;
using System.Threading.Tasks;

namespace BusinessLogic.Services
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

        public async Task<ICollection<EmployeeDto>> GetPagedAsync(int page, int pageSize)
        {
            ICollection<Employee> entities = await _employeeRepository.GetPagedAsync(page, pageSize);
            return _mapper.Map<ICollection<Employee>, ICollection<EmployeeDto>>(entities);
        }

        public async Task<EmployeeDto> GetByIdAsync(Guid id)
        {
            var Employee = await _employeeRepository.GetAsync(id);
            return _mapper.Map<EmployeeDto>(Employee);
        }
    }
}

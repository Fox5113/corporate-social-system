using BS.Contracts.Employee;
using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Repositories.Abstractions
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {

        Task<List<Employee>> GetAllEmployee();
        Task<Employee> GetByIdAsync(Guid id);
        Task<Guid> CreateOrUpdate(Employee employee);
    }
}

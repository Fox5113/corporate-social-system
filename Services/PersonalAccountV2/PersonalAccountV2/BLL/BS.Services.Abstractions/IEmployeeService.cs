using BS.Contracts.Employee;

namespace BS.Services.Abstractions
{

    public interface IEmployeeService
    {
        Task<ICollection<EmployeeDto>> GetAllEmployee();

        Task<EmployeeDto> GetByIdAsync(Guid id);

        public void CreateOrUpdate(EmployeeDto employees);
    }
}

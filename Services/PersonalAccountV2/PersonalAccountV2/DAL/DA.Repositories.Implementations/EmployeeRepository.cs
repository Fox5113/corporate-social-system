using DA.Context;
using DA.Entities;
using DA.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DA.Repositories.Implementations
{
    public class EmployeeRepository : Repository<Employee, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(DataContext context) : base(context)
        {
        }

        public async Task<List<Employee>> GetCollection(Employee filterEmployee)
        {
            if (string.IsNullOrEmpty(filterEmployee.Firstname) && string.IsNullOrEmpty(filterEmployee.Surname))
                return await GetAll().ToListAsync();

            var query = GetAll();
            var collection = await query
                .Where(x => (filterEmployee.Firstname == null || x.Firstname.Contains(filterEmployee.Firstname))
                            && (filterEmployee.Surname == null || x.Surname.Contains(filterEmployee.Surname)))
                    .OrderByDescending(x => x.Firstname)
                    .ThenByDescending(x => x.Surname)
                    .ToListAsync();

            return collection;
        }

        public void CreateOrUpdateRange(ICollection<Employee> employees)
        {
            foreach (var employee in employees)
            {
                Employee emp = null;
                if (employee.Id != Guid.Empty)
                    emp = Get(employee.Id);

                if (emp != null)
                {
                    emp.Surname = employee.Surname;
                    emp.Firstname = employee.Firstname;
                    emp.Position = employee.Position;
                    emp.IsDeleted = employee.IsDeleted;
                    emp.IsAdmin = employee.IsAdmin;
                    emp.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        public async Task<List<Employee>> GetAllEmployee()
        {
            var query = GetAll();
            return await query.ToListAsync<Employee>();
        }

        public async Task<Guid> UpdateEmployee(Employee employee)
        {
            Employee emp = null;
            if (employee.Id != Guid.Empty)
                emp = Get(employee.Id);

            if (emp != null)
            {
                emp.Surname = employee.Surname;
                emp.Firstname = employee.Firstname;
                emp.Position = employee.Position;
                emp.Birthdate = employee.Birthdate;
                emp.OfficeAddress = employee.OfficeAddress;
                emp.UpdatedAt = DateTime.UtcNow;
                emp.About = employee.About;
                emp.MainEmail = employee.MainEmail;
                emp.MainTelephoneNumber = employee.MainTelephoneNumber;
                emp.IsAdmin = employee.IsAdmin;
                emp.IsDeleted = employee.IsDeleted;

                return emp.Id;
            }
            else
            {
                return (await AddAsync(employee)).Id;
            }
        }
    }
}

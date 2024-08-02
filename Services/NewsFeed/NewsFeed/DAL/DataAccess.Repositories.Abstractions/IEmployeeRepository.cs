using DataAccess.Entities;
using Services.Repositories.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using BusinessLogic.Contracts.Employee;

namespace DataAccess.Repositories.Abstractions
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
        /// <summary>
        /// Получить постраничный список.
        /// </summary>
        /// <param name="page"> Номер страницы. </param>
        /// <param name="itemsPerPage"> Количество элементов на странице. </param>
        /// <returns> Список сотрудников. </returns>
        Task<List<Employee>> GetPagedAsync(int page, int itemsPerPage);
        Task<List<Employee>> GetCollection(EmployeeFilterDto employee);
    }
}

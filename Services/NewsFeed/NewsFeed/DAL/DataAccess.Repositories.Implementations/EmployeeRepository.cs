﻿using DataAccess.Entities;
using DataAccess.EntityFramework;
using DataAccess.Repositories.Abstractions;
using Infrastructure.Repositories.Implementations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Data.Entity;
using BusinessLogic.Contracts.Employee;

namespace DataAccess.Repositories
{
    public class EmployeeRepository : Repository<Employee, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// Получить постраничный список.
        /// </summary>
        /// <param name="page"> Номер страницы. </param>
        /// <param name="itemsPerPage"> Количество элементов на странице. </param>
        /// <returns> Список сотрудников. </returns>
        public async Task<List<Employee>> GetPagedAsync(int page, int itemsPerPage)
        {
            var query = GetAll();
            return await query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();
        }

        /// <summary>
        /// Получить список пользователей по фильтрам
        /// </summary>
        /// <param name="filterEmployee">Фильтры</param>
        /// <returns></returns>
        public async Task<List<Employee>> GetCollection(EmployeeFilterDto filterEmployee)
        {
            if (string.IsNullOrEmpty(filterEmployee.Firstname) && string.IsNullOrEmpty(filterEmployee.Surname))
                return await GetAll().ToListAsync();

            var query = GetAll();
            var collection = await query
                .Where(x => (filterEmployee.Firstname != null ? x.Firstname.Contains(filterEmployee.Firstname) : true) 
                            && (filterEmployee.Surname != null ? x.Surname.Contains(filterEmployee.Surname) : true))
                    .OrderByDescending(x => x.Firstname)
                    .ThenByDescending(x => x.Surname)
                    .ToListAsync();
            
            return collection;
        }
    }
}

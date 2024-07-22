using Microsoft.EntityFrameworkCore;
using NewsFeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using NewsFeed.Abstract;
using NewsFeed.Common;

namespace NewsFeed.Services
{
    public class EmployeeService : BaseService
    {

        public EmployeeService(DataContext context) : base(context)
        {
        }

        public override IQueryable<Employee> GetCollection(Mapping mapping)
        {
            if (mapping == null)
                return null;

            var script = SqlScriptPreparerService.GetSelectQuery(mapping);
            var collection = _dbContext.Employee.FromSqlRaw(script);
            return collection;
        }

        public override List<Employee> GetCollection<Employee>(int skip, int count)
        {
            var collection = _dbContext.Employee.Skip(skip).Take(count).ToList();
            return collection as List<Employee>;
        }

        public override List<Employee> GetCollection<Employee>(List<Guid> ids)
        {
            return _dbContext.Employee.Where(x => ids.Contains(x.Id)).ToList() as List<Employee>;
        }

        /// <summary>
        /// Получить пользователя по Id
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        public override Employee GetEntity(Guid id)
        {
            return _dbContext.Employee.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Получить список пользователей по имени и фамилии
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="lastname">Фамилия</param>
        /// <returns></returns>
        public ICollection<Employee> GetEmployees(string name, string lastname)
        {
            if (String.IsNullOrEmpty(name) && String.IsNullOrEmpty(lastname))
                return GetEmployees();

            return _dbContext.Employee.Where(x => (name != null ? x.Firstname.Contains(name) : true) && (lastname != null ? x.Surname.Contains(lastname) : true)).ToList();
        }

        /// <summary>
        /// Получить полный список пользователей
        /// </summary>
        /// <returns></returns>
        public ICollection<Employee> GetEmployees()
        {
            return _dbContext.Employee.ToList();
        }

        public override Employee CreateEntity<Employee>(Employee newObject)
        {
            _dbContext.Employee.Add(newObject as NewsFeed.Models.Employee);
            _dbContext.SaveChanges();

            return newObject;
        }
    }
}

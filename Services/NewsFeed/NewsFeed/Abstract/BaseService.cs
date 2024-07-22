using Microsoft.EntityFrameworkCore;
using NewsFeed.Common;
using NewsFeed.Models;
using NewsFeed.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewsFeed.Abstract
{
    public abstract class BaseService
    {
        protected readonly DataContext _dbContext;

        public BaseService(DataContext context)
        {
            _dbContext = context;
        }

        #region Public abstract

        /// <summary>
        /// Получение коллекции по объекту Mapping
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <returns></returns>
        public abstract IQueryable<object> GetCollection(Mapping mapping);

        public abstract List<T> GetCollection<T>(int skip, int count);

        public abstract List<T> GetCollection<T>(List<Guid> ids);

        public abstract object GetEntity(Guid id);

        public abstract T CreateEntity<T>(T newObject);

        #endregion

        #region Public methods

        /// <summary>
        /// Получение коллекции по json запросу
        /// </summary>
        /// <param name="jsonData">Запрос</param>
        /// <returns></returns>
        public object GetCollection(string jsonData, string objectName)
        {
            var type = Type.GetType(objectName);
            var obj = JsonSerializer.Deserialize(jsonData, type);
            if (obj != null)
            {
                var invoker = new Invoker(this.GetType().FullName.ToString(), "GetCollection", false,
                    new[] { new Tuple<Type, object>(_dbContext.GetType(), _dbContext) },
                    new[] { new Tuple<Type, object>(type, obj) });
                return invoker.InvokeMethod();
            }

            return null;
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filters"></param>
        public int Delete(string tableName, List<FieldFilter> filters)
        {
            if (filters == null || filters.Count == 0 || String.IsNullOrEmpty(tableName))
                return 0;
            
            var mapping = new Mapping()
            {
                MainTableName = tableName,
                TableFilter = new TableFilter()
                {
                    FieldsFilter = filters
                }
            };
            
            var deleteQuery = SqlScriptPreparerService.GetDeleteQuery(mapping);
            return _dbContext.Database.ExecuteSqlRaw(deleteQuery);
        }

        #endregion
    }
}

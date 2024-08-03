using DataAccess.Common;
using DataAccess.Common.SqlQuery;
using DataAccess.EntityFramework;
using DataAccess.Repositories.Abstractions;
using Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly DataContext _dataContext;
        public BaseRepository(DataContext context)
        {
            _dataContext = context;
        }

        public object GetSomeCollectionFromMapping(MappingQuery mapping)
        {
            if (mapping == null || string.IsNullOrEmpty(mapping.MainTableName))
                return null;

            var repositoryPath = Constants.TableAndRepositoryPath[mapping.MainTableName];

            if (repositoryPath == null)
                return null;

            var invoker = new Invoker(repositoryPath, "GetCollection", false,
                new[] { new Tuple<Type, object>(_dataContext.GetType(), _dataContext) },
                new[] { new Tuple<Type, object>(mapping.GetType(), mapping) });
            
            return invoker.InvokeMethod();
        }

        public object GetSomeCollectionByIds(List<Guid> ids, string tableName)
        {
            if (ids == null || ids.Count == 0 || string.IsNullOrEmpty(tableName))
                return null;

            var repositoryPath = Constants.TableAndRepositoryPath[tableName];
            if (string.IsNullOrEmpty(repositoryPath))
                return null;

            var invoker = new Invoker(repositoryPath, "GetCollection", false,
                new[] { new Tuple<Type, object>(_dataContext.GetType(), _dataContext) },
                new[] { new Tuple<Type, object>(ids.GetType(), ids) });

            return invoker.InvokeMethod();
        }

        public object GetEntity(Guid id, string tableName)
        {
            if (id == Guid.Empty || string.IsNullOrEmpty(tableName))
                return null;

            var servicePath = Constants.TableAndRepositoryPath[tableName];
            if (string.IsNullOrEmpty(servicePath))
                return null;

            var invoker = new Invoker(servicePath, "Get", false,
                new[] { new Tuple<Type, object>(_dataContext.GetType(), _dataContext) },
                new[] { new Tuple<Type, object>(id.GetType(), id) });

            return invoker.InvokeMethod();
        }

        public object GetCollectionByJsonString(string jsonData, string jsonObjectName, string tableName)
        {
            if (string.IsNullOrEmpty(jsonData) || string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(jsonObjectName))
                return null;

            var servicePath = Constants.TableAndRepositoryPath[tableName];
            if (string.IsNullOrEmpty(servicePath))
                return null;

            var classPath = Constants.ClassPathByName[jsonObjectName];
            if (string.IsNullOrEmpty(classPath))
                return null;

            var invoker = new Invoker(servicePath, "GetCollection", false,
                new[] { new Tuple<Type, object>(_dataContext.GetType(), _dataContext) },
                new[] { new Tuple<Type, object>(jsonData.GetType(), jsonData), new Tuple<Type, object>(classPath.GetType(), classPath) });

            return invoker.InvokeMethod();
        }

        public object Delete(List<FieldFilter> filters, string tableName)
        {
            if (filters == null || filters.Count == 0 || string.IsNullOrEmpty(tableName))
                return null;

            var servicePath = Constants.TableAndRepositoryPath[tableName];
            if (string.IsNullOrEmpty(servicePath))
                return null;

            var invoker = new Invoker(servicePath, "Delete", false,
                new[] { new Tuple<Type, object>(_dataContext.GetType(), _dataContext) },
                new[] { new Tuple<Type, object>(tableName.GetType(), tableName), new Tuple<Type, object>(filters.GetType(), filters) });

            return invoker.InvokeMethod();
        }
    }
}

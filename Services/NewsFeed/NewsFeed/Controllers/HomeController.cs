using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewsFeed.Common;
using NewsFeed.Models;
using NewsFeed.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewsFeed.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly DataContext _dbContext;
        IWebHostEnvironment _appEnvironment;

        public HomeController(DataContext context, IWebHostEnvironment appEnvironment)
        {
            _dbContext = context;
            _appEnvironment = appEnvironment;
        }

        [Route("GetSomeCollectionFromMapping")]
        [HttpGet]
        public IActionResult GetSomeCollectionFromMapping([FromBody] Mapping mapping)
        {
            if(mapping == null || String.IsNullOrEmpty(mapping.MainTableName))
                return NotFound();

            var servicePath = Constants.TableAndServicePath[mapping.MainTableName];

            if(servicePath == null)
                return NotFound();

            var invoker = new Invoker(servicePath, "GetCollection", false, 
                new[] { new Tuple<Type, object>(_dbContext.GetType(), _dbContext) }, 
                new[] { new Tuple<Type, object>(mapping.GetType(), mapping) });
            var result = invoker.InvokeMethod();
            return new ObjectResult(result);
        }

        [Route("GetSomeCollectionFromGenericMethod")]
        [HttpGet]
        public IActionResult GetSomeCollectionFromGenericMethod(int skip, int count, string tableName)
        {
            if (String.IsNullOrEmpty(tableName) || count <= 0 || skip < 0)
                return NotFound();

            var servicePath = Constants.TableAndServicePath[tableName];
            if (String.IsNullOrEmpty(servicePath))
                return NotFound();

            var classPath = Constants.ClassPathByName[tableName];
            if (String.IsNullOrEmpty(classPath))
                return NotFound();

            var invoker = new Invoker(servicePath, "GetCollection", true,
                new[] { new Tuple<Type, object>(_dbContext.GetType(), _dbContext) },
                new[] { new Tuple<Type, object>(skip.GetType(), skip), new Tuple<Type, object>(count.GetType(), count) },
                new[] { Type.GetType(classPath) });
            
            var result = invoker.InvokeMethod();
            return new ObjectResult(result);
        }

        [Route("GetSomeCollectionFromGenericMethod2")]
        [HttpGet]
        public IActionResult GetSomeCollectionFromGenericMethod2([FromBody]List<Guid> ids, string tableName)
        {
            if (ids == null || ids.Count == 0 || String.IsNullOrEmpty(tableName))
                return NotFound();

            var servicePath = Constants.TableAndServicePath[tableName];
            if (String.IsNullOrEmpty(servicePath))
                return NotFound();

            var classPath = Constants.ClassPathByName[tableName];
            if (String.IsNullOrEmpty(classPath))
                return NotFound();

            var invoker = new Invoker(servicePath, "GetCollection", true,
                new[] { new Tuple<Type, object>(_dbContext.GetType(), _dbContext) },
                new[] { new Tuple<Type, object>(ids.GetType(), ids) },
                new[] { Type.GetType(classPath) });

            var result = invoker.InvokeMethod();
            return new ObjectResult(result);
        }

        [Route("GetEntity")]
        [HttpGet]
        public IActionResult GetEntity(Guid id, string tableName)
        {
            if (id == Guid.Empty || String.IsNullOrEmpty(tableName))
                return NotFound();

            var servicePath = Constants.TableAndServicePath[tableName];
            if (String.IsNullOrEmpty(servicePath))
                return NotFound();

            var invoker = new Invoker(servicePath, "GetEntity", false,
                new[] { new Tuple<Type, object>(_dbContext.GetType(), _dbContext) },
                new[] { new Tuple<Type, object>(id.GetType(), id) });
            
            var result = invoker.InvokeMethod();
            return new ObjectResult(result);
        }

        [Route("GetCollectionByJsonString")]
        [HttpGet]
        public IActionResult GetCollectionByJsonString(string jsonData, string jsonObjectName, string tableName)
        {
            if (String.IsNullOrEmpty(jsonData) || String.IsNullOrEmpty(tableName) || String.IsNullOrEmpty(jsonObjectName))
                return NotFound();

            var servicePath = Constants.TableAndServicePath[tableName];
            if (String.IsNullOrEmpty(servicePath))
                return NotFound();
            
            var classPath = Constants.ClassPathByName[jsonObjectName];
            if (String.IsNullOrEmpty(classPath))
                return NotFound();

            var invoker = new Invoker(servicePath, "GetCollection", false,
                new[] { new Tuple<Type, object>(_dbContext.GetType(), _dbContext) },
                new[] { new Tuple<Type, object>(jsonData.GetType(), jsonData), new Tuple<Type, object>(classPath.GetType(), classPath) });

            var result = invoker.InvokeMethod();
            return new ObjectResult(result);
        }

        //Не протестировано
        [Route("CreateNews")]
        [HttpPost]
        public IActionResult CreateNews([FromBody]News newNews)
        {
            var service = new NewsService(_dbContext);
            var result = service.CreateEntity<News>(newNews);
            return new ObjectResult(result);
        }

        //Не протестировано
        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete([FromBody] List<FieldFilter> filters, string tableName)
        {
            if (filters == null || filters.Count == 0 || String.IsNullOrEmpty(tableName))
                return NotFound();

            var servicePath = Constants.TableAndServicePath[tableName];
            if (String.IsNullOrEmpty(servicePath))
                return NotFound();

            var invoker = new Invoker(servicePath, "Delete", false,
                new[] { new Tuple<Type, object>(_dbContext.GetType(), _dbContext) },
                new[] { new Tuple<Type, object>(tableName.GetType(), tableName), new Tuple<Type, object>(filters.GetType(), filters) } );

            var result = invoker.InvokeMethod();
            return new ObjectResult(result);
        }
    }
}

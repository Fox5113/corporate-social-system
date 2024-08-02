using AutoMapper;
using BusinessLogic.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsFeed.WebApi.Common;
using NewsFeed.WebApi.Common.SqlQuery;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IBaseService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IBaseService service,
            ILogger<HomeController> logger,
            IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [Route("GetSomeCollectionFromMapping")]
        [HttpGet]
        public IActionResult GetSomeCollectionFromMapping([FromBody] MappingQuery mapping)
        {
            if (mapping == null || string.IsNullOrEmpty(mapping.MainTableName))
                return NotFound();

            if(!Constants.TableAndRepositoryPath.ContainsKey(mapping.MainTableName))
                return NotFound();

            var servicePath = Constants.TableAndRepositoryPath[mapping.MainTableName];

            if (servicePath == null)
                return NotFound();

            return new ObjectResult(Ok(_service.GetSomeCollectionFromMapping(mapping)));
        }

        [Route("GetSomeCollectionByIds")]
        [HttpGet]
        public IActionResult GetSomeCollectionByIds([FromBody] List<Guid> ids, string tableName)
        {
            if (ids == null || ids.Count == 0 || string.IsNullOrEmpty(tableName))
                return NotFound();

            if (!Constants.TableAndRepositoryPath.ContainsKey(tableName))
                return NotFound();

            var servicePath = Constants.TableAndRepositoryPath[tableName];
            if (string.IsNullOrEmpty(servicePath))
                return NotFound();

            return new ObjectResult(Ok(_service.GetSomeCollectionByIds(ids, tableName)));
        }

        [Route("GetEntity")]
        [HttpGet]
        public IActionResult GetEntity(Guid id, string tableName)
        {
            if (id == Guid.Empty || string.IsNullOrEmpty(tableName))
                return NotFound();

            if (!Constants.TableAndRepositoryPath.ContainsKey(tableName))
                return NotFound();

            var servicePath = Constants.TableAndRepositoryPath[tableName];
            if (string.IsNullOrEmpty(servicePath))
                return NotFound();

            return new ObjectResult(Ok(_service.GetEntity(id, tableName)));
        }

        [Route("GetCollectionByJsonString")]
        [HttpGet]
        public IActionResult GetCollectionByJsonString(string jsonData, string jsonObjectName, string tableName)
        {
            if (string.IsNullOrEmpty(jsonData) || string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(jsonObjectName))
                return NotFound();

            if (!Constants.TableAndRepositoryPath.ContainsKey(tableName))
                return NotFound();

            var servicePath = Constants.TableAndRepositoryPath[tableName];
            if (string.IsNullOrEmpty(servicePath))
                return NotFound();

            if (!Constants.ClassPathByName.ContainsKey(jsonObjectName))
                return NotFound();

            var classPath = Constants.ClassPathByName[jsonObjectName];
            if (string.IsNullOrEmpty(classPath))
                return NotFound();

            return new ObjectResult(Ok(_service.GetCollectionByJsonString(jsonData, jsonObjectName, tableName)));
        }

        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete([FromBody] List<FieldFilter> filters, string tableName)
        {
            if (filters == null || filters.Count == 0 || string.IsNullOrEmpty(tableName))
                return NotFound();

            if (!Constants.TableAndRepositoryPath.ContainsKey(tableName))
                return NotFound();

            var servicePath = Constants.TableAndRepositoryPath[tableName];
            if (string.IsNullOrEmpty(servicePath))
                return NotFound();

            return new ObjectResult(Ok(_service.Delete(filters, tableName)));
        }
    }
}

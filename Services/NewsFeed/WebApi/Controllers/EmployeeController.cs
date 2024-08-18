using AutoMapper;
using BusinessLogic.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using WebApi.Models.Employee;
using System.Linq;
using BusinessLogic.Contracts.Employee;
using System.Text.Json;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(
            IEmployeeService service,
            ILogger<EmployeeController> logger,
            IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [Route("GetAsync")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(_mapper.Map<EmployeeModel>(await _service.GetByIdAsync(id)));
        }

        [Route("GetListAsync")]
        [HttpGet("list/{page}/{itemsPerPage}")]
        public async Task<IActionResult> GetListAsync(EmployeeFilterModel filterModel)
        {
            return Ok(_mapper.Map<List<EmployeeModel>>(await _service.GetPagedAsync(filterModel.Page, filterModel.ItemsPerPage)));
        }

        [Route("CreateOrUpdateEmployeeRange")]
        [HttpPost]
        public IActionResult CreateOrUpdateEmployeeRange(string jsonData)
        {
            if (jsonData != null)
            {
                var employees = JsonSerializer.Deserialize<List<ShortEmployeeModel>>(jsonData);

                if (employees != null && employees.Count > 0)
                    _service.CreateOrUpdateRange(employees.Select(x => _mapper.Map<ShortEmployeeModel, ShortEmployeeDto>(x)).ToList());
            }
            
            return Ok();
        }
    }
}

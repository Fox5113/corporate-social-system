﻿using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Contracts.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Employee;
using WebApi.RabbitMq;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IRabbitMqService _rabbitMqService;

        public EmployeeController(
            IEmployeeService service,
            ILogger<EmployeeController> logger,
            IMapper mapper,
            IRabbitMqService rabbitMqService)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
            _rabbitMqService = rabbitMqService;
        }

        [Route("GetAsync")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("EmployeeController.GetAsync: id is empty.");
                return BadRequest(GetBadRequestObject("EmployeeController.GetAsync: id is empty."));
            }

            try
            {
                return Ok(_mapper.Map<EmployeeModel>(await _service.GetByIdAsync(id)));
            }
            catch (Exception ex)
            {
                return BadRequest(GetBadRequestObject($"EmployeeController.GetAsync: {ex}."));
            }
        }

        [Route("GetListAsync")]
        [HttpGet]
        public async Task<IActionResult> GetListAsync(int page, int itemsPerPage)
        {
            if(!CheckParams(page, itemsPerPage, "EmployeeController.GetListAsync: page or itemsPerPage is 0."))
                return BadRequest("EmployeeController.GetListAsync: page or itemsPerPage is 0.");

            try
            {
                return Ok(_mapper.Map<List<EmployeeModel>>(await _service.GetPagedAsync(page, itemsPerPage)));
            }
            catch (Exception ex)
            {
                return BadRequest(GetBadRequestObject($"EmployeeController.GetListAsync: {ex}."));
            }
        }

        [Route("CreateAsync")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CreatingEmployeeModel employeeModel)
        {
            if (employeeModel == null)
            {
                _logger.LogError("EmployeeController.CreateAsync: employeeModel is null.");
                return BadRequest(GetBadRequestObject("EmployeeController.CreateAsync: employeeModel is null."));
            }

            try
            {
                var result = await _service.CreateAsync(_mapper.Map<CreatingEmployeeDto>(employeeModel));
                var shortEmployeeData = new ShortEmployeeModel()
                {
                    Id = result,
                    Firstname = employeeModel.Firstname,
                    Surname = employeeModel.Surname,
                    Position = employeeModel.Position,
                    IsAdmin = employeeModel.IsAdmin,
                    IsDeleted = false
                };
                _rabbitMqService.SendMessage(new List<ShortEmployeeModel>() { shortEmployeeData });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(GetBadRequestObject($"EmployeeController.CreateAsync: {ex}"));
            }
        }

        [Route("EditAsync")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(Guid id, UpdatingEmployeeModel employeeModel)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("EmployeeController.EditAsync: id is empty.");
                return BadRequest(GetBadRequestObject("EmployeeController.EditAsync: id is empty."));
            }
            if (employeeModel == null)
            {
                _logger.LogError("EmployeeController.EditAsync: employeeModel is null.");
                return BadRequest(GetBadRequestObject("EmployeeController.EditAsync: employeeModel is null."));
            }

            try
            {
                await _service.UpdateAsync(id, _mapper.Map<UpdatingEmployeeModel, UpdatingEmployeeDto>(employeeModel));
                var shortEmployeeData = new ShortEmployeeModel()
                {
                    Id = id,
                    Firstname = employeeModel.Firstname,
                    Surname = employeeModel.Surname,
                    Position = employeeModel.Position,
                    IsAdmin = employeeModel.IsAdmin,
                    IsDeleted = employeeModel.IsDeleted
                };
                _rabbitMqService.SendMessage(new List<ShortEmployeeModel>() { shortEmployeeData });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(GetBadRequestObject($"EmployeeController.EditAsync: {ex}"));
            }
        }

        [Route("Delete")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("EmployeeController.DeleteAsync: id is empty.");
                return BadRequest(GetBadRequestObject("EmployeeController.DeleteAsync: id is empty."));
            }

            try
            {
                await _service.DeleteAsync(id);

                var employeeModel = _mapper.Map<EmployeeModel>(await _service.GetByIdAsync(id));

                var shortEmployeeData = new ShortEmployeeModel()
                {
                    Id = id,
                    Firstname = employeeModel.Firstname,
                    Surname = employeeModel.Surname,
                    Position = employeeModel.Position,
                    IsAdmin = employeeModel.IsAdmin,
                    IsDeleted = true
                };
                _rabbitMqService.SendMessage(new List<ShortEmployeeModel>() { shortEmployeeData });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(GetBadRequestObject($"EmployeeController.DeleteAsync: {ex}"));
            }
        }

        [Route("CheckIsAdmin")]
        [HttpGet]
        public async Task<IActionResult> CheckIsAdmin(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                _logger.LogError("EmployeeController.CheckIsAdmin: employeeId is empty.");
                return BadRequest(GetBadRequestObject("EmployeeController.CheckIsAdmin: employeeId is empty."));
            }

            try
            {
                var employee = _mapper.Map<EmployeeModel>(await _service.GetByIdAsync(employeeId));
                return Ok(employee != null && employee.IsAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(GetBadRequestObject($"EmployeeController.CheckIsAdmin: {ex}."));
            }
        }

        private bool CheckParams(int page, int itemsPerPage, string methodName)
        {
            if (page == 0)
            {
                _logger.LogError($"{methodName}: page number can't be 0.");
                return false;
            }
            if (itemsPerPage == 0)
            {
                _logger.LogError($"{methodName}: items per page number can't be 0.");
                return false;
            }

            return true;
        }

        private object GetBadRequestObject(string error)
        {
            return new { Status = "400", Error = error };
        }
    }
}

using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Contracts.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.News;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<NewsController> _logger;

        public NewsController(
            INewsService service,
            ILogger<NewsController> logger,
            IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(_mapper.Map<NewsModel>(await _service.GetByIdAsync(id)));
        }

        [Route("CreateAsync")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreatingNewsModel newsModel)
        {
            return Ok(await _service.CreateAsync(_mapper.Map<CreatingNewsDto>(newsModel)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(Guid id, [FromBody] UpdatingNewsModel newsModel)
        {
            await _service.UpdateAsync(id, _mapper.Map<UpdatingNewsModel, UpdatingNewsDto>(newsModel));
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [Route("GetListAsync")]
        [HttpGet("list/{page}/{itemsPerPage}")]
        public async Task<IActionResult> GetListAsync(NewsFilterModel filterModel)
        {
            return Ok(_mapper.Map<List<NewsModel>>(await _service.GetPagedAsync(filterModel.Page, filterModel.ItemsPerPage)));
        }
    }
}

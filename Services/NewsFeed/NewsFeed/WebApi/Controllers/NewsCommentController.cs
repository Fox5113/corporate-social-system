using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Contracts.NewsComment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using WebApi.Models.NewsComment;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsCommentController : ControllerBase
    {
        private readonly INewsCommentService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<NewsCommentController> _logger;

        public NewsCommentController(
            INewsCommentService service,
            ILogger<NewsCommentController> logger,
            IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(_mapper.Map<NewsCommentModel>(await _service.GetByIdAsync(id)));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatingNewsCommentModel NewsCommentModel)
        {
            return Ok(await _service.CreateAsync(_mapper.Map<CreatingNewsCommentDto>(NewsCommentModel)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(Guid id, UpdatingNewsCommentModel NewsCommentModel)
        {
            await _service.UpdateAsync(id, _mapper.Map<UpdatingNewsCommentModel, UpdatingNewsCommentDto>(NewsCommentModel));
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("list/{page}/{itemsPerPage}")]
        public async Task<IActionResult> GetListAsync(NewsCommentFilterModel filterModel)
        {
            return Ok(_mapper.Map<List<NewsCommentModel>>(await _service.GetPagedAsync(filterModel.Page, filterModel.ItemsPerPage)));
        }
    }
}

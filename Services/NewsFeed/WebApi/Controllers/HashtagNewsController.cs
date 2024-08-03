using AutoMapper;
using BusinessLogic.Contracts.HashtagNews;
using BusinessLogic.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using WebApi.Models.HashtagNews;

namespace WebApi.Controllers
{
    public class HashtagNewsController : ControllerBase
    {
        private readonly IHashtagNewsService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<HashtagNewsController> _logger;

        public HashtagNewsController(
            IHashtagNewsService service,
            ILogger<HashtagNewsController> logger,
            IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(_mapper.Map<HashtagNewsModel>(await _service.GetByIdAsync(id)));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatingHashtagNewsModel HashtagNewsModel)
        {
            return Ok(await _service.CreateAsync(_mapper.Map<CreatingHashtagNewsDto>(HashtagNewsModel)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}

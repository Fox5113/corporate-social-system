using AutoMapper;
using BusinessLogic.Contracts.Hashtag;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using WebApi.Models.Hashtag;
using BusinessLogic.Services.Abstractions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HashtagController : ControllerBase
    {
        private readonly IHashtagService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<HashtagController> _logger;

        public HashtagController(
            IHashtagService service,
            ILogger<HashtagController> logger,
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
            return Ok(_mapper.Map<HashtagModel>(await _service.GetByIdAsync(id)));
        }

        [Route("CreateAsync")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatingHashtagModel HashtagModel)
        {
            return Ok(await _service.CreateAsync(_mapper.Map<CreatingHashtagDto>(HashtagModel)));
        }
    }
}

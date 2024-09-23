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
            if(id ==  Guid.Empty)
            {
                _logger.LogError("HashtagController.GetAsync: id is empty.");
                return BadRequest(GetBadRequestObject("HashtagController.GetAsync: id is empty."));
            }

            try
            {
                return Ok(_mapper.Map<HashtagModel>(await _service.GetByIdAsync(id)));
            }
            catch (Exception ex)
            {
                return BadRequest(GetBadRequestObject($"HashtagController.GetAsync: {ex}."));
            }
        }

        [Route("CreateAsync")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatingHashtagModel hashtagModel)
        {
            if (hashtagModel == null)
            {
                _logger.LogError("HashtagController.CreateAsync: hashtagModel is null.");
                return BadRequest(GetBadRequestObject("HashtagController.CreateAsync: hashtagModel is null."));
            }

            try
            {
                return Ok(await _service.CreateAsync(_mapper.Map<CreatingHashtagDto>(hashtagModel)));
            }
            catch (Exception ex)
            {
                return BadRequest(GetBadRequestObject($"HashtagController.CreateAsync: {ex}"));
            }
        }

        private object GetBadRequestObject(string error)
        {
            return new { Status = "400", Error = error };
        }
    }
}

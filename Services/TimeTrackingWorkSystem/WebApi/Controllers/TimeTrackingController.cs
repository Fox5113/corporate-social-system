using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Contracts.TimeTracker;
using WebApi.Models.TimeTracking;

namespace WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TimeTrackerController : ControllerBase
	{
		private readonly ITimeTrackerService _service;
		private readonly IMapper _mapper;
		private readonly ILogger<TimeTrackerController> _logger;

		public TimeTrackerController(ITimeTrackerService service, ILogger<TimeTrackerController> logger, IMapper mapper)
		{
			_service = service;
			_logger = logger;
			_mapper = mapper;
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			var timeTrackers = await _service.GetAllAsync(new CancellationToken());
			return Ok(_mapper.Map<List<TimeTrackerModel>>(timeTrackers));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetAsync(Guid id)
		{
			return Ok(_mapper.Map<TimeTrackerModel>(await _service.GetByIdAsync(id)));
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(CreatingTimeTrackerModel courseModel)
		{
			return Ok(await _service.CreateAsync(_mapper.Map<CreatingTimeTrackerDto>(courseModel)));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> EditAsync(Guid id, UpdatingTimeTrackerModel courseModel)
		{
			await _service.UpdateAsync(id, _mapper.Map<UpdatingTimeTrackerModel, UpdatingTimeTrackerDto>(courseModel));
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(Guid id)
		{
			await _service.DeleteAsync(id);
			return Ok();
		}

		[HttpPost("list")]
		public async Task<IActionResult> GetListAsync(TimeTrackerFilterModel filterModel)
		{
			var filterDto = _mapper.Map<TimeTrackerFilterModel, TimeTrackerFilterDto>(filterModel);
			return Ok(_mapper.Map<List<TimeTrackerModel>>(await _service.GetPagedAsync(filterDto)));
		}
	}
}
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Contracts.News;
using DataAccess.Common;
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

        [Route("Publish")]
        [HttpPut]
        public async Task<IActionResult> Publish(Guid id, [FromBody] UpdatingNewsModel newsModel)
        {
            var newsDto = _mapper.Map<UpdatingNewsModel, UpdatingNewsDto>(newsModel);
            newsDto.IsPublished = true;
            newsDto.IsArchived = false;

            await _service.UpdateAsync(id, newsDto);
            return Ok();
        }

        [Route("Cancel")]
        [HttpPut]
        public async Task<IActionResult> Cancel(Guid id, [FromBody] UpdatingNewsModel newsModel)
        {
            var newsDto = _mapper.Map<UpdatingNewsModel, UpdatingNewsDto>(newsModel);
            newsDto.IsPublished = false;
            newsDto.IsArchived = true;

            await _service.UpdateAsync(id, newsDto);
            return Ok();
        }

        [Route("Archive")]
        [HttpPut]
        public async Task<IActionResult> Archive(Guid id, [FromBody] UpdatingNewsModel newsModel)
        {
            var newsDto = _mapper.Map<UpdatingNewsModel, UpdatingNewsDto>(newsModel);
            newsDto.IsPublished = true;
            newsDto.IsArchived = true;

            await _service.UpdateAsync(id, newsDto);
            return Ok();
        }

        [Route("SendOnModeration")]
        [HttpPut]
        public async Task<IActionResult> SendOnModeration(Guid id, [FromBody] UpdatingNewsModel newsModel)
        {
            var newsDto = _mapper.Map<UpdatingNewsModel, UpdatingNewsDto>(newsModel);
            newsDto.IsPublished = false;
            newsDto.IsArchived = false;

            await _service.UpdateAsync(id, newsDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [Route("GetPublishedListAsync")]
        [HttpGet]
        public async Task<IActionResult> GetPublishedListAsync(int page, int itemsPerPage)
        {
            var newsSearch = new NewsSearch() { Skip = (page - 1) * itemsPerPage , Take = itemsPerPage, IsPublished = true, IsArchived = false };
            return Ok(_mapper.Map<List<NewsModel>>(await _service.GetCollection(newsSearch)));
        }

        [Route("GetOnModerationListAsync")]
        [HttpGet]
        public async Task<IActionResult> GetOnModerationListAsync(int page, int itemsPerPage)
        {
            var newsSearch = new NewsSearch() { Skip = (page - 1) * itemsPerPage, Take = itemsPerPage, IsPublished = false, IsArchived = false };
            return Ok(_mapper.Map<List<NewsModel>>(await _service.GetCollection(newsSearch)));
        }

        [Route("GetArchivedListAsync")]
        [HttpGet]
        public async Task<IActionResult> GetArchivedListAsync(int page, int itemsPerPage, Guid authorId)
        {
            var newsSearch = new NewsSearch() { Skip = (page - 1) * itemsPerPage, Take = itemsPerPage, IsPublished = true, IsArchived = true, AuthorId = authorId };
            return Ok(_mapper.Map<List<NewsModel>>(await _service.GetCollection(newsSearch)));
        }

        [Route("GetCancelledListAsync")]
        [HttpGet]
        public async Task<IActionResult> GetCancelledListAsync(int page, int itemsPerPage, Guid authorId)
        {
            var newsSearch = new NewsSearch() { Skip = (page - 1) * itemsPerPage, Take = itemsPerPage, IsPublished = false, IsArchived = true, AuthorId = authorId };
            return Ok(_mapper.Map<List<NewsModel>>(await _service.GetCollection(newsSearch)));
        }

        [Route("CheckIsAuthor")]
        [HttpGet]
        public async Task<IActionResult> CheckIsAuthor(Guid newsId, Guid authorId)
        {
            var news = _mapper.Map<NewsModel>(await _service.GetByIdAsync(newsId));
            return Ok(news != null && news.AuthorId == authorId);
        }
    }
}

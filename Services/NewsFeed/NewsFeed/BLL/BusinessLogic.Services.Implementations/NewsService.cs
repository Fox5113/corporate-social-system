using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Contracts.News;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class NewsService : INewsService
    {
        private readonly IMapper _mapper;
        private readonly INewsRepository _newsRepository;

        public NewsService(IMapper mapper, INewsRepository newsRepository)
        {
            _mapper = mapper;
            _newsRepository = newsRepository;
        }

        public async Task<ICollection<NewsDto>> GetPagedAsync(int page, int pageSize)
        {
            ICollection<News> entities = await _newsRepository.GetPagedAsync(page, pageSize);
            return _mapper.Map<ICollection<News>, ICollection<NewsDto>>(entities);
        }

        public async Task<NewsDto> GetByIdAsync(Guid id)
        {
            var news = await _newsRepository.GetAsync(id);
            return _mapper.Map<NewsDto>(news);
        }

        public async Task<Guid> CreateAsync(CreatingNewsDto creatingNewsDto)
        {
            var news = _mapper.Map<CreatingNewsDto, News>(creatingNewsDto);
            var createdNews = await _newsRepository.AddAsync(news);
            await _newsRepository.SaveChangesAsync();
            return createdNews.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            _newsRepository.DeleteRelatedEntities(id);
            _newsRepository.Delete(id);
            await _newsRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, UpdatingNewsDto updatingNewsDto)
        {
            var news = await _newsRepository.GetAsync(id);
            if (news == null)
            {
                throw new Exception($"Новость с идентфикатором {id} не найдена");
            }

            news.Title = updatingNewsDto.Title;
            news.ShortDescription = updatingNewsDto.ShortDescription;
            news.Content = updatingNewsDto.Content;
            news.Likes = updatingNewsDto.Likes;
            news.UpdatedAt = DateTime.Now;
            _newsRepository.Update(news);
            await _newsRepository.SaveChangesAsync();
        }
    }
}

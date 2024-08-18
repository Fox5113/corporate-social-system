using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Contracts.HashtagNews;
using BusinessLogic.Contracts.News;
using DataAccess.Common;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Получить постраничный список.
        /// </summary>
        /// <param name="page"> Номер страницы. </param>
        /// <param name="itemsPerPage"> Количество элементов на странице. </param>
        /// <returns> Список новостей. </returns>
        public async Task<ICollection<NewsDto>> GetPagedAsync(int page, int pageSize)
        {
            ICollection<News> entities = await _newsRepository.GetPagedAsync(page, pageSize);
            _newsRepository.JoinEntities(entities);
            return _mapper.Map<ICollection<News>, ICollection<NewsDto>>(entities);
        }

        /// <summary>
        /// Получение коллекции Новостей по объекту NewsSearch
        /// </summary>
        /// <param name="newsSearch">Объект расширенного поиска новостей</param>
        /// <returns>Коллекция новостей</returns>
        public async Task<ICollection<NewsDto>> GetCollection(NewsSearch newsSearch)
        {
            ICollection<News> entities = await _newsRepository.GetCollection(newsSearch);
            _newsRepository.JoinEntities(entities);
            return _mapper.Map<ICollection<News>, ICollection<NewsDto>>(entities);
        }

        /// <summary>
        /// Получить новость.
        /// </summary>
        /// <param name="id"> Идентификатор. </param>
        /// <returns> ДТО новости. </returns>
        public async Task<NewsDto> GetByIdAsync(Guid id)
        {
            var news = await _newsRepository.GetAsync(id);
            _newsRepository.JoinEntities(new List<News>() { news });
            return _mapper.Map<NewsDto>(news);
        }

        /// <summary>
        /// Создать новость.
        /// </summary>
        /// <param name="creatingNewsDto"> ДТО создаваемой новости. </param>
        public async Task<NewsDto> CreateAsync(CreatingNewsDto creatingNewsDto)
        {
            var news = _mapper.Map<CreatingNewsDto, News>(creatingNewsDto);
            var createdNews = await _newsRepository.AddAsync(news);
            await _newsRepository.SaveChangesAsync();
            _newsRepository.JoinAuthor(createdNews);
            _newsRepository.ClearLinks(createdNews);
            return _mapper.Map<NewsDto>(createdNews);
        }

        /// <summary>
        /// Удалить новость.
        /// </summary>
        /// <param name="id"> Идентификатор. </param>
        public async Task DeleteAsync(Guid id)
        {
            _newsRepository.DeleteRelatedEntities(id);
            _newsRepository.Delete(id);
            await _newsRepository.SaveChangesAsync();
        }

        /// /// <summary>
        /// Изменить новость.
        /// </summary>
        /// <param name="id"> Иентификатор. </param>
        /// <param name="updatingNewsDto"> ДТО редактируемой новости. </param>
        public async Task UpdateAsync(Guid id, UpdatingNewsDto updatingNewsDto)
        {
            var news = await _newsRepository.GetAsync(id);
            if (news == null)
            {
                throw new Exception($"Новость с идентификатором {id} не найдена");
            }
            _newsRepository.JoinEntities(new List<News>() { news });

            news.Title = updatingNewsDto.Title;
            news.ShortDescription = updatingNewsDto.ShortDescription;
            news.Content = updatingNewsDto.Content;
            news.Likes = updatingNewsDto.Likes;
            news.UpdatedAt = DateTime.Now;
            news.IsArchived = updatingNewsDto.IsArchived;
            news.IsPublished = updatingNewsDto.IsPublished;

            if((updatingNewsDto.HashtagNewsList != null && updatingNewsDto.HashtagNewsList.Count > 0) ||
                (news.HashtagNewsList != null && news.HashtagNewsList.Count > 0))
            {
                news.HashtagNewsList = _newsRepository.GetNewHashtagNewsList(news.HashtagNewsList, updatingNewsDto.HashtagNewsList.Select(x => _mapper.Map<CreatingHashtagNewsDto, HashtagNews>(x)).ToList());
            }

            _newsRepository.Update(news);
            await _newsRepository.SaveChangesAsync();
        }
    }
}

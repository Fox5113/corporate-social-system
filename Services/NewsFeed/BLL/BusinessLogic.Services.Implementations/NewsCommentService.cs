using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Contracts.NewsComment;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class NewsCommentService : INewsCommentService
    {
        private readonly IMapper _mapper;
        private readonly INewsCommentRepository _newsCommentRepository;

        public NewsCommentService(IMapper mapper, INewsCommentRepository newsCommentRepository)
        {
            _mapper = mapper;
            _newsCommentRepository = newsCommentRepository;
        }

        public async Task<ICollection<NewsCommentDto>> GetPagedAsync(int page, int pageSize)
        {
            ICollection<NewsComment> entities = await _newsCommentRepository.GetPagedAsync(page, pageSize);
            return _mapper.Map<ICollection<NewsComment>, ICollection<NewsCommentDto>>(entities);
        }

        public async Task<NewsCommentDto> GetByIdAsync(Guid id)
        {
            var news = await _newsCommentRepository.GetAsync(id);
            return _mapper.Map<NewsCommentDto>(news);
        }

        public async Task<Guid> CreateAsync(CreatingNewsCommentDto creatingNewsCommentDto)
        {
            var news = _mapper.Map<CreatingNewsCommentDto, NewsComment>(creatingNewsCommentDto);
            var createdNews = await _newsCommentRepository.AddAsync(news);
            await _newsCommentRepository.SaveChangesAsync();
            return createdNews.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            _newsCommentRepository.Delete(id);
            await _newsCommentRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, UpdatingNewsCommentDto updatingNewsCommentDto)
        {
            var news = await _newsCommentRepository.GetAsync(id);
            if (news == null)
            {
                throw new Exception($"Комментарий с идентфикатором {id} не найдена");
            }

            news.Content = updatingNewsCommentDto.Content;
            news.UpdatedAt = DateTime.Now;
            _newsCommentRepository.Update(news);
            await _newsCommentRepository.SaveChangesAsync();
        }
    }
}

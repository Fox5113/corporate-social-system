using AutoMapper;
using BusinessLogic.Contracts.HashtagNews;
using BusinessLogic.Services.Abstractions;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class HashtagNewsService : IHashtagNewsService
    {
        private readonly IMapper _mapper;
        private readonly IHashtagNewsRepository _hashtagNewsRepository;

        public HashtagNewsService(IMapper mapper, IHashtagNewsRepository hashtagNewsRepository)
        {
            _mapper = mapper;
            _hashtagNewsRepository = hashtagNewsRepository;
        }

        public async Task<HashtagNewsDto> GetByIdAsync(Guid id)
        {
            var HashtagNews = await _hashtagNewsRepository.GetAsync(id);
            return _mapper.Map<HashtagNewsDto>(HashtagNews);
        }

        public async Task<Guid> CreateAsync(CreatingHashtagNewsDto creatingHashtagNewsDto)
        {
            var HashtagNews = _mapper.Map<CreatingHashtagNewsDto, HashtagNews>(creatingHashtagNewsDto);
            var createdHashtagNews = await _hashtagNewsRepository.AddAsync(HashtagNews);
            await _hashtagNewsRepository.SaveChangesAsync();
            return createdHashtagNews.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            _hashtagNewsRepository.Delete(id);
            await _hashtagNewsRepository.SaveChangesAsync();
        }
    }
}

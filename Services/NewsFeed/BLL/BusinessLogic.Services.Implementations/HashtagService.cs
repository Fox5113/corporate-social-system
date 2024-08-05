using AutoMapper;
using BusinessLogic.Contracts.Hashtag;
using BusinessLogic.Services.Abstractions;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class HashtagService : IHashtagService
    {
        private readonly IMapper _mapper;
        private readonly IHashtagRepository _hashtagRepository;

        public HashtagService(IMapper mapper, IHashtagRepository hashtagRepository)
        {
            _mapper = mapper;
            _hashtagRepository = hashtagRepository;
        }

        /// <summary>
        /// Получить хештег.
        /// </summary>
        /// <param name="id"> Идентификатор. </param>
        /// <returns> ДТО хештега. </returns>
        public async Task<HashtagDto> GetByIdAsync(Guid id)
        {
            var HashtagNews = await _hashtagRepository.GetAsync(id);
            return _mapper.Map<HashtagDto>(HashtagNews);
        }

        /// <summary>
        /// Создать хештег.
        /// </summary>
        /// <param name="creatingHashtagDto"> ДТО хештега. </param>
        public async Task<Guid> CreateAsync(CreatingHashtagDto creatingHashtagDto)
        {
            var hashtag = _mapper.Map<CreatingHashtagDto, Hashtag>(creatingHashtagDto);
            var createdHashtag = await _hashtagRepository.AddAsync(hashtag);
            await _hashtagRepository.SaveChangesAsync();
            return createdHashtag.Id;
        }
    }
}

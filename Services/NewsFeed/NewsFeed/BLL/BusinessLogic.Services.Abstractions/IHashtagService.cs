using System.Threading.Tasks;
using System;
using BusinessLogic.Contracts.Hashtag;

namespace BusinessLogic.Services.Abstractions
{
    public interface IHashtagService
    {
        /// <summary>
        /// Получить хештег.
        /// </summary>
        /// <param name="id"> Идентификатор. </param>
        /// <returns> ДТО хештега. </returns>
        Task<HashtagDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Создать хештег.
        /// </summary>
        /// <param name="creatingHashtagDto"> ДТО хештега. </param>
        Task<Guid> CreateAsync(CreatingHashtagDto creatingHashtagDto);
    }
}

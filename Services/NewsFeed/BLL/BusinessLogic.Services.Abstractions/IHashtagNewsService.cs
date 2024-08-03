using System.Threading.Tasks;
using System;
using BusinessLogic.Contracts.HashtagNews;

namespace BusinessLogic.Services.Abstractions
{
    public interface IHashtagNewsService
    {

        /// <summary>
        /// Получить связку.
        /// </summary>
        /// <param name="id"> Идентификатор. </param>
        /// <returns> ДТО связки. </returns>
        Task<HashtagNewsDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Создать связку.
        /// </summary>
        /// <param name="creatingHashtagNewsDto"> ДТО создаваемой связки. </param>
        Task<Guid> CreateAsync(CreatingHashtagNewsDto creatingHashtagNewsDto);

        /// <summary>
        /// Удалить связку.
        /// </summary>
        /// <param name="id"> Идентификатор. </param>
        Task DeleteAsync(Guid id);
    }
}

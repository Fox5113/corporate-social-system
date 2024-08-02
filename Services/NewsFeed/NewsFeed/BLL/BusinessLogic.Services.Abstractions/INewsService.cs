using BusinessLogic.Contracts.News;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Abstractions
{
    /// <summary>
    /// Cервис работы с новостями (интерфейс).
    /// </summary>
    public interface INewsService
    {
        /// <summary>
        /// Получить список новостей.
        /// </summary>
        /// <param name="page"> Номер страницы. </param>
        /// <param name="pageSize"> Объем страницы. </param>
        /// <returns> Список новостей. </returns>
        Task<ICollection<NewsDto>> GetPagedAsync(int page, int pageSize);

        /// <summary>
        /// Получить новость.
        /// </summary>
        /// <param name="id"> Идентификатор. </param>
        /// <returns> ДТО новости. </returns>
        Task<NewsDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Создать новость.
        /// </summary>
        /// <param name="creatingNewsDto"> ДТО создаваемой новости. </param>
        Task<Guid> CreateAsync(CreatingNewsDto creatingNewsDto);

        /// <summary>
        /// Изменить новость.
        /// </summary>
        /// <param name="id"> Иентификатор. </param>
        /// <param name="updatingNewsDto"> ДТО редактируемой новости. </param>
        Task UpdateAsync(Guid id, UpdatingNewsDto updatingNewsDto);

        /// <summary>
        /// Удалить новость.
        /// </summary>
        /// <param name="id"> Идентификатор. </param>
        Task DeleteAsync(Guid id);
    }
}
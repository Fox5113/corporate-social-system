using BusinessLogic.Contracts.NewsComment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Abstractions
{
    /// <summary>
    /// Cервис работы с новостями (интерфейс).
    /// </summary>
    public interface INewsCommentService
    {
        /// <summary>
        /// Получить список комментариев.
        /// </summary>
        /// <param name="page"> Номер страницы. </param>
        /// <param name="pageSize"> Объем страницы. </param>
        /// <returns> Список комментариев. </returns>
        Task<ICollection<NewsCommentDto>> GetPagedAsync(int page, int pageSize);

        /// <summary>
        /// Получить комментарий новости.
        /// </summary>
        /// <param name="id"> Идентификатор. </param>
        /// <returns> ДТО комментария. </returns>
        Task<NewsCommentDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Создать комментарий новости.
        /// </summary>
        /// <param name="creatingNewsCommentDto"> ДТО создаваемого комментария. </param>
        Task<Guid> CreateAsync(CreatingNewsCommentDto creatingNewsCommentDto);

        /// <summary>
        /// Изменить комментарий новости.
        /// </summary>
        /// <param name="id"> Иентификатор. </param>
        /// <param name="updatingNewsCommentDto"> ДТО редактируемого комментария. </param>
        Task UpdateAsync(Guid id, UpdatingNewsCommentDto updatingNewsCommentDto);

        /// <summary>
        /// Удалить комментарий новости.
        /// </summary>
        /// <param name="id"> Идентификатор. </param>
        Task DeleteAsync(Guid id);
    }
}
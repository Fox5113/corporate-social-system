using DataAccess.Entities;
using Services.Repositories.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace DataAccess.Repositories.Abstractions
{
    public interface INewsCommentRepository : IRepository<NewsComment, Guid>
    {
        /// <summary>
        /// Получить постраничный список.
        /// </summary>
        /// <param name="page"> Номер страницы. </param>
        /// <param name="itemsPerPage"> Количество элементов на странице. </param>
        /// <returns> Список комментариев. </returns>
        Task<List<NewsComment>> GetPagedAsync(int page, int itemsPerPage);

        /// <summary>
        /// Получение коллекции комментариев к Новости
        /// </summary>
        /// <param name="newsId">Id новости</param>
        /// <returns>Коллекция комментариев по новости</returns>
        Task<List<NewsComment>> GetCollectionByNewsId(Guid newsId);

        /// <summary>
        /// Удаление комментариев по новости
        /// </summary>
        /// <param name="newsId">Id новости</param>
        void DeleteByNewsId(Guid newsId);

        /// <summary>
        /// Удаление комментариев по автору.
        /// </summary>
        /// <param name="authorId">Id автора</param>
        void DeleteByAuthorId(Guid authorId);
    }
}

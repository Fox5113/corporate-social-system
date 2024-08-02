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
        Task<List<NewsComment>> GetCollectionByNewsId(Guid newsId);
        void DeleteByNewsId(Guid newsId);
        void DeleteByAuthorId(Guid authorId);
    }
}

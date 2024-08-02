using DataAccess.Entities;
using NewsFeed.WebApi.Common;
using Services.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstractions
{
    public interface INewsRepository : IRepository<News, Guid>
    {
        /// <summary>
        /// Получить постраничный список.
        /// </summary>
        /// <param name="page"> Номер страницы. </param>
        /// <param name="itemsPerPage"> Количество элементов на странице. </param>
        /// <returns> Список новостей. </returns>
        Task<List<News>> GetPagedAsync(int page, int itemsPerPage);
        void DeleteRelatedEntities(Guid id);
        Task<List<News>> GetCollection(NewsSearch post);
    }
}

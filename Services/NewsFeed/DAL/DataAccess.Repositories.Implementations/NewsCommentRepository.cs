using DataAccess.Entities;
using DataAccess.EntityFramework;
using DataAccess.Repositories.Abstractions;
using Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class NewsCommentRepository : Repository<NewsComment, Guid>, INewsCommentRepository
    {
        public NewsCommentRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// Получить постраничный список.
        /// </summary>
        /// <param name="page"> Номер страницы. </param>
        /// <param name="itemsPerPage"> Количество элементов на странице. </param>
        /// <returns> Список комментариев. </returns>
        public async Task<List<NewsComment>> GetPagedAsync(int page, int itemsPerPage)
        {
            var query = GetAll();
            return await query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();
        }

        /// <summary>
        /// Получение коллекции комментариев к Новости
        /// </summary>
        /// <param name="newsId">Id новости</param>
        /// <returns></returns>
        public async Task<List<NewsComment>> GetCollectionByNewsId(Guid newsId)
        {
            var query = GetAll();
            return await query
                .Where(x => x.NewsId == newsId)
                .ToListAsync();
        }

        /// <summary>
        /// Удаление комментариев по новости
        /// </summary>
        /// <param name="newsId">Id новости</param>
        public async void DeleteByNewsId(Guid newsId)
        {
            var collection = await GetAll().Where(x => x.NewsId == newsId).ToListAsync();

            foreach (var item in collection)
            {
                Delete(item);
            }

            SaveChanges();
        }

        /// <summary>
        /// Удаление комментариев по автору.
        /// </summary>
        /// <param name="authorId">Id автора</param>
        public async void DeleteByAuthorId(Guid authorId)
        {
            var collection = await GetAll().Where(x => x.AuthorId == authorId).ToListAsync();

            foreach (var item in collection)
            {
                Delete(item);
            }

            SaveChanges();
        }
    }
}

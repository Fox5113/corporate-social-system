using BusinessLogic.Contracts.HashtagNews;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using DataAccess.Repositories.Abstractions;
using Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class HashtagNewsRepository : Repository<HashtagNews, Guid>, IHashtagNewsRepository
    {
        public HashtagNewsRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// Получение записей промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="hashtagNewsDto">Дто связки</param>
        /// <returns></returns>
        public async Task<List<HashtagNews>> GetCollection(CreatingHashtagNewsDto hashtagNewsDto)
        {
            var query = GetAll();
            return await query
                .Where(x => x.HashtagId == hashtagNewsDto.HashtagId && x.NewsId == hashtagNewsDto.NewsId)
                .ToListAsync();
        }

        /// <summary>
        /// Получение записей промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="postIds">Id новостей</param>
        /// <returns></returns>
        public async Task<List<HashtagNews>> GetCollectionByNewsId(List<Guid> postIds)
        {
            var query = GetAll();
            return await query
                .Where(x => postIds.Contains(x.NewsId))
                .ToListAsync();
        }

        /// <summary>
        /// Получение записей промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="hashtagIds">Id хэштегов</param>
        /// <returns></returns>
        public async Task<List<HashtagNews>> GetCollectionByHashtagId(List<Guid> hashtagIds)
        {
            var query = GetAll();
            return await query
                .Where(x => hashtagIds.Contains(x.HashtagId))
                .ToListAsync();
        }

        /// <summary>
        /// Удаление записи промежуточной таблицы, связывающей хэштеги и новости
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
    }
}

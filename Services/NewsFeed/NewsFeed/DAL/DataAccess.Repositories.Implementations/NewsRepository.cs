using DataAccess.Entities;
using DataAccess.EntityFramework;
using DataAccess.Repositories.Abstractions;
using Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using NewsFeed.WebApi.Common;
using NewsFeed.WebApi.Common.SqlQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Репозиторий работы с новостями.
    /// </summary>
    public class NewsRepository : Repository<News, Guid>, INewsRepository
    {
        protected readonly DataContext _dataContext;
        public NewsRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        /// <summary>
        /// Получить постраничный список.
        /// </summary>
        /// <param name="page"> Номер страницы. </param>
        /// <param name="itemsPerPage"> Количество элементов на странице. </param>
        /// <returns> Список новостей. </returns>
        public async Task<List<News>> GetPagedAsync(int page, int itemsPerPage)
        {
            var query = GetAll();
            return await query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();
        }

        /// <summary>
        /// Получение коллекции Новостей по объекту NewsSearch
        /// </summary>
        /// <param name="post">NewsSearch</param>
        /// <returns></returns>
        public async Task<List<News>> GetCollection(NewsSearch post)
        {   
            var query = GetAll();
            var collection = await query
                .Where(x => !string.IsNullOrEmpty(post.Title) ? x.Title.Contains(post.Title) : true)
                    .Where(x => !string.IsNullOrEmpty(post.Body) ? x.Content.Contains(post.Body) : true)
                    .Where(x => post.From != DateTime.MinValue ? x.CreatedAt >= post.From : true)
                    .Where(x => post.To != DateTime.MinValue ? x.CreatedAt <= post.To : true)
                    .Where(x => post.AuthorId != Guid.Empty ? x.AuthorId == post.AuthorId : true)
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(post.Skip)
                    .Take(post.Take > 0 ? post.Take : 1000)
                    .ToListAsync();

            if (post.Hashtags != null)
            {
                var names = post.Hashtags.Select(x => x.Name).ToArray();
                var hashtagsMapping = new MappingQuery()
                {
                    MainTableName = nameof(Hashtag),
                    TableFilter = new TableFilter() { FieldsFilter = new List<FieldFilter>() { new FieldFilter() { Name = nameof(Hashtag.Name), Data = names } } }
                };
                var hashtagsFromDb = new HashtagRepository(_dataContext).GetCollection(hashtagsMapping);
                var hashtagIds = hashtagsFromDb.Select(x => x).Select(x => (object)x.Id).ToArray();

                var hashtagNewsMapping = new MappingQuery()
                {
                    MainTableName = nameof(HashtagNews),
                    TableFilter = new TableFilter() { FieldsFilter = new List<FieldFilter>() { new FieldFilter() { Name = nameof(HashtagNews.HashtagId), Data = hashtagIds } } }
                };
                var hashtagsPosts = new HashtagNewsRepository(_dataContext).GetCollection(hashtagNewsMapping);
                var postsIds = hashtagsPosts.Select(x => x).Select(x => x.NewsId).ToList();

                query = query.Where(x => postsIds.Contains(x.Id));
            }

            return collection;
        }

        /// <summary>
        /// Удаление связанных сущностей.
        /// </summary>
        /// <param name="newsId"> Id новости </param>
        public void DeleteRelatedEntities(Guid newsId)
        {
            var hashtagsNewsRep = new HashtagNewsRepository(_dataContext);
            hashtagsNewsRep.DeleteByNewsId(newsId);

            var newsCommentRep = new NewsCommentRepository(_dataContext);
            newsCommentRep.DeleteByNewsId(newsId);
        }
    }
}
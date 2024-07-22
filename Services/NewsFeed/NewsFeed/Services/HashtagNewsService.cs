using Microsoft.EntityFrameworkCore;
using NewsFeed.Abstract;
using NewsFeed.Common;
using NewsFeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Services
{
    public class HashtagNewsService : BaseService
    {
        public HashtagNewsService(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// Получение записи промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="hashtagId">Id хэштега</param>
        /// <param name="postId">Id новости</param>
        /// <returns></returns>
        public HashtagNews GetHashtagNews(Guid hashtagId, Guid postId)
        {
            return _dbContext.HashtagNews.FirstOrDefault(x => x.HashtagId == hashtagId && x.NewsId == postId);
        }

        /// <summary>
        /// Получение записей промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="postIds">Id новостей</param>
        /// <returns></returns>
        public ICollection<HashtagNews> GetHashtagNewsCollectionByNewsId(List<Guid> postIds)
        {
            return _dbContext.HashtagNews.Where(x => postIds.Contains(x.NewsId)).ToList();
        }

        /// <summary>
        /// Получение записей промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="hashtagIds">Id хэштегов</param>
        /// <returns></returns>
        public ICollection<HashtagNews> GetHashtagNewsCollectionByHashtagId(List<Guid> hashtagIds)
        {
            return _dbContext.HashtagNews.Where(x => hashtagIds.Contains(x.HashtagId)).ToList();
        }

        /// <summary>
        /// Создание записи промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="hashtagId">Id хэштега</param>
        /// <param name="postId">Id новости</param>
        /// <returns></returns>
        public HashtagNews CreateHashtagNewsEntity(Guid hashtagId, Guid postId)
        {
            var hashtagPost = new HashtagNews() { Id = Guid.NewGuid(), HashtagId = hashtagId, NewsId = postId };
            _dbContext.HashtagNews.Add(hashtagPost);
            _dbContext.SaveChanges();
            return hashtagPost;
        }

        /// <summary>
        /// Удаление записи промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="postId">Id новости</param>
        public void DeleteHashtagNews(Guid postId)
        {
            var postHashtags = _dbContext.HashtagNews.Where(x => x.NewsId == postId);

            _dbContext.HashtagNews.RemoveRange(postHashtags);
            _dbContext.SaveChanges();
        }

        public override IQueryable<HashtagNews> GetCollection(Mapping mapping)
        {
            if (mapping == null)
                return null;

            var script = SqlScriptPreparerService.GetSelectQuery(mapping);
            var collection = _dbContext.HashtagNews.FromSqlRaw(script);
            return collection;
        }

        public override List<HashtagNews> GetCollection<HashtagNews>(int skip, int count)
        {
            var collection = _dbContext.HashtagNews.Skip(skip).Take(count).ToList();
            return collection as List<HashtagNews>;
        }

        public override List<HashtagNews> GetCollection<HashtagNews>(List<Guid> ids)
        {
            return _dbContext.HashtagNews.Where(x => ids.Contains(x.Id)).ToList() as List<HashtagNews>;
        }

        public override HashtagNews GetEntity(Guid id)
        {
            return _dbContext.HashtagNews.FirstOrDefault(x => x.Id == id);
        }

        public override HashtagNews CreateEntity<HashtagNews>(HashtagNews newObject)
        {
            _dbContext.HashtagNews.Add(newObject as NewsFeed.Models.HashtagNews);
            _dbContext.SaveChanges();

            return newObject;
        }
    }
}

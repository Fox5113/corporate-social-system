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
    public class NewsService : BaseService
    {

        public NewsService(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// Получение коллекции Новостей без фильтров
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override List<News> GetCollection<News>(int skip, int count)
        {
            var collection = _dbContext.News.OrderByDescending(e => e.CreatedAt).Skip(skip).Take(count).ToList();
            return collection as List<News>;
        }

        /// <summary>
        /// Получение коллекции Новостей по коллекции Id
        /// </summary>
        /// <param name="ids">Id новостей</param>
        /// <returns></returns>
        public override List<News> GetCollection<News>(List<Guid> ids)
        {
            var collection = _dbContext.News.Where(x => ids.Contains(x.Id)).ToList();
            return collection as List<News>;
        }

        public override IQueryable<News> GetCollection(Mapping mapping)
        {
            if (mapping == null)
                return null;

            var script = SqlScriptPreparerService.GetSelectQuery(mapping);
            var collection = _dbContext.News.FromSqlRaw(script);
            return collection;
        }

        /// <summary>
        /// Получение коллекции Новостей по объекту NewsSearch
        /// </summary>
        /// <param name="post">NewsSearch</param>
        /// <returns></returns>
        public ICollection<News> GetCollection(NewsSearch post)
        {
            var query = _dbContext.News
                    .Where(x => !String.IsNullOrEmpty(post.Title) ? x.Title.Contains(post.Title) : true)
                    .Where(x => !String.IsNullOrEmpty(post.Body) ? x.Content.Contains(post.Body) : true)
                    .Where(x => post.From != DateTime.MinValue ? x.CreatedAt >= (DateTime)post.From : true)
                    .Where(x => post.To != DateTime.MinValue ? x.CreatedAt <= (DateTime)post.To : true)
                    .Where(x => post.AuthorId != Guid.Empty ? x.AuthorId == post.AuthorId : true);

            if (post.Hashtags != null)
            {
                var names = post.Hashtags.Select(x => x.Name).ToArray();
                var hashtagsMapping = new Mapping()
                {
                    MainTableName = nameof(Hashtag),
                    TableFilter = new TableFilter() { FieldsFilter = new List<FieldFilter>() { new FieldFilter() { Name = nameof(Hashtag.Name), Data = names } } }
                };
                var hashtagsFromDb = new HashtagService(_dbContext).GetCollection(hashtagsMapping);
                var hashtagIds = hashtagsFromDb.Select(x => (Hashtag)x).Select(x => (object)x.Id).ToArray();

                var hashtagNewsMapping = new Mapping()
                {
                    MainTableName = nameof(HashtagNews),
                    TableFilter = new TableFilter() { FieldsFilter = new List<FieldFilter>() { new FieldFilter() { Name = nameof(HashtagNews.HashtagId), Data = hashtagIds } } }
                };
                var hashtagsPosts = new HashtagNewsService(_dbContext).GetCollection(hashtagNewsMapping);
                var postsIds = hashtagsPosts.Select(x => (HashtagNews)x).Select(x => x.NewsId).ToList();

                query = query.Where(x => postsIds.Contains(x.Id));
            }

            var collection = query.OrderByDescending(x => x.CreatedAt)
                .Skip(post.Skip)
                .Take(post.Take > 0 ? post.Take : 1000)
                .ToList();

            return collection;
        }

        /// <summary>
        /// Получение Новости по Id
        /// </summary>
        /// <param name="id">Id новости</param>
        /// <returns></returns>
        public override News GetEntity(Guid id)
        {
            var post = _dbContext.News.FirstOrDefault(x => x.Id == id);

            return post;
        }

        /// <summary>
        /// Создание Новости
        /// </summary>
        /// <param name="newPost">Новость</param>
        /// <param name="hashtags">Коллекция хэштегов</param>
        /// <returns></returns>
        public News CreateNews(News newPost, List<Hashtag> hashtags = null)
        {
            if (newPost.CreatedAt == DateTime.MinValue)
            {
                newPost.CreatedAt = DateTime.Now;
            }
            if (newPost.UpdatedAt == DateTime.MinValue)
            {
                newPost.UpdatedAt = DateTime.Now;
            }

            _dbContext.News.Add(newPost);
            _dbContext.SaveChanges();

            if (hashtags != null)
            {
                foreach (var hashtag in hashtags)
                {
                    var hashtagsMapping = new Mapping()
                    {
                        MainTableName = nameof(Hashtag),
                        TableFilter = new TableFilter() { FieldsFilter = new List<FieldFilter>() { new FieldFilter() { Name = nameof(Hashtag.Name), Data = new[] { hashtag.Name }, ComparisonType = 3 } } }
                    };
                    var newHashtag = new Hashtag();
                    var hashtagsDb = new HashtagService(_dbContext).GetCollection(hashtagsMapping).ToList();

                    if (hashtagsDb == null || hashtagsDb.Count == 0)
                    {
                        var service = new HashtagService(_dbContext);
                        newHashtag = service.CreateHashtag(hashtag.Name);
                    }
                    else
                        newHashtag = hashtagsDb.FirstOrDefault();

                    var hnService = new HashtagNewsService(_dbContext);
                    var hashtagPost = hnService.CreateHashtagNewsEntity(newHashtag.Id, newPost.Id);

                    if (newPost.HashtagNewsList == null)
                        newPost.HashtagNewsList = new List<HashtagNews>();

                    newPost.HashtagNewsList.Add(hashtagPost);
                    
                }
            }

            return newPost;
        }

        public void JoinAdditionalEntities(News post)
        {
            if (post == null)
                return;

            var commentsMapping = new Mapping()
            {
                MainTableName = "NewsComments",
                TableFilter = new TableFilter() { FieldsFilter = new List<FieldFilter>() { new FieldFilter() { Name = nameof(NewsComment.NewsId), Data = new[] { (object)post.Id }, ComparisonType = 3, TableName = "NewsComments" } } }
            };
            post.NewsCommentList = new NewsCommentService(_dbContext).GetCollection(commentsMapping)?.ToList();

            var hashtagNewsMapping = new Mapping()
            {
                MainTableName = nameof(HashtagNews),
                TableFilter = new TableFilter() { FieldsFilter = new List<FieldFilter>() { new FieldFilter() { Name = nameof(HashtagNews.NewsId), Data = new[] { (object)post.Id }, ComparisonType = 3, TableName = "HashtagNews" } } }
            };
            post.HashtagNewsList = new HashtagNewsService(_dbContext).GetCollection(hashtagNewsMapping)?.ToList();
        }

        public override News CreateEntity<News>(News newObject)
        {
            var obj = newObject as NewsFeed.Models.News;
            if (obj.CreatedAt == DateTime.MinValue)
            {
                obj.CreatedAt = DateTime.Now;
            }
            if (obj.UpdatedAt == DateTime.MinValue)
            {
                obj.UpdatedAt = DateTime.Now;
            }

            _dbContext.News.Add(obj);
            _dbContext.SaveChanges();

            return newObject;
        }
    }
}

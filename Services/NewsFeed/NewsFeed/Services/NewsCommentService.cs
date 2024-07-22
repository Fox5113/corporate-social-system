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
    public class NewsCommentService : BaseService
    {

        public NewsCommentService(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// Получение коллекции комментариев к Новости
        /// </summary>
        /// <param name="postId">Id новости</param>
        /// <returns></returns>
        public ICollection<NewsComment> GetComments(Guid postId)
        {
            return _dbContext.NewsComment.Where(x => x.NewsId == postId).ToList();
        }

        /// <summary>
        /// Добавление комментария к новости
        /// </summary>
        /// <param name="comment">Комментарий</param>
        /// <returns></returns>
        public NewsComment CreateNewsComment(NewsComment comment)
        {
            comment.Id = Guid.NewGuid();
            _dbContext.NewsComment.Add(comment);
            _dbContext.SaveChanges();
            return comment;
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id">Id комментария</param>
        public void DeleteNewsComment(Guid id)
        {
            var comment = _dbContext.NewsComment.Where(x => x.Id == id);

            _dbContext.NewsComment.RemoveRange(comment);
            _dbContext.SaveChanges();
        }

        public override IQueryable<NewsComment> GetCollection(Mapping mapping)
        {
            if (mapping == null)
                return null;

            var script = SqlScriptPreparerService.GetSelectQuery(mapping);
            var collection = _dbContext.NewsComment.FromSqlRaw(script);
            return collection;
        }

        public override List<NewsComment> GetCollection<NewsComment>(int skip, int count)
        {
            var collection = _dbContext.NewsComment.Skip(skip).Take(count).ToList();
            return collection as List<NewsComment>;
        }

        public override List<NewsComment> GetCollection<NewsComment>(List<Guid> ids)
        {
            return _dbContext.NewsComment.Where(x => ids.Contains(x.Id)).ToList() as List<NewsComment>;
        }

        public override NewsComment GetEntity(Guid id)
        {
            return _dbContext.NewsComment.FirstOrDefault(x => x.Id == id);
        }

        public override NewsComment CreateEntity<NewsComment>(NewsComment newObject)
        {
            var obj = newObject as NewsFeed.Models.NewsComment;
            if (obj.CreatedAt == DateTime.MinValue)
            {
                obj.CreatedAt = DateTime.Now;
            }

            _dbContext.NewsComment.Add(obj);
            _dbContext.SaveChanges();

            return newObject;
        }
    }
}

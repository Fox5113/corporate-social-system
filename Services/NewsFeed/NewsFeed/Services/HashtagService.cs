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
    public class HashtagService : BaseService
    {
        public HashtagService(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// Получение коллекции хэштегов по коллекции Id
        /// </summary>
        /// <param name="hashtagIds">Id хэштегов</param>
        /// <returns></returns>
        public ICollection<Hashtag> GetHashtagsCollection(ICollection<Guid> hashtagIds)
        {
            return _dbContext.Hashtag.Where(x => hashtagIds.Contains(x.Id)).ToList();
        }

        public override List<Hashtag> GetCollection<Hashtag>(int skip, int count)
        {
            var collection = _dbContext.Hashtag.Skip(skip).Take(count).ToList();
            return collection as List<Hashtag>;
        }

        public override List<Hashtag> GetCollection<Hashtag>(List<Guid> ids)
        {
            return _dbContext.Hashtag.Where(x => ids.Contains(x.Id)).ToList() as List<Hashtag>;
        }

        /// <summary>
        /// Получение коллекции хэштегов по названиям хэштегов
        /// </summary>
        /// <param name="hashtagNames">Коллекция наименований</param>
        /// <returns></returns>
        public ICollection<Hashtag> GetHashtagsCollection(ICollection<string> hashtagNames)
        {
            return _dbContext.Hashtag.Where(x => hashtagNames.Contains(x.Name)).ToList();
        }

        /// <summary>
        /// Получение хэштега по Id
        /// </summary>
        /// <param name="id">Id хэштега</param>
        /// <returns></returns>
        public override Hashtag GetEntity(Guid id)
        {
            return _dbContext.Hashtag.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Получение хэштега по названию
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <returns></returns>
        public Hashtag GetHashtag(string name)
        {
            return _dbContext.Hashtag.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Создание хэштега
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <returns></returns>
        public Hashtag CreateHashtag(string name)
        {
            var hashtag = new Hashtag() { Id = Guid.NewGuid(), Name = name };
            _dbContext.Hashtag.Add(hashtag);
            _dbContext.SaveChanges();
            return hashtag;
        }

        public override IQueryable<Hashtag> GetCollection(Mapping mapping)
        {
            if (mapping == null)
                return null;

            var script = SqlScriptPreparerService.GetSelectQuery(mapping);
            var collection = _dbContext.Hashtag.FromSqlRaw(script);
            return collection;
        }

        public override Hashtag CreateEntity<Hashtag>(Hashtag newObject)
        {
            _dbContext.Hashtag.Add(newObject as NewsFeed.Models.Hashtag);
            _dbContext.SaveChanges();

            return newObject;
        }
    }
}

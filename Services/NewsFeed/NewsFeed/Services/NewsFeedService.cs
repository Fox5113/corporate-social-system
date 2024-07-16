using Microsoft.EntityFrameworkCore;
using NewsFeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace NewsFeed.Services
{
    public class NewsFeedService
    {
        #region Post

        /// <summary>
        /// Получение коллекции Новостей по json запросу
        /// </summary>
        /// <param name="jsonData">Запрос</param>
        /// <returns></returns>
        public ICollection<News> GetNewsCollection(string jsonData, string objectName)
        {
            if(objectName == "Mapping")
            {
                var mapping = JsonSerializer.Deserialize<Mapping>(jsonData);
                return GetNewsCollection(mapping);
            }
            else if(objectName == "NewsSearch")
            {
                var postSearch = JsonSerializer.Deserialize<NewsSearch>(jsonData);
                return GetNewsCollection(postSearch);
            }

            return null;
        }

        /// <summary>
        /// Получение коллекции Новостей по объекту Mapping
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <returns></returns>
        public ICollection<News> GetNewsCollection(Mapping mapping)
        {
            var script = new SqlScriptPreparerService().GetSelectQuery(mapping);
            using (var dbContext = new DataContext())
            {
                var collection = dbContext.News.FromSqlRaw(script).ToList();
                foreach (var post in collection)
                {
                    JoinAdditionalEntities(post);
                }
                return collection;
            }
        }

        /// <summary>
        /// Получение коллекции Новостей по объекту PostSearch
        /// </summary>
        /// <param name="post">NewsSearch</param>
        /// <returns></returns>
        public ICollection<News> GetNewsCollection(NewsSearch post)
        {
            using (var dbContext = new DataContext())
            {
                var query = dbContext.News
                    .Where(x => !String.IsNullOrEmpty(post.Title) ? x.Title.Contains(post.Title) : true)
                    .Where(x => !String.IsNullOrEmpty(post.Body) ? x.Content.Contains(post.Body) : true)
                    .Where(x => post.From != null ? x.CreatedAt >= (DateTime)post.From : true)
                    .Where(x => post.To != null ? x.CreatedAt <= (DateTime)post.To : true)
                    .Where(x => post.AuthorId != Guid.Empty ? x.AuthorId == post.AuthorId : true);
                
                if(post.Hashtags != null)
                {
                    var names = post.Hashtags.Select(x => x.Name).ToList();
                    var hashtagsFromDb = GetHashtagsCollection(names);
                    var hashtagsPosts = GetHashtagNewsCollectionByHashtagId(hashtagsFromDb.Select(x => x.Id).ToList());
                    var postsIds = hashtagsPosts.Select(x => x.NewsId).ToList();
                    query = query.Where(x => postsIds.Contains(x.Id));
                }
                
                var collection = query.OrderByDescending(x => x.CreatedAt)
                    .Skip(post.Skip)
                    .Take(post.Take > 0 ? post.Take : 1000)
                    .ToList();

                foreach (var item in collection)
                {
                    JoinAdditionalEntities(item);
                }
                return collection;
            }
        }

        /// <summary>
        /// Получение коллекции Новостей без фильтров
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ICollection<News> GetNewsCollection(int skip, int count)
        {
            using (var dbContext = new DataContext())
            {
                var collection = dbContext.News.OrderByDescending(e => e.CreatedAt).Skip(skip).Take(count).ToList();
                foreach (var post in collection)
                {
                    JoinAdditionalEntities(post);
                }
                return collection;
            }
        }

        /// <summary>
        /// Получение коллекции Новостей по коллекции Id
        /// </summary>
        /// <param name="ids">Id новостей</param>
        /// <returns></returns>
        public ICollection<News> GetNewsCollection(ICollection<Guid> ids)
        {
            using (var dbContext = new DataContext())
            {
                var collection = dbContext.News.Where(x => ids.Contains(x.Id)).ToList();
                foreach(var post in collection)
                {
                    JoinAdditionalEntities(post);
                }
                return collection;
            }
        }

        /// <summary>
        /// Получение Новости по Id
        /// </summary>
        /// <param name="id">Id новости</param>
        /// <returns></returns>
        public News GetNewsById(Guid id)
        {
            using (var dbContext = new DataContext())
            {
                var post = dbContext.News.FirstOrDefault(x => x.Id == id);
                JoinAdditionalEntities(post);

                return post;
            }
        }

        public void JoinAdditionalEntities(News post)
        {
            post.NewsComments = GetComments(post.Id);
            post.NewsHashtags = GetHashtagNewsCollectionByNewsId(new List<Guid>() { post.Id });
            if (post.NewsHashtags.Count > 0)
                post.Hashtags = GetHashtagsCollection(post.NewsHashtags.Select(x => x.HashtagId).ToList());
        }

        /// <summary>
        /// Создание Новости
        /// </summary>
        /// <param name="newPost">Новость</param>
        /// <param name="hashtags">Коллекция хэштегов</param>
        /// <returns></returns>
        public News CreateNews(News newPost, ICollection<Hashtag> hashtags = null)
        {
            using (var dbContext = new DataContext())
            {
                dbContext.News.Add(newPost);
                dbContext.SaveChanges();

                if(hashtags != null)
                {
                    foreach (var hashtag in hashtags)
                    {
                        var hashtagDb = GetHashtag(hashtag.Name);
                        if (hashtagDb == null)
                        {
                            hashtagDb = CreateHashtag(hashtag.Name);
                        }

                        var hashtagPost = CreateHashtagNewsEntity(hashtagDb.Id, newPost.Id);

                        if (newPost.NewsHashtags == null)
                            newPost.NewsHashtags = new List<HashtagNews>();
                        if (newPost.Hashtags == null)
                            newPost.Hashtags = new List<Hashtag>();

                        newPost.NewsHashtags.Add(hashtagPost);
                        newPost.Hashtags.Add(hashtagDb);
                    }
                }
                
                return newPost;
            }
        }

        /// <summary>
        /// Удаление Новости
        /// </summary>
        /// <param name="id">Id новости</param>
        public void DeleteNewsById(Guid id)
        {
            using (var dbContext = new DataContext())
            {
                DeleteHashtagNews(id);
                var post = dbContext.News.FirstOrDefault(x => x.Id == id);

                dbContext.News.Remove(post);
                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Comment

        /// <summary>
        /// Получение коллекции комментариев к Новости
        /// </summary>
        /// <param name="postId">Id новости</param>
        /// <returns></returns>
        public ICollection<NewsComment> GetComments(Guid postId)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.NewsComments.Where(x => x.NewsId == postId).ToList();
            }
        }

        /// <summary>
        /// Добавление комментария к новости
        /// </summary>
        /// <param name="comment">Комментарий</param>
        /// <returns></returns>
        public NewsComment CreateNewsComment(NewsComment comment)
        {
            using (var dbContext = new DataContext())
            {
                comment.Id = Guid.NewGuid();
                dbContext.NewsComments.Add(comment);
                dbContext.SaveChanges();
                return comment;
            }
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id">Id комментария</param>
        public void DeleteNewsComment(Guid id)
        {
            using (var dbContext = new DataContext())
            {
                var comment = dbContext.NewsComments.Where(x => x.Id == id);

                dbContext.NewsComments.RemoveRange(comment);
                dbContext.SaveChanges();
            }
        }

        #endregion

        #region HashtagPost

        /// <summary>
        /// Получение записи промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="hashtagId">Id хэштега</param>
        /// <param name="postId">Id новости</param>
        /// <returns></returns>
        public HashtagNews GetHashtagNews(Guid hashtagId, Guid postId)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.HashtagNews.FirstOrDefault(x => x.HashtagId == hashtagId && x.NewsId == postId);
            }
        }

        /// <summary>
        /// Получение записей промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="postIds">Id новостей</param>
        /// <returns></returns>
        public ICollection<HashtagNews> GetHashtagNewsCollectionByNewsId(ICollection<Guid> postIds)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.HashtagNews.Where(x => postIds.Contains(x.NewsId)).ToList();
            }
        }

        /// <summary>
        /// Получение записей промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="hashtagIds">Id хэштегов</param>
        /// <returns></returns>
        public ICollection<HashtagNews> GetHashtagNewsCollectionByHashtagId(ICollection<Guid> hashtagIds)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.HashtagNews.Where(x => hashtagIds.Contains(x.HashtagId)).ToList();
            }
        }

        /// <summary>
        /// Создание записи промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="hashtagId">Id хэштега</param>
        /// <param name="postId">Id новости</param>
        /// <returns></returns>
        public HashtagNews CreateHashtagNewsEntity(Guid hashtagId, Guid postId)
        {
            using (var dbContext = new DataContext())
            {
                var hashtagPost = new HashtagNews() { Id = Guid.NewGuid(), HashtagId = hashtagId, NewsId = postId };
                dbContext.HashtagNews.Add(hashtagPost);
                dbContext.SaveChanges();
                return hashtagPost;
            }
        }

        /// <summary>
        /// Удаление записи промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="postId">Id новости</param>
        public void DeleteHashtagNews(Guid postId)
        {
            using (var dbContext = new DataContext())
            {
                var postHashtags = dbContext.HashtagNews.Where(x => x.NewsId == postId);

                dbContext.HashtagNews.RemoveRange(postHashtags);
                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Hashtag

        /// <summary>
        /// Получение коллекции хэштегов по коллекции Id
        /// </summary>
        /// <param name="hashtagIds">Id хэштегов</param>
        /// <returns></returns>
        public ICollection<Hashtag> GetHashtagsCollection(ICollection<Guid> hashtagIds)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Hashtags.Where(x => hashtagIds.Contains(x.Id)).ToList();
            }
        }

        /// <summary>
        /// Получение коллекции хэштегов по названиям хэштегов
        /// </summary>
        /// <param name="hashtagNames">Коллекция наименований</param>
        /// <returns></returns>
        public ICollection<Hashtag> GetHashtagsCollection(ICollection<string> hashtagNames)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Hashtags.Where(x => hashtagNames.Contains(x.Name)).ToList();
            }
        }

        /// <summary>
        /// Получение хэштега по названию
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <returns></returns>
        public Hashtag GetHashtag(string name)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Hashtags.FirstOrDefault(x => x.Name == name);
            }
        }

        /// <summary>
        /// Получение хэштега по Id
        /// </summary>
        /// <param name="id">Id хэштега</param>
        /// <returns></returns>
        public Hashtag GetHashtag(Guid id)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Hashtags.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Создание хэштега
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <returns></returns>
        public Hashtag CreateHashtag(string name)
        {
            using (var dbContext = new DataContext())
            {
                var hashtag = new Hashtag() { Id = Guid.NewGuid(), Name = name };
                dbContext.Hashtags.Add(hashtag);
                dbContext.SaveChanges();
                return hashtag;
            }
        }

        #endregion

        #region User

        /// <summary>
        /// Получить пользователя по Id
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        public Employee GetEmployee(Guid id)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Employees.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Получить список пользователей по имени и фамилии
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="lastname">Фамилия</param>
        /// <returns></returns>
        public ICollection<Employee> GetEmployees(string name, string lastname)
        {
            if (String.IsNullOrEmpty(name) && String.IsNullOrEmpty(lastname))
                return GetEmployees();
            
            using (var dbContext = new DataContext())
            {
                return dbContext.Employees.Where(x => (name != null ? x.Firstname.Contains(name) : true) && (lastname != null ? x.Surname.Contains(lastname) : true)).ToList();
            }
        }

        /// <summary>
        /// Получить полный список пользователей
        /// </summary>
        /// <returns></returns>
        public ICollection<Employee> GetEmployees()
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Employees.ToList();
            }
        }

        /// <summary>
        /// Получить список пользователей по коллекции Id
        /// </summary>
        /// <param name="ids">Id пользователей</param>
        /// <returns></returns>
        public ICollection<Employee> GetEmployees(ICollection<Guid> ids)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Employees.Where(x => ids.Contains(x.Id)).ToList(); ;
            }
        }

        #endregion
    }
}

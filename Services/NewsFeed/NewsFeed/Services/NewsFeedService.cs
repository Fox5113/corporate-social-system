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
        public ICollection<Post> GetPosts(string jsonData, string objectName)
        {
            if(objectName == "Mapping")
            {
                var mapping = JsonSerializer.Deserialize<Mapping>(jsonData);
                return GetPosts(mapping);
            }
            else if(objectName == "PostSearch")
            {
                var postSearch = JsonSerializer.Deserialize<PostSearch>(jsonData);
                return GetPosts(postSearch);
            }

            return null;
        }

        /// <summary>
        /// Получение коллекции Новостей по объекту Mapping
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <returns></returns>
        public ICollection<Post> GetPosts(Mapping mapping)
        {
            var script = new SqlScriptPreparerService().GetSelectQuery(mapping);
            using (var dbContext = new DataContext())
            {
                return dbContext.Posts.FromSqlRaw(script).ToList();
            }
        }

        /// <summary>
        /// Получение коллекции Новостей по объекту PostSearch
        /// </summary>
        /// <param name="post">PostSearch</param>
        /// <returns></returns>
        public ICollection<Post> GetPosts(PostSearch post)
        {
            using (var dbContext = new DataContext())
            {
                var query = dbContext.Posts
                    .Where(x => !String.IsNullOrEmpty(post.Title) ? x.Title.Contains(post.Title) : true)
                    .Where(x => !String.IsNullOrEmpty(post.Body) ? x.Body.Contains(post.Body) : true)
                    .Where(x => post.From != null ? x.CreatedAt >= (DateTime)post.From : true)
                    .Where(x => post.To != null ? x.CreatedAt <= (DateTime)post.To : true)
                    .Where(x => post.AuthorId != Guid.Empty ? x.AuthorId == post.AuthorId : true);
                
                if(post.Hashtags != null)
                {
                    var names = post.Hashtags.Select(x => x.Name).ToList();
                    var hashtagsFromDb = GetHashtagsCollection(names);
                    var hashtagsPosts = GetHashtagPostCollectionByHashtagId(hashtagsFromDb.Select(x => x.Id).ToList());
                    var postsIds = hashtagsPosts.Select(x => x.PostId).ToList();
                    query = query.Where(x => postsIds.Contains(x.Id));
                }
                
                return query.OrderByDescending(x => x.CreatedAt)
                    .Skip(post.Skip)
                    .Take(post.Take > 0 ? post.Take : 1000)
                    .ToList();
            }
        }

        /// <summary>
        /// Получение коллекции Новостей без фильтров
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ICollection<Post> GetPosts(int skip, int count)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Posts.OrderByDescending(e => e.CreatedAt).Skip(skip).Take(count).ToList();
            }
        }

        /// <summary>
        /// Получение коллекции Новостей по коллекции Id
        /// </summary>
        /// <param name="ids">Id новостей</param>
        /// <returns></returns>
        public ICollection<Post> GetPosts(ICollection<Guid> ids)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Posts.Where(x => ids.Contains(x.Id)).ToList();
            }
        }

        /// <summary>
        /// Получение Новости по Id
        /// </summary>
        /// <param name="id">Id новости</param>
        /// <returns></returns>
        public Post GetPost(Guid id)
        {
            using (var dbContext = new DataContext())
            {
                var post = dbContext.Posts.FirstOrDefault(x => x.Id == id);
                post.PostComments = GetComments(id);
                post.PostHashtags = GetHashtagPostCollectionByPostId(new List<Guid>() { id });

                return post;
            }
        }

        /// <summary>
        /// Создание Новости
        /// </summary>
        /// <param name="newPost">Новость</param>
        /// <param name="hashtags">Коллекция хэштегов</param>
        /// <returns></returns>
        public Post CreatePost(Post newPost, ICollection<Hashtag> hashtags = null)
        {
            using (var dbContext = new DataContext())
            {
                newPost.Id = Guid.NewGuid();
                dbContext.Posts.Add(newPost);
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

                        var hashtagPost = CreateHashtagPostEntity(hashtagDb.Id, newPost.Id);

                        if (newPost.PostHashtags == null)
                            newPost.PostHashtags = new List<HashtagPost>();
                        if (newPost.Hashtags == null)
                            newPost.Hashtags = new List<Hashtag>();

                        newPost.PostHashtags.Add(hashtagPost);
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
        public void DeletePost(Guid id)
        {
            using (var dbContext = new DataContext())
            {
                DeleteHashtagPost(id);
                var post = dbContext.Posts.FirstOrDefault(x => x.Id == id);

                dbContext.Posts.Remove(post);
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
        public ICollection<PostComment> GetComments(Guid postId)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.PostComments.Where(x => x.PostId == postId).ToList();
            }
        }

        /// <summary>
        /// Добавление комментария к новости
        /// </summary>
        /// <param name="comment">Комментарий</param>
        /// <returns></returns>
        public PostComment CreatePostComment(PostComment comment)
        {
            using (var dbContext = new DataContext())
            {
                comment.Id = Guid.NewGuid();
                dbContext.PostComments.Add(comment);
                dbContext.SaveChanges();
                return comment;
            }
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id">Id комментария</param>
        public void DeletePostComment(Guid id)
        {
            using (var dbContext = new DataContext())
            {
                var comment = dbContext.PostComments.Where(x => x.Id == id);

                dbContext.PostComments.RemoveRange(comment);
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
        public HashtagPost GetHashtagPost(Guid hashtagId, Guid postId)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.HashtagPost.FirstOrDefault(x => x.HashtagId == hashtagId && x.PostId == postId);
            }
        }

        /// <summary>
        /// Получение записей промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="postIds">Id новостей</param>
        /// <returns></returns>
        public ICollection<HashtagPost> GetHashtagPostCollectionByPostId(ICollection<Guid> postIds)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.HashtagPost.Where(x => postIds.Contains(x.PostId)).ToList();
            }
        }

        /// <summary>
        /// Получение записей промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="hashtagIds">Id хэштегов</param>
        /// <returns></returns>
        public ICollection<HashtagPost> GetHashtagPostCollectionByHashtagId(ICollection<Guid> hashtagIds)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.HashtagPost.Where(x => hashtagIds.Contains(x.HashtagId)).ToList();
            }
        }

        /// <summary>
        /// Создание записи промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="hashtagId">Id хэштега</param>
        /// <param name="postId">Id новости</param>
        /// <returns></returns>
        public HashtagPost CreateHashtagPostEntity(Guid hashtagId, Guid postId)
        {
            using (var dbContext = new DataContext())
            {
                var hashtagPost = new HashtagPost() { Id = Guid.NewGuid(), HashtagId = hashtagId, PostId = postId };
                dbContext.HashtagPost.Add(hashtagPost);
                dbContext.SaveChanges();
                return hashtagPost;
            }
        }

        /// <summary>
        /// Удаление записи промежуточной таблицы, связывающей хэштеги и новости
        /// </summary>
        /// <param name="postId">Id новости</param>
        public void DeleteHashtagPost(Guid postId)
        {
            using (var dbContext = new DataContext())
            {
                var postHashtags = dbContext.HashtagPost.Where(x => x.PostId == postId);

                dbContext.HashtagPost.RemoveRange(postHashtags);
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
        public User GetUser(Guid id)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Users.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Получить список пользователей по имени и фамилии
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="lastname">Фамилия</param>
        /// <returns></returns>
        public ICollection<User> GetUsers(string name, string lastname)
        {
            if (String.IsNullOrEmpty(name) && String.IsNullOrEmpty(lastname))
                return GetUsers();
            
            using (var dbContext = new DataContext())
            {
                return dbContext.Users.Where(x => (name != null ? x.Name.Contains(name) : true) && (lastname != null ? x.Lastname.Contains(lastname) : true)).ToList();
            }
        }

        /// <summary>
        /// Получить полный список пользователей
        /// </summary>
        /// <returns></returns>
        public ICollection<User> GetUsers()
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Users.ToList();
            }
        }

        /// <summary>
        /// Получить список пользователей по коллекции Id
        /// </summary>
        /// <param name="ids">Id пользователей</param>
        /// <returns></returns>
        public ICollection<User> GetUsers(ICollection<Guid> ids)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Users.Where(x => ids.Contains(x.Id)).ToList(); ;
            }
        }

        #endregion
    }
}

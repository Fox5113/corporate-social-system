﻿using DataAccess.Common;
using DataAccess.Entities;
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

        /// <summary>
        /// Удаление связанных сущностей.
        /// </summary>
        /// <param name="newsId"> Id новости </param>
        void DeleteRelatedEntities(Guid newsId);

        /// <summary>
        /// Получение коллекции Новостей по объекту NewsSearch
        /// </summary>
        /// <param name="newsSearch">Объект расширенного поиска новостей</param>
        /// <returns>Коллекция новостей</returns>
        Task<List<News>> GetCollection(NewsSearch newsSearch);

        /// <summary>
        /// Джоин связанных сущностей
        /// </summary>
        /// <param name="news">Список новостей</param>
        void JoinEntities(ICollection<News> news);

        /// <summary>
        /// Джоин сущности автора
        /// </summary>
        /// <param name="news">Новость</param>
        void JoinAuthor(News news);

        /// <summary>
        /// Зачистка ссылок для предотвращения зацикливания
        /// </summary>
        /// <param name="news">Новость</param>
        void ClearLinks(News news);

        /// <summary>
        /// Обновление списка хештегов новости
        /// </summary>
        /// <param name="oldList">Старый список</param>
        /// <param name="newList">Новый список</param>
        /// <returns>Новый список</returns>
        List<HashtagNews> GetNewHashtagNewsList(ICollection<HashtagNews> oldList, ICollection<HashtagNews> newList);

        /// <summary>
        /// Лайкнуть новость
        /// </summary>
        /// <param name="id">Id Новости</param>
        /// <param name="employeeId">Id Сотрудника</param>
        /// <returns></returns>
        Task Like(Guid id, Guid employeeId);

        /// <summary>
        /// Получить инфо по лайкам
        /// </summary>
        /// <param name="newsIds">Список новостей</param>
        /// <param name="currentEmployeeId">Пользователь, запросивший инфо</param>
        /// <returns>Инфо по лайкам с учетом лайков конкретного пользовател</returns>
        Task<ICollection<LikedNewsInfo>> GetLikes(ICollection<Guid> newsIds, Guid currentEmployeeId);
        Task<ICollection<News>> GetLikedNewsByEmployee(Guid employeeId, int page, int itemsPerPage);
    }
}

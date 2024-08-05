using DataAccess.Entities;
using Services.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstractions
{
    public interface IHashtagRepository : IRepository<Hashtag, Guid>
    {
        /// <summary>
        /// Получение коллекции хэштегов по названиям хэштегов
        /// </summary>
        /// <param name="hashtagNames">Коллекция наименований</param>
        /// <returns>Коллекция хештегов</returns>
        Task<List<Hashtag>> GetCollectionByNames(ICollection<string> hashtagNames);
    }
}

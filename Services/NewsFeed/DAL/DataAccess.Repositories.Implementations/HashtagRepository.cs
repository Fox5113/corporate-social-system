using DataAccess.Entities;
using DataAccess.EntityFramework;
using DataAccess.Repositories.Abstractions;
using Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class HashtagRepository : Repository<Hashtag, Guid>, IHashtagRepository
    {
        public HashtagRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// Получение коллекции хэштегов по названиям хэштегов
        /// </summary>
        /// <param name="hashtagNames">Коллекция наименований</param>
        /// <returns></returns>
        public async Task<List<Hashtag>> GetCollectionByNames(ICollection<string> hashtagNames)
        {
            var query = GetAll();
            return await query
                .Where(x => hashtagNames.Contains(x.Name))
                .ToListAsync();
        }
    }
}

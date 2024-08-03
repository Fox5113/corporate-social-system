using DataAccess.Entities;
using Services.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstractions
{
    public interface IHashtagRepository : IRepository<Hashtag, Guid>
    {
        Task<List<Hashtag>> GetCollectionByNames(ICollection<string> hashtagNames);
    }
}

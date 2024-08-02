using BusinessLogic.Contracts.HashtagNews;
using DataAccess.Entities;
using Services.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstractions
{
    public interface IHashtagNewsRepository : IRepository<HashtagNews, Guid>
    {
        Task<List<HashtagNews>> GetCollection(CreatingHashtagNewsDto hashtagNewsDto);
        Task<List<HashtagNews>> GetCollectionByNewsId(List<Guid> postIds);
        Task<List<HashtagNews>> GetCollectionByHashtagId(List<Guid> hashtagIds);
        void DeleteByNewsId(Guid newsId);
    }
}

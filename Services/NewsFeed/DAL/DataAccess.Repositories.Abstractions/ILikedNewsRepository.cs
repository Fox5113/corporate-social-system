using DataAccess.Entities;
using Services.Repositories.Abstractions;
using System;

namespace DataAccess.Repositories
{
    public interface ILikedNewsRepository : IRepository<LikedNews, Guid>
    {
    }
}

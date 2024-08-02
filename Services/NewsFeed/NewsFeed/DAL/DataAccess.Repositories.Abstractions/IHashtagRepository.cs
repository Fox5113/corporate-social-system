using DataAccess.Entities;
using Services.Repositories.Abstractions;
using System;

namespace DataAccess.Repositories.Abstractions
{
    public interface IHashtagRepository : IRepository<Hashtag, Guid>
    {
    }
}

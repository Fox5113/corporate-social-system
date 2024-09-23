using DataAccess.Entities;
using DataAccess.EntityFramework;
using Infrastructure.Repositories.Implementations;
using System;

namespace DataAccess.Repositories
{
    public class LikedNewsRepository : Repository<LikedNews, Guid>, ILikedNewsRepository
    {
        public LikedNewsRepository(DataContext context) : base(context)
        {
        }
    }
}

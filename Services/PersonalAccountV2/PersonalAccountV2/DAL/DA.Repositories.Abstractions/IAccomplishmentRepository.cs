using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Repositories.Abstractions
{
    public interface IAccomplishmentRepository : IRepository<Accomplishment, Guid>
    {

        Task<List<Accomplishment>> GetAllAccomplishmentEmployee(Guid employee);
        Task<Accomplishment> GetByIdAsync(Guid id);
        void CreateOrUpdateRange(List<Accomplishment> events);
    }
}

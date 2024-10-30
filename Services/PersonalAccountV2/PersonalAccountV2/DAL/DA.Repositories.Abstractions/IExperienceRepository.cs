using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Repositories.Abstractions
{
    public interface IExperienceRepository : IRepository<Experience, Guid>
    {

        Task<List<Experience>> GetAllExperienceEmployee(Guid employee);
        Task<Experience> GetByIdAsync(Guid id);
        Task<Guid> CreateOrUpdate(Experience experience);
    }
}

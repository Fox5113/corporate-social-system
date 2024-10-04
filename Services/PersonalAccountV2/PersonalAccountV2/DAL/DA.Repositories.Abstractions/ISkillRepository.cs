using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Repositories.Abstractions
{
    public interface ISkillRepository : IRepository<Skill, Guid>
    {

        Task<List<Skill>> GetAllSkillEmployee(Guid employee);
        Task<Skill> GetByIdAsync(Guid id);
        void CreateOrUpdateRange(List<Skill> events);
    }
}

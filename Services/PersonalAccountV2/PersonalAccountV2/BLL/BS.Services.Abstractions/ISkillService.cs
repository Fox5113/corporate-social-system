using BS.Contracts.Event;
using BS.Contracts.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Abstractions
{
    public interface ISkillService
    {
        Task<ICollection<SkillDto>> GetAllSkillEmployee(Guid employee);

        Task<SkillDto> GetByIdAsync(Guid id);

        public Task<Guid> CreateOrUpdate(SkillDto skillEmployee);
    }
}

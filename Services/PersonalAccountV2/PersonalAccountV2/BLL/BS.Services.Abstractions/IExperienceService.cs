using BS.Contracts.Experience;
using BS.Contracts.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Abstractions
{
    public interface IExperienceService
    {
        Task<ICollection<ExperienceDto>> GetAllExperienceEmployee(Guid employee);

        Task<ExperienceDto> GetByIdAsync(Guid id);

        public void CreateOrUpdate(ExperienceDto experienceEmployee);
    }
}

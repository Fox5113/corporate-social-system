using AutoMapper;
using BS.Contracts.Experience;
using BS.Contracts.Skill;
using BS.Services.Abstractions;
using DA.Entities;
using DA.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Implementations
{

    public class SkillService : ISkillService
    {
        private readonly IMapper _mapper;
        private readonly ISkillRepository _skillRepository;

        public SkillService(IMapper mapper, ISkillRepository skillRepository)
        {
            _mapper = mapper;
            _skillRepository = skillRepository;
        }

        public async Task<Guid> CreateOrUpdate(SkillDto skillEmployee)
        {
            var item = _mapper.Map<SkillDto, Skill>(skillEmployee);
            var id = await _skillRepository.CreateOrUpdate(item);
            await _skillRepository.SaveChangesAsync();
            return id;
        }

        public async Task<ICollection<SkillDto>> GetAllSkillEmployee(Guid employee)
        {
            var AllSkill = await _skillRepository.GetAllSkillEmployee(employee);
            return _mapper.Map<ICollection<Skill>, ICollection<SkillDto>>(AllSkill);
        }

        public async Task<SkillDto> GetByIdAsync(Guid id)
        {
            var _skill = await _skillRepository.GetAsync(id);
            return _mapper.Map<SkillDto>(_skill);
        }
    }
}

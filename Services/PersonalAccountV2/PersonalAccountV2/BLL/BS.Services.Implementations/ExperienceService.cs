using AutoMapper;
using BS.Contracts.Event;
using BS.Contracts.Experience;
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

    public class ExperienceService : IExperienceService
    {
        private readonly IMapper _mapper;
        private readonly IExperienceRepository _experienceRepository;

        public ExperienceService(IMapper mapper, IExperienceRepository experienceRepository)
        {
            _mapper = mapper;
            _experienceRepository = experienceRepository;
        }

        public async Task<Guid> CreateOrUpdate(ExperienceDto experienceEmployee)
        {
            var item = _mapper.Map<ExperienceDto, Experience>(experienceEmployee);
            var id = await _experienceRepository.CreateOrUpdate(item);
            await _experienceRepository.SaveChangesAsync();
            return id;
        }

        public async Task<ICollection<ExperienceDto>> GetAllExperienceEmployee(Guid employee)
        {
            var AllExperience = await _experienceRepository.GetAllExperienceEmployee(employee);
            return _mapper.Map<ICollection<Experience>, ICollection<ExperienceDto>>(AllExperience);
        }

        public  async Task<ExperienceDto> GetByIdAsync(Guid id)
        {
            var _event = await _experienceRepository.GetAsync(id);
            return _mapper.Map<ExperienceDto>(_event);
        }
    }
}

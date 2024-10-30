using AutoMapper;
using BS.Contracts.Accomplishment;
using BS.Contracts.Employee;
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
    public class AccomplishmentService : IAccomplishmentService
    {
        private readonly IMapper _mapper;
        private readonly IAccomplishmentRepository _accomplishmentRepository;

        public AccomplishmentService(IMapper mapper, IAccomplishmentRepository accomplishmentRepository)
        {
            _mapper = mapper;
            _accomplishmentRepository = accomplishmentRepository;
        }

        public async Task<Guid> CreateOrUpdate(AccomplishmentDto accomplishmentEmployee)
        {
            var item = _mapper.Map<AccomplishmentDto, Accomplishment>(accomplishmentEmployee);
            var id = await _accomplishmentRepository.CreateOrUpdate(item);
            await _accomplishmentRepository.SaveChangesAsync();
            return id;
        }

        public async Task<ICollection<AccomplishmentDto>> GetAllAccomplishmentEmployee(Guid employee)
        {
            var AllAccomplishment = await _accomplishmentRepository.GetAllAccomplishmentEmployee(employee);
            return _mapper.Map<ICollection<Accomplishment>, ICollection<AccomplishmentDto>>(AllAccomplishment);
        }

        public async Task<AccomplishmentDto> GetByIdAsync(Guid id)
        {
            var Employee = await _accomplishmentRepository.GetAsync(id);
            return _mapper.Map<AccomplishmentDto>(Employee);
        }
    }
}

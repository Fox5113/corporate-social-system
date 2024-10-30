using AutoMapper;
using BS.Contracts.Communication;
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
    public class CommunicationService : ICommunicationService
    {
        private readonly IMapper _mapper;
        private readonly ICommunicationRepository _communicationRepository;

        public CommunicationService(IMapper mapper, ICommunicationRepository communicationRepository)
        {
            _mapper = mapper;
            _communicationRepository = communicationRepository;
        }

        public async Task<Guid> CreateOrUpdate(CommunicationDto communicationEmployee)
        {
            var item = _mapper.Map<CommunicationDto, Communication>(communicationEmployee);
            var id = await _communicationRepository.CreateOrUpdate(item);
            await _communicationRepository.SaveChangesAsync();
            return id;
        }

        public async Task<ICollection<CommunicationDto>> GetAllCommunicationEmployee(Guid employee)
        {
            var AllCommunication = await _communicationRepository.GetAllCommunicationEmployee(employee);
            return _mapper.Map<ICollection<Communication>, ICollection<CommunicationDto>>(AllCommunication);
        }

        public async  Task<CommunicationDto> GetByIdAsync(Guid id)
        {
            var _communication = await _communicationRepository.GetAsync(id);
            return _mapper.Map<CommunicationDto>(_communication);
        }
    }
}

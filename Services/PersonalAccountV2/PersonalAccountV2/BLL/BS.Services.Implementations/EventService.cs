using AutoMapper;
using BS.Contracts.Accomplishment;
using BS.Contracts.Event;
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
    public class EventService : IEventService
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;

        public EventService(IMapper mapper, IEventRepository eventRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
        }

        public async Task<Guid> CreateOrUpdate(EventDto eventEmployee)
        {
            var item = _mapper.Map<EventDto, Event>(eventEmployee);
            var id = await _eventRepository.CreateOrUpdate(item);
            await _eventRepository.SaveChangesAsync();
            return id;
        }

        public async Task<ICollection<EventDto>> GetAllEventEmployee(Guid employee)
        {
            var AllEvent = await _eventRepository.GetAllEventEmployee(employee);
            return _mapper.Map<ICollection<Event>, ICollection<EventDto>>(AllEvent);
        }

        public async Task<EventDto> GetByIdAsync(Guid id)
        {
            var _event = await _eventRepository.GetAsync(id);
            return _mapper.Map<EventDto>(_event);
        }
    }
}

using AutoMapper;
using BS.Contracts.Employee;
using BS.Contracts.Event;
using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Implementations.Mapping
{

    public class EventMappingsProfile : Profile
    {
        public EventMappingsProfile()
        {
            CreateMap<Event, EventDto>();

            CreateMap<EventDto, Event>();
        }
    }
}

using AutoMapper;
using BS.Contracts.Communication;
using BS.Contracts.Event;
using PersonalAccountV2.Models.Communication;
using PersonalAccountV2.Models.Event;

namespace PersonalAccountV2.Mapping
{

    public class EventMappingsProfile : Profile
    {
        public EventMappingsProfile()
        {
            CreateMap<EventDto, EventModel>();
        }
    }
}

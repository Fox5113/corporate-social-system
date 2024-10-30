using BS.Contracts.Employee;
using BS.Contracts.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Abstractions
{
    public interface IEventService
    {
        Task<ICollection<EventDto>> GetAllEventEmployee(Guid employee);

        Task<EventDto> GetByIdAsync(Guid id);

        public Task<Guid> CreateOrUpdate(EventDto eventEmployee);
    }
}

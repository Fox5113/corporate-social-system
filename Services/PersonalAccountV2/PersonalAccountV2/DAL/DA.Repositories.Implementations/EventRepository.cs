using DA.Context;
using DA.Entities;
using DA.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Repositories.Implementations
{
    public class EventRepository : Repository<Event, Guid>, IEventRepository
    {

        public EventRepository(DataContext context) : base(context)
        {
        }
        public async Task<Guid> CreateOrUpdate(Event _event)
        {
                var even = Get(_event.Id);

                if (even != null)
                {
                    even.IsAcrive = _event.IsAcrive;
                    even.UpdatedAt = DateTime.Now;
                    Update(even);
                    return even.Id;
                }
                else
                {
                    return Add(_event).Id;
                }
            
        }

        public async Task<List<Event>> GetAllEventEmployee(Guid employee)
        {
            var query = GetAll();
            return await query.Where(x => x.EmployeeId == employee).ToListAsync<Event>();
        }

        public async Task<Event> GetByIdAsync(Guid id)
        {
            var query = GetAll();
            return (await query.Where(x => x.Id == id).ToListAsync<Event>()).FirstOrDefault();
        }
    }
}

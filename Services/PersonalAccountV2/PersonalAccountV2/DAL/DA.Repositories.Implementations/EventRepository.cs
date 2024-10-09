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
        public void CreateOrUpdateRange(List<Event> events)
        {
            foreach (var item in events)
            {
                var even = Get(item.Id);

                if (even != null)
                {
                    even.IsAcrive = item.IsAcrive;
                    even.UpdatedAt = DateTime.Now;
                    Update(even);
                }
                else
                {
                    Add(item);
                }
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

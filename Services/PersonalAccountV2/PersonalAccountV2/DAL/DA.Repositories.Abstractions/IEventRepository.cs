using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Repositories.Abstractions
{
    public interface IEventRepository : IRepository<Event, Guid>
    {

        Task<List<Event>> GetAllEventEmployee(Guid employee);
        Task<Event> GetByIdAsync(Guid id);
        void CreateOrUpdateRange(List<Event> events);
    }
}

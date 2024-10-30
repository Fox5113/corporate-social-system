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
    public class CommunicationRepository : Repository<Communication, Guid>, ICommunicationRepository
    {
        public CommunicationRepository(DataContext context) : base(context)
        {
        }
        public async Task<Guid> CreateOrUpdate(Communication communication)
        {
            var com = Get(communication.Id);

            if (com != null)
            {
                com.Value = communication.Value;
                com.UpdatedAt = DateTime.Now;
                Update(com);
                return com.Id;
            }
            else
            {
                return Add(communication).Id;
            }
        }

        public async Task<List<Communication>> GetAllCommunicationEmployee(Guid employee)
        {
            var query = GetAll();
            return await query.Where(x => x.EmployeeId == employee).ToListAsync<Communication>();
        }

        public async Task<Communication> GetByIdAsync(Guid id)
        {
            var query = GetAll();
            return (await query.Where(x => x.Id == id).ToListAsync<Communication>()).FirstOrDefault();
        }
    }
}

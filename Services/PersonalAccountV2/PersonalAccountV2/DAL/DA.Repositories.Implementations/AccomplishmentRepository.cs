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
    public class AccomplishmentRepository : Repository<Accomplishment, Guid>, IAccomplishmentRepository
    {
        public AccomplishmentRepository(DataContext context) : base(context)
        {
        }

        public async Task<List<Accomplishment>> GetAllAccomplishmentEmployee(Guid employee)
        {
            var query = GetAll();
            return await query.Where(x => x.EmployeeId == employee).ToListAsync<Accomplishment>();
        }

        public async Task<Accomplishment> GetByIdAsync(Guid id)
        {
            var query = GetAll();
            return (await query.Where(x => x.Id == id).ToListAsync<Accomplishment>()).FirstOrDefault();
        }

        public async Task<Guid> CreateOrUpdate(Accomplishment accomplishment)
        {
                var acc = Get(accomplishment.Id);

                if (acc != null)
                {
                    acc.Description = accomplishment.Description;
                    acc.Date = accomplishment.Date;
                    acc.UpdatedAt = DateTime.Now;
                    Update(acc);
                    return acc.Id;
                }
                else
                {
                    return Add(accomplishment).Id;
                
                }
        }
    }
}

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
    public class SkillRepository : Repository<Skill, Guid>, ISkillRepository
    {
        public SkillRepository(DataContext context) : base(context)
        {
        }
        public void CreateOrUpdateRange(List<Skill> skills)
        {
            foreach (var item in skills)
            {
                var sk = Get(item.Id);

                if (sk != null)
                {
                    sk.Description = item.Description;
                    sk.UpdatedAt = DateTime.Now;
                    Update(sk);
                }
                else
                {
                    Add(item);
                }
            }
        }

        public async Task<List<Skill>> GetAllSkillEmployee(Guid employee)
        {
            var query = GetAll();
            return await query.Where(x => x.EmployeeId == employee).ToListAsync<Skill>();
        }

        public Task<Skill> GetByIdAsync(Guid id)
        {
            var query = GetAll();
            return (await query.Where(x => x.Id == id).ToListAsync<Skill>()).FirstOrDefault();
        }
    }
}

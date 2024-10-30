using DA.Context;
using DA.Entities;
using DA.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Repositories.Implementations
{
    public class ExperienceRepository : Repository<Experience, Guid>, IExperienceRepository
    {
        public ExperienceRepository(DataContext context) : base(context)
        {
        }

        public async Task<Guid> CreateOrUpdate(Experience experience)
        {
            var exp = Get(experience.Id);
            if (exp != null)
            {
                exp.Company = experience.Company;
                exp.EmployementDate = experience.EmployementDate;
                exp.DescriptionWork = experience.DescriptionWork;
                exp.DismissalDate = experience.DismissalDate;
                exp.DescriptionCompany = experience.DescriptionCompany;
                exp.DescriptionWork = experience.DescriptionWork.ToString();
                exp.UpdatedAt = DateTime.Now;
                Update(exp);
                return exp.Id;
            }
            else
            {
                return Add(experience).Id;
            }
        }

        public async Task<List<Experience>> GetAllExperienceEmployee(Guid employee)
        {
            var query = GetAll();
            return await query.Where(x => x.EmployeeId == employee).ToListAsync<Experience>();
        }

        public async Task<Experience> GetByIdAsync(Guid id)
        {
            var query = GetAll();
            return (await query.Where(x => x.Id == id).ToListAsync<Experience>()).FirstOrDefault();
        }
    }
}

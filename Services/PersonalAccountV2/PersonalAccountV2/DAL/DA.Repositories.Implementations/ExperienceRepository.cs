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

        public void CreateOrUpdateRange(List<Experience> experiences)
        {
            foreach (var item in experiences)
            {
                var exp = Get(item.Id);

                if (exp != null)
                {
                    exp.Company = item.Company;
                    exp.EmployementDate = item.EmployementDate;
                    exp.DescriptionWork = item.DescriptionWork;
                    exp.DismissalDate = item.DismissalDate;
                    exp.DescriptionCompany = item.DescriptionCompany;
                    exp.DescriptionWork = item.DescriptionWork.ToString();
                    exp.UpdatedAt = DateTime.Now;
                    Update(exp);
                }
                else
                {
                    Add(item);
                }
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

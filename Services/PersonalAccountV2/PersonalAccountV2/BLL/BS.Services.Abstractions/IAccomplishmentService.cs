using BS.Contracts.Accomplishment;
using BS.Contracts.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Abstractions
{

    public interface IAccomplishmentService
    {
        Task<ICollection<AccomplishmentDto>> GetAllAccomplishmentEmployee(Guid employee);

        Task<AccomplishmentDto> GetByIdAsync(Guid id);

        public Task<Guid> CreateOrUpdate(AccomplishmentDto accomplishmentEmployee);
    }
}

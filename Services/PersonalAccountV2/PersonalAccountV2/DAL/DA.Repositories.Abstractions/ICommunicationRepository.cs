using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Repositories.Abstractions
{
    public interface ICommunicationRepository : IRepository<Communication, Guid>
    {

        Task<List<Communication>> GetAllCommunicationEmployee(Guid employee);
        Task<Communication> GetByIdAsync(Guid id);
        void CreateOrUpdateRange(List<Communication> communications);
    }
}

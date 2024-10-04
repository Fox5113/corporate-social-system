using BS.Contracts.Communication;
using BS.Contracts.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.Abstractions
{
    public interface ICommunicationService
    {
        Task<ICollection<CommunicationDto>> GetAllCommunicationEmployee(Guid employee);

        Task<CommunicationDto> GetByIdAsync(Guid id);

        public void CreateOrUpdate(CommunicationDto communicationEmployee);
    }
}

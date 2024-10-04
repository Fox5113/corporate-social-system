using BS.Contracts.Base;
using BS.Contracts.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Contracts.Communication
{
    public class UpdatingCommunicationDto
    {
        public string Value { get; set; }
        public Guid EmployeeId { get; set; }
    }
}

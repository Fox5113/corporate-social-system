using BS.Contracts.Base;
using BS.Contracts.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Contracts.Accomplishment
{
    public class UpdatingAccomplishmentDto
    {
        public int Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Guid EmployeeId { get; set; }
    }
}

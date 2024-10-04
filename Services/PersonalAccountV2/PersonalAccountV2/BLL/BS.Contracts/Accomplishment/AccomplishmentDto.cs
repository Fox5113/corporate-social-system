using BS.Contracts.Base;
using BS.Contracts.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Contracts.Accomplishment
{
    public class AccomplishmentDto
    {
        public Guid Id { get; set; }
        public TypeAccomplishment Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public EmployeeDto Employee { get; set; }
        public Guid EmployeeId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Contracts.Employee
{
    class EmployeeFilterDto
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public string MainEmail { get; set; }
        public string MainTelephoneNumber { get; set; }
        public string About { get; set; }
        public DateTime Birthdate { get; set; }
        public string OfficeAddress { get; set; }
    }
}

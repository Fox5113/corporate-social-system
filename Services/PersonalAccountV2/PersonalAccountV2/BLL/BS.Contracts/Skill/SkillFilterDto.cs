using BS.Contracts.Base;
using BS.Contracts.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Contracts.Skill
{
    public class SkillFilterDto
    {
        public int Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public Guid EmployeeId { get; set; }
    }
}

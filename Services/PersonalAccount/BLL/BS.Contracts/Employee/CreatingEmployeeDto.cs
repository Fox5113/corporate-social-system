using BS.Contracts.Accomplishment;
using BS.Contracts.Communication;
using BS.Contracts.Experience;
using BS.Contracts.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Contracts.Employee
{
    internal class CreatingEmployeeDto
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public string MainEmail { get; set; }
        public string MainTelephoneNumber { get; set; }
        public string About { get; set; }
        public DateTime Birthdate { get; set; }
        public string OfficeAddress { get; set; }
        public DateTime EmploymentDate { get; set; }
        public List<AccomplishmentDto> AccomplishmentsList { get; set; }
        public List<SkillDto> SkillsList { get; set; }
        public List<CommunicationDto> CommunicationsList { get; set; }
        public List<ExperienceDto> ExperienceList { get; set; }
}
}

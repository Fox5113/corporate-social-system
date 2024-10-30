using BS.Contracts.Accomplishment;
using BS.Contracts.Communication;
using BS.Contracts.Event;
using BS.Contracts.Experience;
using BS.Contracts.Skill;
using PersonalAccountV2.Models.Accomplishment;
using PersonalAccountV2.Models.Communication;
using PersonalAccountV2.Models.Event;
using PersonalAccountV2.Models.Experience;
using PersonalAccountV2.Models.Skill;

namespace PersonalAccountV2.Models.Employee
{
    public class EmployeeModel
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
        public List<AccomplishmentModel> AccomplishmentsList { get; set; }
        public List<SkillModel> SkillsList { get; set; }
        public List<CommunicationModel> CommunicationsList { get; set; }
        public List<ExperienceModel> ExperienceList { get; set; }
        public List<EventModel> EventList { get; set; }
    }
}

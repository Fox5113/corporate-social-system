using BS.Contracts.Base;
using BS.Contracts.Employee;
using PersonalAccountV2.Models.Employee;

namespace PersonalAccountV2.Models.Communication
{
    public class CommunicationModel //SkillModel
    {
        public Guid Id { get; set; }
        public TypeCommunication Type { get; set; }
        public string Value { get; set; }
        public EmployeeModel Employee { get; set; }
        public Guid EmployeeId { get; set; }
    }
}

namespace PersonalAccountV2.Models.Employee
{
    public class EmployeeFilterModel
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public string MainEmail { get; set; }
        public string MainTelephoneNumber { get; set; }
        public string About { get; set; }
        public DateTime Birthdate { get; set; }
        public string OfficeAddress { get; set; }
        public DateTime EmploymentDate { get; set; }
    }
}

﻿namespace WebApi.Models.Employee
{
    public class EmployeeFilterModel
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public int ItemsPerPage { get; set; }
        public int Page { get; set; }
    }
}

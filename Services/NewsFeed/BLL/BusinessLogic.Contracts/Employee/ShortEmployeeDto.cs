﻿using System;

namespace BusinessLogic.Contracts.Employee
{
    public class ShortEmployeeDto
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
    }
}

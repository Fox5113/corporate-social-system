﻿using BS.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Entities
{
    public class Skill : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public virtual Employee Employee { get; set; }
        public Guid EmployeeId { get; set; }

    }
}

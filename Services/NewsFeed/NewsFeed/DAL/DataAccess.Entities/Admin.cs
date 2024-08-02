using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Admin : Employee
    {
        public bool Publish { get; }
        public bool CancelPublish { get; }
        public bool Delete { get; }
        public bool Archive { get; }
    }
}

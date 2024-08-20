using BS.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Contracts.Accomplishment
{
    class AccomplishmentDto
    {
        public Guid Id { get; set; }
        public TypeAccomplishment Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}

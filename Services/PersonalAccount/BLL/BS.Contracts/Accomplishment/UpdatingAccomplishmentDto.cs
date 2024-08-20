using BS.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Contracts.Accomplishment
{
    internal class UpdatingAccomplishmentDto
    {
        public int Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Contracts.Experience
{
    class ExperienceFilterDto
    {
        public string Company { get; set; }
        public DateTime EmployementDate { get; set; }
        public DateTime DismissalDate { get; set; }
        public string DescriptionWork { get; set; }
        public string DescriptionCompany { get; set; }
    }
}

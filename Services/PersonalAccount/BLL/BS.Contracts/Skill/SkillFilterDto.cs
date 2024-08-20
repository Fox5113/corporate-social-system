using BS.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Contracts.Skill
{
    internal class SkillFilterDto
    {
        public int Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NewsFeed.Enums.Common;

namespace NewsFeed.Models
{
    public class Group
    {
        public ICollection<Field> Fields { get; set; }
        public ICollection<Group> Groups { get; set; }
        public LogicalOperationStrict Operation { get; set; }
    }
}

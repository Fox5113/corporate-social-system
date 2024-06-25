using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NewsFeed.Enums.Common;

namespace NewsFeed.Models
{
    public class Mapping
    {
        public ICollection<Field> Fields { get; set; }
        public ICollection<Mapping> Groups { get; set; }
        public LogicalOperationStrict Operation { get; set; }
    }
}

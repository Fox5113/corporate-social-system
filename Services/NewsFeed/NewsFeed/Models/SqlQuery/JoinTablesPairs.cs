using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NewsFeed.Enums.Common;

namespace NewsFeed.Models
{
    public class JoinTablesPairs
    {
        public string FirstTableName { get; set; }
        public string FirstTableColumnName { get; set; }
        public string SecondTableName { get; set; }
        public string SecondTableColumnName { get; set; }
        public FilterComparisonType ComparisonType { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NewsFeed.Common.Enums;

namespace NewsFeed.Common
{
    public class JoinTables
    {
        public ICollection<JoinTablesPairs> TablePairs { get; set; }
        public JoinType JoinType { get; set; }
        public LogicalOperationStrict Operation { get; set; }
        public string JoiningTableName { get; set; }
    }
}

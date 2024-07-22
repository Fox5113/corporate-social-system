using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NewsFeed.Common.Enums;

namespace NewsFeed.Common
{
    public class OrderBySqlQuery
    {
        public string TableName { get; set; }
        public  string ColumnName { get; set; }
        public OrderBy OrderBy { get; set; }
    }
}

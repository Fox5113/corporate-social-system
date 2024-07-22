using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NewsFeed.Common.Enums;

namespace NewsFeed.Common
{
    public class FieldFilter
    {
        public string Name { get; set; }
        public ICollection<object> Data { get; set; }
        public string DataType { get; set; }
        public int ComparisonType { get; set; }
        public string TableName { get; set; }
    }
}

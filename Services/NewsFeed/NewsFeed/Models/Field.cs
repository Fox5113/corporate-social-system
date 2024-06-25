using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NewsFeed.Enums.Common;

namespace NewsFeed.Models
{
    public class Field
    {
        public string Name { get; set; }
        public ICollection<object> Data { get; set; }
        public string DataType { get; set; }
        public FilterComparisonType ComparisonType { get; set; }
        public string TableName { get; set; }
    }
}

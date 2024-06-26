using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class SelectQuery
    {
        public string Columns { get; set; }
        public string MainTable { get; set; }
        public string Joins { get; set; }
        public string Filters { get; set; }
        public string OrderBy { get; set; }
        public string GroupBy { get; set; }
    }
}

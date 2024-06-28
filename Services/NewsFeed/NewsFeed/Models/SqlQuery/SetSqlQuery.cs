using NewsFeed.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class SetSqlQuery
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public object Value { get; set; }
    }
}

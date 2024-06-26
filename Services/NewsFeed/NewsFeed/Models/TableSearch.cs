using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class TableSearch
    {
        public ICollection<TableWithFieldsNames> Tables { get; set; }
        public ICollection<JoinTables> Joins { get; set; }
        public Mapping Mapping { get; set; }
        public ICollection<OrderBySqlQuery> OrderBy { get; set; }
        public string MainTableName { get; set; }
    }
}

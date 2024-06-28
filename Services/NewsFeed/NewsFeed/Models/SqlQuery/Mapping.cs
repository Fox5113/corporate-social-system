using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class Mapping
    {
        public ICollection<TableWithFieldsNames> Tables { get; set; }
        public ICollection<JoinTables> Joins { get; set; }
        public Group Group { get; set; }
        public ICollection<OrderBySqlQuery> OrderBy { get; set; }
        public ICollection<SetSqlQuery> Sets { get; set; }
        public int Offset { get; set; }
        public int Fetch { get; set; }
        public string MainTableName { get; set; }
    }
}

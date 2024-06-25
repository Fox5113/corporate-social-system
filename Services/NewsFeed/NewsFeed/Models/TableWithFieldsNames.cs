using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class TableWithFieldsNames
    {
        public string TableName { get; set; }
        public ICollection<string> Fields { get; set; }
    }
}

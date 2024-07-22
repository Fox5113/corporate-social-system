using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NewsFeed.Common.Enums;

namespace NewsFeed.Common
{
    public class TableFilter
    {
        public ICollection<FieldFilter> FieldsFilter { get; set; }
        public ICollection<TableFilter> InnerTableFilter { get; set; }
        public int Operation { get; set; }
    }
}

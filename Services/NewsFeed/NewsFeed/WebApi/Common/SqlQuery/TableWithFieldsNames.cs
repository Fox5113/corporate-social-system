using System.Collections.Generic;

namespace NewsFeed.WebApi.Common.SqlQuery
{
    public class TableWithFieldsNames
    {
        public string TableName { get; set; }
        public ICollection<string> Fields { get; set; }
    }
}

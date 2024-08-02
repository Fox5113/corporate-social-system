using static NewsFeed.WebApi.Common.Enums;

namespace NewsFeed.WebApi.Common.SqlQuery
{
    public class OrderBySqlQuery
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public OrderBy OrderBy { get; set; }
    }
}

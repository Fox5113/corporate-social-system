using NewsFeed.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class UpdateQuery : QueryBase
    {
        private readonly string _queryType = "UPDATE";
        private readonly string _command = "SET";
        public string ColumnsWithValues { get; set; }
        public string QueryType { get { return _queryType; } }
        public string Command { get { return _command; } }

        public UpdateQuery(string mainTableName)
        {
            MainTable = mainTableName;
        }

        public override string PrepareSqlString()
        {
			var newQuery = new List<string>();
			newQuery.Add(QueryType);
            newQuery.Add(MainTable);
            newQuery.Add(Command);
            newQuery.Add(ColumnsWithValues);
			if (!String.IsNullOrEmpty(Filters.Trim()))
			{
				newQuery.Add(Where);
				newQuery.Add(Filters);
			}

			return String.Join(" ", newQuery);
		}
    }
}

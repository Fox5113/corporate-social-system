using NewsFeed.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class SelectQuery : QueryBase
    {
        private readonly string _queryType = "SELECT";
		private readonly string _orderBy = "ORDER BY";
		public string Columns { get; set; }
        public string Joins { get; set; }
        public string OrdersBy { get; set; }
        public string GroupBy { get; set; }
        public string QueryType { get { return _queryType; } }
		public string OrderByStartString { get { return _orderBy; } }
		public int Offset { get; set; }
		public int Fetch { get; set; }

		public SelectQuery(string mainTableName)
        {
			MainTable = mainTableName;
        }

		/// <summary>
		/// Создание SQL Запроса
		/// </summary>
		/// <returns>Строка sql</returns>
		public override string PrepareSqlString()
		{
			var newQuery = new List<string>();
			newQuery.Add(QueryType);
			newQuery.Add(Columns);
			newQuery.Add(From);
			newQuery.Add(MainTable);
			if (!String.IsNullOrEmpty(Joins.Trim()))
				newQuery.Add(Joins);
			if (!String.IsNullOrEmpty(Filters.Trim()))
			{
				newQuery.Add(Where);
				newQuery.Add(Filters);
			}
			if (!String.IsNullOrEmpty(OrdersBy.Trim()))
			{
				newQuery.Add(OrderByStartString);
				newQuery.Add(OrdersBy);
			}
			if(Offset > 0)
            {
				newQuery.Add($"OFFSET {Offset} ROWS");
			}
			if (Fetch > 0)
			{
				newQuery.Add($"FETCH NEXT {Fetch} ROWS ONLY");
			}

			return String.Join(" ", newQuery);
		}
	}
}

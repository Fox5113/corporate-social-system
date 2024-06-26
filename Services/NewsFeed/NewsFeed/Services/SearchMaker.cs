using NewsFeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NewsFeed.Enums.Common;

namespace NewsFeed.Services
{
    public class SearchMaker
    {
		public string GetQuery(TableSearch tableSearch)
        {
			var selectQuery = new SelectQuery();
			selectQuery.Columns = GetColumns(tableSearch.Tables);
			selectQuery.Joins = GetJoins(tableSearch.Joins);
			selectQuery.Filters = GetFilters(tableSearch.Mapping);
			//нужно допилить order by и может group by

			return PrepareSqlString(selectQuery);
        }

		public string PrepareSqlString(SelectQuery selectQuery)
        {
			var newQuery = new List<string>();
			newQuery.Add("SELECT ");
			newQuery.Add(" FROM " + selectQuery.MainTable);
			if (!String.IsNullOrEmpty(selectQuery.Joins.Trim()))
				newQuery.Add(" \n" + selectQuery.Joins + "\n ");
			if (!String.IsNullOrEmpty(selectQuery.Filters.Trim()))
				newQuery.Add(" WHERE " + selectQuery.Filters);

			return newQuery.ToString();
		}

		private string GetColumns(ICollection<TableWithFieldsNames> tables)
        {
			var columns = new List<string>();
			foreach (var table in tables)
			{
				if (table.Fields == null)
				{
					columns.Add(" * ");
				}
				else
				{
					foreach (var column in table.Fields)
					{
						columns.Add(table.TableName + '.' + column);
					}
				}
			}

			return String.Join(", ", columns);
		}

		private string GetJoins(ICollection<JoinTables> joinTables)
        {
			var joins = new List<string>();
			foreach(var join in joinTables)
            {
				var joinStr = new List<string>();
				foreach(var pair in join.TablePairs)
                {
					joinStr.Add(pair.FirstTableName + "." + pair.FirstTableColumnName);
					joinStr.Add(GetOperation(pair.ComparisonType));
					joinStr.Add(pair.SecondTableName + "." + pair.SecondTableColumnName);
				}

				joins.Add(join.JoinType.ToString() + " JOIN ON " + String.Join(" " + join.Operation.ToString() + " ", joinStr));
			}

			return String.Join("\n ", joins);
        }

		private string GetFilters(Mapping map)
		{
			var newGroup = new List<string>();
			var operation = ' ' + map.Operation.ToString() + ' ';
			if (map.Groups != null)
			{
				foreach (var mapping in map.Groups)
				{
					newGroup.Add(GetFilters(mapping));
				}
			}
			foreach (var field in map.Fields)
			{
				var tableName = field.TableName;
				if (field.Data != null && field.Data.Count > 0)
                {
					if (field.ComparisonType == FilterComparisonType.Equal)
					{
						if(field.Data.Count == 1)
							newGroup.Add(tableName + '.' + field.Name + GetOperation(field.ComparisonType) + field.Data.First().ToString());
						else
                        {
							var values = String.Join(", ", field.Data.Select(x => "\'" + x.ToString() + "\'").ToArray());
							newGroup.Add(tableName + '.' + field.Name + " in (" + values + ") ");
						}
					}
					else if (field.ComparisonType == FilterComparisonType.Contain)
                    {
						newGroup.Add(tableName + '.' + field.Name + " like \'%" + field.Data.First().ToString() + "\'%");
					}
					else if (field.ComparisonType == FilterComparisonType.Between)
					{
						if(field.Data.Count == 2)
                        {
							newGroup.Add(tableName + '.' + field.Name + " >= \'" + field.Data.First().ToString() + "\'");
							newGroup.Add(tableName + '.' + field.Name + " <= \'" + field.Data.Last().ToString() + "\'");
						}
					}
					else if (field.ComparisonType == FilterComparisonType.Greater ||
							field.ComparisonType == FilterComparisonType.GreaterOrEqual ||
							field.ComparisonType == FilterComparisonType.Less ||
							field.ComparisonType == FilterComparisonType.LessOrEqual)
					{
						newGroup.Add(tableName + '.' + field.Name + GetOperation(field.ComparisonType) + "\'" + field.Data.First().ToString() + "\'");
					}
					else if (field.ComparisonType == FilterComparisonType.NotEqual)
					{
						if (field.Data.Count == 1)
							newGroup.Add(tableName + '.' + field.Name + GetOperation(field.ComparisonType) + "\'" + field.Data.First().ToString() + "\'");
						else
						{
							var values = String.Join(", ", field.Data.Select(x => "\'" + x.ToString() + "\'").ToArray());
							newGroup.Add(tableName + '.' + field.Name + " not in (" + values + ") ");
						}
					}
				}
				else
                {
					if (field.ComparisonType == FilterComparisonType.IsNull || field.ComparisonType == FilterComparisonType.IsNotNull)
					{
						newGroup.Add(tableName + '.' + field.Name + GetOperation(field.ComparisonType));
					}
				}
			}
			
			return '(' + String.Join(operation, newGroup) + ')';
		}

		private string GetOperation(FilterComparisonType type)
        {
			if (type == FilterComparisonType.Equal)
			{
				return " = ";
			}
			if (type == FilterComparisonType.NotEqual)
			{
				return " != ";
			}
			if (type == FilterComparisonType.Less)
			{
				return " < ";
			}
			if (type == FilterComparisonType.Greater)
			{
				return " > ";
			}
			if (type == FilterComparisonType.LessOrEqual)
			{
				return " <= ";
			}
			if (type == FilterComparisonType.GreaterOrEqual)
			{
				return " >= ";
			}
			if (type == FilterComparisonType.IsNotNull)
			{
				return " IS NOT NULL ";
			}
			if (type == FilterComparisonType.IsNull)
			{
				return " IS NULL ";
			}

			return null;
		}

	}
}

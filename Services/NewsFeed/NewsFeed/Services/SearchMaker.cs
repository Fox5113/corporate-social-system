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
			var newQuery = new List<string>();
			var columns = new List<string>();
			var filters = "";
			var joins = "";

			foreach(var table in tableSearch.Tables)
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

			if(tableSearch.Joins != null)
            {
				joins = GetJoins(tableSearch.Joins);
            }

			if(tableSearch.Mapping != null)
            {
				filters = GetFilters(tableSearch.Mapping);
			}

			newQuery.Add("SELECT ");
			newQuery.Add(String.Join(", ", columns));
			newQuery.Add(" FROM " + tableSearch.MainTableName);
			if (!String.IsNullOrEmpty(joins.Trim()))
				newQuery.Add(" \n" + joins + "\n ");
			if (!String.IsNullOrEmpty(filters.Trim()))
				newQuery.Add(" WHERE " + filters);

			return newQuery.ToString();
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
				
				var val = join.JoinType.ToString() + "JOIN ON " + String.Join(join.JoinType.ToString(), joinStr);
				joins.Add(val);
			}

			return String.Join("\n", joins);
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
						newGroup.Add(tableName + '.' + field.Name + GetOperation(field.ComparisonType) + " \'" + field.Data.First().ToString() + "\'");
					}
					else if (field.ComparisonType == FilterComparisonType.NotEqual)
					{
						if (field.Data.Count == 1)
							newGroup.Add(tableName + '.' + field.Name + " != \'" + field.Data.First().ToString() + "\'");
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

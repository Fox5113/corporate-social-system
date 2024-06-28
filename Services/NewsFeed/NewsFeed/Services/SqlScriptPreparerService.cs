using NewsFeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NewsFeed.Enums.Common;

namespace NewsFeed.Services
{
    public class SqlScriptPreparerService
    {
		/// <summary>
		/// Получение скрипта удаления
		/// </summary>
		/// <param name="mapping">Маппинг удаления</param>
		/// <returns></returns>
		public string GetDeleteQuery(Mapping mapping)
		{
			if (String.IsNullOrEmpty(mapping.MainTableName))
				return null;

			var deleteQuery = new DeleteQuery(mapping.MainTableName);
			deleteQuery.Filters = mapping.Group == null ? "" : GetFilters(mapping.Group);

			return deleteQuery.PrepareSqlString();
		}

		/// <summary>
		/// Получение скрипта обновления
		/// </summary>
		/// <param name="mapping">Маппинг обновления</param>
		/// <returns></returns>
		public string GetUpdateQuery(Mapping mapping)
        {
			if (String.IsNullOrEmpty(mapping.MainTableName) || mapping.Sets == null || mapping.Sets.Count == 0)
				return null;

			var updateQuery = new UpdateQuery(mapping.MainTableName);
			updateQuery.ColumnsWithValues = GetSets(mapping.Sets);
			updateQuery.Filters = mapping.Group == null ? "" : GetFilters(mapping.Group);

			return updateQuery.PrepareSqlString();
		}

		/// <summary>
		/// Получение скрипта запроса данных
		/// </summary>
		/// <param name="mapping">Маппинг запроса</param>
		/// <returns></returns>
		public string GetSelectQuery(Mapping mapping)
		{
			if (String.IsNullOrEmpty(mapping.MainTableName))
				return null;

			var selectQuery = new SelectQuery(mapping.MainTableName);
			selectQuery.Columns = mapping.Tables == null ? "*" : GetColumns(mapping.Tables);
			selectQuery.Joins = mapping.Joins == null ? "" : GetJoins(mapping.Joins);
			selectQuery.Filters = mapping.Group == null ? "" : GetFilters(mapping.Group);
			selectQuery.OrdersBy = mapping.OrderBy == null ? "" : GetOrderBy(mapping.OrderBy);

			return selectQuery.PrepareSqlString();
		}

		/// <summary>
		/// Получить строку установок новых значений
		/// </summary>
		/// <param name="sets">Маппинг устновок значений колонок</param>
		/// <returns></returns>
		private string GetSets(ICollection<SetSqlQuery> sets)
        {
			var setsString = new List<string>();
			foreach (var set in sets)
			{
				if (!String.IsNullOrEmpty(set.ColumnName))
				{
					setsString.Add(set.ColumnName + " = \'" + set.Value.ToString() + "\'");
				}
			}

			return String.Join(", ", setsString);
		}

		/// <summary>
		/// Получить строку сортировки
		/// </summary>
		/// <param name="orderBySqls">Маппинг сортировок по колонкам</param>
		/// <returns></returns>
		private string GetOrderBy(ICollection<OrderBySqlQuery> orderBySqls)
        {
			var columns = new List<string>();
			foreach(var orderBy in orderBySqls)
            {
				columns.Add(orderBy.TableName + '.' + orderBy.ColumnName + " " + orderBy.OrderBy.ToString());
            }

			return String.Join(", ", columns);
        }

		/// <summary>
		/// Получение колонок для sql запроса
		/// </summary>
		/// <param name="tables">Таблицы</param>
		/// <returns>Строка sql</returns>
		private string GetColumns(ICollection<TableWithFieldsNames> tables)
		{
			var columns = new List<string>();
			foreach (var table in tables)
			{
				if (table.Fields == null)
				{
					columns.Add("*");
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

		/// <summary>
		/// Создание джойнов sql
		/// </summary>
		/// <param name="joinTables">Маппинг джойнов</param>
		/// <returns>Строка sql</returns>
		private string GetJoins(ICollection<JoinTables> joinTables)
		{
			var joins = new List<string>();
			foreach (var join in joinTables)
			{
				var joinValues = new List<string>();
				foreach (var pair in join.TablePairs)
				{
					joinValues.Add(pair.FirstTableName + "." + pair.FirstTableColumnName);
					joinValues.Add(GetOperation(pair.ComparisonType));
					joinValues.Add(pair.SecondTableName + "." + pair.SecondTableColumnName);
				}

				var joinStr = new List<string>();
				joinStr.Add(join.JoinType.ToString());
				joinStr.Add("JOIN");
				joinStr.Add(join.JoiningTableName);
				joinStr.Add("ON");
				joinStr.Add(String.Join(" " + join.Operation.ToString() + " ", joinValues));

				joins.Add(String.Join(" ", joinStr));
			}

			return String.Join("\n ", joins);
		}

		//Надо допилить оставшиеся операции
		/// <summary>
		/// Создание строки фильтров по маппингу
		/// </summary>
		/// <param name="map">Маппинг</param>
		/// <returns>Строка sql</returns>
		private string GetFilters(Group map)
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
						if (field.Data.Count == 1)
							newGroup.Add(tableName + '.' + field.Name + GetOperation(field.ComparisonType) + "\'" + field.Data.First().ToString() + "\'");
						else
						{
							var values = String.Join(", ", field.Data.Select(x => "\'" + x.ToString() + "\'").ToArray());
							newGroup.Add(tableName + '.' + field.Name + " in (" + values + ")");
						}
					}
					else if (field.ComparisonType == FilterComparisonType.Contain)
					{
						newGroup.Add(tableName + '.' + field.Name + " like \'%" + field.Data.First().ToString() + "%\'");
					}
					else if (field.ComparisonType == FilterComparisonType.Between)
					{
						if (field.Data.Count == 2)
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
							newGroup.Add(tableName + '.' + field.Name + " not in (" + values + ")");
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

		/// <summary>
		/// Получить строку оператора сравнения по типу
		/// </summary>
		/// <param name="type">Тип сравнения</param>
		/// <returns></returns>
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

using EmojiCSharp;
using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlQuery;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;
using static SqlQuery.Enums;

namespace FrontEnd.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private readonly NewsService _newsService;
        private int _itemsPerPage = 10;

        public NewsController(ILogger<NewsController> logger, NewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var newsList = await _newsService.GetPublishedListAsync(1, _itemsPerPage, new Guid("35044f8e-065d-4b59-9d4c-f393ea8a90b6"));
            var pagingInfo = new PagingInfo() { CurrentPage = 1, ItemsPerPage = _itemsPerPage };
            var info = new NewsListViewModel { PagingInfo = pagingInfo, News = newsList};
            return View(info);
        }

        [HttpPost]
        public async Task<IActionResult> LoadMore(int page)
        {
            var newsList = await _newsService.GetPublishedListAsync(page, _itemsPerPage, new Guid("35044f8e-065d-4b59-9d4c-f393ea8a90b6"));
            var pagingInfo = new PagingInfo() { CurrentPage = page, ItemsPerPage = _itemsPerPage };
            var info = new NewsListViewModel { PagingInfo = pagingInfo, News = newsList};
            return Ok(info);
        }

        [HttpPost]
        public async Task<IActionResult> Like(string newsId)
        {
            if (!string.IsNullOrEmpty(newsId) && Guid.TryParse(newsId, out var id))
            {

                var likesInfo = await _newsService.Like(id, new Guid("35044f8e-065d-4b59-9d4c-f393ea8a90b6"));
                return Ok(likesInfo);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Search(int page, string title, string authorName, string authorSurname, DateTime start, DateTime end, string hashtags)
        {
            var query = PrepareSelectQuery(page > 0 ? page - 1 : 0, title, authorName, authorSurname, start, end, hashtags);
            var data = await _newsService.Search(query, new Guid("35044f8e-065d-4b59-9d4c-f393ea8a90b6"));
            var pagingInfo = new PagingInfo() { CurrentPage = page, ItemsPerPage = _itemsPerPage };
            var info = new NewsListViewModel { PagingInfo = pagingInfo, News = data, Filters = new Dictionary<string, object>() {
                { "title", title }, { "authorName", authorName }, { "authorSurname", authorSurname }, { "start", start.ToString("yyyy-MM-dd") }, { "end", end.ToString("yyyy-MM-dd") }, { "hashtags", hashtags }
            },
                FiltersEmpty = false
            };
            if (page == 1)
                return View(info);

            return Ok(info);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string title, string shortDescription, string content, string hashtags)
        {
            if (String.IsNullOrEmpty(title) || String.IsNullOrEmpty(content))
                return View();

            var hashArr = !String.IsNullOrEmpty(hashtags) ? hashtags.Split(' ')
                .Where(x => !String.IsNullOrEmpty(x.Trim()))
                .Select(x => x.Trim())
                .ToList() : null;
            var query = new CreatingNewsModel()
            {
                Title = title,
                AuthorId = new Guid("35044f8e-065d-4b59-9d4c-f393ea8a90b6"),
                Content = content,
                ShortDescription = shortDescription
            };

            if(hashArr != null && hashArr.Count > 0)
            {
                query.HashtagNewsList = new List<CreatingHashtagNewsModel>();
                foreach(var item in hashArr)
                {
                    query.HashtagNewsList.Add(new CreatingHashtagNewsModel()
                    {
                        Hashtag = new CreatingHashtagModel()
                        {
                            Name = item
                        }
                    });
                }
            }

            var success = await _newsService.Create(query);

            return success ? RedirectToAction("CreatedNews") : RedirectToAction("FailedCreatingNews");
        }

        public IActionResult Create()
        {
            var emoji = EmojiData.Emoji.All;
            var model = new List<CreateNewsViewModel>();
            foreach(var item in emoji)
            {
                model.Add(new CreateNewsViewModel() { EmojiString = item.ToString(), Emoji = item });
            }

			return View(model);
        }

        public IActionResult CreatedNews()
        {
            return View();
        }

        public IActionResult FailedCreatingNews()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private MappingQuery PrepareSelectQuery(int page, string title, string authorName, string authorSurname, DateTime start, DateTime end, string hashtags)
        {
            var hashtagsArr = hashtags?.Split(' ').Where(x => x.Trim() != String.Empty).Select(x => x.Trim()).ToList();
            var mapping = new MappingQuery()
            {
                MainTableName = "News",
                Offset = page * _itemsPerPage,
                Fetch = _itemsPerPage,
                OrderBy = new List<OrderBySqlQuery>() 
                { 
                    new OrderBySqlQuery()
                    {
                        TableName = "News",
                        ColumnName = "CreatedAt",
                        OrderBy = OrderBy.DESC
                    }
                },
                TableFilter = new TableFilter()
                {
                    FieldsFilter = new List<FieldFilter>(),
                    Operation = (int)Enums.LogicalOperationStrict.AND
                },
                Tables = new List<TableWithFieldsNames>()
                {
                    new TableWithFieldsNames()
                    {
                        TableName = "News",
                        Fields = new List<string>()
                        {
                            "Id", "AuthorId", "Title", "Content", "ShortDescription", "CreatedAt", "UpdatedAt", "IsArchived", "IsPublished"
                        }
                    }
                },
                Joins = new List<JoinTables>()
                {
                    new JoinTables()
                    {
                        JoiningTableName = "HashtagNews",
                        JoinType = JoinType.LEFT,
                        Operation = LogicalOperationStrict.AND,
                        TablePairs = new List<JoinTablesPairs>()
                        {
                            new JoinTablesPairs()
                            {
                                FirstTableName = "HashtagNews",
                                SecondTableName = "News",
                                FirstTableColumnName = "NewsId",
                                SecondTableColumnName = "Id",
                                ComparisonType = (int)Enums.FilterComparisonType.Equal
                            }
                        }
                    },
                    new JoinTables()
                    {
                        JoiningTableName = "Hashtag",
                        JoinType = JoinType.LEFT,
                        Operation = LogicalOperationStrict.AND,
                        TablePairs = new List<JoinTablesPairs>()
                        {
                            new JoinTablesPairs()
                            {
                                FirstTableName = "Hashtag",
                                SecondTableName = "HashtagNews",
                                FirstTableColumnName = "Id",
                                SecondTableColumnName = "HashtagId",
                                ComparisonType = (int)Enums.FilterComparisonType.Equal
                            }
                        }
                    },
                    new JoinTables()
                    {
                        JoiningTableName = "Employee",
                        JoinType = JoinType.LEFT,
                        Operation = LogicalOperationStrict.AND,
                        TablePairs = new List<JoinTablesPairs>()
                        {
                            new JoinTablesPairs()
                            {
                                FirstTableName = "Employee",
                                SecondTableName = "News",
                                FirstTableColumnName = "Id",
                                SecondTableColumnName = "AuthorId",
                                ComparisonType = (int)Enums.FilterComparisonType.Equal
                            }
                        }
                    }
                }
            };
            if (!String.IsNullOrEmpty(title))
            {
                mapping.TableFilter.FieldsFilter.Add(new FieldFilter()
                {
                    TableName = "News",
                    Name = "Title",
                    ComparisonType = (int)Enums.FilterComparisonType.Contain,
                    Data = new List<object>() { title }
                });
            }
            if (!String.IsNullOrEmpty(authorName))
            {
                mapping.TableFilter.FieldsFilter.Add(
                        new FieldFilter()
                        {
                            TableName = "Employee",
                            Name = "Firstname",
                            ComparisonType = (int)Enums.FilterComparisonType.Contain,
                            Data = new List<object>() { authorName }
                        }
                    );
            }
            if (!String.IsNullOrEmpty(authorSurname))
            {
                mapping.TableFilter.FieldsFilter.Add(
                        new FieldFilter()
                        {
                            TableName = "Employee",
                            Name = "Surname",
                            ComparisonType = (int)Enums.FilterComparisonType.Contain,
                            Data = new List<object>() { authorSurname }
                        }
                    );
            }
            if (start != DateTime.MinValue && end != DateTime.MinValue)
            {
                mapping.TableFilter.FieldsFilter.Add(new FieldFilter()
                {
                    TableName = "News",
                    Name = "CreatedAt",
                    ComparisonType = (int)Enums.FilterComparisonType.Between,
                    Data = new List<object>() { start, end }
                });
            }
            else if (start != DateTime.MinValue)
            {
                mapping.TableFilter.FieldsFilter.Add(new FieldFilter()
                {
                    TableName = "News",
                    Name = "CreatedAt",
                    ComparisonType = (int)Enums.FilterComparisonType.GreaterOrEqual,
                    Data = new List<object>() { start }
                });
            }
            else if (end != DateTime.MinValue)
            {
                mapping.TableFilter.FieldsFilter.Add(new FieldFilter()
                {
                    TableName = "News",
                    Name = "CreatedAt",
                    ComparisonType = (int)Enums.FilterComparisonType.LessOrEqual,
                    Data = new List<object>() { start }
                });
            }
            if (hashtagsArr != null && hashtagsArr.Count > 0)
            {
                mapping.TableFilter.FieldsFilter.Add(new FieldFilter()
                {
                    TableName = "Hashtag",
                    Name = "Name",
                    ComparisonType = (int)Enums.FilterComparisonType.Equal,
                    Data = hashtagsArr.Select(x => (object)x).ToArray()
                });
            }
            mapping.TableFilter.FieldsFilter.Add(new FieldFilter()
            {
                TableName = "News",
                DataType = "bool",
                Name = "IsPublished",
                ComparisonType = (int)Enums.FilterComparisonType.Equal,
                Data = new List<object> { 1 }
            });
            mapping.TableFilter.FieldsFilter.Add(new FieldFilter()
            {
                TableName = "News",
                DataType = "bool",
                Name = "IsArchived",
                ComparisonType = (int)Enums.FilterComparisonType.Equal,
                Data = new List<object> { 0 }
            });

            return mapping;
        }
    }
}

﻿using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlQuery;
using System.Diagnostics;
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

        private async Task InitSessionData()
        {
            try
            {
                if (HttpContext?.Request?.Cookies[Constants.UserIdCookieKey] != null)
                {
                    try
                    {
                        var userModel = await _newsService.GetPersonalAccountData(HttpContext.Request.Cookies[Constants.UserIdCookieKey].ToString());
                        ViewData[Constants.PersonalAccountDataKey] = userModel;
                        HttpContext.Session.SetString(User.Identity.Name, userModel.Id.ToString());
                        HttpContext.Session.SetString(User.Identity.Name + Constants.FullNamePrefix, userModel.Firstname + " " + userModel.Surname);
                        HttpContext.Session.SetString(User.Identity.Name + Constants.LanguagePrefix, !String.IsNullOrEmpty(userModel.Language) ? userModel.Language : Constants.LanguageBase);
                    }
                    catch (Exception ex) { }
                }

                if (!String.IsNullOrEmpty(User?.Identity?.Name) && String.IsNullOrEmpty(HttpContext.Session.GetString(User.Identity.Name)))
                {
                    var userModel = await _newsService.GetUserByLogin(User.Identity.Name);
                    if (userModel != null)
                    {
                        HttpContext.Session.SetString(User.Identity.Name, userModel.Id.ToString());
                        HttpContext.Session.SetString(User.Identity.Name + Constants.FullNamePrefix, userModel.Name);
                        HttpContext.Session.SetString(User.Identity.Name + Constants.LanguagePrefix, Constants.LanguageBase);
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void InitViewData()
        {
            var lang = HttpContext.Session.GetString(User.Identity.Name + Constants.LanguagePrefix);
            if (!String.IsNullOrEmpty(User?.Identity?.Name) && !String.IsNullOrEmpty(lang))
            {
                ViewData[Constants.CaptionsKey] = Constants.Dictionaries[lang];
            }

            if (ViewData[Constants.CaptionsKey] == null)
            {
                ViewData[Constants.CaptionsKey] = Constants.Dictionaries[Constants.LanguageBase];
            }

            ViewData[Constants.UserFullNameKey] = HttpContext.Session.GetString(User?.Identity?.Name + Constants.FullNamePrefix);
        }

        #region View methods
        public async Task<IActionResult> Index()
        {
            try
            {
                if (!String.IsNullOrEmpty(User?.Identity?.Name) && String.IsNullOrEmpty(HttpContext.Session.GetString(User.Identity.Name)))
                {
                    await InitSessionData();
                }
                InitViewData();

                if (Guid.TryParse(HttpContext.Session.GetString(User.Identity.Name), out var userId))
                {
                    try
                    {
                        var newsList = await _newsService.GetPublishedListAsync(1, _itemsPerPage, userId);
                        var pagingInfo = new PagingInfo() { CurrentPage = 1, ItemsPerPage = _itemsPerPage };
                        var info = new NewsListViewModel { PagingInfo = pagingInfo, News = newsList };
                        ViewData[Constants.NewsListViewModelKey] = info;
                    }
                    catch (Exception ex) { }
                }

                return View();
            }
            catch (Exception ex) 
            { 
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(int page, string title, string authorName, string authorSurname, DateTime start, DateTime end, string hashtags)
        {
            try
            {
                if (!String.IsNullOrEmpty(User?.Identity?.Name) && String.IsNullOrEmpty(HttpContext.Session.GetString(User.Identity.Name)))
                {
                    await InitSessionData();
                }
                InitViewData();

                if (Guid.TryParse(HttpContext.Session.GetString(User.Identity.Name), out var userId))
                {
                    var query = PrepareSelectQuery(page > 0 ? page - 1 : 0, title, authorName, authorSurname, start, end, hashtags);
                    var data = await _newsService.Search(query, userId);
                    var pagingInfo = new PagingInfo() { CurrentPage = page, ItemsPerPage = _itemsPerPage };
                    var info = new NewsListViewModel
                    {
                        PagingInfo = pagingInfo,
                        News = data,
                        Filters = new Dictionary<string, object>() {
                            { "title", title }, { "authorName", authorName }, { "authorSurname", authorSurname }, { "start", start.ToString("yyyy-MM-dd") }, { "end", end.ToString("yyyy-MM-dd") }, { "hashtags", hashtags }
                        },
                        FiltersEmpty = false
                    };
                    if (page == 1)
                    {
                        ViewData[Constants.NewsListViewModelKey] = info;
                        return View();
                    }

                    return Ok(info); //ajax
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Create()
        {
            InitViewData();
            var emoji = EmojiData.Emoji.All;
            var model = new List<EmojiViewModel>();
            foreach (var item in emoji)
            {
                model.Add(new EmojiViewModel() { EmojiString = item.ToString(), Emoji = item, Hashcode = item.Sequence.GetHashCode() });
            }
            ViewData[Constants.EmojiViewModelListKey] = model;
            return View();
        }

        public IActionResult CreatedNews()
        {
            InitViewData();
            return View();
        }

        public IActionResult FailedCreatingNews()
        {
            InitViewData();
            return View();
        }

        public IActionResult UpdatedNews()
        {
            InitViewData();
            return View();
        }

        public IActionResult FailedUpdatingNews()
        {
            InitViewData();
            return View();
        }

        public IActionResult DeletedNews()
        {
            InitViewData();
            return View();
        }

        public IActionResult FailedDeletingNews()
        {
            InitViewData();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid newsId)
        {
            InitViewData();
            var emoji = EmojiData.Emoji.All;
            var list = new List<EmojiViewModel>();
            foreach (var item in emoji)
            {
                var emojiViewModel = new EmojiViewModel() { EmojiString = item.ToString(), Emoji = item, Hashcode = item.Sequence.GetHashCode() };
                list.Add(emojiViewModel);
            }
            var news = await _newsService.GetAsync(newsId);
            var model = new CreateNewsViewModel()
            {
                Emoji = list,
                News = news
            };
            ViewData[Constants.CreateNewsViewModelKey] = model;
            return View();
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> Create(string title, string shortDescription, string content, string hashtags)
        {
            if (String.IsNullOrEmpty(title) || String.IsNullOrEmpty(content))
                return View();

            try
            {
                if (Guid.TryParse(HttpContext.Session.GetString(User.Identity.Name), out var userId))
                {
                    var hashArr = !String.IsNullOrEmpty(hashtags) ? hashtags.Split(' ')
                        .Where(x => !String.IsNullOrEmpty(x.Trim()))
                        .Select(x => x.Trim())
                        .ToList() : null;
                    var query = new CreatingNewsModel()
                    {
                        Title = title,
                        AuthorId = userId,
                        Content = content,
                        ShortDescription = shortDescription
                    };

                    if (hashArr != null && hashArr.Count > 0)
                    {
                        query.HashtagNewsList = new List<CreatingHashtagNewsModel>();
                        foreach (var item in hashArr)
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

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNews(Guid newsId, string title, string shortDescription, string content, string hashtags)
        {
            try
            {
                if (Guid.TryParse(HttpContext.Session.GetString(User.Identity.Name), out var userId))
                {
                    var isAuthor = await _newsService.CheckIsAuthor(newsId, userId);
                    if (isAuthor)
                    {
                        var hashArr = !String.IsNullOrEmpty(hashtags) ?
                            hashtags.Split(' ').Where(x => !String.IsNullOrEmpty(x.Trim())).Select(x => x.Trim()).ToList() : null;
                        var model = new UpdatingNewsModel()
                        {
                            Id = newsId,
                            Title = title,
                            Content = content,
                            ShortDescription = shortDescription
                        };
                        if (hashArr != null && hashArr.Count > 0)
                        {
                            model.HashtagNewsList = new List<CreatingHashtagNewsModel>();
                            foreach (var item in hashArr)
                            {
                                model.HashtagNewsList.Add(new CreatingHashtagNewsModel()
                                {
                                    Hashtag = new CreatingHashtagModel()
                                    {
                                        Name = item
                                    }
                                });
                            }
                        }
                        var isUpdated = await _newsService.Update(model);
                        return isUpdated ? RedirectToAction("UpdatedNews") : RedirectToAction("FailedUpdatingNews");
                    }
                    else
                        return RedirectToAction("FailedUpdatingNews");
                }
                ViewData[Constants.UserFullNameKey] = HttpContext.Session.GetString(User?.Identity?.Name + Constants.FullNamePrefix);
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public async Task<IActionResult> DeleteNews(Guid newsId)
        {
            try
            {
                if (Guid.TryParse(HttpContext.Session.GetString(User.Identity.Name), out var userId))
                {
                    var isAuthor = await _newsService.CheckIsAuthor(newsId, userId);
                    if (isAuthor)
                    {
                        var isDeleted = await _newsService.Delete(newsId);
                        return isDeleted ? RedirectToAction("DeletedNews") : RedirectToAction("FailedDeletingNews");
                    }
                    else
                        return RedirectToAction("FailedDeletingNews");
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        #region Ajax methods

        [HttpPost]
        public async Task<IActionResult> LoadMore(int page)
        {
            try
            {
                if (Guid.TryParse(HttpContext.Session.GetString(User.Identity.Name), out var userId))
                {
                    var newsList = await _newsService.GetPublishedListAsync(page, _itemsPerPage, userId);
                    var pagingInfo = new PagingInfo() { CurrentPage = page, ItemsPerPage = _itemsPerPage };
                    var info = new NewsListViewModel { PagingInfo = pagingInfo, News = newsList };
                    return Ok(info);
                }

                return Ok(new NewsListViewModel());
            }
            catch (Exception ex)
            {
                return Ok(new NewsListViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Like(string newsId)
        {
            try
            {
                if (Guid.TryParse(HttpContext.Session.GetString(User.Identity.Name), out var userId))
                {
                    if (!string.IsNullOrEmpty(newsId) && Guid.TryParse(newsId, out var id))
                    {

                        var likesInfo = await _newsService.Like(id, userId);
                        return Ok(likesInfo);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }
        #endregion

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

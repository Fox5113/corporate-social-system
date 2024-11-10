﻿using FrontEnd.Models;
using FrontEnd.Models.News;
using SqlQuery;
using System.Text.Json;

namespace FrontEnd.Services
{
    public class NewsService
    {
        private readonly HttpClient _httpClient;

        public NewsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NewsViewModel>> GetPublishedListAsync(int page, int itemsPerPage, Guid userId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                {"page", page.ToString()},
                {"itemsPerPage", itemsPerPage.ToString()}
            };

            var queryString = string.Join("&", queryParameters
                .Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value)}"));

            var url = $"https://localhost:7175/api/News/GetPublishedListAsync?{queryString}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var news = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var list = JsonSerializer.Deserialize<List<NewsModel>>(news, options);
                return await PrepareNewsList(list, userId);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return null;
            }
            else
            {
                throw new HttpRequestException("News service is not available.");
            }
        }

        public async Task<EmployeeModel> GetEmployeeInfo(Guid id)
        {
            var queryParameters = new Dictionary<string, string>
            {
                {"id", id.ToString()}
            };

            var queryString = string.Join("&", queryParameters
                .Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value)}"));

            var url = $"https://localhost:7175/api/Employee/GetAsync?{queryString}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var employee = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<EmployeeModel>(employee, options);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<LikedNewsInfoModel>> GetLikesInfo(List<Guid> newsIds, Guid currentEmployeeId)
        {
            var data = new {currentEmployeeId, newsIds};
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7175/api/News/GetLikes", data);

            if (response.IsSuccessStatusCode)
            {
                var likes = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<List<LikedNewsInfoModel>>(likes, options);
            }
            else
            {
                return null;
            }
        }

        public async Task<LikedNewsInfoModel> Like(Guid newsId, Guid currentEmployeeId)
        {
            var newsIds = new List<Guid>() { newsId };
            var data = new { currentEmployeeId, newsIds };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7175/api/News/Like", data);

            if (response.IsSuccessStatusCode)
            {
                var like = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<LikedNewsInfoModel>(like, options);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<NewsViewModel>> Search(MappingQuery mapping, Guid userId)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7175/api/Home/GetSomeCollectionFromMapping", mapping);
            if (response.IsSuccessStatusCode)
            {
                var newsStr = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var news = JsonSerializer.Deserialize<JsonResponseNews>(newsStr, options);
                return await PrepareNewsList(news.Value, userId);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Create(CreatingNewsModel newsModel)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7175/api/News/CreateAsync", newsModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<HashtagModel>> GetHashtags(List<Guid> ids)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7175/api/Hashtag/GetCollection", ids);

            if (response.IsSuccessStatusCode)
            {
                var hashtags = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<List<HashtagModel>>(hashtags, options);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<HashtagNewsModel>> GetHashtagNewsByNewsIds(List<Guid> newsIds)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7175/api/HashtagNews/GetCollectionByNewsIds", newsIds);

            if (response.IsSuccessStatusCode)
            {
                var hashtags = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<List<HashtagNewsModel>>(hashtags, options);
            }
            else
            {
                return null;
            }
        }

        private async Task<List<NewsViewModel>> PrepareNewsList(List<NewsModel> list, Guid userId)
        {
            var likes = list.Count > 0 ? await GetLikesInfo(list.Select(x => x.Id).ToList(), userId) : null;
            var modelList = new List<NewsViewModel>();
            var employees = new List<EmployeeModel>();
            foreach (var newsItem in list)
            {
                var newNews = new NewsViewModel()
                {
                    Id = newsItem.Id,
                    Title = newsItem.Title,
                    Content = newsItem.Content,
                    ShortDescription = newsItem.ShortDescription,
                    CreatedAt = newsItem.CreatedAt,
                    UpdatedAt = newsItem.UpdatedAt,
                    AuthorId = newsItem.AuthorId
                };

                if (newsItem.Author == null)
                {
                    var isInList = employees.FirstOrDefault(x => x.Id == newsItem.AuthorId) != null;
                    var employee = isInList ? employees.FirstOrDefault(x => x.Id == newsItem.AuthorId) : await GetEmployeeInfo(newsItem.AuthorId);

                    if (employee != null)
                    {
                        if (!isInList)
                            employees.Add(employee);

                        newNews.AuthorFullName = employee.Firstname + ' ' + employee.Surname;
                    }
                }
                else
                {
                    if (!employees.Contains(newsItem.Author))
                        employees.Add(newsItem.Author);

                    newNews.AuthorFullName = newsItem.Author.Firstname + ' ' + newsItem.Author.Surname;
                }

                if (newsItem.HashtagNewsList != null)
                {
                    newNews.HashtagNewsList = newsItem.HashtagNewsList;
                }
                else
                {
                    var hashtagNews = await GetHashtagNewsByNewsIds(new List<Guid>() { newsItem.Id });
                    newNews.HashtagNewsList = hashtagNews;
                }

                if(newNews.HashtagNewsList != null)
                {
                    var hashtagIds = newNews.HashtagNewsList.Select(x => x.HashtagId).ToList();
                    var hashtags = hashtagIds != null && hashtagIds.Count > 0 ? await GetHashtags(hashtagIds) : null;
                    newNews.HashtagList = hashtags;
                    var names = hashtags?.Select(x => "#" + x.Name).ToList();
                    newNews.Hashtags = names != null && names.Count > 0 ? string.Join(" ", names) : "";
                }

                var likeInfo = likes?.FirstOrDefault(x => x.NewsId == newsItem.Id);
                if (likeInfo != null)
                {
                    newNews.IsLikedByCurrentUser = likeInfo.IsLiked;
                    newNews.Likes = likeInfo.LikesCount;
                }

                modelList.Add(newNews);
            }
            return modelList;
        }
    }
}

using System.Collections.Generic;
using System;
using WebApi.Models.NewsComment;
using WebApi.Models.HashtagNews;
using WebApi.Models.Employee;

namespace WebApi.Models.News
{
    public class NewsModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AuthorId { get; set; }
        public EmployeeModel Author { get; set; }
        public int Likes { get; set; }
        public List<NewsCommentModel> NewsCommentList { get; set; }
        public List<HashtagNewsModel> HashtagNewsList { get; set; }
    }
}

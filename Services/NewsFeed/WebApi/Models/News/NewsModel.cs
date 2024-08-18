using System;
using System.Collections.Generic;
using WebApi.Models.Employee;
using WebApi.Models.HashtagNews;
using WebApi.Models.NewsComment;

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
        public bool IsArchived { get; set; }
        public bool IsPublished { get; set; }
        public EmployeeModel Author { get; set; }
        public int Likes { get; set; }
        public List<NewsCommentModel> NewsCommentList { get; set; }
        public List<HashtagNewsModel> HashtagNewsList { get; set; }
    }
}

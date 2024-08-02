using System.Collections.Generic;
using System;
using BusinessLogic.Contracts.NewsComment;
using BusinessLogic.Contracts.HashtagNews;
using BusinessLogic.Contracts.Employee;

namespace BusinessLogic.Contracts.News
{
    public class NewsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AuthorId { get; set; }
        public  EmployeeDto Author { get; set; }
        public int Likes { get; set; }
        public List<NewsCommentDto> NewsCommentList { get; set; }
        public List<HashtagNewsDto> HashtagNewsList { get; set; }
    }
}

using BusinessLogic.Contracts.HashtagNews;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Contracts.News
{
    public class CreatingNewsDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public Guid AuthorId { get; set; }
        public List<CreatingHashtagNewsDto> HashtagNewsList { get; set; }
    }
}

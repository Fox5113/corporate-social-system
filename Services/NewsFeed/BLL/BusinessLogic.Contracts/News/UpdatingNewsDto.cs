using BusinessLogic.Contracts.HashtagNews;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Contracts.News
{
    public class UpdatingNewsDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public bool IsPublished { get; set; }
        public bool IsArchived { get; set; }
        public int Likes { get; set; }
        public List<CreatingHashtagNewsDto> HashtagNewsList { get; set; }
    }
}

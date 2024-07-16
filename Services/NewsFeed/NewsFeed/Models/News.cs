using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class News
    {
        public Guid Id { get; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AuthorId { get; set; }
        public Employee Author { get; set; }
        
        //Пока не надо
        public int Likes { get; set; }
        public ICollection<NewsComment> NewsComments { get; set; }
        public ICollection<HashtagNews> NewsHashtags { get; set; }
        public ICollection<Hashtag> Hashtags { get; set; }

    }
}

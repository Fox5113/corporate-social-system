using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class NewsSearch
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public ICollection<Hashtag> Hashtags { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}

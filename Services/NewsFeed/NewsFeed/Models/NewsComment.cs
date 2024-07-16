using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class NewsComment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid AuthorId { get; set; }
        public Employee Author { get; set; }
        public Guid NewsId { get; set; }
        public News News { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

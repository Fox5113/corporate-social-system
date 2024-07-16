using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class HashtagNews
    {
        public Guid Id { get; set; }
        public Guid HashtagId { get; set; }
        public Guid NewsId { get; set; }
        public Hashtag Hashtag { get; set; }
        public News News { get; set; }
    }
}

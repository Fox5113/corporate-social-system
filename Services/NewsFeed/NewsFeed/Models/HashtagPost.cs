using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class HashtagPost
    {
        public Guid Id { get; set; }
        public Guid HashtagId { get; set; }
        public Guid PostId { get; set; }
        public Hashtag Hashtag { get; set; }
        public Post Post { get; set; }
    }
}

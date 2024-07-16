using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class Hashtag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<HashtagNews> HashtagNews { get; set; }
    }
}

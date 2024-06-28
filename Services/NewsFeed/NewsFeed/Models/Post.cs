using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid AuthorId { get; set; }
        public User Author { get; set; }
        public int Likes { get; set; }
        public ICollection<PostComment> PostComments { get; set; }
        public ICollection<HashtagPost> PostHashtags { get; set; }
        public ICollection<Hashtag> Hashtags { get; set; }

    }
}

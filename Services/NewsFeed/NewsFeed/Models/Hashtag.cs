using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class Hashtag
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [NotMapped]
        public virtual ICollection<HashtagNews> HashtagNewsList { get; set; }

        public Hashtag()
        {
            HashtagNewsList = new HashSet<HashtagNews>();
        }
    }
}

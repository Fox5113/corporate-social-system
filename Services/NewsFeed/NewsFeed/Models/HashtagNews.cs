using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class HashtagNews
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid HashtagId { get; set; }
        [Required]
        public Guid NewsId { get; set; }
        [NotMapped]
        public virtual Hashtag Hashtag { get; set; }
        [NotMapped]
        public virtual News News { get; set; }
    }
}

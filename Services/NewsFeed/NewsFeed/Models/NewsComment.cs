using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class NewsComment
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public Guid AuthorId { get; set; }
        [NotMapped]
        public virtual Employee Author { get; set; }
        [Required]
        public Guid NewsId { get; set; }
        [NotMapped]
        public virtual News News { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

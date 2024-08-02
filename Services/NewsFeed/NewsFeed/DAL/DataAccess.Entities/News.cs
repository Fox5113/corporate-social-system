using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class News : IEntity<Guid>
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        public Guid AuthorId { get; set; }
        public virtual Employee Author { get; set; }
        public int Likes { get; set; }
        public virtual ICollection<NewsComment> NewsCommentList { get; set; }
        public virtual ICollection<HashtagNews> HashtagNewsList { get; set; }

    }
}

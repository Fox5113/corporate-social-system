using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class NewsComment : IEntity<Guid>
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public Guid AuthorId { get; set; }
        public virtual Employee Author { get; set; }
        [Required]
        public Guid NewsId { get; set; }
        public virtual News News { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

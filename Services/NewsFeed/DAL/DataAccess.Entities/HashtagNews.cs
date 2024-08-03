using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class HashtagNews : IEntity<Guid>
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid HashtagId { get; set; }
        [Required]
        public Guid NewsId { get; set; }
        public virtual Hashtag Hashtag { get; set; }
        public virtual News News { get; set; }
    }
}

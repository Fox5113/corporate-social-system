using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Hashtag : IEntity<Guid>
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<HashtagNews> HashtagNewsList { get; set; }

        public Hashtag()
        {
            HashtagNewsList = new HashSet<HashtagNews>();
        }
    }
}

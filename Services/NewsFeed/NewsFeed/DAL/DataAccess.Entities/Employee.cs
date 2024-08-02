using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Employee : IEntity<Guid>
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public virtual ICollection<News> NewsList { get; set; }
        public virtual ICollection<NewsComment> NewsCommentList { get; set; }

        public Employee()
        {
            NewsList = new List<News>();
            NewsCommentList = new List<NewsComment>();
        }
    }
}

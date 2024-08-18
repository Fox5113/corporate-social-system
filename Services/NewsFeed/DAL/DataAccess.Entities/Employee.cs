using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Employee : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public virtual ICollection<News> NewsList { get; set; }
        public virtual ICollection<NewsComment> NewsCommentList { get; set; }
    }
}

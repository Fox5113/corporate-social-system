using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Models
{
    public class Employee
    {
        public Guid Id { get; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public ICollection<News> NewsList { get; set; }
        public ICollection<NewsComment> NewsComments { get; set; }
    }
}

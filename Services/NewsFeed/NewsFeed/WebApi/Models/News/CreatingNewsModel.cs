using System;

namespace WebApi.Models.News
{
    public class CreatingNewsModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public Guid AuthorId { get; set; }
    }
}

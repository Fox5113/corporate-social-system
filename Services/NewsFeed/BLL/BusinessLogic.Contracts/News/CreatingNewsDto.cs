using System;

namespace BusinessLogic.Contracts.News
{
    public class CreatingNewsDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public Guid AuthorId { get; set; }
    }
}

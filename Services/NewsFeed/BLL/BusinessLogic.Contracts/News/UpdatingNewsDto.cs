using System;

namespace BusinessLogic.Contracts.News
{
    public class UpdatingNewsDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public int Likes { get; set; }
    }
}

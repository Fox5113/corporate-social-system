namespace FrontEnd.Models
{
    public class UpdatingNewsModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public List<CreatingHashtagNewsModel>? HashtagNewsList { get; set; }
    }
}

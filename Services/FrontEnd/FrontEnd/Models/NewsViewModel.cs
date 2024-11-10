namespace FrontEnd.Models
{
    public class NewsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorFullName { get; set; }
        public int Likes { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public List<NewsCommentModel>? NewsCommentList { get; set; }
        public List<HashtagNewsModel>? HashtagNewsList { get; set; }
        public List<HashtagModel>? HashtagList { get; set; }
        public string? Hashtags { get; set; }
    }
}

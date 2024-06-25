using System;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
    }

    public class Hashtag
    {
        [Key]
        public Guid Id { get; set; }
        public required string MessageHashtag { get; set; }
    }

    public class HashtagPost
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HashtagId { get; set; }
        public Guid PostId { get; set; }
    }

    public class File
    {
        [Key]
        public Guid Id { get; set; }
        public int Size { get; set; }
        public string Title { get; set; }
        public byte[] Data { get; set; }
        public string MIMEtype { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PostComment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
using System.Collections.Generic;

namespace FeedleDataTier.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public string AuthorUserName { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        // public byte[] images;
        public List<Comment> Comments { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace FeedleDataTier.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }
}
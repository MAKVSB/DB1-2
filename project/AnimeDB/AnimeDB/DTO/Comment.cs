using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace AnimeDB.DTO
{
    public class Comment
    {
        public int Id { get; set; }
        public int AnimeId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public int? Rating { get; set; }
        public int? HiddenById { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int? InReplyToId { get; set; }

        //artificial columns
        public Anime Anime { get; set; }
        public User User { get; set; }
        public User HiddenBy { get; set; }
        public Comment? InReplyTo { get; set; }

        public override string ToString()
        {
            return $"{Id}, ({AnimeId} {UserId}) {Text} ({(HiddenById == null ? "shown" : "hidden")}) [{Replies}]";
        }

        //artificial columns for writing structured comments

        public string? UserName { get; set; }
        public int? Replies { get; set; }
    }
}
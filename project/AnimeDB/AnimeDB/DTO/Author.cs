
using System.Xml.Linq;

namespace AnimeDB.DTO
{
    public class Author
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Authorname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Bibliography { get; set; }
        public string? PhotoUrl { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string? PrefferedStyle { get; set; }
        public DateTime? DeletedAt { get; set; }

        //artificial columns

        public override string ToString()
        {
            return $"{Id}, {Authorname} ({Firstname} {Lastname})";
        }
    }
}
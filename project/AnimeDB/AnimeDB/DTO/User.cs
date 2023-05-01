using static System.Net.Mime.MediaTypeNames;

namespace AnimeDB.DTO
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string? Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PhotoUrl { get; set; }
        public string Language { get; set; }
        public int Role { get; set; }
        public DateTime? Paid { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime Updated { get; set; }
        public DateTime LastLogin { get; set; }
        public bool EmailNotifications { get; set; }
        public bool EmailMarketing { get; set; }
        public string? PrefferedStyle { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? DeletedBySelf { get; set; }

        //artificial columns

        public override string ToString()
        {
            return $"{Id}, ({Firstname} {Lastname})";
        }
    }
}
namespace AnimeDB.DTO
{
    public class Anime
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Length { get; set; }
        public int? EpisodeNumber { get; set; }
        public int? SeriesNumber { get; set; }
        public string Desc { get; set; }
        public string Shortdesc { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int Viewcount { get; set; }
        public string? Language { get; set; }
        public int? ParentAnimeId { get; set; }
        public DateTime? DeletedAt { get; set; }

        //artificial columns
        public List<Author> Authors { get; set; }
        public Anime? ParentAnime { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name} ({EpisodeNumber}, {SeriesNumber})";
        }
    }
}
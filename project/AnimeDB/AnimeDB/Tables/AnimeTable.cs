using AnimeDB.DBS;
using AnimeDB.DTO;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace AnimeDB.Tables
{
    public class AnimeTable
    {
        private static String TABLE_NAME = "Anime";

        private static String SQL_SELECT_BASE = "SELECT * FROM Anime WHERE parentAnimeId IS NULL AND deletedAt IS NULL";
        private static String SQL_SELECT_EPISODES = "SELECT * FROM Anime WHERE parentAnimeId = @parentEpisodeId  AND deletedAt IS NULL ORDER BY episodeNumber ASC";
        private static String SQL_SELECT_BY_EPISODE = "SELECT * FROM Anime WHERE episodeNumber = @episodeNumber AND parentAnimeId = @parentAnimeId AND deletedAt IS NULL";
        private static String SQL_SELECT_ID = "SELECT * FROM Anime WHERE animeId=@animeId AND deletedAt IS NULL";
        private static String SQL_SELECT_NAME = "SELECT * FROM Anime WHERE name=@name AND deletedAt IS NULL";
        private static String SQL_INSERT = "INSERT INTO Anime (name, length, episodeNumber, seriesNumber, \"desc\", shortDesc, created, updated, viewCount, language, parentAnimeId, deletedAt) VALUES (@name, @length, @episodeNumber, @seriesNumber, @desc, @shortdesc, @created, @updated, @viewcount, @language, @parentAnimeId, @deletedAt)";
        private static String SQL_DELETE_ID = "UPDATE Anime SET deletedAt = @deletedAt WHERE animeId=@animeId";
        private static String SQL_UPDATE = "UPDATE Anime SET name = @name, length = @length, episodeNumber = @episodeNumber, seriesNumber = @seriesNumber, \"desc\" = @desc, shortdesc = @shortdesc, updated = @updated, viewcount = @viewcount, language = @language, parentAnimeId = @parentAnimeId, deletedAt = @deletedAt WHERE animeId=@animeId";

        private static String SQL_MOVE_EPISODE = "exec SwapEpisodes @episodeId, @direction";

        /// <summary>
        /// Insert the record.
        /// </summary>
        public static int Insert(Anime anime)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, anime);
            command.Parameters.AddWithValue("@created", DateTime.Now);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }

        /// <summary>
        /// Update the record.
        /// </summary>
        /// <param name="Anime"></param>
        /// <returns></returns>
        public static int Update(Anime anime)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, anime);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        /// <summary>
        /// Select records.
        /// </summary>
        public static Collection<Anime> Select()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_BASE);
            SqlDataReader reader = db.Select(command);

            Collection<Anime> animes = Read(reader);
            reader.Close();
            db.Close();
            return animes;
        }

        /// <summary>
        /// Select records for anime.
        /// </summary>
        public static Anime Select(int animeId)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);

            command.Parameters.AddWithValue("@animeId", animeId);
            SqlDataReader reader = db.Select(command);

            Collection<Anime> animes = Read(reader);
            Anime anime = null;
            if (animes.Count == 1)
            {
                anime = animes[0];
            }
            reader.Close();
            db.Close();
            return anime;
        }

        /// <summary>
        /// Select anime episodes.
        /// </summary>
        public static Collection<Anime> SelectEpisodes(int animeId)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_SELECT_EPISODES);

            command.Parameters.AddWithValue("@parentEpisodeId", animeId);
            SqlDataReader reader = db.Select(command);

            Collection<Anime> animes = Read(reader);
            reader.Close();
            db.Close();
            return animes;
        }

        public static int MoveEpisode(int animeId, string direction)
        {
            Database db = new Database();
            db.Connect();
            db.BeginTransaction();
            int returnValue = 0;
            try
            {
                SqlCommand command = db.CreateCommand(SQL_MOVE_EPISODE);

                command.Parameters.AddWithValue("@direction", direction);
                command.Parameters.AddWithValue("@episodeId", animeId);
                returnValue = db.ExecuteNonQuery(command);
                db.EndTransaction();
            }
            catch
            {
                db.Rollback();
            }

            db.Close();
            return returnValue;
        }


        /// <summary>
        /// Delete the record.
        /// </summary>
        public static int Delete(int animeId)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);

            command.Parameters.AddWithValue("@animeId", animeId);
            command.Parameters.AddWithValue("@deletedAt", DateTime.Now);
            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }

        public static Anime? SelectByAnimeAndEpisodeNumber(int? animeId, int? episodeNumber)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_SELECT_BY_EPISODE);

            command.Parameters.AddWithValue("@parentAnimeId", animeId);
            command.Parameters.AddWithValue("@episodeNumber", episodeNumber);
            SqlDataReader reader = db.Select(command);

            Collection<Anime> animes = Read(reader);
            Anime anime = null;
            if (animes.Count == 1)
            {
                anime = animes[0];
            }
            reader.Close();
            db.Close();
            return anime;
        }

        public static (Anime?, Anime?) FindPrevNext(int animeId, int episodeNumber)
        {
            return FindPrevNext(SelectByAnimeAndEpisodeNumber(animeId, episodeNumber));
        }

        public static (Anime?, Anime?) FindPrevNext(Anime anime)
        {
            return (SelectByAnimeAndEpisodeNumber(anime.ParentAnimeId, anime.EpisodeNumber - 1), SelectByAnimeAndEpisodeNumber(anime.ParentAnimeId, anime.EpisodeNumber + 1));
        }

        /// <summary>
        /// Prepare a command.
        /// </summary>
        private static void PrepareCommand(SqlCommand command, Anime anime)
        {
            command.Parameters.AddWithValue("@animeId", anime.Id);
            command.Parameters.AddWithValue("@name", anime.Name == null ? DBNull.Value : anime.Name);
            command.Parameters.AddWithValue("@length", anime.Length == null ? DBNull.Value : anime.Length);
            command.Parameters.AddWithValue("@episodeNumber", anime.EpisodeNumber == null ? DBNull.Value : anime.EpisodeNumber);
            command.Parameters.AddWithValue("@seriesNumber", anime.SeriesNumber == null ? DBNull.Value : anime.SeriesNumber);
            command.Parameters.AddWithValue("@desc", anime.Desc);
            command.Parameters.AddWithValue("@shortdesc", anime.Shortdesc);
            command.Parameters.AddWithValue("@updated", DateTime.Now);
            command.Parameters.AddWithValue("@viewcount", anime.Viewcount);
            command.Parameters.AddWithValue("@language", anime.Language == null ? DBNull.Value : anime.Language);
            command.Parameters.AddWithValue("@parentAnimeId", anime.ParentAnimeId == null ? DBNull.Value : anime.ParentAnimeId);
            command.Parameters.AddWithValue("@deletedAt", anime.DeletedAt == null ? DBNull.Value : anime.DeletedAt);
        }

        /// <summary>
        /// Select the record for a name.
        /// </summary>
        public static Anime SelectForAnimeName(string name, Database pDb = null)
        {
            Database db;
            if (pDb == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = pDb;
            }
            SqlCommand command = db.CreateCommand(SQL_SELECT_NAME);
            command.Parameters.AddWithValue("@name", name);
            SqlDataReader reader = db.Select(command);
            Collection<Anime> animes = Read(reader);
            Anime anime = null;
            if (animes.Count == 1)
            {
                anime = animes[0];
            }
            reader.Close();
            if (pDb == null)
            {
                db.Close();
            }
            return anime;
        }

        private static Collection<Anime> Read(SqlDataReader reader)
        {
            Collection<Anime> animes = new Collection<Anime>();

            while (reader.Read())
            {                
                Anime anime = new Anime();
                anime.Id = reader.GetInt32(reader.GetOrdinal("animeId"));
                anime.Name = reader.GetString(reader.GetOrdinal("name"));
                if (!reader.IsDBNull(reader.GetOrdinal("length"))){
                    anime.Length = reader.GetInt32(reader.GetOrdinal("length"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("episodeNumber")))
                {
                    anime.EpisodeNumber = reader.GetInt32(reader.GetOrdinal("episodeNumber"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("seriesNumber")))
                {
                    anime.SeriesNumber = reader.GetInt32(reader.GetOrdinal("seriesNumber"));
                }
                anime.Desc = reader.GetString(reader.GetOrdinal("desc"));
                anime.Shortdesc = reader.GetString(reader.GetOrdinal("shortdesc"));
                anime.Created = reader.GetDateTime(reader.GetOrdinal("created"));
                anime.Updated = reader.GetDateTime(reader.GetOrdinal("updated"));
                anime.Viewcount = reader.GetInt32(reader.GetOrdinal("viewcount"));
                if (!reader.IsDBNull(reader.GetOrdinal("language")))
                {
                    anime.Language = reader.GetString(reader.GetOrdinal("language"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("parentAnimeId")))
                {
                    anime.ParentAnimeId = reader.GetInt32(reader.GetOrdinal("parentAnimeId"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("deletedAt")))
                {
                    anime.DeletedAt = reader.GetDateTime(reader.GetOrdinal("deletedAt"));
                }
                animes.Add(anime);
            }
            return animes;
        }
    }
}
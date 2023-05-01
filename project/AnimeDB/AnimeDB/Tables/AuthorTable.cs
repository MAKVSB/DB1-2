using AnimeDB.DBS;
using AnimeDB.DTO;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace AnimeDB.Tables
{
    public class AuthorTable
    {
        private static String TABLE_NAME = "Author";

        private static String SQL_SELECT = "SELECT * FROM Author WHERE deletedAt IS NULL";
        private static String SQL_SELECT_ID = "SELECT * FROM Author WHERE authorId=@idAuthor AND deletedAt IS NULL";
        private static String SQL_SELECT_NAME = "SELECT * FROM Author WHERE authorName=@authorName AND deletedAt IS NULL";
        private static String SQL_INSERT = "INSERT INTO Author (authorId, firstname, lastname, authorname, birthdate, bibliography, photourl, type, created, updated, prefferedStyle, deletedAt) VALUES (@authorId, @firstname, @lastname, @authorname, @birthdate, @bibliography, @photourl, @type, @created, @updated, @prefferedStyle, @deletedAt)";
        private static String SQL_DELETE_ID = "UPDATE Author SET deletedAt = @deletedAt WHERE authorId=@authorId";
        private static String SQL_UPDATE = "UPDATE Author SET firstname = @firstname, lastname = @lastname, authorname = @authorname, birthdate = @birthdate, bibliography = @bibliography, photourl = @photourl, type = @type, updated = @updated, prefferedStyle = @prefferedStyle, deletedAt = @deletedAt WHERE authorId=@authorId";

        /// <summary>
        /// Insert the record.
        /// </summary>
        public static int Insert(Author author)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, author);
            command.Parameters.AddWithValue("@created", DateTime.Now);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }

        /// <summary>
        /// Update the record.
        /// </summary>
        /// <param name="Author"></param>
        /// <returns></returns>
        public static int Update(Author author)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, author);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        /// <summary>
        /// Select records.
        /// </summary>
        public static Collection<Author> Select()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            Collection<Author> authors = Read(reader);
            reader.Close();
            db.Close();
            return authors;
        }

        /// <summary>
        /// Select records for author.
        /// </summary>
        public static Author Select(int authorId)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);

            command.Parameters.AddWithValue("@authorId", authorId);
            SqlDataReader reader = db.Select(command);

            Collection<Author> authors = Read(reader);
            Author author = null;
            if (authors.Count == 1)
            {
                author = authors[0];
            }
            reader.Close();
            db.Close();
            return author;
        }

        /// <summary>
        /// Delete the record.
        /// </summary>
        public static int Delete(int authorId)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);

            command.Parameters.AddWithValue("@authorId", authorId);
            command.Parameters.AddWithValue("@deletedAt", DateTime.Now);
            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }

        /// <summary>
        /// Prepare a command.
        /// </summary>
        private static void PrepareCommand(SqlCommand command, Author author)
        {
            command.Parameters.AddWithValue("@authorId", author.Id);
            command.Parameters.AddWithValue("@firstName", author.Firstname == null ? DBNull.Value : author.Firstname);
            command.Parameters.AddWithValue("@lastName", author.Lastname == null ? DBNull.Value : author.Lastname);
            command.Parameters.AddWithValue("@authorname", author.Authorname == null ? DBNull.Value : author.Authorname);
            command.Parameters.AddWithValue("@birthDate", author.Birthdate == null ? DBNull.Value : author.Birthdate);
            command.Parameters.AddWithValue("@bibliography", author.Bibliography);
            command.Parameters.AddWithValue("@photoUrl", author.PhotoUrl == null ? DBNull.Value : author.PhotoUrl);
            command.Parameters.AddWithValue("@type", author.Type);
            command.Parameters.AddWithValue("@updated", DateTime.Now);
            command.Parameters.AddWithValue("@prefferedStyle", author.PrefferedStyle == null ? DBNull.Value : author.PrefferedStyle);
            command.Parameters.AddWithValue("@prefferedStyle", author.DeletedAt ?? null);
        }

        /// <summary>
        /// Select the record for a name.
        /// </summary>
        public static Author SelectForAuthorName(string authorName, Database pDb = null)
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
            command.Parameters.AddWithValue("@authorName", authorName);
            SqlDataReader reader = db.Select(command);
            Collection<Author> authors = Read(reader);
            Author author = null;
            if (authors.Count == 1)
            {
                author = authors[0];
            }
            reader.Close();
            if (pDb == null)
            {
                db.Close();
            }
            return author;
        }

        private static Collection<Author> Read(SqlDataReader reader)
        {
            Collection<Author> authors = new Collection<Author>();

            while (reader.Read())
            {
                Author author = new Author();
                author.Id = reader.GetInt32(reader.GetOrdinal("authorId"));
                if (!reader.IsDBNull(reader.GetOrdinal("firstName")))
                {
                    author.Firstname = reader.GetString(reader.GetOrdinal("firstName"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("lastName")))
                {
                    author.Lastname = reader.GetString(reader.GetOrdinal("lastName"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("authorname")))
                {
                    author.Authorname = reader.GetString(reader.GetOrdinal("authorname"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("birthdate")))
                {
                    author.Birthdate = reader.GetDateTime(reader.GetOrdinal("birthdate"));
                }
                author.Bibliography = reader.GetString(reader.GetOrdinal("bibliography"));
                if (!reader.IsDBNull(reader.GetOrdinal("photoUrl")))
                {
                    author.PhotoUrl = reader.GetString(reader.GetOrdinal("photoUrl"));
                }
                author.Type = reader.GetString(reader.GetOrdinal("type"));
                author.Created = reader.GetDateTime(reader.GetOrdinal("created"));
                author.Updated = reader.GetDateTime(reader.GetOrdinal("updated"));
                if (!reader.IsDBNull(reader.GetOrdinal("prefferedStyle")))
                {
                    author.PrefferedStyle = reader.GetString(reader.GetOrdinal("prefferedStyle"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("deletedAt")))
                {
                    author.DeletedAt = reader.GetDateTime(reader.GetOrdinal("deletedAt"));
                }
                authors.Add(author);
            }
            return authors;
        }
    }
}
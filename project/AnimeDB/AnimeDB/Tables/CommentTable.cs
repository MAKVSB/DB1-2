using AnimeDB.DBS;
using AnimeDB.DTO;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace AnimeDB.Tables
{
    public class CommentTable
    {
        private static String TABLE_NAME = "comment";

        private static String SQL_SELECT = "SELECT * FROM comment";
        private static String SQL_SELECT_ID = "SELECT * FROM comment WHERE commentId=@commentId";
        // public static String SQL_SELECT_EMAIL = "SELECT * FROM comment WHERE email=@email";
        private static String SQL_SELECT_BY_ANIME = @"
           SELECT *, c.commentId,
                CASE
                 WHEN u.deletedAt IS NOT NULL THEN 'Deleted user'
                 ELSE u.firstName
                END as userName,
            CASE
             WHEN c.hiddenById IS NOT NULL THEN 'Comment has been deleted'
             ELSE c.text
            END as text,
            c.rating, (
             SELECT count(*)
             FROM comment c2
             WHERE c2.inReplyToId = c.commentId AND (C2.hiddenById IS NULL OR (
             SELECT count(*)
             FROM comment c3
             WHERE c3.inReplyToId = c2.commentId AND c3.hiddenById IS NULL
             ) != null)
            ) as replies
            FROM Comment c LEFT JOIN  [User] u on c.userId = u.userId
            WHERE
            animeId = @animeID AND
            inReplyToId IS NULL AND
            (c.hiddenById IS NULL OR (
             SELECT count(*)
             FROM comment c2
             WHERE c2.inReplyToId = c.commentId AND (C2.hiddenById IS NULL OR (
             SELECT count(*)
             FROM comment c4
             WHERE c4.inReplyToId = c2.commentId AND c4.hiddenById IS NULL
             ) != 0)
            ) > 0);
        ";
        private static String SQL_SELECT_BY_PARENT_COMMENT = @"
           SELECT *, c.commentId,
                CASE
                 WHEN u.deletedAt IS NOT NULL THEN 'Deleted user'
                 ELSE u.firstName
                END as userName,
            CASE
             WHEN c.hiddenById IS NOT NULL THEN 'Comment has been deleted'
             ELSE c.text
            END as text,
            c.rating, (
             SELECT count(*)
             FROM comment c2
             WHERE c2.inReplyToId = c.commentId AND (C2.hiddenById IS NULL OR (
             SELECT count(*)
             FROM comment c3
             WHERE c3.inReplyToId = c2.commentId AND c3.hiddenById IS NULL
             ) != null)
            ) as replies
            FROM Comment c LEFT JOIN  [User] u on c.userId = u.userId
            WHERE
            animeId = @animeId AND
            inReplyToId = @inReplyTo AND
            (c.hiddenById IS NULL OR (
             SELECT count(*)
             FROM comment c2
             WHERE c2.inReplyToId = c.commentId AND (C2.hiddenById IS NULL OR (
             SELECT count(*)
             FROM comment c4
             WHERE c4.inReplyToId = c2.commentId AND c4.hiddenById IS NULL
             ) != 0)
            ) > 0);
        ";

        private static String SQL_INSERT = "INSERT INTO comment (animeId, userId, text, rating, hiddenById, created, updated, inReplyToId) VALUES (@animeId, @userId, @text, @rating, @hiddenById, @created, @updated, @inReplyToId)";
        private static String SQL_UPDATE = "UPDATE comment SET animeId = @animeId, userId = @userId, text = @text, rating = @rating, hiddenById = @hiddenById, updated = @updated, inReplyToId = @inReplyToId WHERE commentId=@commentId";

        /// <summary>
        /// Insert the record.
        /// </summary>
        public static int Insert(Comment comment)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, comment);
            command.Parameters.AddWithValue("@created", DateTime.Now);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }

        /// <summary>
        /// Update the record.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static int Update(Comment comment)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, comment);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        /// <summary>
        /// Select records.
        /// </summary>
        public static Collection<Comment> Select()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            Collection<Comment> comments = Read(reader);
            reader.Close();
            db.Close();
            return comments;
        }

        /// <summary>
        /// Select records for comment.
        /// </summary>
        public static Comment Select(int commentId)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);

            command.Parameters.AddWithValue("@commentId", commentId);
            SqlDataReader reader = db.Select(command);

            Collection<Comment> comments = Read(reader);
            Comment comment = null;
            if (comments.Count == 1)
            {
                comment = comments[0];
            }
            reader.Close();
            db.Close();
            return comment;
        }

        /// <summary>
        /// Prepare a command.
        /// </summary>
        private static void PrepareCommand(SqlCommand command, Comment comment)
        {
            command.Parameters.AddWithValue("@commentId", comment.Id);
            command.Parameters.AddWithValue("@animeId", comment.AnimeId);
            command.Parameters.AddWithValue("@userId", comment.UserId);
            command.Parameters.AddWithValue("@text", comment.Text);
            command.Parameters.AddWithValue("@rating", comment.Rating == null ? DBNull.Value : comment.Rating);
            command.Parameters.AddWithValue("@hiddenById", comment.HiddenById == null ? DBNull.Value : comment.HiddenById);
            command.Parameters.AddWithValue("@updated", DateTime.Now);
            command.Parameters.AddWithValue("@inReplyToId", comment.InReplyToId == null ? DBNull.Value : comment.InReplyToId);
        }

        /// <summary>
        /// Select the record for a name.
        /// </summary>
        public static Collection<Comment> SelectForAnime(int animeId, Database pDb = null)
        {
            Database db = Database.CheckOrCreate(pDb);
            SqlCommand command = db.CreateCommand(SQL_SELECT_BY_ANIME);
            command.Parameters.AddWithValue("@animeId", animeId);
            SqlDataReader reader = db.Select(command);
            Collection<Comment> comments = Read(reader);
            reader.Close();
            if (pDb == null)
            {
                db.Close();
            }
            return comments;
        }

        /// <summary>
        /// Select the record for a name.
        /// </summary>
        public static Collection<Comment> SelectForParentComment(int animeId, int inReplyTo, Database pDb = null)
        {
            Database db = Database.CheckOrCreate(pDb);
            SqlCommand command = db.CreateCommand(SQL_SELECT_BY_PARENT_COMMENT);
            command.Parameters.AddWithValue("@animeId", animeId);
            command.Parameters.AddWithValue("@inReplyTo", inReplyTo);
            SqlDataReader reader = db.Select(command);
            Collection<Comment> comments = Read(reader);
            reader.Close();
            if (pDb == null)
            {
                db.Close();
            }
            return comments;
        }

        private static Collection<Comment> Read(SqlDataReader reader)
        {
            Collection<Comment> comments = new Collection<Comment>();

            while (reader.Read())
            {
                Comment comment = new Comment();
                comment.Id = reader.GetInt32(reader.GetOrdinal("commentId"));
                comment.AnimeId = reader.GetInt32(reader.GetOrdinal("animeId"));
                comment.UserId = reader.GetInt32(reader.GetOrdinal("userId"));
                comment.Text = reader.GetString(reader.GetOrdinal("text"));
                if (!reader.IsDBNull(reader.GetOrdinal("rating")))
                {
                    comment.Rating = reader.GetInt32(reader.GetOrdinal("rating"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("hiddenById")))
                {
                    comment.HiddenById = reader.GetInt32(reader.GetOrdinal("hiddenById"));
                }
                comment.Created = reader.GetDateTime(reader.GetOrdinal("created"));
                comment.Updated = reader.GetDateTime(reader.GetOrdinal("updated"));
                if (!reader.IsDBNull(reader.GetOrdinal("inReplyToId")))
                {
                    comment.InReplyToId = reader.GetInt32(reader.GetOrdinal("inReplyToId"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("inReplyToId")))
                {
                    comment.InReplyToId = reader.GetInt32(reader.GetOrdinal("inReplyToId"));
                }
                if (HasColumn(reader, "userName") && HasColumn(reader, "replies"))
                {
                    comment.Replies = reader.GetInt32(reader.GetOrdinal("replies"));
                    comment.UserName = reader.GetString(reader.GetOrdinal("userName"));
                }
                comments.Add(comment);
            }
            return comments;
        }


        private static bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
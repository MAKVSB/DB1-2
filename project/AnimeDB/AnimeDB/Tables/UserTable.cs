using AnimeDB.DBS;
using AnimeDB.DTO;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace AnimeDB.Tables
{
    public class UserTable
    {
        public static String TABLE_NAME = "User";

        private static String SQL_SELECT = "SELECT * FROM [User]";
        private static String SQL_SELECT_ID = "SELECT * FROM [User] WHERE userId=@idUser";
        private static String SQL_SELECT_EMAIL = "SELECT * FROM [User] WHERE email=@email";
        private static String SQL_INSERT = "INSERT INTO [User] (firstName, lastName, email, password, address, city, photoUrl, language, role, paid, registeredDate, updated, lastLogin, emailNotifications, emailMarketing, prefferedStyle) VALUES (@firstName, @lastName, @email, @password, @address, @city, @photoUrl, @language, @role, @paid, @registeredDate, @updated, @lastLogin, @emailNotifications, @emailMarketing, @prefferedStyle)";
        private static String SQL_UPDATE = "UPDATE [User] SET firstName = @firstName, lastName = @lastName, email = @email, password = @password, address = @address, city = @city, photoUrl = @photoUrl, language = @language, role = @role, paid = @paid, updated = @updated, lastLogin = @lastLogin, emailNotifications = @emailNotifications, emailMarketing = @emailMarketing, prefferedStyle = @prefferedStyle, deletedAt = @deletedAt, deletedBySelf = @deletedBySelf, registeredDate = @registeredDate WHERE userId=@userId";

        /// <summary>
        /// Insert the record.
        /// </summary>
        public static int Insert(User user)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, user);
            command.Parameters.AddWithValue("@registeredDate", DateTime.Now);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }

        /// <summary>
        /// Update the record.
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public static int Update(User user)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, user);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        /// <summary>
        /// Select records.
        /// </summary>
        public static Collection<User> Select()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            Collection<User> users = Read(reader);
            reader.Close();
            db.Close();
            return users;
        }

        /// <summary>
        /// Select records for user.
        /// </summary>
        public static User Select(int userId)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);

            command.Parameters.AddWithValue("@userId", userId);
            SqlDataReader reader = db.Select(command);

            Collection<User> users = Read(reader);
            User user = null;
            if (users.Count == 1)
            {
                user = users[0];
            }
            reader.Close();
            db.Close();
            return user;
        }

        /// <summary>
        /// Prepare a command.
        /// </summary>
        private static void PrepareCommand(SqlCommand command, User user)
        {
            command.Parameters.AddWithValue("@userId", user.Id);
            command.Parameters.AddWithValue("@firstName", user.Firstname);
            command.Parameters.AddWithValue("@lastName", user.Lastname == null ? DBNull.Value : user.Lastname);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@address", user.Address == null ? DBNull.Value : user.Address);
            command.Parameters.AddWithValue("@city", user.City == null ? DBNull.Value : user.City);
            command.Parameters.AddWithValue("@photoUrl", user.PhotoUrl == null ? DBNull.Value : user.PhotoUrl);
            command.Parameters.AddWithValue("@language", user.Language);
            command.Parameters.AddWithValue("@role", user.Role);
            command.Parameters.AddWithValue("@paid", user.Paid == null ? DBNull.Value : user.Paid);
            command.Parameters.AddWithValue("@updated", DateTime.Now);
            command.Parameters.AddWithValue("@lastLogin", user.LastLogin);
            command.Parameters.AddWithValue("@emailNotifications", user.EmailNotifications);
            command.Parameters.AddWithValue("@emailMarketing", user.EmailMarketing);
            command.Parameters.AddWithValue("@prefferedStyle", user.PrefferedStyle == null ? DBNull.Value : user.PrefferedStyle);
            command.Parameters.AddWithValue("@deletedAt", user.DeletedAt == null ? DBNull.Value : user.DeletedAt);
            command.Parameters.AddWithValue("@deletedBySelf", user.DeletedBySelf == null ? DBNull.Value : user.DeletedBySelf);
        }

        /// <summary>
        /// Select the record for a name.
        /// </summary>
        public static User SelectForEmail(string email, Database pDb = null)
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
            SqlCommand command = db.CreateCommand(SQL_SELECT_EMAIL);
            command.Parameters.AddWithValue("@email", email);
            SqlDataReader reader = db.Select(command);
            Collection<User> users = Read(reader);
            User user = null;
            if (users.Count == 1)
            {
                user = users[0];
            }
            reader.Close();
            if (pDb == null)
            {
                db.Close();
            }
            return user;
        }

        private static Collection<User> Read(SqlDataReader reader)
        {
            Collection<User> users = new Collection<User>();

            while (reader.Read())
            {
                User user = new User();
                user.Id = reader.GetInt32(reader.GetOrdinal("userId"));
                user.Firstname = reader.GetString(reader.GetOrdinal("firstName"));
                if (!reader.IsDBNull(reader.GetOrdinal("lastName")))
                {
                    user.Lastname = reader.GetString(reader.GetOrdinal("lastName"));
                }
                user.Email = reader.GetString(reader.GetOrdinal("email"));
                user.Password = reader.GetString(reader.GetOrdinal("password"));
                if (!reader.IsDBNull(reader.GetOrdinal("address")))
                {
                    user.Address = reader.GetString(reader.GetOrdinal("address"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("city")))
                {
                    user.City = reader.GetString(reader.GetOrdinal("city"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("photoUrl")))
                {
                    user.PhotoUrl = reader.GetString(reader.GetOrdinal("photoUrl"));
                }
                user.Language = reader.GetString(reader.GetOrdinal("language"));
                user.Role = reader.GetInt32(reader.GetOrdinal("role"));
                if (!reader.IsDBNull(reader.GetOrdinal("paid")))
                {
                    user.Paid = reader.GetDateTime(reader.GetOrdinal("paid"));
                }
                user.Updated = reader.GetDateTime(reader.GetOrdinal("updated"));
                user.LastLogin = reader.GetDateTime(reader.GetOrdinal("lastLogin"));
                user.EmailNotifications = reader.GetBoolean(reader.GetOrdinal("emailNotifications"));
                user.EmailMarketing = reader.GetBoolean(reader.GetOrdinal("emailMarketing"));
                if (!reader.IsDBNull(reader.GetOrdinal("prefferedStyle")))
                {
                    user.PrefferedStyle = reader.GetString(reader.GetOrdinal("prefferedStyle"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("deletedAt")))
                {
                    user.DeletedAt = reader.GetDateTime(reader.GetOrdinal("deletedAt"));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("deletedBySelf")))
                {
                    user.DeletedBySelf = reader.GetBoolean(reader.GetOrdinal("deletedBySelf"));
                }
                user.RegisteredDate = reader.GetDateTime(reader.GetOrdinal("registeredDate"));
                users.Add(user);
            }
            return users;
        }
    }
}
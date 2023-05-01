using AnimeDB.DBS;
using AnimeDB.DTO;
using AnimeDB.Tables;
using System.Data.SqlClient;

namespace AnimeDB
{
    public class Setup
    {
        public static void CreateTables()
        {
            string createScript = @"
                CREATE TABLE anime 
                    (
                     animeId INTEGER NOT NULL IDENTITY(1,1), 
                     name VARCHAR (100) NOT NULL, 
                     length INTEGER , 
                     episodeNumber INTEGER , 
                     seriesNumber INTEGER , 
                     ""desc"" VARCHAR (500) NOT NULL , 
                     shortDesc VARCHAR (100) NOT NULL , 
                     created DATETIME NOT NULL , 
                     updated DATETIME NOT NULL , 
                     viewCount INTEGER NOT NULL , 
                     language VARCHAR (5),
                     parentAnimeId INTEGER,
                     deletedAt DATETIME NULL
                    );

                ALTER TABLE anime ADD CONSTRAINT anime_PK PRIMARY KEY CLUSTERED (animeId)
                     WITH (
                     ALLOW_PAGE_LOCKS = ON , 
                     ALLOW_ROW_LOCKS = ON );

                CREATE TABLE anime_author 
                    (
                     animeId INTEGER NOT NULL , 
                     authorId INTEGER NOT NULL 
                    );

                ALTER TABLE anime_author ADD CONSTRAINT Relation_79_PK PRIMARY KEY CLUSTERED (animeId, authorId)
                     WITH (
                     ALLOW_PAGE_LOCKS = ON , 
                     ALLOW_ROW_LOCKS = ON );

                CREATE TABLE author 
                    (
                     authorId INTEGER NOT NULL IDENTITY(1,1), 
                     firstName VARCHAR (50) , 
                     lastName VARCHAR (50) , 
                     authorName VARCHAR (30) , 
                     birthDate DATE , 
                     bibliography VARCHAR (500) NOT NULL , 
                     photoUrl VARCHAR (100) , 
                     type VARCHAR (10) NOT NULL , 
                     created DATETIME NOT NULL , 
                     updated DATETIME NOT NULL , 
                     prefferedStyle VARCHAR (50),
                     deletedAt DATETIME
                    );

                ALTER TABLE author ADD CONSTRAINT author_PK PRIMARY KEY CLUSTERED (authorId)
                     WITH (
                     ALLOW_PAGE_LOCKS = ON , 
                     ALLOW_ROW_LOCKS = ON );

                CREATE TABLE comment 
                    (
                     commentId INTEGER NOT NULL IDENTITY(1,1), 
                     animeId INTEGER NOT NULL , 
                     userId INTEGER NOT NULL , 
                     text VARCHAR (500) NOT NULL , 
                     rating INTEGER , 
                     hiddenById INTEGER , 
                     created DATETIME NOT NULL , 
                     updated DATETIME NOT NULL , 
                     inReplyToId INTEGER 
                    );

                ALTER TABLE comment ADD CONSTRAINT comment_PK PRIMARY KEY CLUSTERED (commentId)
                     WITH (
                     ALLOW_PAGE_LOCKS = ON , 
                     ALLOW_ROW_LOCKS = ON );

                CREATE TABLE ""like"" 
                    (
                     likeId INTEGER NOT NULL IDENTITY(1,1), 
                     animeId INTEGER NOT NULL , 
                     userId INTEGER NOT NULL , 
                     active BIT NOT NULL , 
                     created DATETIME NOT NULL , 
                     updated DATETIME NOT NULL 
                    );

                ALTER TABLE ""like"" ADD CONSTRAINT like_PK PRIMARY KEY CLUSTERED (likeId)
                     WITH (
                     ALLOW_PAGE_LOCKS = ON , 
                     ALLOW_ROW_LOCKS = ON );

                CREATE TABLE request 
                    (
                     requestId INTEGER NOT NULL IDENTITY(1,1), 
                     assignedUserId INTEGER , 
                     userId INTEGER NOT NULL , 
                     text VARCHAR (500) NOT NULL , 
                     created DATETIME NOT NULL , 
                     updated DATETIME NOT NULL 
                    );

                ALTER TABLE request ADD CONSTRAINT request_PK PRIMARY KEY CLUSTERED (requestId)
                     WITH (
                     ALLOW_PAGE_LOCKS = ON , 
                     ALLOW_ROW_LOCKS = ON );

                CREATE TABLE source 
                    (
                     sourceId INTEGER NOT NULL IDENTITY(1,1), 
                     animeId INTEGER NOT NULL , 
                     name VARCHAR (50) NOT NULL , 
                     url VARCHAR (100) NOT NULL , 
                     addDate DATETIME NOT NULL , 
                     lastVerifyDate DATETIME NOT NULL , 
                     quality VARCHAR (15) NOT NULL , 
                     created DATETIME NOT NULL , 
                     updated DATETIME NOT NULL , 
                     ""external"" BIT NOT NULL , 
                     paymentRequired BIT NOT NULL , 
                     language VARCHAR (5) , 
                     subtitles VARCHAR (50) 
                    );

                ALTER TABLE source ADD CONSTRAINT source_PK PRIMARY KEY CLUSTERED (sourceId)
                     WITH (
                     ALLOW_PAGE_LOCKS = ON , 
                     ALLOW_ROW_LOCKS = ON );

                CREATE TABLE ""user"" 
                    (
                     userId INTEGER NOT NULL IDENTITY(1,1), 
                     firstName VARCHAR (50) NOT NULL , 
                     lastName VARCHAR (50) , 
                     email VARCHAR (62) NOT NULL , 
                     password VARCHAR (256) NOT NULL , 
                     address VARCHAR (100) , 
                     city VARCHAR (50) , 
                     photoUrl VARCHAR , 
                     language VARCHAR (5) NOT NULL , 
                     role INTEGER NOT NULL , 
                     paid DATETIME NOT NULL , 
                     registeredDate DATETIME NOT NULL , 
                     updated DATETIME NOT NULL , 
                     lastLogin DATETIME NOT NULL , 
                     emailNotifications BIT NOT NULL , 
                     emailMarketing BIT NOT NULL , 
                     prefferedStyle VARCHAR (50),
                     deletedAt DATETIME,
                     deletedBySelf BIT
                    );

                ALTER TABLE ""user"" ADD CONSTRAINT user_PK PRIMARY KEY CLUSTERED (userId)
                     WITH (
                     ALLOW_PAGE_LOCKS = ON , 
                     ALLOW_ROW_LOCKS = ON );

                CREATE TABLE ""view"" 
                    (
                     viewId INTEGER NOT NULL IDENTITY(1,1), 
                     animeId INTEGER NOT NULL , 
                     userId INTEGER NOT NULL , 
                     created DATETIME NOT NULL , 
                     startTime INTEGER , 
                     endTime INTEGER 
                    );

                ALTER TABLE ""view"" ADD CONSTRAINT view_PK PRIMARY KEY CLUSTERED (viewId)
                     WITH (
                     ALLOW_PAGE_LOCKS = ON , 
                     ALLOW_ROW_LOCKS = ON );

                ALTER TABLE comment 
                    ADD CONSTRAINT comment_anime_FK FOREIGN KEY 
                    ( 
                     animeId
                    ) 
                    REFERENCES anime 
                    ( 
                     animeId 
                    ) 
                    ON DELETE NO ACTION 
                    ON UPDATE NO ACTION;

                ALTER TABLE comment 
                    ADD CONSTRAINT comment_comment_FK FOREIGN KEY 
                    ( 
                     inReplyToId
                    ) 
                    REFERENCES comment 
                    ( 
                     commentId 
                    ) 
                    ON DELETE NO ACTION 
                    ON UPDATE NO ACTION ;

                ALTER TABLE comment 
                    ADD CONSTRAINT comment_user_FK FOREIGN KEY 
                    ( 
                     userId
                    ) 
                    REFERENCES ""user""
                    (
                     userId
                    )
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION;

                ALTER TABLE ""like""
                    ADD CONSTRAINT like_anime_FK FOREIGN KEY
                    (
                     animeId
                    )
                    REFERENCES anime
                    (
                     animeId
                    )
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION;

                ALTER TABLE ""like""
                    ADD CONSTRAINT like_user_FK FOREIGN KEY
                    (
                     userId
                    )
                    REFERENCES ""user""
                    (
                     userId
                    )
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION;

                ALTER TABLE anime_author
                    ADD CONSTRAINT Relation_79_anime_FK FOREIGN KEY
                    (
                     animeId
                    )
                    REFERENCES anime
                    (
                     animeId
                    )
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION;

                ALTER TABLE anime_author
                    ADD CONSTRAINT Relation_79_author_FK FOREIGN KEY
                    (
                     authorId
                    )
                    REFERENCES author
                    (
                     authorId
                    )
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION;

                ALTER TABLE request
                    ADD CONSTRAINT request_user_FK FOREIGN KEY
                    (
                     userId
                    )
                    REFERENCES ""user""
                    (
                     userId
                    )
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION;

                ALTER TABLE request
                    ADD CONSTRAINT request_user_FKv1 FOREIGN KEY
                    (
                     assignedUserId
                    )
                    REFERENCES ""user""
                    (
                     userId
                    )
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION;

                ALTER TABLE source
                    ADD CONSTRAINT source_anime_FK FOREIGN KEY
                    (
                     animeId
                    )
                    REFERENCES anime
                    (
                     animeId
                    )
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION;

                ALTER TABLE ""view""
                    ADD CONSTRAINT view_anime_FK FOREIGN KEY
                    (
                     animeId
                    )
                    REFERENCES anime
                    (
                     animeId
                    )
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION;

                ALTER TABLE ""view""
                    ADD CONSTRAINT view_user_FK FOREIGN KEY
                    (
                     userId
                    )
                    REFERENCES ""user""
                    (
                     userId
                            )
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION;

                ALTER TABLE anime
                    ADD CONSTRAINT anime_anime_FK FOREIGN KEY
                    (
                     parentAnimeId
                    )
                    REFERENCES anime
                    (
                     animeId
                            )
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION;
                ";
            using (Database db = new Database())
            {
                db.Connect();
                using(SqlCommand cmd = db.CreateCommand(createScript))
                {
                    db.ExecuteNonQuery(cmd);
                }
            }
        }

        public static void DestroyTables()
        {
            string deleteScript = @"
                DROP TABLE IF EXISTS request;
                DROP TABLE IF EXISTS ""view"" ;
                DROP TABLE IF EXISTS comment;
                DROP TABLE IF EXISTS ""like"";
                DROP TABLE IF EXISTS source;
                DROP TABLE IF EXISTS anime_author;
                DROP TABLE IF EXISTS anime;
                DROP TABLE IF EXISTS author;
                DROP TABLE IF EXISTS ""user"" ;
                ";
            using (Database db = new Database())
            {
                db.Connect();
                using (SqlCommand cmd = db.CreateCommand(deleteScript))
                {
                    db.ExecuteNonQuery(cmd);
                }
            }
        }

        public static void PrepareAdvancedData()
        {
            UserTable.Insert(new User()
            {
                Firstname = "Daniel",
                Lastname = "Makovský",
                Email = "mak0065@vsb.cz",
                Password = "test123",
                Address = "Čajkovského 2016",
                City = "Karviná",
                Language = "en",
                Role = 0,
                Paid = DateTime.Now,
                LastLogin = DateTime.Now,
                EmailNotifications = true,
                EmailMarketing = true
            });
            UserTable.Insert(new User()
            {
                Firstname = "Admin",
                Lastname = "Admin",
                Email = "Admin@vsb.cz",
                Password = "Admin",
                Address = "Admin",
                City = "Admin",
                Language = "en",
                Role = 1,
                Paid = DateTime.Now,
                LastLogin = DateTime.Now,
                EmailNotifications = true,
                EmailMarketing = true
            });
            UserTable.Insert(new User()
            {
                Firstname = "test123",
                Lastname = "test123",
                Email = "test123@vsb.cz",
                Password = "test123",
                Address = "test123",
                City = "test123",
                Language = "en",
                Role = 1,
                Paid = DateTime.Now,
                LastLogin = DateTime.Now,
                EmailNotifications = true,
                EmailMarketing = true,
                DeletedAt = DateTime.Now,
                DeletedBySelf = true
            });
        }

        public static void InsertEpisodes()
        {
            AnimeTable.Insert(new Anime()
            {
                Name = "Wanna Hurry Up, and Do It?",
                Length = 24,
                EpisodeNumber = 2,
                SeriesNumber = 0,
                Desc = "Short description of My dress-up darling ep2",
                Shortdesc = "Long description of My dress-up darling ep2",
                Viewcount = 1231,
                Language = "jp",
                ParentAnimeId = 1,
            });

            AnimeTable.Insert(new Anime()
            {
                Name = "Then Why Don't We?",
                Length = 24,
                EpisodeNumber = 3,
                SeriesNumber = 0,
                Desc = "Short description of My dress-up darling ep3",
                Shortdesc = "Long description of My dress-up darling ep3",
                Viewcount = 1231,
                Language = "jp",
                ParentAnimeId = 1,
            });

            AnimeTable.Insert(new Anime()
            {
                Name = "Are These Your Girlfriend's?",
                Length = 24,
                EpisodeNumber = 4,
                SeriesNumber = 0,
                Desc = "Short description of My dress-up darling ep4",
                Shortdesc = "Long description of My dress-up darling ep4",
                Viewcount = 1231,
                Language = "jp",
                ParentAnimeId = 1,
            });

            AnimeTable.Insert(new Anime()
            {
                Name = "It's Probably Because This Is the Best Boob Bag Here",
                Length = 24,
                EpisodeNumber = 5,
                SeriesNumber = 0,
                Desc = "Short description of My dress-up darling ep5",
                Shortdesc = "Long description of My dress-up darling ep5",
                Viewcount = 1231,
                Language = "jp",
                ParentAnimeId = 1,
            });

            AnimeTable.Insert(new Anime()
            {
                Name = "For Real?!",
                Length = 24,
                EpisodeNumber = 6,
                SeriesNumber = 0,
                Desc = "Short description of My dress-up darling ep6",
                Shortdesc = "Long description of My dress-up darling ep6",
                Viewcount = 1231,
                Language = "jp",
                ParentAnimeId = 1,
            });

            AnimeTable.Insert(new Anime()
            {
                Name = "A Home Date with the Guy I Wuv Is the Best",
                Length = 24,
                EpisodeNumber = 7,
                SeriesNumber = 0,
                Desc = "Short description of My dress-up darling ep7",
                Shortdesc = "Long description of My dress-up darling ep7",
                Viewcount = 1231,
                Language = "jp",
                ParentAnimeId = 1,
            });
            AnimeTable.Insert(new Anime()
            {
                Name = "Backlighting Is the Best",
                Length = 24,
                EpisodeNumber = 8,
                SeriesNumber = 0,
                Desc = "Short description of My dress-up darling ep8",
                Shortdesc = "Long description of My dress-up darling ep8",
                Viewcount = 1231,
                Language = "jp",
                ParentAnimeId = 1,
            });
            AnimeTable.Insert(new Anime()
            {
                Name = "A Lot Happened After I Saw That Photo",
                Length = 24,
                EpisodeNumber = 9,
                SeriesNumber = 0,
                Desc = "Short description of My dress-up darling ep9",
                Shortdesc = "Long description of My dress-up darling ep9",
                Viewcount = 1231,
                Language = "jp",
                ParentAnimeId = 1,
            });
            AnimeTable.Insert(new Anime()
            {
                Name = "We've All Got Struggles",
                Length = 24,
                EpisodeNumber = 10,
                SeriesNumber = 0,
                Desc = "Short description of My dress-up darling ep10",
                Shortdesc = "Long description of My dress-up darling ep10",
                Viewcount = 1231,
                Language = "jp",
                ParentAnimeId = 1,
            });
            AnimeTable.Insert(new Anime()
            {
                Name = "I Am Currently at a Love Hotel",
                Length = 24,
                EpisodeNumber = 11,
                SeriesNumber = 0,
                Desc = "Short description of My dress-up darling ep11",
                Shortdesc = "Long description of My dress-up darling ep11",
                Viewcount = 1231,
                Language = "jp",
                ParentAnimeId = 1,
            });
            AnimeTable.Insert(new Anime()
            {
                Name = "My Dress-Up Darling",
                Length = 24,
                EpisodeNumber = 12,
                SeriesNumber = 0,
                Desc = "Short description of My dress-up darling ep12",
                Shortdesc = "Long description of My dress-up darling ep12",
                Viewcount = 1231,
                Language = "jp",
                ParentAnimeId = 1,
            });
        }

        public static void InsertComments()
        {
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 1,
                Text = "comment X1",
                Rating = 7,
                InReplyToId = 1,
            });
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 2,
                Text = "comment X2",
                Rating = 7,
                InReplyToId = 1,
            });
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 1,
                Text = "comment X3",
                Rating = 7,
                InReplyToId = 1,
            });
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 1,
                Text = "comment X4",
                Rating = 7,
                InReplyToId = 1,
            });
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 1,
                Text = "comment X5",
                Rating = 7,
                InReplyToId = 1,
            });
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 1,
                Text = "comment X7",
                Rating = 7,
                InReplyToId = 6,
            });
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 1,
                Text = "comment X7",
                Rating = 7,
                InReplyToId = 9,
            });
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 1,
                Text = "comment X7",
                Rating = 7,
                InReplyToId = 10,
            });
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 1,
                Text = "comment X7",
                Rating = 7,
                InReplyToId = 11,
            });
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 3,
                Text = "comment X7",
                Rating = 7,
                InReplyToId = 4,
            });
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 3,
                Text = "omg X7",
                Rating = 7,
                InReplyToId = 4,
            });
            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 3,
                Text = "omg 15",
                Rating = 7,
                InReplyToId = 4,
                HiddenById = 3
            });

            CommentTable.Insert(new Comment()
            {
                AnimeId = 1,
                UserId = 3,
                Text = "omg 15",
                Rating = 7,
                InReplyToId = 15,
            });
        }
    }
}

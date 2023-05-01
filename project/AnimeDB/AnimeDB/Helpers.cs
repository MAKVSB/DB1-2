using AnimeDB.DTO;
using AnimeDB.Tables;

namespace AnimeDB
{
    public class Helpers
    {
        public static void WriteInnerComments(int anime, int commentId, ref int tabs)
        {
            tabs++;
            foreach (Comment c in CommentTable.SelectForParentComment(1, commentId))
            {
                for (int i = 0; i < tabs; i++)
                {
                    Console.Write("\t");
                }
                Console.WriteLine(c);
                if (c.Replies != 0 && c.Replies != null)
                {
                    WriteInnerComments(anime, c.Id, ref tabs);
                }
            }
            tabs--;
        }
    }
}

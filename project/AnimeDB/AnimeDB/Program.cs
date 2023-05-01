using AnimeDB;
using AnimeDB.DBS;
using AnimeDB.DTO;
using AnimeDB.Tables;
using System.Collections.ObjectModel;

Console.WriteLine("Dropping all tables ....... \n");
Setup.DestroyTables();
Console.WriteLine("Creating all tables ....... \n");
Setup.CreateTables();

Console.WriteLine("Inserting default data required for testing advanced functions............ \n");
Setup.PrepareAdvancedData();


Console.WriteLine("Testing function -> 1.1 - Nové anime:");
Console.WriteLine("Debug: {0} row added", AnimeTable.Insert(new Anime()
{
    Name = "My dress-up darling",
    Length = 390,
    EpisodeNumber = 0,
    SeriesNumber = 0,
    Desc = "Short description of My dress-up darling",
    Shortdesc = "Long description of My dress-up darling",
    Viewcount = 123451,
    Language = "jp"
}));
Console.WriteLine($"Res: {AnimeTable.Select(1)}");
Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Testing function -> 1.1 - Nové anime: (epizoda)");
Console.WriteLine("Debug: {0} row added", AnimeTable.Insert(new Anime()
{
    Name = "Someone Who Lives in the Exact Opposite World as Me",
    Length = 24,
    EpisodeNumber = 1,
    SeriesNumber = 0,
    Desc = "Short description of My dress-up darling ep1",
    Shortdesc = "Long description of My dress-up darling ep1",
    Viewcount = 1231,
    Language = "jp",
    ParentAnimeId = 1,
}));
Console.WriteLine($"Res: {AnimeTable.Select(1)}");
Console.WriteLine();
Console.WriteLine();
Setup.InsertEpisodes();



Console.WriteLine("Testing function -> 1.2 - Seznam anime:");
Collection<Anime> animes1 = AnimeTable.Select();
foreach (Anime a in animes1)
{
    Console.WriteLine(a);
}
Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Testing function -> 1.3 - Detail anime:");
Console.WriteLine(AnimeTable.Select(10));
Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Testing function -> 1.4 - Seznam epizod anime:");
Collection<Anime> episodes1 = AnimeTable.SelectEpisodes(1);
foreach (Anime e in episodes1)
{
    Console.WriteLine(e);
}
Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Testing function -> 1.5 - Aktualizace anime:");
Console.WriteLine("Debug: {0} row added", AnimeTable.Insert(new Anime()
{
    Name = "推しの子",
    Length = 190,
    EpisodeNumber = 0,
    SeriesNumber = 0,
    Desc = "Short description of oshi no ko",
    Shortdesc = "Long description of oshi no ko",
    Viewcount = 11,
    Language = "jp",
}));
Anime a14 = AnimeTable.Select(14);
Console.WriteLine($"Old: {a14}");
a14.Name = "Oshi no ko";
AnimeTable.Update(a14);
Console.WriteLine($"Res: {AnimeTable.Select(14)}");
Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Testing function -> 1.6 - Smazání anime:");
Console.WriteLine("Debug: {0} row deleted", AnimeTable.Delete(14));
Anime a14v2 = AnimeTable.Select(14);
if (a14v2 == null)
{
    Console.WriteLine("DOES NOT EXIST");
}
else
{
    Console.WriteLine(a14v2);
}
Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Testing function -> 1.7 - Obnovení anime:");
Anime a14v3 = AnimeTable.Select(14);
if (a14v3 == null)
{
    Console.WriteLine("DOES NOT EXIST");
} else
{
    Console.WriteLine(a14v3);
}
a14.DeletedAt = null;
AnimeTable.Update(a14);
Console.WriteLine(AnimeTable.Select(14));
Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Testing function -> 1.8 - Předchozí a následující anime:");
Console.WriteLine("Searching for episodes near episode 8 of anime ID 1");
(Anime? a5, Anime? a7) = AnimeTable.FindPrevNext(1, 6);
Console.WriteLine(a5);
Console.WriteLine(a7);
Console.WriteLine();
Console.WriteLine();

Console.WriteLine("Testing function -> 1.8 - Předchozí a následující anime:");
Console.WriteLine("Searching for episodes near episode 4 of anime ID 1");
(Anime? aA, Anime? aB) = AnimeTable.FindPrevNext(1, 1);
Console.WriteLine(aA);
Console.WriteLine(aB);
Console.WriteLine();
Console.WriteLine();

Console.WriteLine("Testing function -> 1.9 - Změna pořadí epizod (transtakce):");
AnimeTable.MoveEpisode(9, "down");
AnimeTable.MoveEpisode(9, "down");
AnimeTable.MoveEpisode(9, "down");
AnimeTable.MoveEpisode(9, "down");
Collection<Anime> episodes2 = AnimeTable.SelectEpisodes(1);
foreach (Anime e in episodes2)
{
    Console.WriteLine(e);
}
Console.WriteLine();
Console.WriteLine();


Console.WriteLine("Testing function -> 6.1 - Přidání komentáře:");
Console.WriteLine("Debug: {0} row added", CommentTable.Insert(new Comment()
{
    AnimeId = 1,
    UserId = 1,
    Text = "Nejlepší anime která jsem kdy viděl",
    Rating = 7,
}));
Console.WriteLine($"Res: {CommentTable.Select(1)}");
Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Testing function -> 6.2 - Aktualizace komentáře:");
CommentTable.Insert(new Comment()
{
    AnimeId = 1,
    UserId = 1,
    Text = "Nemyslím si",
    Rating = 7,
    InReplyToId = 1,
});
Comment c2 = CommentTable.Select(2);
Console.WriteLine($"Old: {c2}");
c2.Text = "Možná že máš pravdu";
CommentTable.Update(c2);
Console.WriteLine($"Res: {CommentTable.Select(2)}");
Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Testing function -> 6.3 - Skrytí komentáře:");
CommentTable.Insert(new Comment()
{
    AnimeId = 1,
    UserId = 1,
    Text = "Sprosté slovo :D :D",
    Rating = 7,
    InReplyToId = 1,
});
Comment c3 = CommentTable.Select(3);
Console.WriteLine($"Old: {c3}");
c3.HiddenById = 2;
CommentTable.Update(c3);
Console.WriteLine($"Res: {CommentTable.Select(3)}");
Console.WriteLine();
Console.WriteLine();

Setup.InsertComments();



Console.WriteLine("Testing function -> 6.4 & 6.5 - Seznam komentářů k anime a ke komentáři:");
Console.WriteLine("Výpis formou zanořené struktury:");
Collection<Comment> comments1 = CommentTable.SelectForAnime(1);
int Tabs = 0;
foreach (Comment c in comments1)
{
    Console.WriteLine(c);
    if (c.Replies != 0 && c.Replies != null)
    {
        Helpers.WriteInnerComments(1, c.Id, ref Tabs);
    }
}
Console.WriteLine();
Console.WriteLine();















Database db = new Database();
db.Connect();

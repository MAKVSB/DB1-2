/* 1;1;2367; Vypsat jméno, délku každého anime a datum poslední úpravy*/
SELECT name, length, CONVERT(date, updated)
FROM proj_anime pa

/* 1;2;5; Vypsat komentáře pro anime "Howl no Ugoku Shiro" */
SELECT pc.text, pc.rating
FROM proj_comment pc
	join proj_anime pa on pc.animeId = pa.animeId
WHERE pa.name = 'Howl no Ugoku Shiro'
ORDER BY pc.rating

/* 1;3;1; Vypsat průměrné hodnocení pro anime "Howl no Ugoku Shiro" */
SELECT AVG(pc.rating)
FROM proj_comment pc
	join proj_anime pa on pc.animeId = pa.animeId
WHERE pa.name = 'Howl no Ugoku Shiro'

/* 1;4;100;Vypisuje pro každé anime počet lidí, kteří na anime pracovali seřazené sestupně */
SELECT pan.name, count(paa.authorId)
FROM proj_anime pan
	join proj_anime_author paa on paa.animeId = pan.animeId
WHERE pan.episodeNumber = 0 AND pan.seriesNumber = 0
GROUP BY pan.name
ORDER BY count(paa.authorId) DESC

/* 2;1;23;Vypsat jména anime, které mají v součtu všech epizod déle než 10 hodin, nebo jazyk originálu je EN*/
SELECT pa.name
from proj_anime pa
WHERE pa.episodeNumber = 0 AND pa.seriesNumber = 0 AND (pa.length > 600 OR pa.language = 'EN')

/* 2;2;25; Vypsat jména anime, které mají v součtu všech epizod déle než 10 hodin, nebo jazyk originálu není JP*/
SELECT pa.name
from proj_anime pa
WHERE pa.episodeNumber = 0 AND pa.seriesNumber = 0 AND (pa.length > 600 OR pa.language != 'JP')

/* 2;3;3; Vypsat veškeré autory kteří mají v uměleckém  jménu apostrof */
SELECT pa.authorName
from proj_author pa
WHERE pa.authorName like '%''%' 

/* 2;4;15; Vypsat autory, kteří mají jiné umělecké jméno než pravé, nebo nemají zadané pravé jméno*/
SELECT authorName
from proj_author pa
WHERE authorName != CONCAT(firstName, ' ',lastName) AND NOT lastName = authorName 

/*3;1;41;Výpis všech anime které jsou filmy (nemají epizody ani série).*/
SELECT name
FROM proj_anime
EXCEPT (SELECT name FROM proj_anime WHERE episodeNumber !=  0)

/*3;2;41;Výpis všech anime které jsou filmy (nemají epizody ani série).*/
SELECT name
FROM proj_anime pa
WHERE NOT EXISTS (SELECT DISTINCT name FROM proj_anime pa2 WHERE episodeNumber != 0 and pa.name = pa2.name)

/*3;3;41;Výpis všech anime které jsou filmy (nemají epizody ani série).*/
SELECT name
FROM proj_anime
WHERE name != ALL (SELECT name FROM proj_anime WHERE episodeNumber !=  0)

/*3;4;41;Výpis všech anime které jsou filmy (nemají epizody ani série).*/
SELECT name
FROM proj_anime
WHERE name NOT IN (SELECT name FROM proj_anime WHERE episodeNumber !=  0)

/* 4;1;59; Počet epizod pro každou anime sérii (Z první části projektu: Anime je série pokud existuje záznam kde episodeNumber není 0)*/
SELECT name, max(episodeNumber)
from proj_anime
WHERE episodeNumber != 0
GROUP BY name

/* 4;2;824; Počty autorů se stejným příjmením*/
SELECT pa.lastName, count(pa.firstName)
from proj_author pa
GROUP BY pa.lastName

/* 4;3;4; Pro každý jazyk vypsat délku nejdelšího anime seriálu*/
SELECT pa.language, max(pa.length)
from proj_anime pa
WHERE pa.episodeNumber = 0
GROUP BY pa.language

/* 4;4;2; Vypiš všechny anime, které mají průměr z hodnocení komentářů větší než 4, neohodnocené anime ignorovat*/
SELECT pa.name, AVG(pc.rating) avg
from proj_anime pa
	join proj_comment pc on pc.animeId = pa.animeId
GROUP BY pa.name
HAVING AVG(pc.rating)  > 4

/* 5;1;37; Výpis všech anime, které mají alespoň jeden komentář */
SELECT DISTINCT pa.name
from proj_anime pa
	join proj_comment pc on pc.animeId = pa.animeId
ORDER BY pa.name

/* 5;2;37; Výpis všech anime, které mají alespoň jeden komentář */
SELECT pa.name
from proj_anime pa
WHERE pa.animeId IN (
	SELECT pc.animeId
	from proj_comment pc
)

/* 5;3;100;Výpis všech anime, počtu komentářů, počtu autorů, a počtu zdrojů. Výpis musí obsahovat i anime bez zdroje */
SELECT pa.animeId, pa.name, COUNT(distinct pc.commentId) [Pocet komentaru], COUNT(distinct pau.authorId) [Pocet autoru], COUNT(distinct ps.sourceId) [Pocet zdroju]
FROM proj_anime pa
	LEFT JOIN proj_comment pc on pc.animeId = pa.animeId
	LEFT JOIN proj_anime_author pau on pau.animeId = pa.animeId
	LEFT JOIN proj_source ps on ps.animeId = pa.animeId
WHERE pa.episodeNumber = 0
GROUP BY pa.animeId, pa.name

/* 5;4;7;Výpis všech anime, počtu komentářů, počtu autorů, a počtu zdrojů. Výpis musí obsahovat i anime bez zdroje. Pouze anime na kterých spolupracoval kdokoliv se jménem Shinichi */
SELECT pautour.authorName, pa.animeId, pa.name, COUNT(distinct pc.commentId) [Pocet komentaru], COUNT(distinct pau.authorId) [Pocet autoru], COUNT(distinct ps.sourceId) [Pocet zdroju]
FROM proj_anime pa
	LEFT JOIN proj_comment pc on pc.animeId = pa.animeId
	LEFT JOIN proj_anime_author pau on pau.animeId = pa.animeId
	LEFT JOIN proj_source ps on ps.animeId = pa.animeId
	JOIN proj_author pautour on pautour.authorId = pau.authorId
WHERE pa.episodeNumber = 0 AND pautour.firstName = 'Shinichi'
GROUP BY pautour.authorName, pa.animeId, pa.name

/* 6;1;100; Výpis všech anime a průměrný počet shlédnutí všech jeho epizod */
SELECT pa.name, pa.viewCount, (
	SELECT AVG(pa2.viewCount) 
	FROM proj_anime pa2
	WHERE pa2.episodeNumber != 0 AND pa2.name = pa.name
) averageViewOfEpisodes
FROM proj_anime pa
WHERE pa.episodeNumber = 0

/*6;2;112; Vypíše jméno, příjmení a počet napsaných komentářů pro každého uživatele, který napsal více komentářů než je průměr */
SELECT pc.userId, count(pc.commentId) [commentCount]
from proj_comment pc
GROUP BY pc.userId
HAVING count(pc.commentId) >= (
	SELECT AVG(t.commentCount)
	FROM (
		SELECT pc.userId, count(pc.commentId) [commentCount]
		from proj_comment pc
		GROUP BY pc.userId
	) t
)

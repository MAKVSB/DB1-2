ALTER PROCEDURE SwapEpisodes
	@episode_id int,
	@direction varchar(50)  
AS
	--return @direction
	declare @dirInt int

	IF @direction like 'down'
       SET @dirInt = 1;
	ELSE 
		IF @direction like 'up'
		   SET @dirInt = -1;
		ELSE  
			BEGIN
				SELECT 'Invalid arguments' as [Error]
				RETURN
			END

	declare @parent_anime_id int = -1
	declare @episode_number int = -1

	SELECT @parent_anime_id = parentAnimeId, @episode_number = episodeNumber
	FROM Anime
	WHERE animeId = @episode_id;

	if @parent_anime_id = -1 OR @episode_number = -1
		BEGIN
			SELECT 'Episode not found, or is not an episode.' as [Error]
			RETURN;
		END

	IF (SELECT count(*)
		FROM Anime
		WHERE parentAnimeId = @parent_anime_id AND episodeNumber = @episode_number + @dirInt
	) != 1
		BEGIN
			SELECT 'Move out of range, or episode numbers are not sequencial' as [Error]
			RETURN;
		END
		

	--Upravíme pořadové číslo epizody, se kterou je potřeba epizodu prohodit
	UPDATE Anime SET episodeNumber = @episode_number
	WHERE parentAnimeId = @parent_anime_id AND episodeNumber = @episode_number + @dirInt
	--Nastavíme správné číslo pořadové číslo epizodě, kterou se uživatel snaží posunout
	UPDATE Anime SET episodeNumber = @episode_number + @dirInt
	WHERE animeId = @episode_id

	SELECT 1 AS [Success]
	RETURN;
RETURN 1

GO

exec SwapEpisodes 9, 'down';
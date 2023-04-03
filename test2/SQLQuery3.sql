DELETE FROM game_plays_players
WHERE player_id IN (
	SELECT player_id
	FROM player_info
	WHERE player_info.nationality = 'CZE'
)

DELETE FROM game_skater_stats
WHERE player_id IN (
	SELECT player_id
	FROM player_info
	WHERE player_info.nationality = 'CZE'
)

DELETE FROM game_goalie_stats
WHERE player_id IN (
	SELECT player_id
	FROM player_info
	WHERE player_info.nationality = 'CZE'
)

DELETE FROM player_info
WHERE player_info.nationality = 'CZE'

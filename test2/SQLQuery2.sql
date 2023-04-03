alter table team_info
ADD totalPlayers2 INT NOT NULL DEFAULT 0;


update team_info
SET totalPlayers = (
	SELECT COUNT(DISTINCT player_id) from game_skater_stats
	WHERE team_id = team_info.team_id
)

select * from team_info
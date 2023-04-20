SELECT DISTINCT gts2.team_id, ti1.teamName
from game_teams_stats gts2
	join team_info ti1 on ti1.team_id = gts2.team_id
WHERE not exists (
	SELECT gts1.team_id, g1.season, count(*)
	from game_teams_stats gts1
		join game g1 on g1.game_id = gts1.game_id
	WHERE gts1.won = 'False' AND gts1.team_id = gts2.team_id
	GROUP BY gts1.team_id, g1.season
	HAVING count(*) > 50
)

with 
	saves as (
		SELECT ggs1.player_id, ggs1.game_id, ggs1.saves as saves
		from game_goalie_stats ggs1
	),
	maxes as (
		SELECT saves.game_id, Max(saves.saves) as maxSaves
		from saves
		GROUP BY saves.game_id
	)
SELECT DISTINCT ggs1.player_id, pi1.firstName, pi1.lastName, s1.saves, m1.maxSaves, g1.game_id
from game g1
	join game_goalie_stats ggs1 on ggs1.game_id = g1.game_id
	join saves s1 on s1.game_id = g1.game_id AND ggs1.player_id = s1.player_id
	join maxes m1 on m1.game_id = g1.game_id
	join player_info pi1 on pi1.player_id = ggs1.player_id

WHERE cast(g1.date_time_GMT as date) = '2019-11-11' AND
		s1.saves = m1.maxSaves
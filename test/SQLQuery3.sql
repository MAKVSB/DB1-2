select DISTINCT ggs1.player_id, pi1.firstName, ti1.teamName, (
	SELECT count(*)
	from game_plays gp2
		join game_plays_players gpp2 on gpp2.play_id = gp2.play_id
	WHERE gp2.secondaryType = 'Hi-sticking' AND gpp2.playerType = 'PenaltyOn' AND gpp2.player_id = ggs1.player_id
	group by gpp2.player_id
)
from game_goalie_stats ggs1
	join game g1 on g1.game_id = ggs1.game_id
	join player_info pi1 on pi1.player_id = ggs1.player_id
	join team_info ti1 on ggs1.team_id = ti1.team_id
WHERE g1.season = '20012002' AND ti1.teamName = 'Avalanche';
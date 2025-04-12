CREATE OR REPLACE TRIGGER TDeleteGameEvent BEFORE DELETE ON Game_plays FOR EACH ROW
DECLARE
    v_homeID NUMBER;
    v_goalCount NUMBER;
BEGIN
    IF :old.event = 'Goal' THEN
        SELECT home_team_id INTO v_homeID FROM game WHERE game_id = :old.game_id;
        IF :old.team_id_for = v_homeID THEN
            SELECT home_goals INTO v_goalCount FROM Game WHERE game_id = :old.game_id;
            UPDATE Game SET home_goals = v_goalCount-1 WHERE game_id = :old.game_id;
        ELSE
            SELECT away_goals INTO v_goalCount FROM Game WHERE game_id = :old.game_id;
            UPDATE Game SET away_goals = v_goalCount-1 WHERE game_id = :old.game_id;
        END IF;
        SELECT goals NITO v_goalCount FROM Game_teams_stats WHERE game_id = :old.game_id and team_id = :old.team_id_for;
        UPDATE Game_teams_stats SET goals = v_goalCount-1 WHERE game_id = :old.game_id and team_id = :old.team_id_for;
    END IF;
    IF :old.event = 'Shot' THEN
        SELECT shots NITO v_goalCount FROM Game_goalie_stats WHERE game_id = :old.game_id and team_id = :old.team_id_for;
        UPDATE Game_teams_stats SET shots = v_goalCount-1 WHERE game_id = :old.game_id and team_id = :old.team_id_for;
    END IF;
END;
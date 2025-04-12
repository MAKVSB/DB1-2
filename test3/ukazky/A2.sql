create or replace FUNCTION Test3_2 RETURN NUMBER AS
    v_counter NUMBER;
    v_teams NUMBER := 0;
BEGIN
    FOR c_team in (select distinct team_id from team_info) LOOP
        select count(*) into v_counter
        from game_plays 
                join game_plays_players on game_plays.play_id = game_plays_players.play_id
        where event = 'Penalty' and playertype = 'DrewBy' and team_id_for = c_team.team_id;
        IF v_counter < 750 THEN
            update team_info set aggresivity = 0 where team_id = c_team.team_id;
        ELSIF v_counter > 1250 THEN
            update team_info set aggresivity = 2 where team_id = c_team.team_id;
        ELSE 
            update team_info set aggresivity = 1 where team_id = c_team.team_id;
        END IF;
        v_teams := v_teams + 1;
    END LOOP;
    COMMIT;
    RETURN v_teams;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RETURN 0;
END;
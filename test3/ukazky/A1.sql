CREATE OR REPLACE PROCEDURE myProc(v_id game.game_id%TYPE, v_name game_officials.official_name%TYPE, v_type game_officials.official_type%TYPE) AS
    v_counter INT := 0;
    v_activeGames INT := 0;
    v_season INT;
BEGIN
    SELECT COALESCE(COUNT(*), 0) INTO v_counter FROM game where game_id = v_id;
    IF v_counter = 0 THEN
        print('GAME NOT FOUND');
        RETURN;
    ELSIF v_type != 'referee' AND v_type != 'linesman' THEN
        print('Wrong referee type');
        RETURN;
    END IF;

    SELECT season
    INTO v_season 
    FROM game 
    WHERE game_id = v_id;
    
    SELECT count(*)
    INTO v_activeGames
    FROM game
    JOIN game_officials ON game_officials.game_id = game.game_id
    WHERE game_officials.official_name = v_name AND game.season = v_season;
    
    IF v_activeGames > 100 THEN
        print('referee is pretizen');
        RETURN;
    ELSE
        print('Inserting...');
        INSERT INTO game_officials VALUES (v_id, v_name, v_type);
    END IF;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
    ROLLBACK;
END;

EXECUTE myProc(2000020123, 'Tim Nowak', 'referee');
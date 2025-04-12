CREATE OR REPLACE FUNCTION CopyOfficial RETURN NUMBER AS
    v_id NUMBER := 0;
    v_ref NUMBER;
    v_line NUMBER;
    v_type VARCHAR2(20);
    v_name VARCHAR2(100);
BEGIN
    FOR c_ofic IN (SELECT DISTINCT official_name FROM game_officials) LOOP
        v_id := v_id + 1;
        v_anme := c_ofic.official_name;
        SELECT coalesce(count(game_id), 0) into v_ref from game_officials where v_name and official_type = 'Referee';
        SELECT coalesce(count(game_id), 0) into v_line from game_officials where v_name and official_type = 'Linesman';
        IF v_ref = v_line THEN
            v_type := 'unknown';
        ELSIF v_ref > v_line THEN
            v_type := 'referee';
        ELSE
            v_type := 'linesman';
        END IF;
        INSERT INTO officials(official_id, official_name, official_type) values (v_id, v_name, v_type);
    END LOOP;
    RETURN v_id;
EXCEPTION
    WHEN OTHERS THEN
        RETURN 0;
END;
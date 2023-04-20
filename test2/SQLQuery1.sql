create table venue
(
	venue_id INT IDENTITY PRIMARY KEY NOT NULL,
	name varchar(100),
);

insert into venue (name)
SELECT DISTINCT game.venue
from game

alter table game
ADD venue_id INT NULL CONSTRAINT gm_venue_id FOREIGN KEY REFERENCES venue (venue_id);

update game
SET game.venue_id = (
SELECT venue.venue_id from venue where venue.name = game.venue
)


select game.venue, game.venue_id, venue.*
from game join venue on venue.venue_id = game.venue_id
ALTER TABLE team_info
drop DF_team_info_created_at
go 
ALTER TABLE team_info
drop column created_at
go




ALTER TABLE team_info
ADD created_at DATETIME NOT NULL
	CONSTRAINT DF_team_info_created_at DEFAULT CURRENT_TIMESTAMP;

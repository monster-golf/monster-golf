--select * from mg_Users where  LastName = 'Gies'

-- Thayer
update mg_users set Handicap = 21.5 where UserId = 359
--Callahan
update mg_users set Handicap = 2.2 where UserId = 420
--Levitt, M
update mg_users set Handicap = 1.9 where UserId = 52
--Levitt, R
update mg_users set Handicap = 14.0 where UserId = 377
--Clark
update mg_users set Handicap = 5.0 where UserId = 124
--McCrory
update mg_users set Handicap = 6.3 where UserId = 430
--Haseleu
update mg_users set Handicap = 7.5 where UserId = 366
--Francis, S
update mg_users set Handicap = 11.0 where UserId = 355
--Francis, J
update mg_users set Handicap = 17.9 where UserId = 423
--Kosanovich
update mg_users set Handicap = 27.7 where UserId = 426
--Holso
update mg_users set Handicap = 11.8 where UserId = 425
--Gulbransen
update mg_users set Handicap = 30.3 where UserId = 404
--Podolak, D
update mg_users set Handicap = 27.1 where UserId = 373
--Podolak, W
update mg_users set Handicap = 14.3 where UserId = 367
--Dadisman
update mg_users set Handicap = 15.4 where UserId = 376
--Hinners
update mg_users set Handicap = 7.3 where UserId = 175
--McDowell
update mg_users set Handicap = 8.5 where UserId = 418
--Cruzan
update mg_users set Handicap = 22.6 where UserId = 402
--Pullis
update mg_users set Handicap = 9.3 where UserId = 427
--Wurmlinger [found no record]
update mg_users set Handicap = 12.7 where UserId = 428
--Stork
update mg_users set Handicap = 10.5 where UserId = 100
--Redfern
update mg_users set Handicap = 12.5 where UserId = 49
--Irwin
update mg_users set Handicap = 14.6 where UserId = 230
--Erie
update mg_users set Handicap = 12.3, FirstName='Curt',LastName='Erie' where UserId = 313
--Walker
update mg_users set Handicap = 19.7,LastName='Walker' where UserId = 219
--Haug
update mg_users set Handicap = 8.3 where UserId = 431
--C Gies
update mg_users set Handicap = 21.6 where UserId = 429

-- delete collette
delete mg_TourneyTeamPlayers 
where TeamID = 
	(Select TeamID from mg_TourneyTeams where TeamName = 'Podolak - Podolak' and TournamentID = 22)
delete mg_TourneyTeams where TeamName = 'Podolak - Podolak' and TournamentID = 22


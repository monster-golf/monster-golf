-- This will get a list of teams and handicaps for the tournament
Declare @tId int;
Set @tId = 23;
select tt.TeamId, tt.Flight, ttp1.UserId, ttu1.FirstName, ttu1.LastName, ttu1.WebId, ttu1.HcpIndex as 'TourneyUserHCP', ttp1.Handicap as 'TourneyTeamPlayersHCP', u1.Handicap as 'UsersHCP', ts1_1.HCP AS 'TourneyScoresR1HCP', ts1_2.HCP AS 'TourneyScoresR2HCP',
	u1.GHIN as 'HCP Location', s1.DateOfRound, s1.DateEntered,
	ttp2.UserID, ttu2.FirstName, ttu2.LastName, ttu2.WebId, ttu2.HcpIndex as 'TourneyUserHCP', ttp2.Handicap as 'TourneyTeamPlayersHCP', u2.Handicap as 'UsersHCP', ts2_1.HCP AS 'TourneyScoresR1HCP', ts2_2.HCP AS 'TourneyScoresR2HCP', u2.GHIN as 'HCP Location', s1.DateOfRound, s1.DateEntered
from mg_TourneyTeams tt 
	join mg_TourneyTeamPlayers ttp1 on tt.TeamId = ttp1.TeamId AND ttp1.UserID = (Select Min(UserId) from mg_TourneyTeamPlayers innerttp1  WHERE innerttp1.TeamId = tt.TeamId)
	join mg_TourneyUsers ttu1 on ttu1.UserId = ttp1.UserId 
	join mg_Users u1 on u1.UserId = ttu1.WebId
	left join mg_scores s1 on s1.ID = (select Max(ID) from mg_scores WHERE UserId = u1.UserID)
	left join mg_Tourneyscores ts1_1 on ts1_1.UserId = u1.UserID AND ts1_1.TourneyId = tt.TournamentId AND ts1_1.RoundNum = 1
	left join mg_Tourneyscores ts1_2 on ts1_2.UserId = u1.UserID AND ts1_2.TourneyId = tt.TournamentId AND ts1_2.RoundNum = 2
	join mg_TourneyTeamPlayers ttp2 on tt.TeamId = ttp2.TeamId AND ttp2.UserID = (Select Max(UserId) from mg_TourneyTeamPlayers innerttp2  WHERE innerttp2.TeamId = tt.TeamId)
	join mg_TourneyUsers ttu2 on ttu2.UserId = ttp2.UserId 
	join mg_Users u2 on u2.UserId = ttu2.WebId
	left join mg_scores s2 on s2.ID = (select Max(ID) from mg_scores WHERE UserId = u2.UserID)
	left join mg_Tourneyscores ts2_1 on ts2_1.UserId = u2.UserID AND ts2_1.TourneyId = tt.TournamentId AND ts2_1.RoundNum = 1
	left join mg_Tourneyscores ts2_2 on ts2_2.UserId = u2.UserID AND ts2_2.TourneyId = tt.TournamentId AND ts2_2.RoundNum = 2
where tt.TournamentId = @tId
order by tt.Flight, (ttp1.Handicap+ttp2.Handicap), tt.teamid

select * from mg_scores
select *from mg_tourneyscores where TourneyId=23 and RoundNum=1 order by StartingHole, GroupId, Name;
update mg_tourneyscores set StartingHole = NULL WHERE TourneyId=23 
select * from mg_Tourneyscores where tourneyId = 23 
select * from mg_tourneyTeamplayers where tournamentid = 23 TeamID = 1434 userId = 161 order by TeamID
select * from mg_tourneyscores where UserId In (select WebId from mg_tourneyUsers where UserId In (161,162))
select * from mg_tourneyUsers where UserId In (161,162)
select * from mg_Tourneyscores where UserID = 161 and TourneyId = 23
select * from mg_tourney


update mg_users set Handicap = 5.4 where UserId = 451
select UserId,GroupId,Name,StartingHole,* from mg_tourneyscores ts
join mg_tourneyTeamPlayers tp on tp.WebId = ts.UserId
 where TourneyId=23

select t.TournamentID, Location, Slogan, Description, tc.CourseID, tc.[Round], Course, DateOfRound, ID, TeeNumber, Slope, Rating, Par1, Par2, Par3, Par4, Par5, Par6, Par7, Par8, Par9, Par10, Par11, Par12, Par13, Par14, Par15, Par16, Par17, Par18, Handicap1, Handicap2, Handicap3, Handicap4, Handicap5, Handicap6, Handicap7, Handicap8, Handicap9, Handicap10, Handicap11, Handicap12, Handicap13, Handicap14, Handicap15, Handicap16, Handicap17, Handicap18 
from mg_Tourney t 
join mg_tourneycourses tc on tc.tournamentid = t.tournamentid 
join mg_TourneyCourseDetails tcd on tcd.CourseID = tc.CourseId 
where t.TournamentID = @tId 
order by tc.[Round], TeeNumber

select MobileEmail, Email, ttu.FirstName + ' ' + ttu.LastName as Name, u.userid from mg_TourneyTeams tt 
	join mg_TourneyTeamPlayers ttp on tt.TeamId = ttp.TeamId 
	join mg_TourneyUsers ttu on ttu.UserId = ttp.UserId 
	join mg_users u on ttu.WebId = u.UserId
where tt.TournamentId = @tId
order by name


--update mg_users set mobileemail = 'dougorangewhip@gmail.com' where userid = 29
--update mg_users set mobileemail = 'monster@monstergolf.org' where userid = 313
--update mg_users set mobileemail = 'aaronwald@hotmail.com' where userid = 257

select tp.* from mg_TourneyTeamPlayers tp
 where tp.tournamentId =@tId
 
 select * from mg_tourneyusers
 
 
select * from mg_tourneyScores where tourneyId =@tId
order by roundnum, name


select tp.*, ts.hcp, tu.hcpindex, ts.name from mg_TourneyTeamPlayers tp
join mg_tourneyusers tu on tp.userId = tu.userId
left join mg_tourneyScores ts on ts.UserId = tu.webid and ts.tourneyid = tp.tournamentid and ts.roundnum = 1
 where tp.tournamentId =@tId
 
-- update mg_TourneyTeamPlayers set mg_TourneyTeamPlayers.Handicap = ts.hcp
-- FROM mg_TourneyTeamPlayers tp
--join mg_tourneyusers tu on tp.userId = tu.userId
--join mg_tourneyScores ts on ts.UserId = tu.webid and ts.tourneyid = tp.tournamentid and ts.roundnum = 1
-- where tp.tournamentId =@tId
 

-- score with course 
 select ttp.ID, ttu.UserId, mu.Handicap, ttu.WebId, Max(mc.DateOfRound)  
 from mg_TourneyTeamPlayers ttp 
 join mg_TourneyUsers ttu on ttu.UserId = ttp.UserId 
 join mg_users mu on mu.UserId = ttu.WebId 
 join mg_tourneyCourses mc on mc.TournamentId = ttp.TournamentId 
 where ttp.TournamentId = @tId
 GROUP BY ttp.ID, ttu.UserId, mu.Handicap, ttu.WebId
 
 select * from mg_tourneyscores where tourneyid >= @tId and userid = 370
 
 SELECT tp.TeamID, t.TeamName, t.Flight, tp.TeeNumber, tp.Handicap AS TPHandicap, ts.*, u.LastName, u.FirstName, u.Image 
 FROM mg_tourneyTeamPlayers AS tp 
	INNER JOIN mg_tourneyTeams AS t on t.TeamID = tp.TeamID
	INNER JOIN mg_tourneyUsers AS u on u.userid = tp.UserID 
	LEFT OUTER JOIN mg_TourneyScores AS ts on u.WebID = ts.UserID AND tp.TournamentID = ts.TourneyId 
WHERE tp.TournamentID = 23 ORDER BY ts.RoundNum, tp.TeamID, u.LastName, u.FirstName


select * from mg_TourneyScores

select * from mg_TourneyCourses order by courseid desc
--update  mg_TourneyCourses set DateOfRound = '5-17-2014 13:00'
--where CourseId = 47

--update mg_tourney set Slogan = 'Monster XXIV', Location = 'Chandler, AZ', Description= '2014' WHERE TournamentID = 23

select * from mg_tourneyTeams
where tournamentid = @tId


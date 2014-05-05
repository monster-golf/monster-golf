-- This will get a list of teams and handicaps for the tournament
Declare @tId int;
Set @tId = 23;
select tt.TeamId,
	(u1.Handicap+u2.Handicap) as TeamHCP, tt.Flight, 
	ttp1.UserId, ttu1.FirstName, ttu1.LastName, ttu1.WebId, ttu1.HcpIndex as 'TourneyUserHCP', ttp1.Handicap as 'TourneyTeamPlayersHCP', u1.Handicap as 'UsersHCP', ts1_1.HCP AS 'TourneyScoresR1HCP', ts1_2.HCP AS 'TourneyScoresR2HCP',
	u1.Email, u1.MobileEmail,
	u1.GHIN as 'HCP Location', s1.DateOfRound, s1.DateEntered,
	ttp2.UserID, ttu2.FirstName, ttu2.LastName, ttu2.WebId, ttu2.HcpIndex as 'TourneyUserHCP', ttp2.Handicap as 'TourneyTeamPlayersHCP', u2.Handicap as 'UsersHCP', ts2_1.HCP AS 'TourneyScoresR1HCP', ts2_2.HCP AS 'TourneyScoresR2HCP', 
	u2.Email, u2.MobileEmail, u2.GHIN as 'HCP Location', 
	s2.DateOfRound, s2.DateEntered
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
order by tt.Flight, (u1.Handicap+u2.Handicap), tt.teamid

select * from mg_scores

select distinct s.*, t.TeamId from mg_tourneyscores s 
join mg_tourneyUsers u on u.WebId = s.UserId
join mg_tourneyTeamplayers t on t.UserId = u.UserId and t.TournamentId = TourneyId
where TourneyId=23 and RoundNum=1 
order by StartingHole, GroupId, TeamId, Name;

select * from delete mg_Tourneyscores where tourneyId = 23 and name like '%Fen%'
select * from mg_tourneyTeamplayers where tournamentid = 23 TeamID = 1434 userId = 161 order by TeamID
select * from mg_tourneyscores where UserId In (select WebId from mg_tourneyUsers where UserId In (161,162))
select * from mg_tourneyUsers where UserId In (161,162)
select * from mg_Tourneyscores where UserID = 161 and TourneyId = 23

select ttu.WebId, ttu.FirstName + ' ' + ttu.LastName as PlayerName, ttp.Handicap, ttp.TeeNumber from mg_TourneyTeamPlayers ttp join mg_TourneyUsers ttu on ttu.UserId = ttp.UserId where ttp.TournamentId = 23

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

select tp.* from mg_TourneyTeamPlayers tp
 where tp.tournamentId =@tId
 
 select * from mg_tourneyusers
 
 
select * from mg_tourneyScores where tourneyId =@tId
order by roundnum, name


select tp.*, ts.hcp, tu.hcpindex, ts.name from mg_TourneyTeamPlayers tp
join mg_tourneyusers tu on tp.userId = tu.userId
left join mg_tourneyScores ts on ts.UserId = tu.webid and ts.tourneyid = tp.tournamentid and ts.roundnum = 1
 where tp.tournamentId =@tId
  
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

select * from mg_tourneyTeams
where tournamentid = @tId


insert into mg_TourneyScores (TourneyId, RoundNum, UserId, Name, UserLookup, CourseName, CourseSlope, CourseRating) select c.TournamentId, c.[Round], 27, 'Dewey Wald', 'e07b75b4a1', c.Course, d.Slope, Round(d.Rating,1) from mg_tourneycourses c join mg_TourneyCourseDetails d on d.CourseId = c.CourseId AND d.TeeNumber = 0 where c.TournamentId = 23 and c.[Round] = 1 and  NOT Exists(select * from mg_TourneyScores WHERE TourneyId = 23 and RoundNum = 1 and userID = 27);

select * from mg_TourneyScores WHERE TourneyId = 23 and RoundNum = 1 and userID = 27;

select * from mg_TourneyScores WHERE TourneyId = 23 and RoundNum = 1 and userID = 27;
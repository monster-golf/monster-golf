-- This will get a list of teams and handicaps for the tournament
Declare @tId int;
Set @tId = 22;
select tt.TeamId, tt.Flight, ttp1.UserId, ttu1.FirstName, ttu1.LastName, ttu1.WebId, ttu1.HcpIndex, ttp1.Handicap, u1.Handicap, u1.GHIN,
	ttp2.UserID, ttu2.FirstName, ttu2.LastName, ttu2.WebId, ttu2.HcpIndex, ttp2.Handicap, u2.Handicap, u2.GHIN
from mg_TourneyTeams tt 
	join mg_TourneyTeamPlayers ttp1 on tt.TeamId = ttp1.TeamId AND ttp1.UserID = (Select Min(UserId) from mg_TourneyTeamPlayers innerttp1  WHERE innerttp1.TeamId = tt.TeamId)
	join mg_TourneyUsers ttu1 on ttu1.UserId = ttp1.UserId 
	join mg_Users u1 on u1.UserId = ttu1.WebId
	join mg_TourneyTeamPlayers ttp2 on tt.TeamId = ttp2.TeamId AND ttp2.UserID = (Select Max(UserId) from mg_TourneyTeamPlayers innerttp2  WHERE innerttp2.TeamId = tt.TeamId)
	join mg_TourneyUsers ttu2 on ttu2.UserId = ttp2.UserId 
	join mg_Users u2 on u2.UserId = ttu2.WebId
where tt.TournamentId = @tId
order by tt.Flight, (ttp1.Handicap+ttp2.Handicap), tt.teamid


select t.TournamentID, Location, Slogan, Description, tc.CourseID, tc.[Round], Course, DateOfRound, ID, TeeNumber, Slope, Rating, Par1, Par2, Par3, Par4, Par5, Par6, Par7, Par8, Par9, Par10, Par11, Par12, Par13, Par14, Par15, Par16, Par17, Par18, Handicap1, Handicap2, Handicap3, Handicap4, Handicap5, Handicap6, Handicap7, Handicap8, Handicap9, Handicap10, Handicap11, Handicap12, Handicap13, Handicap14, Handicap15, Handicap16, Handicap17, Handicap18 
from mg_Tourney t 
join mg_tourneycourses tc on tc.tournamentid = t.tournamentid 
join mg_TourneyCourseDetails tcd on tcd.CourseID = tc.CourseId 
where t.TournamentID = 22 
order by tc.[Round], TeeNumber

select * from mg_tourneyScores where emailgroup = 1

select MobileEmail, Email, ttu.FirstName + ' ' + ttu.LastName as Name, u.userid from mg_TourneyTeams tt 
	join mg_TourneyTeamPlayers ttp on tt.TeamId = ttp.TeamId 
	join mg_TourneyUsers ttu on ttu.UserId = ttp.UserId 
	join mg_users u on ttu.WebId = u.UserId
where tt.TournamentId = 22
order by name


update mg_users set mobileemail = 'dougorangewhip@gmail.com' where userid = 29
update mg_users set mobileemail = 'monster@monstergolf.org' where userid = 313
update mg_users set mobileemail = 'aaronwald@hotmail.com' where userid = 257

select tp.* from mg_TourneyTeamPlayers tp
 where tp.tournamentId =19
 
 select * from mg_tourneyusers
 
 
select * from mg_tourneyScores where tourneyId =22
order by roundnum, name

2013-06-15 13:30:00.000
2013-06-14 09:00:00.000

select tp.*, ts.hcp, tu.hcpindex, ts.name from mg_TourneyTeamPlayers tp
join mg_tourneyusers tu on tp.userId = tu.userId
left join mg_tourneyScores ts on ts.UserId = tu.webid and ts.tourneyid = tp.tournamentid and ts.roundnum = 1
 where tp.tournamentId =22
 
 update mg_TourneyTeamPlayers set mg_TourneyTeamPlayers.Handicap = ts.hcp
 FROM mg_TourneyTeamPlayers tp
join mg_tourneyusers tu on tp.userId = tu.userId
join mg_tourneyScores ts on ts.UserId = tu.webid and ts.tourneyid = tp.tournamentid and ts.roundnum = 1
 where tp.tournamentId =19
 

-- score with course 
 select ttp.ID, ttu.UserId, mu.Handicap, ttu.WebId, Max(mc.DateOfRound)  
 from mg_TourneyTeamPlayers ttp 
 join mg_TourneyUsers ttu on ttu.UserId = ttp.UserId 
 join mg_users mu on mu.UserId = ttu.WebId 
 join mg_tourneyCourses mc on mc.TournamentId = ttp.TournamentId 
 where ttp.TournamentId = 22
 GROUP BY ttp.ID, ttu.UserId, mu.Handicap, ttu.WebId
 
 select * from mg_tourneyscores where tourneyid >= 22 and userid = 370
 update mg_tourneyscores set HCP = 13 where tourneyid >= 22 and userid = 370
 
 SELECT tp.TeamID, t.TeamName, t.Flight, tp.TeeNumber, tp.Handicap AS TPHandicap, ts.*, u.LastName, u.FirstName, u.Image 
 FROM mg_tourneyTeamPlayers AS tp 
	INNER JOIN mg_tourneyTeams AS t on t.TeamID = tp.TeamID
	INNER JOIN mg_tourneyUsers AS u on u.userid = tp.UserID 
	LEFT OUTER JOIN mg_TourneyScores AS ts on u.WebID = ts.UserID AND tp.TournamentID = ts.TourneyId 
WHERE tp.TournamentID = 19 ORDER BY ts.RoundNum, tp.TeamID, u.LastName, u.FirstName


select * from mg_TourneyScores

select * from mg_TourneyCourses
update  mg_TourneyCourses set DateOfRound = '6/15/2013 13:30'
where CourseId = 42

select Distinct(Round) as Round from mg_TourneyScores where TourneyId = 2

select * from mg_tourney

update mg_tourney set Slogan = 'Monster XXIII', Location = 'Tucson, AZ', Description= '2013' WHERE TournamentID = 22

select * from mg_tourneyTeams
where tournamentid = 20



select * from mg_tourneycourses

select * from mg_tourneycourseDetails
where courseid = 39

update mg_tourneycourses set Course = 'Badlands: Desperado-Outlaw' where courseId = 39
update mg_tourneycourses set Course = 'Badlands: Diablo-Outlaw' where courseId = 40

select * from mg_tourneyScores where [roundnum]=2 and tourneyid = 21

update mg_tourneyscores set CourseName = 'Badlands: Diablo-Outlaw', EmailSent = 0
where [roundnum]=2 and tourneyid = 21

update mg_tourneyCourseDetails set
Par1=4,
Par2=5,
Par3=4,
Par4=4,
Par5=3,
Par6=4,
Par7=3,
Par8=5,
Par9=4,
Par10=4,
Par11=3,
Par12=4,
Par13=4,
Par14=4,
Par15=5,
Par16=4,
Par17=3,
Par18=5,
Handicap1=17,
Handicap2=11,
Handicap3=1,
Handicap4=5,
Handicap5=15,
Handicap6=7,
Handicap7=13,
Handicap8=9,
Handicap9=3,
Handicap10=14,
Handicap11=18,
Handicap12=16,
Handicap13=10,
Handicap14=12,
Handicap15=4,
Handicap16=8,
Handicap17=6,
Handicap18=2
where courseid = 40
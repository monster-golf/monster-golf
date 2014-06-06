select u.userid, tu.FIrstName, tu.LastName, GHIN, u.lastName, 
u.firstName, u.handicap, tu.hcpIndex,
u.email, u.mobileemail
, Max(s.DateEntered) as LastScore
, Count(s.ID) as TotalScores
from mg_TourneyUsers tu
left join mg_Users u on tu.webid = u.UserId 
join mg_tourneyTeamplayers tt on tt.UserId = tu.UserId and tt.TournamentId = 21
left join mg_scores s on u.userId = s.userId
--where GHIN IS NOT NULL and GHIN <> ''
--where mobileemail is null or mobileemail = ''
Group by u.userid, tu.LastName,tu.FIrstName, GHIN, u.lastName, u.firstName, u.handicap, tu.hcpIndex,u.email, u.mobileemail
order by TotalScores ASC


select * from mg_Users order by userid DESC

select * from mg_Tourney order by tournamentId Desc
select * from mg_TourneyTeams order by tournamentId Desc
select * from mg_TourneyCourses order by tournamentId Desc
select * from mg_TourneyTeams order by tournamentId Desc


select * from mg_TourneyUsers order by lastname asc
update mg_tourneyUsers set WebId = 55 where userId = 78
update mg_users set mobileemail = 'grobertson@russocorp.com' where userid = 392

update mg_users set mobileemail = 'jonhanson22@gmail.com' where userid = 398

update mg_users set email = 'monster@monstergolf.org' where userid = 405

select * from mg_Tourney
select * from mg_TourneyUsers where lastname = 'Perna' order by userID desc
select * from mg_tourneyTeams where tournamentId = 21
select * from mg_tourneyTeamPlayers where tournamentId = 21
order by userid

select * from mg_groups where friendid = 408
select * from mg_handicaps order by dateposted desc
 where userid = 408

select * from mg_TourneyScores where tourneyId = 21
alter table mg_TourneyScores add EmailSent bit not null constraint df_mg_TourneyScores_EmailSent DEFAult (0)

select
select * from mg_t
select * from mg_users where lastName = 'Larson' where userid = 18
select * from mg_TourneyUsers where lastName = 'Bichler'
select * from mg_TourneyTeams where tournamentid = 20 = 292 
select * from mg_tourneyScores where tourneyid = 20 and emailsent = 0
update mg_tourneyScores set emailsent = 0 where tourneyid = 20 and userID = 7
where userid in (7) and tourneyid = 20



select * from mg_tourneyscores m1 
where m1.TourneyId=20 and m1.RoundNum=1 and m1.GroupId='380d6be537'

update mg_tourneyscores SET
Hole1 =NULL
,Hole2 =NULL
,Hole3 =NULL
,Hole4 =NULL
,Hole5 =NULL
,Hole6 =NULL
,Hole7 =NULL
,Hole8 =NULL
,Hole9 =NULL
,Hole10 =NULL
,Hole11 =NULL
,Hole12 =NULL
,Hole13 =NULL
,Hole14 =NULL
,Hole15 =NULL
,Hole16 =NULL
,Hole17 =NULL
,Hole18 =NULL
,CardSigned = 0
,CardAttested = 0
where TourneyId=20 and RoundNum=1 and GroupId='380d6be537'

	join mg_tourneyscores m2 on m2.GroupId = m1.GroupId 
where m2.TourneyId=20 and m2.RoundNum=1 and m2.GroupId='380d6be537'

select Distinct t.Location, t.Slogan, t.[Description], t.NumRounds, tc.Course, tc.[Round], tc.DateOfRound, ts.GroupId from mg_Tourney t
join mg_tourneyCourses tc on tc.tournamentid = t.tournamentid
join mg_tourneyScores ts on ts.tourneyid = t.tournamentid
where t.tournamentId = 20 and ts.EmailSent <> 0

select DateOfRound from mg_tourneyCourses where tournamentId = 20 and [round] = 1
select * from m
--select * from mg_Users where userid in (402,411)

Declare @lastName varchar(20), @hcp float, @ghin varchar(10), @email varchar(50)

set @lastName = 'Hauge'
set @hcp = 11.8
set @ghin = 'GHIN-AZ'
set @email = ''
 
select u.UserId, u.GHIN, u.Handicap, u.MobileEmail, u.Email, u.FirstName, u.LastName, u.UserName, Max(s.DateEntered) 
from mg_Users u
left join mg_scores s on u.userId = s.userId
where lastname = @lastName
group by u.UserId, u.GHIN, u.Handicap, u.MobileEmail, u.Email, u.FirstName, u.LastName, u.UserName


update mg_Users set MobileEmail = Case when @email = '' THEN MobileEmail ELSE @email END, 
handicap = @hcp,
 GHIN = @ghin,
Email = Case WHEN Email = 'monster@monstergolf.org' THEN @email ELSE Email END
where lastname =@lastName

-- get in tournament list
select tu.userid, tu.firstName, tu.lastname, tu.webid, GHIN, u.lastName, u.firstName, u.handicap, tu.hcpIndex,tt.handicap, tt.TeeNumber,u.email, u.mobileemail, Max(s.DateEntered) as LastScore 
from mg_TourneyUsers tu
left join mg_Users u on tu.webid = u.UserId 
join mg_tourneyTeamplayers tt on tt.UserId = tu.UserId and tt.TournamentId = 22
left join mg_scores s on u.userId = s.userId
--where GHIN IS NOT NULL and GHIN <> ''
Group by tu.userid, tu.firstName, tu.lastname, tu.webid, GHIN, u.lastName, u.firstName, u.handicap, tu.hcpIndex,tt.handicap, tt.TeeNumber,u.email, u.mobileemail
order by lastScore ASC

select * from mg_tourneyTeamPlayers p
join mg_TourneyUsers tu on tu.userid = p.UserId 
where tournamentid =22
select * from mg_TourneyCourses
update mg_TourneyCourses set DateOfRound = '6/17/12 13:00' where CourseId = 38

update mg_TourneyCourses set DateOfRound = '6/18/12 10:00' where CourseId = 39

update mg_TourneyCourses set DateOfRound = '6/19/12 10:00' where CourseId =40


select * from mg_tourneyScores where name = 'ian quarders'



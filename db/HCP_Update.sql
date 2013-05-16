--select * from mg_Users where userid = 394

Declare @lastName varchar(20), @hcp float, @ghin varchar(10), @email varchar(50)

set @lastName = 'Bichler'
set @hcp = 14.9
set @ghin = ''
set @email = 'mgbichler@q.com'
 
select u.UserId, u.GHIN, u.Handicap, u.MobileEmail, u.Email, u.FirstName, u.LastName, u.UserName, Max(s.DateEntered) 
from mg_Users u
left join mg_scores s on u.userId = s.userId
where lastname = @lastName
group by u.UserId, u.GHIN, u.Handicap, u.MobileEmail, u.Email, u.FirstName, u.LastName, u.UserName


update mg_Users set MobileEmail = Case when @email = '' THEN MobileEmail ELSE @email END, 
handicap = @hcp,
 GHIN = @ghin,
 UserName = 'mgbichler',
Email = Case WHEN Email = 'monster@monstergolf.org' THEN @email ELSE Email END
where --lastname =@lastName
UserId = 401

select * delete from mg_users where UserId = 408


select u.userid, GHIN, u.lastName, u.firstName, u.handicap, tu.hcpIndex,u.email, u.mobileemail, Max(s.DateEntered) as LastScore 
from mg_Users u
join mg_TourneyUsers tu on tu.webid = u.UserId 
join mg_tourneyTeamplayers tt on tt.UserId = tu.UserId and tt.TournamentId = 21
left join mg_scores s on u.userId = s.userId
--where GHIN IS NOT NULL and GHIN <> ''
Group by u.userid, GHIN, u.lastName, u.firstName, u.handicap, tu.hcpIndex,u.email, u.mobileemail
order by LastScore ASC

select * from mg_TourneyCourses
update mg_TourneyCourses set DateOfRound = '6/17/12 13:00' where CourseId = 38

update mg_TourneyCourses set DateOfRound = '6/18/12 10:00' where CourseId = 39

update mg_TourneyCourses set DateOfRound = '6/19/12 10:00' where CourseId =40


select * from mg_tourneyScores where name = 'ian quarders'



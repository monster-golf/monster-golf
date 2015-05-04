USE [monstergolf]
GO

DROP VIEW [dbo].[mg_TourneyLiveScoring]

/****** Object:  View [dbo].[mg_TourneyLiveScoring]    Script Date: 5/3/2015 22:14:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[mg_TourneyLiveScoring]
AS
select 
	p.TournamentID,
	s.RoundNum,
	s.Name, 
	s.HCP,
	isnull(Hole1 -  ((CASE WHEN s.HCP % 18 >= d.Handicap1  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par1,  0) +
	isnull(Hole2 -  ((CASE WHEN s.HCP % 18 >= d.Handicap2  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par2,  0) + 
	isnull(Hole3 -  ((CASE WHEN s.HCP % 18 >= d.Handicap3  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par3,  0) + 
	isnull(Hole4 -  ((CASE WHEN s.HCP % 18 >= d.Handicap4  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par4,  0) + 
	isnull(Hole5 -  ((CASE WHEN s.HCP % 18 >= d.Handicap5  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par5,  0) +
	isnull(Hole6 -  ((CASE WHEN s.HCP % 18 >= d.Handicap6  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par6,  0) + 
	isnull(Hole7 -  ((CASE WHEN s.HCP % 18 >= d.Handicap7  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par7,  0) +
	isnull(Hole8 -  ((CASE WHEN s.HCP % 18 >= d.Handicap8  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par8,  0) +
	isnull(Hole9 -  ((CASE WHEN s.HCP % 18 >= d.Handicap9  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par9,  0) + 
	isnull(Hole10 - ((CASE WHEN s.HCP % 18 >= d.Handicap10 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par10, 0) +
	isnull(Hole11 - ((CASE WHEN s.HCP % 18 >= d.Handicap11 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par11, 0) + 
	isnull(Hole12 - ((CASE WHEN s.HCP % 18 >= d.Handicap12 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par12, 0) +
	isnull(Hole13 - ((CASE WHEN s.HCP % 18 >= d.Handicap13 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par13, 0) + 
	isnull(Hole14 - ((CASE WHEN s.HCP % 18 >= d.Handicap14 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par14, 0) + 
	isnull(Hole15 - ((CASE WHEN s.HCP % 18 >= d.Handicap15 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par15, 0) +
	isnull(Hole16 - ((CASE WHEN s.HCP % 18 >= d.Handicap16 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par16, 0) + 
	isnull(Hole17 - ((CASE WHEN s.HCP % 18 >= d.Handicap17 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par17, 0) + 
	isnull(Hole18 - ((CASE WHEN s.HCP % 18 >= d.Handicap18 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par18, 0) as NetTotal,
	(CASE WHEN isnull(Hole1,  0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole2,  0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole3,  0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole4,  0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole5,  0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole6,  0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole7,  0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole8,  0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole9,  0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole10, 0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole11, 0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole12, 0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole13, 0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole14, 0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole15, 0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole16, 0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole17, 0) > 0 THEN 1 ELSE 0 END) +
	(CASE WHEN isnull(Hole18, 0) > 0 THEN 1 ELSE 0 END) as HolesCompleted,
	Hole1 -  ((CASE WHEN s.HCP % 18 >= d.Handicap1  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par1  as Net1, 
	Hole2 -  ((CASE WHEN s.HCP % 18 >= d.Handicap2  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par2  as Net2, 
	Hole3 -  ((CASE WHEN s.HCP % 18 >= d.Handicap3  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par3  as Net3, 
	Hole4 -  ((CASE WHEN s.HCP % 18 >= d.Handicap4  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par4  as Net4, 
	Hole5 -  ((CASE WHEN s.HCP % 18 >= d.Handicap5  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par5  as Net5, 
	Hole6 -  ((CASE WHEN s.HCP % 18 >= d.Handicap6  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par6  as Net6, 
	Hole7 -  ((CASE WHEN s.HCP % 18 >= d.Handicap7  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par7  as Net7, 
	Hole8 -  ((CASE WHEN s.HCP % 18 >= d.Handicap8  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par8  as Net8, 
	Hole9 -  ((CASE WHEN s.HCP % 18 >= d.Handicap9  THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par9  as Net9, 
	Hole10 - ((CASE WHEN s.HCP % 18 >= d.Handicap10 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par10 as Net10, 
	Hole11 - ((CASE WHEN s.HCP % 18 >= d.Handicap11 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par11 as Net11, 
	Hole12 - ((CASE WHEN s.HCP % 18 >= d.Handicap12 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par12 as Net12, 
	Hole13 - ((CASE WHEN s.HCP % 18 >= d.Handicap13 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par13 as Net13, 
	Hole14 - ((CASE WHEN s.HCP % 18 >= d.Handicap14 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par14 as Net14, 
	Hole15 - ((CASE WHEN s.HCP % 18 >= d.Handicap15 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par15 as Net15, 
	Hole16 - ((CASE WHEN s.HCP % 18 >= d.Handicap16 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par16 as Net16, 
	Hole17 - ((CASE WHEN s.HCP % 18 >= d.Handicap17 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par17 as Net17, 
	Hole18 - ((CASE WHEN s.HCP % 18 >= d.Handicap18 THEN 1 ELSE 0 END) + Round(s.HCP/18,0)) - Par18 as Net18
from mg_tourneyteamplayers p
join mg_tourneyusers u on u.UserId = p.UserId
join mg_TourneyScores s on s.UserId = u.WebId and s.TourneyID = p.TournamentID
join mg_tourneycourses c on c.TournamentId = p.TournamentID and c.[Round] = s.RoundNum
join mg_tourneycoursedetails d on d.CourseId = c.CourseId and d.TeeNumber = p.TeeNumber

GO



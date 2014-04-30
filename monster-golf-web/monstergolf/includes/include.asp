<%
Function SetVal(vVal, vCheck, vTrueVal, vFalseVal)
	If vVal = vCheck Then
		SetVal = vTrueVal
	Else
		SetVal = vFalseVal
	End If
End Function

Sub MainHeader(strSelected) %>
<html>
<head>
	<title>Monster Golf</title>
	<style>
		body  {font-family: verdana,arial,san serif; font-size:8.0pt; }
		td {font-family: verdana,arial,san serif; font-size:8.0pt; background:#ffffff;}
		input {font-family: verdana,arial,san serif; font-size:8.0pt; }
		.PageLink {font-family: verdana,arial,san serif;font-size:8.0pt;color:black;}
		.MenuTable { background-color:#000000; }
		.MenuCell  { background-color:#EFE486; width:16.66%; text-align:center;}
		.MenuSelected { background-color:#1F58AE; width:16.66%; text-align:center;}
		.MenuLink:link,.MenuLink:visited,.MenuLink:active,.MenuLink:hover {font-family: verdana,arial,san serif; font-size:8.0pt; color:#1F58AE; font-weight:bold;}
		.MenuLinkSelected:link,.MenuLinkSelected:visited,.MenuLinkSelected:active,.MenuLinkSelected:hover {font-family: verdana,arial,san serif; font-size:8.0pt; color:#EFE486; font-weight:bold;}
	</style>
</head>
<body topmargin=1 leftmargin=1 background='DeweyWave2.jpg'>
<table width='100%' cellpadding=8 cellspacing=1 border=0 class=MenuTable>
	<tr>
		<td nowrap class=<%=SetVal(strSelected, "Home", "MenuSelected><a class=MenuLinkSelected", "MenuCell><a class=MenuLink")%> href='default.asp'>The Monster</a></td>
		<td nowrap class=<%=SetVal(strSelected, "History", "MenuSelected><a class=MenuLinkSelected", "MenuCell><a class=MenuLink")%> href='history.asp'>The Monster<br>History</a></td>
		<td nowrap class=<%=SetVal(strSelected, "Results", "MenuSelected><a class=MenuLinkSelected", "MenuCell><a class=MenuLink")%> href='results.asp'>The Monster<br>Results</a></td>
		<td nowrap class=<%=SetVal(strSelected, "Stats", "MenuSelected><a class=MenuLinkSelected", "MenuCell><a class=MenuLink")%> href='stats.asp'>The Monster<br>Stats</a></td>
		<td nowrap class=<%=SetVal(strSelected, "Photos", "MenuSelected><a class=MenuLinkSelected", "MenuCell><a class=MenuLink")%> href='photogallery.asp'>The Monster<br>Gallery</a></td>
		<td nowrap class=<%=SetVal(strSelected, "Handicap", "MenuSelected><a class=MenuLinkSelected", "MenuCell><a class=MenuLink")%> href='handicap.asp'>The Monster<br>Handicap</a></td>
	</tr>
</table>
<%
End Sub

Sub MainFooter(strSelected) %>
<br><br><br>
<center>
<img src='MonsterLogo2006.gif' border=0>
</center>
</body>
</html>
<%
End Sub

Sub ResultsHeader(strYear, strSelected) %>
<style>
.HeaderSelected {background:#99CC00; width:12.5%; text-align:center;}
.HeaderCell { background:#CCCCCC; width:12.5%; text-align:center;}
</style>
<table width='100%' class=MenuTable border=0 cellpadding=2 cellspacing=1>
	<tr>
		<td align=center nowrap class=<%=SetVal(strSelected, "", "HeaderSelected", "HeaderCell")%>><a href='results.asp?Year=<%=strYear%>&HeaderSelect=' class=MenuLink><%=strYear%><br>Leaderboard</a></td>
		<td align=center nowrap class=<%=SetVal(strSelected, "Teams", "HeaderSelected", "HeaderCell")%>><a href='results.asp?Year=<%=strYear%>&HeaderSelect=Teams' class=MenuLink><%=strYear%><br>Teams</a></td>
		<td align=center nowrap class=<%=SetVal(strSelected, "Day1", "HeaderSelected", "HeaderCell")%>><a href='results.asp?Year=<%=strYear%>&HeaderSelect=Day1' class=MenuLink>Day 1<br>Scores</a></td>
		<td align=center nowrap class=<%=SetVal(strSelected, "Day2", "HeaderSelected", "HeaderCell")%>><a href='results.asp?Year=<%=strYear%>&HeaderSelect=Day2' class=MenuLink>Day 2<br>Scores</a></td>
<% If strYear = "2009" then %>
		<td align=center nowrap class=<%=SetVal(strSelected, "Practice", "HeaderSelected", "HeaderCell")%>><a href='results.asp?Year=<%=strYear%>&HeaderSelect=PracticeLeaders' class=MenuLink>Practice Round<br>Scores</a></td>
<% else %>
		<td align=center nowrap class=<%=SetVal(strSelected, "Practice", "HeaderSelected", "HeaderCell")%>><a href='results.asp?Year=<%=strYear%>&HeaderSelect=Practice' class=MenuLink>Practice Round<br>Scores</a></td>
		<td align=center nowrap class=<%=SetVal(strSelected, "PracticeLeaders", "HeaderSelected", "HeaderCell")%>><a href='results.asp?Year=<%=strYear%>&HeaderSelect=PracticeLeaders' class=MenuLink>Blind Draw<br>Leaderboard</a></td>
<% End If %>
		<td align=center nowrap class=<%=SetVal(strSelected, "Individuals", "HeaderSelected", "HeaderCell")%>><a href='results.asp?Year=<%=strYear%>&HeaderSelect=Individuals' class=MenuLink>Individual Gross<br>Scores</a></td>
		<td align=center nowrap class=<%=SetVal(strSelected, "YearsStats", "HeaderSelected", "HeaderCell")%>><a href='results.asp?Year=<%=strYear%>&HeaderSelect=YearsStats' class=MenuLink><%=strYear%><br />Stats</a></td>
	</tr>
</table>
<br>
<%
End Sub
%>
<%	Option Explicit
	Response.Buffer  = True
	Response.ExpiresAbsolute=#March 20,2000 00:00:00# %>
<!-- #include file='includes\db.asp' -->
<!-- #include file='includes\include.asp' -->
<%
Dim username, rating, slope, score, coursename, dateofround, tournament, userid, x, submit
Dim conn, rs, sql, output

username = ""
rating = ""
slope = ""
score = ""
coursename = ""
dateofround = Date
tournament = "0"
userid = ""
submit = ""
output = ""
x = 0

Set conn = GetConnection

submit = Request("submitbutton")

if submit <> "" then
	AddScores
end if

sub AddScores()
	rating = Request("rating")
	if rating = "" then
		output = output & "<br>Rating Missing"
		exit sub
	end if
	slope = Request("slope")
	if slope = "" then
		output = output & "<br>Slope Missing"
		exit sub
	end if
	coursename = Request("coursename")
	dateofround = Request("dateofround")
	if dateofround = "" then
		output = output & "<br>Date Missing"
		exit sub
	end if
	tournament = Request("tournament")
	if tournament <> "1" then
		tournament = "0"
	end if
	
	for x = 1 To 16
		username = Request("username" & x)
		score = Request("score" & x)
		if username <> "" and score <> "" then
			Set rs = GetRecords(conn, "SELECT UserID FROM MG_Users WHERE UserName LIKE '" & replace(username, "'", "''") & "'")
			if not rs.eof then
				userid = rs("UserID")
				sql = "INSERT INTO MG_Scores (UserID, Rating, Slope, Score, CourseName, DateOfRound, DateEntered, Tournament) VALUES " & _
						"(" & userid & "," & _
						rating & "," & _
						slope & "," & _
						score & ",'" & _
						replace(coursename, "'", "''") & "','" & _
						replace(dateofround, "'", "''") & "','" & _
						replace(Now, "'", "''") & "'," & _
						tournament & ")"
				GetRecords conn, sql
				output = output & "<br>" & username & " score " & score & " uploaded"
			end if
		end if
	next
end sub
%>
<html>
<%	MainHeader "Handicap" %>
<head>
<style>
	body  {font-family: verdana,arial,san serif; font-size:9.0pt; }
	td {font-family: verdana,arial,san serif; font-size:8.0pt; background:#ffffff;}
	input {font-family: verdana,arial,san serif; font-size:8.0pt; }
</style>
<script>
	var usernamecount = 1;
	
	function AddUser(username) {
		if (usernamecount == 17)
			alert('You have selected 16 users, please submit.');
		else {
			formvar = eval("document.upload.username" + usernamecount);
			formvar.value = username;
			formvar = eval("document.upload.score" + usernamecount);
			formvar.focus();
			usernamecount += 1;
		}
	}
</script>
</head>
<body onload='document.upload.rating.focus()'>
<form name='upload' method='post' action='uploadscores.asp'>
<table align=center>
<tr>
<td valign=top>
<table border=1 bordercolor=gray cellspacing=0 cellpadding=2>
<tr><td colspan=4 align=center><a class='MenuLink' href='allhandicap.asp'>All Handicaps</a><%=output%></td></tr>
<tr><td align=right>Rating</td><td colspan=3><input type='text' name='rating' value='<%=rating%>' size='5' tabindex='1'></td></tr>
<tr><td align=right>Slope</td><td colspan=3><input type='text' name='slope' value='<%=slope%>' size='5' tabindex='2'></td></tr>
<tr><td align=right>Date</td><td colspan=3><input type='text' name='dateofround' value='<%=dateofround%>' size='10' tabindex='3'></td></tr>
<tr><td align=right>Course Name</td><td colspan=3><input type='text' name='coursename' value='<%=coursename%>' size='34' tabindex='4'></td></tr>
<tr><td align=right>Tournament</td><td colspan=3><input type='checkbox' name='tournament' value='1' tabindex='5'<% if tournament = "1" then response.write(" checked") end if %>></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username1'></td><td>Score</td><td><input type='text' name='score1' size='4' tabindex='6'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username2'></td><td>Score</td><td><input type='text' name='score2' size='4' tabindex='7'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username3'></td><td>Score</td><td><input type='text' name='score3' size='4' tabindex='8'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username4'></td><td>Score</td><td><input type='text' name='score4' size='4' tabindex='9'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username5'></td><td>Score</td><td><input type='text' name='score5' size='4' tabindex='10'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username6'></td><td>Score</td><td><input type='text' name='score6' size='4' tabindex='11'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username7'></td><td>Score</td><td><input type='text' name='score7' size='4' tabindex='12'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username8'></td><td>Score</td><td><input type='text' name='score8' size='4' tabindex='13'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username9'></td><td>Score</td><td><input type='text' name='score9' size='4' tabindex='14'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username10'></td><td>Score</td><td><input type='text' name='score10' size='4' tabindex='15'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username11'></td><td>Score</td><td><input type='text' name='score11' size='4' tabindex='16'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username12'></td><td>Score</td><td><input type='text' name='score12' size='4' tabindex='17'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username13'></td><td>Score</td><td><input type='text' name='score13' size='4' tabindex='18'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username14'></td><td>Score</td><td><input type='text' name='score14' size='4' tabindex='19'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username15'></td><td>Score</td><td><input type='text' name='score15' size='4' tabindex='20'></td></tr>
<tr><td align=right>User Name</td><td><input type='text' name='username16'></td><td>Score</td><td><input type='text' name='score16' size='4' tabindex='21'></td></tr>
<tr><td colspan=4 align=center><input type='submit' name='submitbutton' value='Submit' tabindex='22'></td></tr>
</table>
</td>
<td>
<table border=1 bordercolor=gray cellspacing=0 cellpadding=2>
<%	sql = "SELECT UserName, FirstName, LastName, (SELECT Count(MG_Scores.ID) FROM MG_Scores WHERE MG_Scores.UserID = MG_Users.UserID) AS Scores FROM MG_Users ORDER BY LastName ASC, FirstName ASC"
	Set rs = GetRecords(conn, sql)
	
	Do Until rs.EOF %>
<tr><td nowrap><a class='PageLink' href="javascript:AddUser('<%=Replace(rs("UserName"), "'", "\'")%>')"><%=rs("LastName") & ", " & rs("FirstName") & " (" & rs("Scores") & ")"%></a></td><%
		rs.MoveNext
		If rs.EOF Then
			Exit Do
		End If %><td nowrap><a class='PageLink' href="javascript:AddUser('<%=Replace(rs("UserName"), "'", "\'")%>')"><%=rs("LastName") & ", " & rs("FirstName") & " (" & rs("Scores") & ")"%></a></td><%
		rs.MoveNext
		If rs.EOF Then
			Exit Do
		End If %><td nowrap><a class='PageLink' href="javascript:AddUser('<%=Replace(rs("UserName"), "'", "\'")%>')"><%=rs("LastName") & ", " & rs("FirstName") & " (" & rs("Scores") & ")"%></a></td><%
		rs.MoveNext
		If rs.EOF Then
			Exit Do
		End If %><td nowrap><a class='PageLink' href="javascript:AddUser('<%=Replace(rs("UserName"), "'", "\'")%>')"><%=rs("LastName") & ", " & rs("FirstName") & " (" & rs("Scores") & ")"%></a></td></tr><%
		rs.MoveNext
	Loop %>
</table>
</td>
</tr>
</table>
</form>
</body>
</html>
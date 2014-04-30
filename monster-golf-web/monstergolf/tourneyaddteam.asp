<!-- #include file='includes\dbresults.asp' -->
<html>
<%
If Request("user") <> "" Then
 If Request("user") = "waldadmin" Then
  Session("AllUser") = Request("user")
 End If
End If

If Session("AllUser") = "" Then %>
<body>
<form name='alluser' method='post' action='tourneyaddteam.asp'>
<center><br><br>User Name <input type='text' name='user' value=''></center>
</form>
</body></html>
<%
Response.End
End If

Dim conn, rsusers, currentyear

Set conn = GetConnection()

dim player1, player2, playercount, toomanyplayers

playercount = 0
toomanyplayers = ""
currentyear = 2014

Function AddNewUser(onnum)
   AddNewUser = -1
   If (Trim(Request("firstname" & onnum)) <> "" and _
       Trim(Request("lastname" & onnum)) <> "") then
      ExecUpdate conn, "exec AddUser " & _
         "'" & Replace(Request("firstname" & onnum), "'", "''") & "'," & _
         "'" & Replace(Request("lastname" & onnum), "'", "''") & "'," & _
         "'" & Replace(Request("gender" & onnum), "'", "''") & "'," & _
         "'" & Replace(Request("nickname" & onnum), "'", "''") & "'"
      Dim rsnew
      Set rsnew = GetRecords(conn, "select Max(userid) as newuserid from users")
      if (Not rsnew.eof) then
         AddNewUser = rsnew("newuserid")
      end if
   End if
End Function

if (request.Form("removeteamid") <> "") then
response.Write "exec RemoveTeam " & Request.Form("removeteamid")
'response.End
   ExecUpdate conn, "exec RemoveTeam " & Request.Form("removeteamid")
end if

if (Request.Form("paid") <> "") then
   for each payer in Request.Form("paid")
      ExecUpdate conn, "update users set paid = true where userid = " & payer
   next
end if

if (Request.Form("teamid") <> "") then
   for each team in Request.Form("teamid")
      ExecUpdate conn, "update Teams set [StartingHole] = '" & Request.Form("StartingHole" & team) & "' where [id] = " & team
   next
end if

Dim newuserid1, newuserid2
newuserid1 = AddNewUser("1")
newuserid2 = AddNewUser("2")

if (Request.Form("usertoteam") <> "") then
   for each player in Request.Form("usertoteam")
      if playercount = 0 then
         player1 = player
      elseif playercount = 1 then
         player2 = player
      elseif playercount = 2 then
         toomanyplayers = "Too many players to add to a team, please try again."
      end if
      
      playercount = playercount + 1
   next
   
   if toomanyplayers = "" then
      'do insert
      ' insert new user
      if (player1 = "-1") then
         player1 = newuserid1
      end if
      if (player2 = "-1") then
         player2 = newuserid2
      end If
      
      if (playercount = 1) then
         if (player1 <> "-1") then
            ExecUpdate conn, "exec AddToTeam " & currentyear & ", 'Tournament', " & player1 & ",-1"
         elseif (player2 <> "-1") then
            ExecUpdate conn, "exec AddToTeam " & currentyear & ", 'Tournament', " & player2 & ",-1"
         end if
      else
         if (player1 <> "-1" and player2 <> "-1") then
            ExecUpdate conn, "exec AddToTeam " & currentyear & ", 'Tournament', " & player1 & ", " & player2
         elseif (player1 <> "-1") then
            ExecUpdate conn, "exec AddToTeam " & currentyear & ", 'Tournament', " & player1 & ",-1"
         elseif (player2 <> "-1") then
            ExecUpdate conn, "exec AddToTeam " & currentyear & ", 'Tournament', " & player2 & ",-1"
         end if
      end if
      
      toomanyplayers = "Team has been added."
   end if
end if

Response.Write toomanyplayers

Set rsusers = GetRecords(conn, "SELECT distinct u.FirstName, u.LastName, u.UserID, t.Year " & _
"from (users u left outer join teams t on t.player1 = u.userid or t.player2=u.userid) order by u.FirstName")

Dim lastid : lastid = -1
%>
<head>
<script>
function doremove(teamid) {
   if (confirm('remove team: ' + teamid)) {
      document.adduser.removeteamid.value = teamid;
      document.adduser.submit();
   }
}
</script>
<style>
	body  {font-family: verdana,arial,san serif; font-size:9.0pt; }
	td {font-family: verdana,arial,san serif; font-size:8.0pt; background:#ffffff;}
	input {font-family: verdana,arial,san serif; font-size:8.0pt; }
	select {font-family: verdana,arial,san serif; font-size:8.0pt; }
	.PageLink {font-family: verdana,arial,san serif;font-size:8.0pt;color:black;}
	.MenuTable { background-color:#000000; }
	.MenuCell  { background-color:#D6D6D6; width:20%; text-align:center;}
	.MenuSelected { background-color:#89BBDE; width:20%; text-align:center;}
	.MenuSelectedRed { background-color:#DA1111; width:20%; text-align:center;}
	.MenuLink {font-family: verdana,arial,san serif; font-size:8.0pt; color:#000033; font-weight:bold;}
	.MenuLinkSelected {font-family: verdana,arial,san serif; font-size:8.0pt; color:#000033; font-weight:bold;}
</style>
</head>
<body>
<form method="post" action="tourneyaddteam.asp" name="adduser">
<input type="hidden" name="removeuser" value="">
<input type="hidden" name="removeteamid" value="">
<input type="submit" name="addteam" value="Add Team"> <br>
<br>
You must add a first name and last name to create a new player.<br>
Click the checkbox next to their name to add the new player to<br>
a team either with a second new player or a player in the available list.<br><br>
Create Player: 
<input type="checkbox" name="usertoteam" value="-1">
First <input type="text" name="firstname1" onchange="document.adduser.usertoteam[0].checked = true;" value="" size="15">
Last <input type="text" name="lastname1" onchange="document.adduser.usertoteam[0].checked = true;" value="" size="15">
Nickname <input type="text" name="nickname1" value="" onchange="document.adduser.usertoteam[0].checked = true;" size="15">
Gender <select name="gender1"><option value="M">Male</option><option value="F">Female</option></select>
<br>
Create Player: 
<input type="checkbox" name="usertoteam" value="-1">
First <input type="text" name="firstname2" onchange="document.adduser.usertoteam[1].checked = true;" value="" size="15" ID="Text1">
Last <input type="text" name="lastname2" onchange="document.adduser.usertoteam[1].checked = true;" value="" size="15" ID="Text2">
Nickname <input type="text" name="nickname2" value="" onchange="document.adduser.usertoteam[1].checked = true;" size="15" ID="Text3">
Gender <select name="gender2" ID="Select1"><option value="M">Male</option><option value="F">Female</option></select>
<br><br>
Available Players:  &nbsp; | &nbsp; <a class="MenuLink" href="allhandicap.asp">Back to All Handicap</a>
<br><br>
<table>
<tr>
<td valign="top">
<table border="1" cellpadding="0" cellspacing="0">
<%
dim x, yearlist, curruserid, currname, incurrentyear, justwrote
x = 0
yearlist = ""
curruserid = -1
currname = ""
incurrentyear = false

Do Until rsusers.EOF
   justwrote= false
   
   if (rsusers("UserID") = lastid) then
      yearlist = yearlist & "," & rsusers("Year")
      if (rsusers("Year") = currentyear) then
         incurrentyear = true
      end if
   else
      if lastid <> -1 then
         if yearlist <> "" then
            yearlist = "(" & yearlist & ")"
         end if
         if (incurrentyear) then
            Response.Write "<td nowrap valign='top'><input disabled type='checkbox' name='usertoteam' value='" & curruserid & "'> " & currname & "<br>" & yearlist & "</td>" & vbCrLf
         else
            Response.Write "<td nowrap valign='top'><input type='checkbox' name='usertoteam' value='" & curruserid & "' id='usertoteam" & curruserid & "'><label for='usertoteam" & curruserid & "'> " & currname & "<br>" & yearlist & "</label></td>" & vbCrLf
         end if
         justwrote= true
         incurrentyear = false
         curruserid = -1
         currname = ""
         yearlist = ""
         if (x = 4) then
            response.Write "</tr>" & vbCrLf
            x= 0
         end if
      end if
      if x = 0 then
         response.Write "<tr>" & vbCrLf
      end if
      yearlist = rsusers("Year")
      if (rsusers("Year") = currentyear) then
         incurrentyear = true
      end if
      curruserid = rsusers("UserID")
      currname = rsusers("FirstName") & " " & rsusers("LastName")
   end if
   if (lastid <> rsusers("userid")) then
      justwrote = false
   end if
   lastid = rsusers("UserID")
   rsusers.MoveNext

   if (rsusers.EOF) then
      if (Not justwrote and currname <> "") then
         if yearlist <> "" then
            yearlist = "(" & yearlist & ")"
         end if
         if (incurrentyear) then
            Response.Write "<td nowrap valign='top'><input disabled type='checkbox' name='usertoteam' value='" & curruserid & "'> " & currname & "<br>" & yearlist & "</td></tr>"
         else
            Response.Write "<td nowrap valign='top'><input type='checkbox' name='usertoteam' value='" & curruserid & "'> " & currname & "<br>" & yearlist & "</td></tr>"
         end if
      end if
   else
      if (rsusers("UserID") <> lastid) then
         x=x+1
      end if
   end if
Loop
%>
</table>
</td>
<td width="10">&nbsp;</td>
<td valign="top" align="center">
<input type="button" name="submitPaid" value="Update Paid/Hole" onclick="document.adduser.submit();"><br><br>
<% 
WriteParticipants currentyear, true
conn.Close()
%>
</td>
</tr>
</table>
</form>
</body>
</html>
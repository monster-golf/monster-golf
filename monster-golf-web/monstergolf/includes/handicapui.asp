<%
Dim g_oConn

Sub ScoreList(NumScores, AllVals, AllUser)
   Dim sDivTitle %>
<table border=1 bordercolor=gray cellspacing=0 cellpadding=2>
	<tr>
<%		If AllUser <> "" Then %>
		<td colspan=10 align=center nowrap>Last <%=NumScores%> Rounds &nbsp; * if used &nbsp; T = Tournament score</td>
<%		Else %>
		<td colspan=8 align=center nowrap>Last <%=NumScores%> Rounds &nbsp; * if used &nbsp; T = Tournament score<br>Diff = The Round Differential. The formula is ((score-rating)x113)/slope</td>
<%		End If %>
	</tr>
	<tr>
		<td align=center>#</td>
<%		If AllUser <> "" Then %>
		<td align=center>Edit</td>
		<td align=center>Del</td>
<%		End If %>
		<td align=center width='40'>Score</td>
		<td align=center width='40'>Rating</td>
		<td align=center width='40'>Slope</td>
		<td align=center width='40'>Diff</td>
<%		If AllUser <> "" Then %>
		<td align=center>Course</td>
		<td align=center>Date of Round</td>
<%		Else %>
		<td align=center>Course/Date</td>
<%		End If %>
	</tr>
<%	For x = 0 to NumScores - 1 %>
	<tr>
		<td align=center><%=x+1%></td>
<%		If AllUser <> "" And AllVals(x, cScrID) > 0 Then %>
		<td align=center><input type=checkbox name="EditScore" value="<%=AllVals(x, cScrID)%>"></td>
		<td align=center><input type=checkbox name="RemoveScore" value="<%=AllVals(x, cScrID)%>"></td>
		<td align=left><table cellspacing=0 border=0 cellpadding=1 width='100%'><tr><td width='33%' align=left><input onchange='CheckEdit(<%=AllVals(x, cScrID)%>)' type=text size=2 name="Score<%=AllVals(x, cScrID)%>" value="<%=AllVals(x, cScore) %>"></td><td width='33%' align=center><%=SetVal(AllVals(x, cHDUse), True, "*", "&nbsp;")%></td><td width='33%' align=right><input onchange='CheckEdit(<%=AllVals(x, cScrID)%>)' type=checkbox name="Tournament<%=AllVals(x, cScriD)%>"<%=SetVal(AllVals(x, cTourn), True, " checked", "")%>></td></tr></table></td>
		<td align=center><input onchange='CheckEdit(<%=AllVals(x, cScrID)%>)' type=text size=3 name="Rating<%=AllVals(x, cScrID)%>" value="<%=AllVals(x, cIndex)%>"></td>
		<td align=center><input onchange='CheckEdit(<%=AllVals(x, cScrID)%>)' type=text size=2 name="Slope<%=AllVals(x, cScrID)%>" value="<%=AllVals(x, cSlope)%>"></td>
		<td align=center><%=AllVals(x, cHD)%></td>
		<td align=center><input onchange='CheckEdit(<%=AllVals(x, cScrID)%>)' type=text size=20 name="CourseName<%=AllVals(x, cScrID)%>" value="<%=AllVals(x, cCName)%>"></td>
		<td align=center><input onchange='CheckEdit(<%=AllVals(x, cScrID)%>)' type=text size=10 name="Date<%=AllVals(x, cScrID)%>" value="<%=AllVals(x, cSDate)%>"></td>
<%		Else 
			If AllUser <> "" Then %>
		<td align=center>&nbsp;</td>
		<td align=center>&nbsp;</td>
<%			End If 
         sDivTitle = "Entered on " & FormatDateTime(AllVals(x, cEDate), 0) & " by " & AllVals(x, cEntBy) %>
		<td align=left><div title="<%=sDivTitle%>"><table cellspacing=0 border=0 cellpadding=1 width='100%'><tr><td width='33%' align=left><%=AllVals(x, cScore) %></td>
		<td width='33%' align=center><%=SetVal(AllVals(x, cHDUse), True, "*", "&nbsp;")%></td>
		<td width='33%' align=right><%=SetVal(AllVals(x, cTourn), True, "T", "&nbsp;")%></td></tr></table></div></td>
		<td align=center><div title="<%=sDivTitle%>"><%=AllVals(x, cIndex)%></div></td>
		<td align=center><div title="<%=sDivTitle%>"><%=AllVals(x, cSlope)%></div></td>
		<td align=center><div title="<%=sDivTitle%>"><%=AllVals(x, cHD)%></div></td>
		<td align=center><div title="<%=sDivTitle%>"><%=AllVals(x, cCName)%>&nbsp;on <%=AllVals(x, cSDate)%></div></td>
<%		End If %>
	</tr>
<%	Next
	If AllUser <> "" Then %>
	<tr>
		<td colspan=10 align=center><input type=button name='SubmitEds' value='Submit Edits' onclick='document.GetScore.SubmitEdits.value = "true"; document.GetScore.submit()'></td>
		<input type=hidden name=SubmitEdits value=''>
	</tr>
<%	End If %>
</table>
<%
End Sub

Sub HeaderDisplay(Handicap, HandicapTrend, AlreadyAUser, FirstName, LastName, UserName, Email, AllUser)
%>
<table width='100%'>
<tr>
<td>
<%=AlreadyAUser%><big>What's up <%=oRS("FirstName")%> your handicap is currently 
<% If AllUser = "waldadminmonster" Then %>
    <input type="text" style="width:50px;font-size:15px;" value="<%=Handicap%>" name="EditHandicap" /></b>
<% Else %>
    <b><%=Handicap%></b>
<% End If %>
</big> &nbsp;&nbsp;(trending to <%=HandicapTrend%>)<br>
<%=FirstName & " " & LastName & " &nbsp;&nbsp;&nbsp;User Name: " & UserName & " &nbsp;&nbsp;&nbsp;Email: " & Email%><br>
Your handicap will only update twice a month on the 15th and last day of the month.<br>Your trend is where your handicap is headed using all of the latest scores.
</td>
<td align="right" valign="top">
<a href="javascript:document.GetScore.LogOut.value = 'True'; document.GetScore.submit();" class=MenuLink>Sign Off</a><br><br>
<a href="javascript:document.GetScore.NewUser.value = 'True'; document.GetScore.submit();" class=MenuLink>Edit Profile</a>
</td>
</tr>
</table>
<%
End Sub

Sub HandicapForm(LastIndex, LastSlope, LastDate, LastCourse)
%>
<input type='hidden' name='SubmitFriendScore' value=''>
<table>
	<tr>
		<td colspan=2>Enter the information (no spaces)</td>
	</tr>
	<tr>
		<td align=right valign="top" nowrap><div style='position:relative; top:4px'>Course Rating: </div></td>
		<td><input type=text size=4 name=Index maxLength=4 value='<%=LastIndex%>'> ex: 70.1</td>
	</tr>
	<tr>
		<td align=right valign="top" nowrap><div style='position:relative; top:4px'>Course Slope: </div></td>
		<td><input type=text size=4 name=Slope maxLength=3 value='<%=LastSlope%>'> ex: 121</td>
	</tr>
	<tr>
		<td align=right valign="top" nowrap><div style='position:relative; top:4px'>Date of Round: </div></td>
		<td><input type=text size=10 name=Date maxLength=10 value='<%=LastDate%>'><br>mm/dd/yyyy</td>
	</tr>
	<tr>
		<td align=right valign="top"><div style='position:relative; top:4px'>Course: </div></td>
		<td nowrap><input type=text size=20 name=CourseName maxLength=50 value="<%=Replace(LastCourse , """", "&quot;")%>"><br>(optional)</td>
	</tr>
	<tr>
		<td align="right">Your Score: </td>
		<td><input type="text" size="4" name="Score" maxLength="3"></td>
	</tr>
<%
Dim rs
Set rs = GetRecords(g_oConn, "SELECT u.UserID, u.FirstName, u.LastName FROM (MG_Users u INNER JOIN MG_Groups g ON g.FriendID = u.UserID) WHERE (g.UserID = " & Session("UserID") & ") AND (g.FriendDenied = 0) AND (g.FriendAllowPost = 1) ORDER BY u.FirstName")
''Set rs = GetRecords(g_oConn, "select u.UserID, u.FirstName, u.LastName from mg_users u join mg_groups g on g.friendid = u.userid and g.FriendDenied = 0 and g.FriendAllowPost = 1 where g.userid = " & Session("UserID") & " order by u.firstname")
Do Until rs.Eof %>
   <tr>
		<td nowrap align="right"><%=rs("FirstName") & " " & rs("LastName")%>: </div></td>
		<td><input type='hidden' name='PostFriend' value="<%=rs("UserID")%>"><input onchange="document.GetScore.SubmitFriendScore.value='true';" type="text" size="4" name="PostScore<%=rs("UserID")%>" maxLength="3"></td>
   </tr>
<% rs.MoveNext
Loop
%>	
	<tr>
		<td colspan=2 align=center nowrap><input type=checkbox name='Tournament'><label for="Tournament"> check if this is a tournament score.</label></td>
	</tr>
	<tr>
		<td colspan=2 align=center><br><input type=submit value='Submit Score(s)' onClick='return CheckInput()' name="doSubmit"></td>
	</tr>
</table>
<%
End Sub

Sub HandicapInfo()
%>
<br><br>
** Remember Monster golfers **<br>
You must use<br><b>EQUITABLE STROKE CONTROL.</b><br>
How to adjust your score<br><br>
<table border=1 bordercolor=silver cellspacing=0 cellpadding=2 ID="Table2"><tr><td>Your Handicap<br>on the course<br>you are playing</td><td>Maximum Score<br>On Any Hole</td></tr>
<tr><td>9 or less</td><td>Double Bogey</td></tr>
<tr><td>10 through 19</td><td>7</td></tr>
<tr><td>20 through 29</td><td>8</td></tr>
<tr><td>30 through 39</td><td>9</td></tr>
<tr><td>40 and above</td><td>10</td></tr>
</table>
<br>
EXAMPLE: If a player has a<br>
Course Handicap of 25,<br>
he or she can post for<br>
handicap purposes,<br>
a maximum hole score of<br>
eight on any hole.<br>
There would be no limit<br>
to the number of eights<br>
that this player could take.<br><br>
So, please adjust your score before<br>
entering it into the system.<br><br>
<%
End Sub

Sub Statistics(UserID)
   Dim lCurrYear, lNumTR, lNumYR, lNum60, lNum70, lNum80, lNum90, lNum100, lNumT, lLScore, sLCourse, sBCourse, lBScore, lBDiff, lCDiff
   Set oRS = GetRecords(g_oConn, "SELECT ID, UserID, Rating, Slope, Score, CourseName, DateOfRound, DateEntered, Tournament FROM MG_Scores WHERE UserID = " & UserID & " ORDER BY DateOfRound DESC, DateEntered DESC")

   lCurrYear = Year(Now)
   lNumTR = 0
   lNumYR = 0
   lNum60 = 0
   lNum70 = 0
   lNum80 = 0
   lNum90 = 0
   lNum100 = 0
   lNumT = 0
   lLScore = 200
   lBDiff = 200

   Do Until oRS.EOF
	   If lCurrYear <> Year(oRS("DateOfRound")) Then
		   Response.Write "<br><b>Stats for " & lCurrYear & "</b><br>"
		   Response.Write "Scores posted: " & lNumYR & "<br>"
		   Response.Write "Tournament Scores posted: " & lNumT & "<br>"
		   If lNum60 > 0 Then
			   Response.Write "Scores lower than 70: " & lNum60 & "<br>"
		   End If
		   If lNum70 > 0 Then
			   Response.Write "Scores in the 70's: " & lNum70 & "<br>"
		   End If
		   If lNum80 > 0 Then
			   Response.Write "Scores in the 80's: " & lNum80 & "<br>"
		   End If
		   If lNum90 > 0 Then
			   Response.Write "Scores in the 90's: " & lNum90 & "<br>"
		   End If
		   If lNum100 > 0 Then
			   Response.Write "Scores higher than 99: " & lNum100 & "<br>"
		   End If
		   lNumYR = 0
		   lNum60 = 0
		   lNum70 = 0
		   lNum80 = 0
		   lNum90 = 0
		   lNum100 = 0
		   lNumT = 0
		   lCurrYear = Year(oRS("DateOfRound"))
	   End If
   	
	   lCDiff = ((oRS("Score") - oRS("Rating"))*113)/oRS("Slope")
   	
	   If lCDiff < lBDiff Then
		   lBDiff = lCDiff
		   lBScore = oRS("Score")
		   If oRS("CourseName") = "" Or IsNull(oRS("CourseName")) Then
			   sBCourse = "at Unknown on " & oRS("DateOfRound")
		   Else
			   sBCourse = "at " & oRS("CourseName") & " on " & oRS("DateOfRound")
		   End If
	   ElseIf lCDiff = lBDiff Then
		   If oRS("CourseName") = "" Or IsNull(oRS("CourseName")) Then
			   sBCourse = sBCourse & ", at Unknown on " & oRS("DateOfRound")
		   Else
			   sBCourse = sBCourse & ", at " & oRS("CourseName") & " on " & oRS("DateOfRound")
		   End If
	   End If
   	
	   If oRS("Score") < lLScore Then
		   lLScore = oRS("Score")
		   If oRS("CourseName") = "" Or IsNull(oRS("CourseName")) Then
			   sLCourse = "at Unknown on " & oRS("DateOfRound")
		   Else
			   sLCourse = "at " & oRS("CourseName") & " on " & oRS("DateOfRound")
		   End If
	   ElseIf oRS("Score") = lLScore Then
		   If oRS("CourseName") = "" Or IsNull(oRS("CourseName")) Then
			   sLCourse = sLCourse & ", at Unknown on " & oRS("DateOfRound")
		   Else
			   sLCourse = sLCourse & ", at " & oRS("CourseName") & " on " & oRS("DateOfRound")
		   End If
	   End If
   	
	   If oRS("Score") < 70 Then
		   lNum60 = lNum60 + 1
	   ElseIf oRS("Score") < 80 Then
		   lNum70 = lNum70 + 1
	   ElseIf oRS("Score") < 90 Then
		   lNum80 = lNum80 + 1
	   ElseIf oRS("Score") < 100 Then
		   lNum90 = lNum90 + 1
	   Else
		   lNum100 = lNum100 + 1
	   End If
   	
	   If oRS("Tournament") Then
		   lNumT = lNumT + 1
	   End If
   	
	   lNumTR = lNumTR + 1
	   lNumYR = lNumYR + 1
   	
	   oRS.MoveNext
   Loop

   If lNumTR > 0 Then
	   Response.Write "<br><b>Stats for " & lCurrYear & "</b><br>"
	   Response.Write "Scores posted: " & lNumYR & "<br>"
	   Response.Write "Tournament Scores posted: " & lNumT & "<br>"
	   If lNum60 > 0 Then
		   Response.Write "Scores lower than 70: " & lNum60 & "<br>"
	   End If
	   If lNum70 > 0 Then
		   Response.Write "Scores in the 70's: " & lNum70 & "<br>"
	   End If
	   If lNum80 > 0 Then
		   Response.Write "Scores in the 80's: " & lNum80 & "<br>"
	   End If
	   If lNum90 > 0 Then
		   Response.Write "Scores in the 90's: " & lNum90 & "<br>"
	   End If
	   If lNum100 > 0 Then
		   Response.Write "Scores higher than 99: " & lNum100 & "<br>"
	   End If
	   Response.Write "<br>Scores posted in Monster Handicapping: " & lNumTR & "<br>"
	   Response.Write "<br>Lowest Score ever posted:<br><b>" & lLScore & "</b> " & sLCourse & "<br>"
	   Response.Write "<br>Best Score ever posted<br>(based on Rating & Slope):<br><b>" & lBScore & "</b> " & sBCourse & "<br>"
   End If
End Sub

Sub SubmitEdits(DoEdits, RemoveScore, EditScore, UserID, EditHandicap)
   If UserID <> "" And EditHandicap <> "" Then
		Response.Write "Updating Handicap To: " & EditHandicap & "<br>"
		GetRecords oConn, "UPDATE MG_Users SET Handicap = " & EditHandicap & " WHERE UserID = " & UserID
   End If

   If DoEdits <> "" Then
	   Dim sID
	   sID = ""
	   
       For Each sID In RemoveScore
		   Response.Write "Deleting Record: " & sID & "<br>"
		   GetRecords oConn, "DELETE FROM MG_Scores WHERE [ID] = " & sID
	   Next
   	
	   sID = ""
   	
	   For Each sID IN EditScore
		   Response.Write "Updating Record: " & sID & "<br>"
		   GetRecords oConn, "UPDATE MG_Scores SET Rating = " & Request("Rating" & sID) & "," & _
								   "Slope = " & Request("Slope" & sID) & "," & _
								   "Score = " & Request("Score" & sID) & "," & _
								   "CourseName = '" & Replace(Request("CourseName" & sID), "'", "''") & "'," & _
								   "DateOfRound = '" & Replace(Request("Date" & sID), "'", "''") & "'," & _
								   "Tournament = " & SetVal(Request("Tournament" & sID), "on", "1", "0") & " " & _
								   "WHERE [ID] = " & sID
	   Next
   End If
End Sub

Sub SubmitFriendScore(SubmitScore, PostFriendList)
   Dim friendid, friends

   If SubmitScore <> "" Then
      friends = Split(PostFriendList, ",")
      For Each friendid in friends
         If (Request("PostScore" & Trim(friendid)) <> "") Then
            GetRecords g_oConn, "INSERT INTO MG_Scores (UserID, Rating, Slope, Score, CourseName, DateOfRound, DateEntered, Tournament, EnteredBy) VALUES " & _
	                           "(" & Trim(friendid) & "," & _
	                           Request("Index") & "," & _
	                           Request("Slope") & "," & _
	                           Request("PostScore" & Trim(friendid)) & ",'" & _
	                           Replace(Request("CourseName"), "'", "''") & "','" & _
	                           Replace(SetVal(Request("Date"), "", Date, Request("Date")), "'", "''") & "','" & _
	                           Replace(Now, "'", "''") & "'," & _
	                           SetVal(Request("Tournament"), "on", "1", "0") & "," & _
	                           Session("UserID") & ")"
         End If
      Next
   End If
End Sub
%>

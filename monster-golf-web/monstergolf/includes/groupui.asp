<%
Sub RemoveFriend(RemoveFriendID, UserID)
   If RemoveFriendID <> "" Then
      GetRecords oConn, "delete from mg_groups where userid = " & UserID & " and friendid = " & RemoveFriendID
   End If
End Sub

Sub AddFriend(AddFriendID, UserID)
   Dim friends, friendid
   If AddFriendID <> "" Then
      friends = Split(AddFriendID, ",")
      For Each friendid in friends
         GetRecords oConn, "insert into mg_groups (UserID, FriendID) select " & UserID & "," & Trim(friendid) & " from mg_users u left join mg_groups g on g.userid = " & UserID & " and g.friendid = " & Trim(friendid) & " where g.userid is null AND u.UserID = " & UserID
      Next
   End If
End Sub

Sub FriendList(UserID) 
   Dim friends, friendid %>
<font color='red'><b>New Feature</b></font><br>
View friends handicaps and<br>post scores for them.<br>
Allow your friends to post for you.<br><br>
<br>
Enter the Course Slope: <input type='text' name='courseslope' value='' size='5' onkeyup='doHandi()' ID="Text3"><br>
to view what you and your friends<br>handicap would be for play on that course.<br>
<% Set oRS = GetRecords(oConn, "SELECT Distinct u.UserID, u.Handicap, u.FirstName, u.LastName, g.FriendDenied FROM (MG_Users u INNER JOIN MG_Groups g ON g.FriendID = u.UserID) WHERE (g.UserID = " & UserID & " or u.UserID = " & UserID & ") ORDER BY u.FirstName")
   ''Set oRS = GetRecords(oConn, "select u.UserID, u.Handicap, u.FirstName, u.LastName, g.FriendAccepted, g.FriendDenied, g.FriendAllowPost from mg_users u join mg_groups g on g.friendid = u.userid where g.userid = " & UserID & " order by u.firstname")
   If (Not oRS.Eof) Then
      Response.Write("<input type='hidden' name='removefriend' value=''><input type='hidden' name='SubmitFriendScore' value=''>")
      Response.Write("<table border='1' bordercolor='silver' cellspacing='0' cellpadding='2'><tr><td>Golfer</td><td>Handicap<br>Index</td><td>Course<br>Handicap</td></tr>")

      Do Until oRS.EOF
         Response.Write("<tr><td nowrap>" & oRS("FirstName") & " " & oRS("LastName") & "<br><a href=""javascript:document.GetScore.removefriend.value='" & oRS("UserID") & "';document.GetScore.submit();"" class='PageLink'>remove</a></td>")
         If (oRS("FriendDenied")) Then
            Response.Write("<td colspan='2'>Denied</td>")
         Else
            Response.Write("<td align='center'><input type='hidden' name='handiuserid' value='" & oRS("UserID") & "'><input type='text' size='4' style='border-width:0px;' readonly name='currhandi" & oRS("UserID") & "' value='" & oRS("Handicap") & "'></td>")
            Response.Write("<td align='center'><input type='text' size='4' style='border-width:0px;' readonly name='coursehandi" & oRS("UserID") & "' value=''></td>")
         End If
         Response.Write("</tr>")
         oRS.MoveNext
      Loop
      Response.Write("</table><br>")
      Response.Write("<br>")
   End If

   Dim permsupdated
   permsupdated = ""
   If Request("doupdate") <> "" Then
      friends = Split(Request("updatefriend"), ",")
      permsupdated = "&nbsp;&nbsp;<font class='MenuLink'>Permissions have been updated.</font>"
      For Each friendid in friends
         GetRecords oConn, "update mg_groups set FriendDenied =" & Request("Deny" & Trim(friendid)) & ",FriendAllowPost = " & Request("AllowPost" & Trim(friendid)) & " where FriendID = " & UserID & " and UserID = " & friendid
      Next
   End If

   Set oRS = GetRecords(oConn, "SELECT u.UserID, u.Handicap, u.FirstName, u.LastName, g.FriendAccepted, g.FriendDenied, g.FriendAllowPost FROM (MG_Users u INNER JOIN MG_Groups g ON g.UserID = u.UserID) WHERE (g.FriendID = " & Session("UserID") & ") ORDER BY u.FirstName")
   ''Set oRS = GetRecords(oConn, "select u.UserID, u.Handicap, u.FirstName, u.LastName, g.FriendAccepted, g.FriendDenied, g.FriendAllowPost from mg_users u join mg_groups g on g.userid = u.userid where g.friendid = " & Session("UserID") & " order by u.firstname")
   If (Not oRS.Eof) Then
      Response.Write("<input type='hidden' name='doupdate' value=''>These golfers have added you to their group:<table border='1' bordercolor='silver' cellspacing='0' cellpadding='2'><tr><td>Golfer</td><td>Allow friend to Post Scores?</td><td>Deny friend any rights?</td></tr>")
      Do Until oRS.EOF
         Response.Write("<tr><td nowrap><input type='hidden' name='updatefriend' value='" & oRS("UserID") & "'>" & oRS("FirstName") & " " & oRS("LastName") & "</td>")
         Response.Write("<td><input type='radio' id='AllowPost" & oRS("UserID") & "Yes' value='1' name='AllowPost" & oRS("UserID") & "'" & SetVal(oRS("FriendAllowPost"), True, " checked", "") & "><label for='AllowPost" & oRS("UserID") & "Yes'>Yes</label><br><input type='radio' id='AllowPost" & oRS("UserID") & "No' value='0' name='AllowPost" & oRS("UserID") & "'" & SetVal(oRS("FriendAllowPost"), True, "", " checked") & "><label for='AllowPost" & oRS("UserID") & "No'>No</label></td>")
         Response.Write("<td><input type='radio' id='Deny" & oRS("UserID") & "Yes' value='1' name='Deny" & oRS("UserID") & "'" & SetVal(oRS("FriendDenied"), True, " checked", "") & "><label for='Deny" & oRS("UserID") & "Yes'>Yes</label><br><input type='radio' id='Deny" & oRS("UserID") & "No' value='0' name='Deny" & oRS("UserID") & "'" & SetVal(oRS("FriendDenied"), True, "", " checked") & "><label for='Deny" & oRS("UserID") & "No'>No</label></td></tr>")
         oRS.MoveNext
      Loop
      Response.Write("</table><input type='button' name='btnupdate' value='Update' onclick=""document.GetScore.doupdate.value='true';document.GetScore.submit();"">" & permsupdated & "<br><br>")
   End If
%>
Add golfers to your group:<br>
<input type="hidden" name="searchfriend" value="" ID="Hidden1">
<input type="hidden" name="addfriend" value="" ID="Hidden2">
First Name <input type="text" name="searchfirstname" maxlength="50" value="<%=Request("searchfirstname")%>" ID="Text1"><br>
Last Name <input type="text" name="searchlastname" maxlength="50" value="<%=Request("searchlastname")%>" ID="Text2"><br><input type="button" name="btnfriend" value="Search" onclick="document.GetScore.searchfriend.value = 'true'; document.GetScore.submit();" ID="Button1"><br><br>
<% If (Request("searchfriend") <> "") Then
      Set oRS = GetRecords(oConn, "select u.FirstName, u.LastName, u.UserID from mg_users u left join mg_groups g on g.friendid = u.userid and g.userid = " & Session("UserID") & " where g.UserID IS Null and u.UserID <> " & Session("UserID") & " and u.FirstName like '" & Replace(Request("searchfirstname"), "'", "''") & "%' and u.LastName like '" & Replace(Request("searchlastname"), "'", "''") & "%' order by u.firstname")
      
      If oRS.EOf Then
         Response.Write "No one found by that name."
      Else
         Do Until oRS.EOF
            Response.Write("<input type='checkbox' id='FriendID" & oRS("UserID") & "' name='FriendID' value='" & oRS("UserID") & "'><label for='FriendID" & oRS("UserID") & "'> " & oRS("FirstName") & " " & oRS("LastName") & "</label><br>")
            oRS.MoveNext
         Loop
         Response.Write("<br><input type='button' name='btnAdd' value='Add Golfer(s) to Group' onclick=""document.GetScore.addfriend.value='true'; document.GetScore.submit();""><br>")
      End If
   End If
End Sub
%>
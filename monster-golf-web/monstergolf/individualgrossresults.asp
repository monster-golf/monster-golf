<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<!-- #include file='includes\funcs.asp' -->


<link rel="STYLESHEET" type="text/css" href="styles.css"></link>

<% 
    Dim oConn, year, day, rsData, rsCourses
    
    Set oConn = GetConnection()
	year = Request("year")
	day = Request("HeaderSelect")

    Set rsCourses = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & year)
 %>

<table align=center>
<tr>
<td colspan=2 align=center>
<table border=1 bordercolor=black cellpadding=1 cellspacing=0>
 <tr><td colspan=2 class=Title align=center>Monster <%=year%>:  <%=rsCourses("Course") %></td></tr>
  <tr>
<%  Dim dayText
    For i = 1 to 3
        If (i = 1) Then
            MoveToDay rsCoures, "Practice", year
            dayText = "Practice"
        ElseIf (i = 2) Then
            MoveToDay rsCoures, "Day1", year
            dayText = "Day 1"
        Else
            MoveToDay rsCoures, "Day2", year
            dayText = "Day 2"
        End If
 %>
   <td class=Score><b><%=rsCourses("Description")%> (<%=dayText%>)</b>
    <table width=100%>
      <tr>
        <td>Men's Tees:</td>
        <td><%=Round(rsCourses("MensRating"), 1)%> / <%=rsCourses("MensSlope")%></td>
        <td align=right>Par <%=rsCourses("MensPar")%></td>
      </tr>
      <tr>
        <td>Ladies Tees:</td>
        <td><%=Round(rsCourses("WomensRating"), 1)%> / <%=rsCourses("WomensSlope")%></td>
        <td align=right>Par <%=rsCourses("WomensPar")%></td>
      </tr>
    </table>
    </td>
  </tr>
<%  Next %>
  </table>
</td>
</tr>
</table>

<%
    
    sql = "SELECT TourneyScores.*, Users.* " & _
        "FROM TourneyScores INNER JOIN Users ON TourneyScores.UserId = Users.UserID " & _
        "WHERE (((TourneyScores.Year)=" & year & ")) " & _
        "ORDER BY Users.LastName, Users.FirstName"
        'Response.Write(sql)
    Set rsData = GetRecords(oConn, sql)

    Dim playerData(1000, 6)
    Dim numPlayers, prevUserId, score, totalPractice, totalPlayersPractice, totalDay1, totalDay2
    Dim aScoresPlayer(18), aNetScoresPlayer(18)
    
    numPlayers = 0
    prevUserId = ""
    Do Until rsData.EOF
        If (prevUserId <> rsData("Users.UserId")) Then
            prevUserId = rsData("Users.UserId")
            numPlayers = numPlayers + 1
            playerData(numPlayers, 1) = prevUserId
            playerData(numPlayers, 2) = rsData("LastName") + ", " + rsData("Firstname")
        End If

        GetUsersScore_work rsData, "", "", aScoresPlayer, aNetScoresPlayer, False, score

        If (rsData("Round") = "Practice") Then
            playerData(numPlayers, 3) = score
            totalPractice = totalPractice + score
            If (score <> "") Then
                totalPlayersPractice = totalPlayersPractice + 1
            End If
        ElseIf (rsData("Round") = "Day1") Then
            playerData(numPlayers, 4) = score
            totalDay1= totalDay1 + score
        ElseIf (rsData("Round") = "Day2") Then
            playerData(numPlayers, 5) = score
            totalDay2 = totalDay2 + score
        End If   
    
        rsData.MoveNext()
    Loop

    Dim itemsPerColumn : itemsPerColumn = numPlayers / 2
    If (numPlayers Mod 2 <> 0) Then
        itemsPerColumn = itemsPerColumn + 1
    End If
 %>

<table align=center>
<tr>
 <td valign=top>
 <table border=1 bordercolor=black cellpadding=1 cellspacing=0>
  <tr>
    <td class=Title nowrap>Name</td>
    <td class=Title nowrap>Practice</td>
    <td class=Title nowrap>Day 1</td>
    <td class=Title nowrap>Day 2</td>
  </tr>
 <% Dim practiceScore
    For i = 1 to itemsPerColumn
        Response.Write("   <tr>" & vbLf)
        Response.Write("    <td class=Name nowrap>" & GetDisplayName(playerData(i, 1), playerData(i, 2)) & "</td>" & vbLf)
        If (playerData(i, 3) = "") Then
            practiceScore = "-"
        Else
        practiceScore = playerData(i, 3)
        End If
        Response.Write("    <td class=Score align=center>" & practiceScore & "</td>" & vbLf)
        Response.Write("    <td class=Score align=center>" & playerData(i, 4) & "</td>" & vbLf)
        Response.Write("    <td class=Score align=center>" & playerData(i, 5) & "</td>" & vbLf)
        Response.Write("   </tr>" & vbLf)
    Next
%>
  </table>
 </td>
 <td>&nbsp;&nbsp;</td>
 
 <td valign=top>
  <table align=right border=1 bordercolor=black cellpadding=1 cellspacing=0>
   <tr>
    <td class=Title nowrap>Name</td>
    <td class=Title nowrap>Practice</td>
    <td class=Title nowrap>Day 1</td>
    <td class=Title nowrap>Day 2</td>
   </tr>
<%  For i = itemsPerColumn + 1 to numPlayers
        Response.Write("   <tr>" & vbLf)
        Response.Write("    <td class=Name nowrap>" & GetDisplayName(playerData(i, 1), playerData(i, 2)) & "</td>" & vbLf)
        If (playerData(i, 3) = "") Then
            practiceScore = "-"
        Else
        practiceScore = playerData(i, 3)
        End If
        Response.Write("    <td class=Score align=center>" & practiceScore & "</td>" & vbLf)
        Response.Write("    <td class=Score align=center>" & playerData(i, 4) & "</td>" & vbLf)
        Response.Write("    <td class=Score align=center>" & playerData(i, 5) & "</td>" & vbLf)
        Response.Write("   </tr>" & vbLf)
    Next 
    
    if (totalPlayersPractice) = 0 then totalPlayersPractice = 1 end if
    if (numPlayers) = 0 then numPlayers = 1 end if
    if (numPlayers) = 0 then numPlayers = 1 end if
    
    %>
    <tr>
     <td class=Name nowrap>Average</td>
     <td nowrap class=Score align=center><%=Round((totalPractice/totalPlayersPractice), 2)%></td>
     <td nowrap class=Score align=center><%=Round((totalDay1/numPlayers), 2)%></td>
     <td nowrap class=Score align=center><%=Round((totalDay2/numPlayers), 2)%></td>
    </tr>
  </table>
 </td>
</tr>
</table>

</center>

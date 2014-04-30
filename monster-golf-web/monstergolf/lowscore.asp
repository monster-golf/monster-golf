<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<!-- #include file='includes\funcs.asp' -->
<%  If (Request("year") = "") Then
        MainHeader "Stats"
    Else
        MainHeader "Results"
		ResultsHeader Request("Year"), Request("HeaderSelect")
		Response.Flush
    End If %>
<link rel="STYLESHEET" type="text/css" href="styles.css"></link>

<%
    Dim oConn, year, day, rs, rsCourses, yearSQL

    Set oConn = GetConnection()
	year = Request("year")
	day = Request("HeaderSelect")

    If (Request("year") <> "") Then
        yearSQL = " AND ((TourneyScores.Year=" & Request("Year") & ")) "
    End If

    If (Request("type") = "avegross" Or Request("type") = "avenet") Then
        sql = "SELECT (Hole1_Score+Hole2_Score+Hole3_Score+Hole4_Score+Hole5_Score+Hole6_Score+Hole7_Score+Hole8_Score+Hole9_Score+Hole10_Score+Hole11_Score+Hole12_Score+Hole13_Score++Hole14_Score+Hole15_Score+Hole16_Score+Hole17_Score+Hole18_Score) AS TotalScore, * " & _
            "FROM TourneyScores INNER JOIN Users ON TourneyScores.UserId = Users.UserID " & _
            "WHERE (((TourneyScores.Round)<>""Practice"")) " & yearSQL & _
            "ORDER BY Users.UserID"
    Else
        sql = "SELECT (Hole1_Score+Hole2_Score+Hole3_Score+Hole4_Score+Hole5_Score+Hole6_Score+Hole7_Score+Hole8_Score+Hole9_Score+Hole10_Score+Hole11_Score+Hole12_Score+Hole13_Score++Hole14_Score+Hole15_Score+Hole16_Score+Hole17_Score+Hole18_Score) AS TotalScore, * " & _
            "FROM TourneyScores INNER JOIN Users ON TourneyScores.UserId = Users.UserID " & _
            "WHERE (((TourneyScores.Round)<>""Practice"")) " & yearSQL & _
            "ORDER BY (Hole1_Score+Hole2_Score+Hole3_Score+Hole4_Score+Hole5_Score+Hole6_Score+Hole7_Score+Hole8_Score+Hole9_Score+Hole10_Score+Hole11_Score+Hole12_Score+Hole13_Score+Hole14_Score+Hole15_Score+Hole16_Score+Hole17_Score+Hole18_Score)"
    End If
    Set rs = GetRecords(oConn, sql)
'Response.Write(sql)
    Set rsCourses = GetRecords(oConn, "SELECT * FROM Courses")

    rs.MoveFirst()

    Dim aData, counter, aScores(18), aNetScores(19), totalScore, netScore, userID
    ReDim aData(10000, 7)

    counter = 0
    If (Request("type") = "net") Then
        While (Not rs.EOF)
            counter = counter + 1
            aData(counter, 1) = rs("Users.UserID")
            aData(counter, 2) = rs("FirstName") + " " + rs("LastName")
            aData(counter, 3) = rs("Year")
            aData(counter, 4) = rs("Round")

            MoveCoursesToCorrectDay rsCourses, rs("Year"), rs("Round")
            GetUsersScore_work rs, rsCourses, rs("Gender"), aScores, aNetScores, True, totalScore

            netScore = 0 
            For i = 1 to 18
                netScore = netScore + aNetScores(i)
            Next
            aData(counter, 5) = netScore
            aData(counter, 6) = rs("Handicap")

            rs.MoveNext()
        Wend
        ArraySort aData, counter, 7, 5, True
    ElseIf (Request("type") = "gross") Then
        While (Not rs.EOF)
            counter = counter + 1
            aData(counter, 1) = rs("Users.UserID")
            aData(counter, 2) = rs("FirstName") + " " + rs("LastName")
            aData(counter, 3) = rs("Year")
            aData(counter, 4) = rs("Round")
            aData(counter, 5) = rs("TotalScore")
            aData(counter, 6) = rs("Handicap")

            rs.MoveNext()
        Wend
    ElseIf (Request("type") = "avegross") Then
        While (Not rs.EOF)
            If (userID <> rs("Users.UserID")) Then
                userID = rs("Users.UserID")
                counter = counter + 1
                aData(counter, 1) = rs("Users.UserID")
                aData(counter, 2) = rs("FirstName") + " " + rs("LastName")
                aData(counter, 3) = rs("Year")
                aData(counter, 4) = rs("Round")
                aData(counter, 5) = rs("TotalScore")
                aData(counter, 6) = rs("Handicap")
                aData(counter, 7) = 1
            Else
                aData(counter, 5) = (((aData(counter, 5) * aData(counter, 7)) + rs("TotalScore")) / (aData(counter, 7) + 1))
                aData(counter, 6) = (((aData(counter, 6) * aData(counter, 7)) + rs("Handicap")) / (aData(counter, 7) + 1))
                aData(counter, 7) = aData(counter, 7) + 1
            End If
            
            rs.MoveNext()
        Wend
        ArraySort aData, counter, 7, 5, True
    ElseIf (Request("type") = "avenet") Then
        While (Not rs.EOF)
            MoveCoursesToCorrectDay rsCourses, rs("Year"), rs("Round")
            GetUsersScore_work rs, rsCourses, rs("Gender"), aScores, aNetScores, True, totalScore
            
            netScore = 0 
            For i = 1 to 18
                netScore = netScore + aNetScores(i)
            Next

            If (userID <> rs("Users.UserID")) Then
                userID = rs("Users.UserID")
                counter = counter + 1
                aData(counter, 1) = rs("Users.UserID")
                aData(counter, 2) = rs("FirstName") + " " + rs("LastName")
                aData(counter, 3) = rs("Year")
                aData(counter, 4) = rs("Round")
                aData(counter, 5) = netScore
                aData(counter, 6) = rs("Handicap")
                aData(counter, 7) = 1
            Else
                aData(counter, 5) = (((aData(counter, 5) * aData(counter, 7)) + netScore) / (aData(counter, 7) + 1))
                aData(counter, 6) = (((aData(counter, 6) * aData(counter, 7)) + rs("Handicap")) / (aData(counter, 7) + 1))
                aData(counter, 7) = aData(counter, 7) + 1
            End If
            
            rs.MoveNext()
        Wend
        ArraySort aData, counter, 7, 5, True
    End If
 %>
<BR />
<center><b>
<%  If (Request("type") = "gross") Then
        Response.Write("Low Gross")
    ElseIf (Request("type") = "avegross") Then
        Response.Write("Average Low Gross")
    ElseIf (Request("type") = "net") Then
        Response.Write("Low Net")
    ElseIf (Request("type") = "avenet") Then
        Response.Write("Average Low Net")
    End If 
    
    If (Request("year") <> "") Then
        Response.Write(" for year " & Request("Year"))
    End If
    
    %>
</b> &nbsp;(top 25 and ties)</center>
<BR />
<table align=center class=tableheadnowidth>
  <tr>
<%  If (Request("type") = "gross" Or Request("type") = "net") Then %>
    <td class="colhead">&nbsp;</td>
    <td class="colhead">Name</td>
<%       If (Request("year") = "") Then %>
    <td class="colhead">Year</td>
<%      End If %>
    <td class="colhead">Day</td>
    <td class="colhead">Score</td>
    <td class="colhead">Handicap</td>
<%  Else %>   
    <td class="colhead">&nbsp;</td>
    <td class="colhead">Name</td>
<%      If (Request("year") = "") Then %>
    <td class="colhead">Monsters</td>
    <td class="colhead">Average Score</td>
    <td class="colhead">Average Handicap</td>
<%      Else %>
    <td class="colhead">Score</td>
    <td class="colhead">Handicap</td>
<%      End If
    End If %>
   </tr>
   
<% Sub DisplayRow(i,lastscore,ioflastscore)
      Dim curYear, curPlayerID, curPlayerRound, curPlayerName, sclass
      if (i mod 2 = 0) then
         sclass=" class=""evenrow"""
      else
         sclass=" class=""oddrow"""
      end if
        curPlayerID = aData(i, 1)
        curPlayerName = aData(i, 2)
        curYear = aData(i, 3)
        curPlayerRound = aData(i, 4)

        If (Request("type") = "gross" Or Request("type") = "net") Then
           if lastscore <> aData(i, 5) then
            ioflastscore = i
           end if
        else
           if lastscore <> Round(aData(i, 5), 2) then
            ioflastscore = i
           end if
        end if

        Response.Write("  <tr>" & vbLf)
        Response.Write("    <td" & sclass & ">" & ioflastscore & "</td>" & vbLf)
        Response.Write("    <td" & sclass & ">" & GetDisplayName(curPlayerId, curPlayerName) & "</td>" & vbLf)
        
        If (Request("type") = "gross" Or Request("type") = "net") Then
            If (Request("year") = "") Then
                Response.Write("    <td" & sclass & "><a href='results.asp?year=" & curYear & "'>" & curYear & "</a></td>" & vbLf)
            End If
            Response.Write("    <td" & sclass & "><a href='results.asp?year=" & curYear & "&HeaderSelect=" & curPlayerRound & "'>" & curPlayerRound & "</a></td>" & vbLf)
            Response.Write("    <td" & sclass & ">" & aData(i, 5) & "</td>" & vbLf)
            Response.Write("    <td" & sclass & ">" & aData(i, 6) & "</td>" & vbLf)
           lastscore= aData(i, 5)
        Else
            If (Request("year") = "") Then
                Response.Write("    <td" & sclass & ">" & aData(i, 7) / 2 & "</td>" & vbLf)
            End If
            Response.Write("    <td" & sclass & " align=center>" & Round(aData(i, 5), 2) & "</td>" & vbLf)
            Response.Write("    <td" & sclass & " align=center>" & Round(aData(i, 6), 2) & "</td>" & vbLf)
           lastscore= Round(aData(i, 5), 2)
        End If
        Response.Write("  </tr>" & vbLf)
   End Sub
    
dim lastscore, ind, nextscore, ioflastscore
lastscore = 0
ioflastscore = 1
    For ind = 1 to 25
       DisplayRow ind,lastscore,ioflastscore
    Next
    If (Request("type") = "gross" Or Request("type") = "net") Then
      nextscore = aData(ind,5)
    else
      nextscore = Round(aData(ind, 5), 2)
    end if      

    Do Until lastscore <> nextscore
      DisplayRow ind,lastscore,ioflastscore
      ind = ind+1
      If (Request("type") = "gross" Or Request("type") = "net") Then
         nextscore = aData(ind,5)
      else
         nextscore = Round(aData(ind, 5), 2)
      end if      
    Loop
 %>
</table>

<%  If (Request("year") = "") Then
	    MainFooter("Stats")
	Else
		MainFooter("Results") 
	End If
	
	 

Sub MoveCoursesToCorrectDay(rsCourses, year, day)
    rsCourses.MoveFirst()

    Do While (Not rsCourses.EOF)
        If (rsCourses("Year") = year And StrComp(rsCourses("Day"), day, vbTextCompare) = 0) Then
            Exit Do
        End If
        rsCourses.MoveNext()
    Loop
End Sub

%>

<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<!-- #include file='includes\funcs.asp' -->
<%	MainHeader "Results"

    Dim oConn, userId, rsUserInfo, fullName, sql, counter, firstlastnameimage, lastfirstnameimage
    
    Set oConn = GetConnection()
	userId = Request("userId")
	
    Set rsUserInfo = GetRecords(oConn, "SELECT * FROM Users WHERE UserID=" + userId)
    firstlastnameimage = rsUserInfo("FirstName") & Replace(rsUserInfo("LastName"), " ", "")
    lastfirstnameimage = Replace(rsUserInfo("LastName"), " ", "") & rsUserInfo("FirstName")
    
    if (IsNull(rsUserInfo("NickName")) Or Trim(rsUserInfo("NickName")) = "") Then
      fullName = rsUserInfo("FirstName") & " " & rsUserInfo("LastName")
    else
      fullName = rsUserInfo("FirstName") & " """ & rsUserInfo("NickName") & """ " & rsUserInfo("LastName")
    end if
%>
<link rel="STYLESHEET" type="text/css" href="styles.css"></link>
&nbsp;&nbsp;
<table border=0 cellspacing=0>
 <tr>
  <td>
<%  Dim image
    Dim fso : Set fso = Server.CreateObject("Scripting.FileSystemObject")
    
    If (fso.FileExists(Server.MapPath(".") + "\images\" + lastfirstnameimage + ".jpg")) Then
        image = lastfirstnameimage + ".jpg"
    ElseIf (fso.FileExists(Server.MapPath(".") + "\images\" + firstlastnameimage + ".jpg")) Then
        image = firstlastnameimage + ".jpg"
    ElseIf (IsNull(rsUserInfo("Image"))) Then
        image = "noperson.jpg"
    Else
        image = rsUserInfo("Image")
    End If %>
    <img style="border:groove 6px silver;" src="images/<%=image%>" alt="<%=fullName%>"/>
  </td>
  <td valign=top nowrap>
   <span class=userName colspan=3><%=fullName%></span><br>
   <b>Gender:</b> 
<%  If (StrComp(rsUserInfo("Gender"), "F", vbTextCompare) = 0) Then
        Response.Write("Female") 
    Else
        Response.Write("Male") 
    End If %>
    <br>
    <%if not isnull(rsUserInfo("History")) then response.Write(Replace(rsUserInfo("History"), vbCrLf, "<br>")) end if%>
  </td>
  <td width=100%>&nbsp;</td>
 </tr>
 <tr>
  <td colspan=3>
<% 
Dim aMonsterTourneyPartners(20, 3), aMonsterPracticeRoundPartners(20, 3)
Dim countTourneyPartners, countPracticeRoundPartners, topTenFinishes, rowClass
topTenFinishes = 0

GetTournamentPartners userId, aMonsterTourneyPartners, countTourneyPartners, "Tournament"

Response.Write("<table border=0><tr><td valign=top>" & vbLf)

Response.Write("<table border=0 class=tablehead>" & vbLf)
Response.Write(" <tr>" & vbLf)
Response.Write("  <td colspan=8 class=header><b>Monster Tournament</b></td>" & vbLf)
Response.Write(" </tr>" & vbLf)

Response.Write(" <tr class=colhead>" & vbLf)
Response.Write("  <td class=colhead><b>Year</b></td>" & vbLf)
Response.Write("  <td class=colhead><b>Name</b></td>" & vbLf)
Response.Write("  <td class=colhead><b>Day 1 Score</b></td>" & vbLf)
Response.Write("  <td class=colhead><b>Day 1 Rank</b></td>" & vbLf)
Response.Write("  <td class=colhead><b>Day 2 Score</b></td>" & vbLf)
Response.Write("  <td class=colhead><b>Day 2 Rank</b></td>" & vbLf)
Response.Write("  <td class=colhead><b>Final Score</b></td>" & vbLf)
Response.Write("  <td class=colhead><b>Final Rank</b></td>" & vbLf)
Response.Write(" </tr>" & vbLf)

Dim year, teamID, teamIDSwapped, aTourneyData, numTeams, partnerId

For q = 1 to countTourneyPartners
    year = aMonsterTourneyPartners(q, 1)
    partnerId = aMonsterTourneyPartners(q, 3)
    teamID = userId & "/" & partnerId
    teamIDSwapped = partnerId & "/" & userId
 
    If (q Mod 2 = 0) Then
        rowClass = "evenrow"
    Else
        rowClass = "oddRow"
    End If

    If (year <> "") Then
        If (CalculateTourneyScoresForYear(year)) Then
            Response.Write(" <tr class=" & rowClass & ">" & vbLf)
            Response.Write("  <td class=" & rowClass & "><a href='roundbreakdown.asp?year=" & year & "&player1=" & userId & "&player2=" & aMonsterTourneyPartners(q, 3) & "'>" & year & "</td>" & vbLf)
            Response.Write("  <td nowrap class=" & rowClass & ">" & GetDisplayName(aMonsterTourneyPartners(q, 3), aMonsterTourneyPartners(q, 2)) & "</td>" & vbLf)

            aTourneyData = Application("MonsterTourneyScores" & year)

            numTeams = Application("MonsterTourneyNumTeams" & year)
            For j = 0 to numTeams - 1
                If (StrComp(aTourneyData(j, 5), teamID, vbTextCompare) = 0 Or _
                    StrComp(aTourneyData(j, 5), teamIDSwapped, vbTextCompare) = 0) Then
                    Response.Write("  <td align=center class=" & rowClass & ">" & aTourneyData(j, 3) & "</td>" & vbLf)
                    Response.Write("  <td align=center class=" & rowClass & ">" & aTourneyData(j, 10) & " of " & numTeams & "</td>" & vbLf)
                    Response.Write("  <td align=center class=" & rowClass & ">" & aTourneyData(j, 4) & "</td>" & vbLf)
                    Response.Write("  <td align=center class=" & rowClass & ">" & aTourneyData(j, 11) & " of " & numTeams & "</td>" & vbLf)
                    Response.Write("  <td align=center class=" & rowClass & ">" & aTourneyData(j, 1) & "</td>" & vbLf)
                                 
                    If (aTourneyData(j, 9) < 11) Then
                        topTenFinishes = topTenFinishes + 1
                    End If
                    Response.Write("  <td align=center class=" & rowClass & ">" & aTourneyData(j, 9) & " of " & numTeams & "</td>" & vbLf)
                End If
            Next
            Response.Write(" </tr>" & vbLf)
        End If
    End If
Next
Response.Write(" </table>" & vbLf)
Response.Write("</td>" & vbLf)
Response.Write("</tr>" & vbLf)

Response.Write("&nbsp;&nbsp;&nbsp" & vbLf)

Response.Write("<tr>" & vbLf)
Response.Write("<td valign=top>" & vbLf)
Response.Write("<table border=0 class=tablehead>" & vbLf)
Response.Write(" <tr>" & vbLf)
Response.Write("  <td colspan=4 class=header><b>Monster Practice Round</b></td>" & vbLf)
Response.Write(" </tr>" & vbLf)

Response.Write(" <tr class=colhead>" & vbLf)
Response.Write("  <td class=colhead><b>Year</b></td>" & vbLf)
Response.Write("  <td class=colhead><b>Name</b></td>" & vbLf)
Response.Write("  <td class=colhead><b>Score</b></td>" & vbLf)
Response.Write("  <td class=colhead><b>Rank</b></td>" & vbLf)
Response.Write(" </tr>" & vbLf)

GetTournamentPartners userId, aMonsterPracticeRoundPartners, countPracticeRoundPartners, "Practice"

Dim aPracticeRoundData
For q = 1 to countPracticeRoundPartners
    year = aMonsterPracticeRoundPartners(q, 1)
    partnerId = aMonsterPracticeRoundPartners(q, 3)
    teamID = userId & "/" & partnerId
    teamIDSwapped = partnerId & "/" & userId

    If (q Mod 2 = 0) Then
        rowClass = "evenrow"
    Else
        rowClass = "oddRow"
    End If

    Response.Write(" <tr>" & vbLf)
    Response.Write("  <td class=" & rowClass & "><a href='results.asp?year=" & year & "'>" & year & "</a></td>" & vbLf)
    Response.Write("  <td class=" & rowClass & ">" & GetDisplayName(partnerId, aMonsterPracticeRoundPartners(q, 2)) & "</td>" & vbLf)

    If (year <> "") Then
        CalculatePracticeRoundScoresForYear year

        aPracticeRoundData = Application("MonsterPracticeRoundScores" & year)
        numTeams = Application("MonsterPracticeRoundNumTeams" & year)

        For j = 0 to numTeams - 1
            If (StrComp(aPracticeRoundData(j, 3), teamID, vbTextCompare) = 0 Or _
                StrComp(aPracticeRoundData(j, 3), teamIDSwapped, vbTextCompare) = 0) Then
                Response.Write("  <td align=center class=" & rowClass & ">" & aPracticeRoundData(j, 1) & "</td>" & vbLf)
                Response.Write("  <td align=center class=" & rowClass & ">" & aPracticeRoundData(j, 4) & " of " & numTeams & "</td>" & vbLf)
            End If
        Next
    End If
    Response.Write(" </tr>" & vbLf)
Next
Response.Write(" </table>" & vbLf)
Response.Write("</td>" & vbLf)
Response.Write("</tr>" & vbLf)

'' Get history in previous monsters
''
sql = "SELECT TourneyScores.*, Users.* " & _
    "FROM TourneyScores INNER JOIN Users ON TourneyScores.UserId = Users.UserID " & _
    "WHERE (((TourneyScores.UserID)=" & userId & ")) " & _
    "ORDER BY TourneyScores.Year"

Set rsScores = GetRecords(oConn, sql)
Dim countRounds, curYear, numEvents, aYears(20), dict, key

Set dict = Server.CreateObject("Scripting.Dictionary")

Do Until rsScores.EOF
    '' Make sure we only add one item to the list.  A person could participate on more than one practice round group
    ''
    key = rsScores("Year") & rsScores("Round")
    If (Not dict.Exists(key)) Then
        countRounds = countRounds + 1
        dict.Add rsScores("Year") & rsScores("Round"), "bacon"
    End If

    runningTotalHandicap = runningTotalHandicap + rsScores("Handicap")
    If (curYear <> rsScores("Year")) Then
        curYear = rsScores("Year")
        numEvents = numEvents + 1
        aYears(numEvents) = curYear
    End If
    
    rsScores.MoveNext()
Loop
    
Response.Write("<tr>" & vbLf)
Response.Write("<td valign=top>" & vbLf)
Response.Write("<table border=0 class=tablehead>" & vbLf)
Response.Write(" <tr class=stathead>" & vbLf)
Response.Write("  <td colspan=6 class=header><b>Monster History Overview</b></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=colhead>" & vbLf)
Response.Write("  <td class=colhead>Monsters</td>" & vbLf)
Response.Write("  <td class=colhead>Top 10 Finishes</td>" & vbLf)
Response.Write("  <td class=colhead>Rounds</td>" & vbLf)
Response.Write("  <td class=colhead>Avg Handicap</td>" & vbLf)
Response.Write("  <td class=colhead>Avg Score</td>" & vbLf)
Response.Write("  <td class=colhead>Avg Net Score</td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=oddrow>" & vbLf)
Response.Write("  <td align=center id=tdEvents>" & numEvents & "</td>" & vbLf)
Response.Write("  <td align=center>" & topTenFinishes & "</td>" & vbLf)
Response.Write("  <td align=center id=tdNumRounds>" & countRounds & "</td>" & vbLf)
Response.Write("  <td align=center id=tdAverageHandicap></td>" & vbLf)
Response.Write("  <td align=center id=tdAverageScore></td>" & vbLf)
Response.Write("  <td align=center id=tdAverageNetScore></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" </table>" & vbLf)
Response.Write("</td>" & vbLf)
Response.Write("</tr>" & vbLf)


Response.Write("<tr>" & vbLf)
Response.Write("<td valign=top>" & vbLf)
Response.Write("<table border=0 class=tablehead>" & vbLf)
Response.Write(" <tr class=stathead>" & vbLf)
Response.Write("  <td colspan=5 class=header><b>Monster Stats</b></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=colhead>" & vbLf)
Response.Write("  <td class=colhead>STAT</td>" & vbLf)
Response.Write("  <td class=colhead>Total</td>" & vbLf)
Response.Write(" </tr>" & vbLf)

'For j = 1 to numEvents
'    curYear = aYears(j)
'    Response.Write(" <tr class=evenrow>" & vbLf)
'    Response.Write("  <td align=left class=evenrow>Monster " & curYear & " scoring average</td>" & vbLf)
'    Response.Write("  <td align=center class=evenrow id=tdMonsterAverage" & curYear & "></td>" & vbLf)
 '   Response.Write(" </tr>" & vbLf)
 '   
 '   Response.Write(" <tr class=evenrow class=evenrow>" & vbLf)
 '   Response.Write("  <td align=left class=evenrow>Monster " & curYear & " net scoring average</td>" & vbLf)
 '   Response.Write("  <td align=center class=evenrow id=tdNetMonsterAverage" & curYear & "></td>" & vbLf)
 '   Response.Write(" </tr>" & vbLf)
'Next

Response.Write(" <tr class=oddrow>" & vbLf)
Response.Write("  <td align=left class=oddrow>Number of double eagles</td>" & vbLf)
Response.Write("  <td align=center class=oddrow id=tdDoubleEagles></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=oddrow>" & vbLf)
Response.Write("  <td align=left class=oddrow>Number of eagles</td>" & vbLf)
Response.Write("  <td align=center class=oddrow id=tdEagles></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=oddrow>" & vbLf)
Response.Write("  <td align=left class=oddrow>Number of birdies</td>" & vbLf)
Response.Write("  <td align=center class=oddrow id=tdBirdies></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=oddrow>" & vbLf)
Response.Write("  <td align=left class=oddrow>Number of pars</td>" & vbLf)
Response.Write("  <td align=center class=oddrow id=tdPars></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=oddrow>" & vbLf)
Response.Write("  <td align=left class=oddrow>Number of bogeys</td>" & vbLf)
Response.Write("  <td align=center class=oddrow id=tdBogeys></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=oddrow>" & vbLf)
Response.Write("  <td align=left class=oddrow>Number of double bogeys+</td>" & vbLf)
Response.Write("  <td align=center class=oddrow id=tdDoubleBogeysPlus></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=evenrow>" & vbLf)
Response.Write("  <td align=left class=evenrow>Number of net double eagles</td>" & vbLf)
Response.Write("  <td align=center class=evenrow id=tdNetDoubleEagles></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=evenrow>" & vbLf)
Response.Write("  <td align=left class=evenrow>Number of net eagles</td>" & vbLf)
Response.Write("  <td align=center class=evenrow id=tdNetEagles></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=evenrow>" & vbLf)
Response.Write("  <td align=left class=evenrow>Number of net birdies</td>" & vbLf)
Response.Write("  <td align=center class=evenrow id=tdNetBirdies></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=evenrow>" & vbLf)
Response.Write("  <td align=left class=evenrow>Number of net pars</td>" & vbLf)
Response.Write("  <td align=center class=evenrow id=tdNetPars></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=evenrow>" & vbLf)
Response.Write("  <td align=left class=evenrow>Number of net bogeys</td>" & vbLf)
Response.Write("  <td align=center class=evenrow id=tdNetBogeys></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" <tr class=evenrow>" & vbLf)
Response.Write("  <td align=left class=evenrow>Number of net double bogeys+</td>" & vbLf)
Response.Write("  <td align=center class=evenrow id=tdNetDoubleBogeysPlus></td>" & vbLf)
Response.Write(" </tr>" & vbLf)
Response.Write(" </table>" & vbLf)
Response.Write("</td>" & vbLf)
Response.Write("</tr>" & vbLf)



Response.Write("<tr>" & vbLf)
Response.Write("<td valign=top>" & vbLf)
Response.Write("<table border=0 class=tablehead>" & vbLf)
Response.Write(" <tr>" & vbLf)
Response.Write("  <td colspan=25 class=header><b>Monster Round History</b></td>" & vbLf)
Response.Write(" </tr>" & vbLf)

Response.Write(" <tr>" & vbLf)
Response.Write("  <td class=evenrow><b>Year</b></td>" & vbLf)
Response.Write("  <td class=evenrow><b>Round</b></td>" & vbLf)
Response.Write("  <td class=evenrow><b>Hcp</b></td>" & vbLf)
For i = 1 to 9
    Response.Write("  <td class=evenrow><b>" & i & "</b></td>" & vbLf)
Next
Response.Write("  <td class=evenrow><b>Out</b></td>" & vbLf)
For i = 10 to 18
    Response.Write("  <td class=evenrow><b>" & i & "</b></td>" & vbLf)
Next
Response.Write("  <td class=evenrow><b>IN</b></td>" & vbLf)
Response.Write("  <td class=evenrow><b>Total</b></td>" & vbLf)
Response.Write("  <td class=evenrow><b>Avg</b></td>" & vbLf)
Response.Write(" </tr>" & vbLf)


Dim countFor9, runningTotal, score, runningTotalHandicap, runningTotalScore, runningTotalNetScore
Dim countDoubleEagles, countEagles, countBirdies, countPars, countBogeys, countDoubleBogeysPlus
Dim countNetDoubleEagles, countNetEagles, countNetBirdies, countNetPars, countNetBogeys, countNetDoubleBogeysPlus

runningTotalHandicap = 0
runningTotalScore = 0

countDoubleEagles = 0
countEagles = 0
countBirdies = 0
countPars = 0
countBogeys = 0
countDoubleBogeysPlus = 0

countNetDoubleEagles = 0
countNetEagles = 0
countNetBirdies = 0
countNetPars = 0
countNetBogeys = 0
countNetDoubleBogeysPlus = 0

Dim scoreForRound, netScoreForRound, countRoundsWithScore, scoreForyear, netScoreForYear
For j = 1 to numEvents
    curYear = aYears(j)

    countRoundsWithScore = 0
    scoreForyear = 0
    netScoreForYear = 0
    If (MoveToDay(rsScores, "Practice", curYear)) Then
        scoreForRound = 0
        netScoreForRound = 0
        Set rsCourseInfo = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & curYear & " AND Day='Practice'")
        DisplayScoreForDay rsScores, rsCourseInfo, runningTotalHandicap, scoreForRound, netScoreForRound
        scoreForyear = scoreForyear + scoreForRound
        runningTotalScore = runningTotalScore + scoreForRound
        countRoundsWithScore = countRoundsWithScore + 1
    End If
    
    If (MoveToDay(rsScores, "Day1", curYear)) Then
        scoreForRound = 0
        netScoreForRound = 0
        Set rsCourseInfo = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & curYear & " AND Day='Day1'")
        DisplayScoreForDay rsScores, rsCourseInfo, runningTotalHandicap, scoreForRound, netScoreForRound
        scoreForyear = scoreForyear + scoreForRound
        netScoreForYear = netScoreForYear + netScoreForRound
        runningTotalScore = runningTotalScore + scoreForRound
        countRoundsWithScore = countRoundsWithScore + 1
    End If
    
    If (MoveToDay(rsScores, "Day2", curYear)) Then
        scoreForRound = 0
        netScoreForRound = 0
        Set rsCourseInfo = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & curYear & " AND Day='Day2'")
        DisplayScoreForDay rsScores, rsCourseInfo, runningTotalHandicap, scoreForRound, netScoreForRound
        scoreForyear = scoreForyear + scoreForRound
        netScoreForYear = netScoreForYear + netScoreForRound
        runningTotalScore = runningTotalScore + scoreForRound
        countRoundsWithScore = countRoundsWithScore + 1
    End If

    If (MoveToDay(rsScores, "Practice", curYear)) Then
        scoreForRound = 0
        netScoreForRound = 0
        Set rsCourseInfo = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & curYear & " AND Day='Practice'")
        DisplayNetScoreForDay rsScores, rsCourseInfo, runningTotalHandicap, scoreForRound, netScoreForRound
        netScoreForYear = netScoreForYear + netScoreForRound
    End If

    If (MoveToDay(rsScores, "Day1", curYear)) Then
        scoreForRound = 0
        netScoreForRound = 0
        Set rsCourseInfo = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & curYear & " AND Day='Day1'")
        DisplayNetScoreForDay rsScores, rsCourseInfo, runningTotalHandicap, scoreForRound, netScoreForRound
        netScoreForYear = netScoreForYear + netScoreForRound
    End If

    If (MoveToDay(rsScores, "Day2", curYear)) Then
        scoreForRound = 0
        netScoreForRound = 0
        Set rsCourseInfo = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & curYear & " AND Day='Day2'")
        DisplayNetScoreForDay rsScores, rsCourseInfo, runningTotalHandicap, scoreForRound, netScoreForRound
        netScoreForYear = netScoreForYear + netScoreForRound
    End If

    Response.Write("<script language=javascript>" & vbLf)
    Response.Write("document.getElementById('tdMonsterAverage" & curYear & "').innerHTML = '" & _
        Round(scoreForyear / countRoundsWithScore, 2) & "';" & vbLf)
    Response.Write("document.getElementById('tdNetMonsterAverage" & curYear & "').innerHTML = '" & _
        Round(netScoreForYear / countRoundsWithScore, 2) & "';" & vbLf)
    Response.Write("</script>" & vbLf)
    Response.Write(" <tr><td colspan=25 class=header>&nbsp;</td></tr>" & vbLf)
Next

Response.Write("<script language=javascript>" & vbLf)
Response.Write("document.getElementById('tdAverageHandicap').innerHTML = '" & Round(runningTotalHandicap / countRounds, 2) & "';" & vbLf)
Response.Write("document.getElementById('tdAverageScore').innerHTML = '" & Round(runningTotalScore / countRounds, 2) & "';" & vbLf)
Response.Write("document.getElementById('tdAverageNetScore').innerHTML = '" & Round(runningTotalNetScore / countRounds, 2) & "';" & vbLf)

Response.Write("document.getElementById('tdDoubleEagles').innerHTML = '" & _
    countDoubleEagles & " (" & Round((countDoubleEagles / (countRounds * 18) * 100), 2) & "%)';" & vbLf)
Response.Write("document.getElementById('tdEagles').innerHTML = '" & _
    countEagles & " (" & Round((countEagles / (countRounds * 18) * 100), 2) & "%)';" & vbLf)
Response.Write("document.getElementById('tdBirdies').innerHTML = '" & _
    countBirdies & " (" & Round((countBirdies / (countRounds * 18) * 100), 2) & "%)';" & vbLf)
Response.Write("document.getElementById('tdPars').innerHTML = '" & _
    countPars & " (" & Round((countPars / (countRounds * 18) * 100), 2) & "%)';" & vbLf)
Response.Write("document.getElementById('tdBogeys').innerHTML = '" & _
    countBogeys & " (" & Round((countBogeys / (countRounds * 18) * 100), 2) & "%)';" & vbLf)
Response.Write("document.getElementById('tdDoubleBogeysPlus').innerHTML = '" & _
    countDoubleBogeysPlus & " (" & Round((countDoubleBogeysPlus / (countRounds * 18) * 100), 2) & "%)';" & vbLf)

Response.Write("document.getElementById('tdNetDoubleEagles').innerHTML = '" & _
    countNetDoubleEagles & " (" & Round((countNetDoubleEagles / (countRounds * 18) * 100), 2) & "%)';" & vbLf)
Response.Write("document.getElementById('tdNetEagles').innerHTML = '" & _
    countNetEagles & " (" & Round((countNetEagles / (countRounds * 18) * 100), 2) & "%)';" & vbLf)
Response.Write("document.getElementById('tdNetBirdies').innerHTML = '" & _
    countNetBirdies & " (" & Round((countNetBirdies / (countRounds * 18) * 100), 2) & "%)';" & vbLf)
Response.Write("document.getElementById('tdNetPars').innerHTML = '" & _
    countNetPars & " (" & Round((countNetPars / (countRounds * 18) * 100), 2) & "%)';" & vbLf)
Response.Write("document.getElementById('tdNetBogeys').innerHTML = '" & _
    countNetBogeys & " (" & Round((countNetBogeys / (countRounds * 18) * 100), 2) & "%)';" & vbLf)
Response.Write("document.getElementById('tdNetDoubleBogeysPlus').innerHTML = '" & _
    countNetDoubleBogeysPlus & " (" & Round((countNetDoubleBogeysPlus / (countRounds * 18) * 100), 2) & "%)';" & vbLf)

Response.Write("</script>" & vbLf)

Response.Write("</table>" & vbLf)

Response.Write("</td></tr></table>" & vbLf)

%>
   </td>
  </tr>
</table>




<%   MainFooter("Results") 

Sub GetTournamentPartners(userId, ByRef aPrevMonsterPartners, ByRef countPartners, roundType)
    Dim rsMonsterPartners
    sql = "SELECT Users.*, Teams.Year, Teams.Type " + _
        "FROM Teams INNER JOIN Users ON Teams.Player2 = Users.UserID " & _
        "WHERE (((Teams.Player1)=" & userId & ") AND ((Teams.Type)='" & roundType & "'))"
    Set rsMonsterPartners = GetRecords(oConn, sql)
    countPartners = 0

    Do Until rsMonsterPartners.EOF
        countPartners = countPartners + 1
        aPrevMonsterPartners(countPartners, 1) = rsMonsterPartners("Year")
        aPrevMonsterPartners(countPartners, 2) = rsMonsterPartners("FirstName") & " " & rsMonsterPartners("LastName")
        aPrevMonsterPartners(countPartners, 3) = rsMonsterPartners("UserId")
        rsMonsterPartners.MoveNext()
    Loop
    Set rsMonsterPartners = Nothing

    sql = "SELECT Users.*, Teams.Year, Teams.Type " + _
        "FROM Teams INNER JOIN Users ON Teams.Player1 = Users.UserID " & _
        "WHERE (((Teams.Player2)=" & userId & ") AND ((Teams.Type)='" & roundType & "'))"
    Set rsMonsterPartners = GetRecords(oConn, sql)

    Do Until rsMonsterPartners.EOF
        countPartners = countPartners + 1
        aPrevMonsterPartners(countPartners, 1) = rsMonsterPartners("Year")
        aPrevMonsterPartners(countPartners, 2) = rsMonsterPartners("FirstName") & " " & rsMonsterPartners("LastName")
        aPrevMonsterPartners(countPartners, 3) = rsMonsterPartners("UserId")
        rsMonsterPartners.MoveNext()
    Loop
    Set rsMonsterPartners = Nothing
    
    ArraySort aPrevMonsterPartners, countPartners, 3, 1, True
End Sub


Sub DisplayScoreForDay(rsScores, rsCourseInfo, ByRef runningTotalHandicap, ByRef runningTotalScore, ByRef netScore)
    Response.Write(" <tr>" & vbLf)
    Response.Write("  <td><a href='results.asp?year=" & rsScores("Year") & "'>" & rsScores("Year") & "</a></td>" & vbLf)
    Response.Write("  <td>" & rsScores("Round") & "</td>" & vbLf)
    Response.Write("  <td>" & rsScores("Handicap") & "</td>" & vbLf)
 
    runningTotalHandicap = runningTotalHandicap + rsScores("Handicap")
    If (curYear <> rsScores("Year")) Then
        curYear = rsScores("Year")
        numEvents = numEvents + 1
    End If

    Dim aScores(18), aNetScores(18)
    GetUsersScore_work rsScores, rsCourseInfo, rsUserInfo("Gender"), aScores, aNetScores, True, dummy

    runningTotal = 0
	For i = 1 to 18
        score = aScores(i)
        countFor9 = countFor9 + score
        runningTotal = runningTotal + score
        Response.Write("  <td width=20>" & score & "</td>" & vbLf)

        If (i = 9 Or i = 18) Then
            Response.Write("  <td class=evenrow width=20>" & countFor9 & "</td>" & vbLf)
            countFor9 = 0
        End If

        UpdateCountOfScore i, score, rsCourseInfo, rsUserInfo("Gender")
    Next
    Response.Write("  <td>" & runningTotal & "</td>" & vbLf)
    
    If (StrComp(rsScores("Round"), "Day2", vbTextCompare) = 0) Then
        Response.Write("<td id='tdMonsterAverage" & rsScores("Year") & "'></td>")
    End If
    
    Response.Write(" </tr>" & vbLf)

    runningTotalScore = runningTotalScore + runningTotal
End Sub


Sub DisplayNetScoreForDay(rsScores, rsCourseInfo, ByRef runningTotalHandicap, ByRef runningTotalScore, ByRef netScore)
    '' Show the net score info
    ''
    Response.Write(" <tr>" & vbLf)
    Response.Write("  <td colspan=3 align=right>" & rsScores("Round") & " Net Score</td>" & vbLf)
    Dim cellScoreClass, overUnderPar
    
    Dim aScores(18), aNetScores(18)
    GetUsersScore_work rsScores, rsCourseInfo, rsUserInfo("Gender"), aScores, aNetScores, True, dummy

    countFor9 = 0
    runningTotal = 0
	For i = 1 to 18
        score = aNetScores(i)
        countFor9 = countFor9 + score
        runningTotal = runningTotal + score

        overUnderPar = UpdateCountOfNetScore(i, score, rsCourseInfo, rsUserInfo("Gender"))

        If (overUnderPar = -1) Then
            cellScoreClass = "under"
        ElseIf (overUnderPar = 0) Then
            cellScoreClass = "even"
        ElseIf (overUnderPar = 1) Then
            cellScoreClass = "over"
        End If

        Response.Write("  <td class=" & cellScoreClass & ">" & score & "</td>" & vbLf)

        If (i = 9 Or i = 18) Then
            Response.Write("  <td class=evenrow>" & countFor9 & "</td>" & vbLf)
            countFor9 = 0
        End If
    Next
    netScore = runningTotal
    Response.Write("  <td class=evenrow>" & netScore & "</td>" & vbLf)

    If (StrComp(rsScores("Round"), "Day2", vbTextCompare) = 0) Then
        Response.Write("<td id='tdNetMonsterAverage" & rsScores("Year") & "'></td>")
    End If

    Response.Write(" </tr>" & vbLf)
    
    runningTotalNetScore = runningTotalNetScore + runningTotal
End Sub


Function UpdateCountOfScore(hole, score, rsCourseInfo, gender)
    Dim parForHole, overUnderPar
    If (StrComp(genderPlayer, "M", vbTextCompare) = 0) Then
        parForHole = rsCourseInfo("MensPar" & hole)
    Else
        parForHole = rsCourseInfo("WomensPar" & hole)
    End If

    If (score - parForHole = -3) Then
        countDoubleEagles = countDoubleEagles + 1
        overUnderPar = -1
    ElseIf (score - parForHole = -2) Then
        countEagles = countEagles + 1
        overUnderPar = -1
    ElseIf (score - parForHole = -1) Then
        countBirdies = countBirdies + 1
        overUnderPar = -1
    ElseIf (score - parForHole = 0) Then
        countPars = countPars + 1
        overUnderPar = 0
    ElseIf (score - parForHole = 1) Then
        countBogeys = countBogeys + 1
        overUnderPar = 1
    ElseIf (score - parForHole > 1) Then
        countDoubleBogeysPlus = countDoubleBogeysPlus + 1
        overUnderPar = 1
    End If
    
    UpdateCountOfScore = overUnderPar
End Function


Function UpdateCountOfNetScore(hole, score, rsCourseInfo, gender)
    Dim parForHole, overUnderPar
    If (StrComp(genderPlayer, "M", vbTextCompare) = 0) Then
        parForHole = rsCourseInfo("MensPar" & hole)       
    Else
        parForHole = rsCourseInfo("WomensPar" & hole)       
    End If

    If (score - parForHole = -3) Then
        countNetdoubleEagles = countNetDoubleEagles + 1
        overUnderPar = -1
    ElseIf (score - parForHole = -2) Then
        countNetEagles = countNetEagles + 1
        overUnderPar = -1
    ElseIf (score - parForHole = -1) Then
        countNetBirdies = countNetBirdies + 1
        overUnderPar = -1
    ElseIf (score - parForHole = 0) Then
        countNetPars = countNetPars + 1
        overUnderPar = 0
    ElseIf (score - parForHole = 1) Then
        countNetBogeys = countNetBogeys + 1
        overUnderPar = 1
    ElseIf (score - parForHole > 1) Then
        countNetDoubleBogeysPlus = countNetDoubleBogeysPlus + 1
        overUnderPar = 1
    End If

    UpdateCountOfNetScore = overUnderPar
End Function


Function MoveToDay(rsScores, day, year)
    Dim foundDay : foundDay = False
    rsScores.MoveFirst()
    
	Do Until rsScores.EOF
	    If (StrComp(rsScores("Round"), day, vbTextCompare) = 0 And rsScores("Year") = year) Then
	        foundDay = True
	        Exit Do
	    End If
	
	    rsScores.MoveNext()
	Loop
	MoveToDay = foundDay
End Function

%>

<%

Function GetDisplayName(userId, name)
    '' Very soon, make this into a hyperlink
    ''
    GetDisplayName = "<a class='userlink' href='userprofile.asp?userId=" & userID & "'>" & name & "</a>"
End Function


Function GetNameFromDB(userID)
    Dim oConn, rsName
	Set oConn = GetConnection()
	Set rsName = GetRecords(oConn, "SELECT * FROM Users WHERE UserId= " & userID)

	If (Not rsName.EOF) Then
	    GetNameFromDB = rsName("FirstName") & " " & rsName("LastName")
	End If
End Function


Function GetGenderFromDB(userID)
    Dim oConn, rsName
	Set oConn = GetConnection()
	Set rsName = GetRecords(oConn, "SELECT Gender FROM Users WHERE UserId= " & userID)

	If (Not rsName.EOF) Then
	    GetGenderFromDB = rsName("Gender")
	End If
End Function


Function GetParPointsForHole(rsParPointsScoring, score, hole, rsCourseInfo, gender)
    Dim parForHole, parPoints
    If (StrComp(gender, "M", vbTextCompare) = 0) Then
        parForHole = rsCourseInfo("MensPar" & hole)
    Else
        parForHole = rsCourseInfo("WomensPar" & hole)
    End If
    'Response.Write("GetParPointsForHole, score [" & score & "], hole [" & hole & "], parForHole [" & parForHole & "]<br>")
    
    Dim shotsOverPar : shotsOverPar = score - parForHole
    
    GetParPointsForHole = CInt( rsParPointsScoring("ShotsOverPar" & shotsOverPar) )
    
    'Response.Write("GetParPointsForHole, score [" & score & "], hole [" & hole & "], parForHole [" & parForHole & "], parPoints [" & parPoints & "]<br>")
End Function

Function CalculateParPointsForHoleForTeam2007(player1ParPointsForHole, player2ParPointsForHole, holePar)
   If (holePar = 4) Then
      CalculateParPointsForHoleForTeam2007 = CalculateParPointsForHoleForTeam("Day1", player1ParPointsForHole, player2ParPointsForHole)
   Else
      CalculateParPointsForHoleForTeam2007 = CalculateParPointsForHoleForTeam("Day2", player1ParPointsForHole, player2ParPointsForHole)
   End If
End Function
Function CalculateParPointsForHoleForTeam(day, player1ParPointsForHole, player2ParPointsForHole)
    If (StrComp(day, "Practice", vbTextCompare) = 0 Or _
        StrComp(day, "Day1", vbTextCompare) = 0 ) Then
        If (player1ParPointsForHole > player2ParPointsForHole) Then
            CalculateParPointsForHoleForTeam = player1ParPointsForHole
        Else
            CalculateParPointsForHoleForTeam = player2ParPointsForHole                   
        End If
    Else
        CalculateParPointsForHoleForTeam = player1ParPointsForHole + player2ParPointsForHole
    End If
End Function
                
                
Function GetUsersScore(oConn, year, day, userID, rsCourseInfo, genderPlayer, aScoresPlayer, aNetScoresPlayer, matchupType)
    Dim sql, dummy
    sql = "SELECT * " + _
        "FROM TourneyScores INNER JOIN Teams ON (TourneyScores.UserId = Teams.Player2) OR (TourneyScores.UserId = Teams.Player1) " + _
        "WHERE (((TourneyScores.UserId)=" & userID & ") AND ((TourneyScores.Year)=" & year & ") " + _
        "  AND ((TourneyScores.Round)='" + day + "') AND ((Teams.Type)='" & matchupType & "'))"
    Set rsScores = GetRecords(oConn, sql)

    '' If there is no data, exit
    ''
    If (rsScores.EOF) Then
        GetUsersScore = False
        Exit Function
    End If
    
    GetUsersScore_work rsScores, rsCourseInfo, genderPlayer, aScoresPlayer, aNetScoresPlayer, True, dummy
End Function


Function GetUsersScore_work(rsScores, rsCourseInfo, genderPlayer, aScores, aNetScores, getNetScore, ByRef outScore)
    Dim handicap, sql, strType, totalScore

    handicap = rsScores("Handicap")

    If (Not rsScores.EOF) Then
	    Dim handicapOfHole, i, j, pointsToSubtractFromGrossScore, tempHandicap
	        
	    '' Get the first line of information, which are all fo the scores totalled up
	    ''
	    For i = 1 to 18
	        aScores(i) = rsScores("Hole" & i & "_Score")
	        totalScore = totalScore + aScores(i)
	    Next

        If (getNetScore) Then
	        '' Display the net score, using the handicap
	        ''
            For i = 1 to 18
                pointsToSubtractFromGrossScore = 0
                
                '' From the handicap, derive the net score
                ''
                If (StrComp(genderPlayer, "M", vbTextCompare) = 0) Then
                    handicapOfHole = rsCourseInfo("MensHandicap" & i)       
                Else
                    handicapOfHole = rsCourseInfo("WomensHandicap" & i)       
                End If

                tempHandicap = handicap
                While (tempHandicap >= handicapOfHole)
                    pointsToSubtractFromGrossScore = pointsToSubtractFromGrossScore + 1
                    tempHandicap = tempHandicap - 18
                Wend

                aNetScores(i) = aScores(i) - pointsToSubtractFromGrossScore
            Next
        End If
    End If
    Set scores = Nothing

    outScore = totalScore
    DisplayUsersScore = True
End Function


Sub ArraySort(aItems, size, columnsInArray, columnToCompare, isAscending)
    For i = 0 to size  
        For j = i+1 to size  
            Dim shouldSwap : shouldSwap = False
            If (Not isAscending) Then
                shouldSwap = (aItems(i, columnToCompare) < aItems(j, columnToCompare))
            Else 
                shouldSwap = (aItems(i, columnToCompare) > aItems(j, columnToCompare))
            End If
            
            If (shouldSwap) Then 
                SwapItems aItems, i, j, columnsInArray
            End if 
        Next  
    Next 
End Sub

Sub SwapItems(aItems, i, j, columnsInArray)
    Dim aTemp, k
    ReDim aTemp(columnsInArray)
    
    For k = 0 to columnsInArray
        aTemp(k) = aItems(i, k)
    Next

    For k = 0 to columnsInArray
        aItems(i, k) = aItems(j, k)
    Next

    For k = 0 to columnsInArray
        aItems(j, k) = aTemp(k)
    Next
End Sub


Function CalculateTourneyScoresForYear(year)
    If (Application("CalculatedMonsterTourneyScore" & year) = True) Then
        CalculateTourneyScoresForYear = True
        Exit FUnction
    End If
    
    Application.Lock

    Dim sql, rsMatchups, usesFlights
    Dim rsCourseInfo, rsParPointsScoringrsMatchups, counter, names
    Dim aScoresPlayer1, aScoresPlayer2, aNetScorePlayer1, aNetScorePlayer2
  	Dim aTotalScores(1000, 11)
  
    Set rsParPointsScoring = GetRecords(oConn, "SELECT * FROM ParPointsScoring WHERE Year=" & year & " and Type='Tournament'")

    '' Get the Day 1 results
    ''
    sql = "SELECT Users.* AS [User1], Users_1.* AS [User2], Teams.* " + _
        "FROM (Teams INNER JOIN Users ON Teams.Player1 = Users.UserID) INNER JOIN Users AS Users_1 ON Teams.Player2 = Users_1.UserID " + _
        "WHERE (((Teams.Year)=" & year & ") AND ((Teams.Player1)=[Users].[UserID]) AND ((Teams.Type)='Tournament')) " & _
        "ORDER BY Users.UserID"
 	Set rsMatchups = GetRecords(oConn, sql)

    If (rsMatchups.EOF) Then
        CalculateTourneyScoresForYear = False
        Exit Function
    End If

    Set rsCourseInfo = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & year & " AND Day='Day1'")

    If (rsCourseInfo.EOF) Then
        CalculateTourneyScoresForYear = False
        Exit Function
    End If

    usesFlights = False
    If (Not rsMatchups.EOF) Then
        If (rsMatchups("Flight") <> "") Then
            usesFlights = True
        End If
    End If

   Dim parForHole
    counter = 0
	Do Until rsMatchups.EOF
        counter = counter + 1

        ReDim aScoresPlayer1(18)
        ReDim aScoresPlayer2(18)
        ReDim aNetScorePlayer1(18)
        ReDim aNetScorePlayer2(18)
  
        genderPlayer1 = rsMatchups("Users.Gender")
        genderPlayer2 = rsMatchups("Users_1.Gender")

        GetUsersScore oConn, year, "Day1", rsMatchups("Users.UserID"), rsCourseInfo, genderPlayer1, aScoresPlayer1, aNetScorePlayer1, "Tournament"
	    GetUsersScore oConn, year, "Day1", rsMatchups("Users_1.UserID"), rsCourseInfo, genderPlayer2, aScoresPlayer2, aNetScorePlayer2, "Tournament"

        For i = 1 to 18
            player1ParPointsForHole = GetParPointsForHole(rsParPointsScoring, aNetScorePlayer1(i), i, rsCourseInfo, genderPlayer1)
            player2ParPointsForHole = GetParPointsForHole(rsParPointsScoring, aNetScorePlayer2(i), i, rsCourseInfo, genderPlayer2)

            if year >= 2007 then
               parForHole = rsCourseInfo("MensPar" & i)
               parPointsForHole = CalculateParPointsForHoleForTeam2007(player1ParPointsForHole, player2ParPointsForHole, parForHole)
            else
               parPointsForHole = CalculateParPointsForHoleForTeam("Day1", player1ParPointsForHole, player2ParPointsForHole)
            end if
            runningCount = runningCount + parPointsForHole
        Next

        names = GetDisplayName(rsMatchups("Users.UserID"), rsMatchups("Users.FirstName") & " " & rsMatchups("Users.LastName")) & " / " & _
            GetDisplayName(rsMatchups("Users_1.UserID"), rsMatchups("Users_1.FirstName") & " " & rsMatchups("Users_1.LastName"))

        aTotalScores(counter, 1) = runningCount
        aTotalScores(counter, 2) = names
        aTotalScores(counter, 3) = runningCount  '' Day 1 score
        aTotalScores(counter, 5) = rsMatchups("Users.UserID") & "/" & rsMatchups("Users_1.UserID")
        aTotalScores(counter, 7) = rsMatchups("Flight")
        aTotalScores(counter, 8) = rsMatchups("StyleForFlight")

        runningCount = 0
        rsMatchups.MoveNext()
    Loop
    Set rsCourseInfo = Nothing
    
    '' Get the Day 2 results
    ''
    rsMatchups.MoveFirst()
    
    Set rsCourseInfo = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & year & " AND Day='Day2'")

    counter = 0
	Do Until rsMatchups.EOF
        counter = counter + 1

        ReDim aScoresPlayer1(18)
        ReDim aScoresPlayer2(18)
        ReDim aNetScorePlayer1(18)
        ReDim aNetScorePlayer2(18)
 
        genderPlayer1 = rsMatchups("Users.Gender")
        genderPlayer2 = rsMatchups("Users_1.Gender")

        GetUsersScore oConn, year, "Day2", rsMatchups("Users.UserID"), rsCourseInfo, genderPlayer1, aScoresPlayer1, aNetScorePlayer1, "Tournament"
	    GetUsersScore oConn, year, "Day2", rsMatchups("Users_1.UserID"), rsCourseInfo, genderPlayer2, aScoresPlayer2, aNetScorePlayer2, "Tournament"

        For i = 1 to 18
            player1ParPointsForHole = GetParPointsForHole(rsParPointsScoring, aNetScorePlayer1(i), i, rsCourseInfo, genderPlayer1)
            player2ParPointsForHole = GetParPointsForHole(rsParPointsScoring, aNetScorePlayer2(i), i, rsCourseInfo, genderPlayer2)

            if year >= 2007 then
               parForHole = rsCourseInfo("MensPar" & i)
               parPointsForHole = CalculateParPointsForHoleForTeam2007(player1ParPointsForHole, player2ParPointsForHole, parForHole)
            else
               parPointsForHole = CalculateParPointsForHoleForTeam("Day2", player1ParPointsForHole, player2ParPointsForHole)
            end if
            
            runningCount = runningCount + parPointsForHole
        Next

        aTotalScores(counter, 1) = aTotalScores(counter, 1) + runningCount
        aTotalScores(counter, 4) = runningCount   '' Day 2
        aTotalScores(counter, 6) = False          '' First place swap
        
        runningCount = 0
        rsMatchups.MoveNext()
    Loop

    '' First, sort things by the first day score, so we can calculate the first day rankings
    ''
    Dim lastScore : lastScore = 0
    Dim rank : rank = 0
    
    ArraySort aTotalScores, counter, 11, 3, False
    
    For i = 0 to counter - 1
        If (lastScore <> aTotalScores(i, 3) ) Then
            rank = i + 1
        End If       

        aTotalScores(i, 10) = rank
        
        lastScore = aTotalScores(i, 3)
    Next
 
    '' Second, sort things by the second day score, so we can calculate the second day rankings
    ''
    lastScore = 0
    rank = 0
 
    ArraySort aTotalScores, counter, 11, 4, False

    For i = 0 to counter - 1
        If (lastScore <> aTotalScores(i, 4) ) Then
            rank = i + 1
        End If       

        aTotalScores(i, 11) = rank
        
        lastScore = aTotalScores(i, 4)
    Next
    
     
    '' Now sort things by the total score
    ''
    ArraySort aTotalScores, counter, 11, 1, False
    
    '' Before we continue, is there more than one #1?  If so, we need to get special data from the DB/
    ''
    Dim teamsTiedForFirstPlace, lastTopScore, playoffDetails
    
    teamsTiedForFirstPlace = 1
    lastTopScore = aTotalScores(0, 1)
    For i = 1 to counter - 1
        If (aTotalScores(i, 1) = lastTopScore And aTotalScores(i, 1) <> 0) Then
            teamsTiedForFirstPlace = teamsTiedForFirstPlace + 1
            lastTopScore = aTotalScores(i, 1) & "<BR>"
        Else
            Exit For
        End If
    Next
    
    If (teamsTiedForFirstPlace > 1) Then
        '' Ok, we have a tie.  This data can't be calculated, it must be retrived from the DB
        ''
        For i = 0 to teamsTiedForFirstPlace
            aTotalScores(i, 6) = True
        Next

        Dim rsFirstPlaceOrder, aOrder
        Set rsFirstPlaceOrder = GetRecords(oConn, "SELECT * FROM Ties WHERE Year=" & year)
        If (Not rsFirstPlaceOrder.EOF) Then
            aOrder = Split(rsFirstPlaceOrder("FirstPlaceOrder"), ",")
            playoffDetails = rsFirstPlaceOrder("Reason")
            
            Dim moreToSwap : moreToSwap = True
            
 	        Do Until Not moreToSwap
 	            moreToSwap = False
                For i = 0 to teamsTiedForFirstPlace
                    For j = 0 to teamsTiedForFirstPlace
                        If (aTotalScores(i, 5) = aOrder(j) And i <> j) Then
                            SwapItems aTotalScores, i, j, 5
                            moreToSwap = True
                            Exit For
                        End If
                    Next
                    
                    If (moreToSwap) Then
                        Exit For
                    End If
                Next
            Loop
        End If
    End If

    '' Calculate the rank for everyone.  Keep ties in mind, chester!
    ''
    lastScore = 0
    rank = 0
    For i = 0 to counter - 1
        If (lastScore <> aTotalScores(i, 1) Or aTotalScores(i, 6)) Then
            rank = i + 1
        End If       

        aTotalScores(i, 9) = rank
        
        lastScore = aTotalScores(i, 1)
    Next

    '' OK, store this information in the application variables
    ''
    Application("CalculatedMonsterTourneyScore" & year) = True
    Application("MonsterTourneyScores" & year) = aTotalScores    
    Application("MonsterTourneyNumTeams" & year) = counter
    Application("MonsterTourneyUsesFlights" & year) = usesFlights
    Application("MonsterTourneyPlayoffDetails" & year) = playoffDetails
    Application.Unlock
    
    CalculateTourneyScoresForYear = True
End Function


Sub CalculatePracticeRoundScoresForYear(year)
    If (Application("CalculatedMonsterPracticeRoundScore" & year) = True) Then
        Exit Sub
    End If
    
    Application.Lock
    
    Dim rsCourseInfo, sql, rsMatchups, aScoresPlayer1, aScoresPlayer2
   	Dim aTotalScores(1000, 4)
  
    Set rsCourseInfo = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & year & " AND Day='Practice'")

    sql = "SELECT Users.* AS [User1], Users_1.* AS [User2], Teams.* " + _
        "FROM (Teams INNER JOIN Users ON Teams.Player1 = Users.UserID) INNER JOIN Users AS Users_1 ON Teams.Player2 = Users_1.UserID " + _
        "WHERE (((Teams.Year)=" & year & ") AND ((Teams.Player1)=[Users].[UserID]) AND ((Teams.Type)='Practice'))"
 	Set rsMatchups = GetRecords(oConn, sql)

    Set rsParPointsScoring = GetRecords(oConn, "SELECT * FROM ParPointsScoring WHERE Year=" & year & " and Type='Practice'")

	counter = 0
	Do Until rsMatchups.EOF
        Dim aNetScorePlayer1, aNetScorePlayer2

        counter = counter + 1
        ReDim aScoresPlayer1(18)
        ReDim aScoresPlayer2(18)
        ReDim aNetScorePlayer1(18)
        ReDim aNetScorePlayer2(18)
  
        genderPlayer1 = rsMatchups("Users.Gender")
        genderPlayer2 = rsMatchups("Users_1.Gender")

        GetUsersScore oConn, year, "Practice", rsMatchups("Users.UserID"), rsCourseInfo, genderPlayer1, aScoresPlayer1, aNetScorePlayer1, "Practice"
	    GetUsersScore oConn, year, "Practice", rsMatchups("Users_1.UserID"), rsCourseInfo, genderPlayer2, aScoresPlayer2, aNetScorePlayer2, "Practice"

        For i = 1 to 18
            player1ParPointsForHole = GetParPointsForHole(rsParPointsScoring, aNetScorePlayer1(i), i, rsCourseInfo, genderPlayer1)
            player2ParPointsForHole = GetParPointsForHole(rsParPointsScoring, aNetScorePlayer2(i), i, rsCourseInfo, genderPlayer2)

            parPointsForHole = CalculateParPointsForHoleForTeam("Practice", player1ParPointsForHole, player2ParPointsForHole)
            
            runningCount = runningCount + parPointsForHole
        Next

        names = GetDisplayName(rsMatchups("Users.UserID"), rsMatchups("Users.FirstName") & " " & rsMatchups("Users.LastName")) & " / " & _
            GetDisplayName(rsMatchups("Users_1.UserID"), rsMatchups("Users_1.FirstName") & " " & rsMatchups("Users_1.LastName"))

        aTotalScores(counter, 1) = runningCount
        aTotalScores(counter, 2) = names
        aTotalScores(counter, 3) = rsMatchups("Users.UserID") & "/" & rsMatchups("Users_1.UserID")
        
        runningCount = 0
        rsMatchups.MoveNext()
    Loop

    ArraySort aTotalScores, counter, 4, 1, False

    '' Calculate the rank for everyone.  Keep ties in mind, chester!
    ''
    Dim lastScore : lastScore = 0
    Dim rank : rank = 0
    For i = 0 to counter - 1
        If (lastScore <> aTotalScores(i, 1)) Then
            rank = i + 1
        End If       

        aTotalScores(i, 4) = rank
        
        lastScore = aTotalScores(i, 1)
    Next
    
    '' OK, store this information in the application variables
    ''
    Application("CalculatedMonsterPracticeRoundScore" & year) = True
    Application("MonsterPracticeRoundScores" & year) = aTotalScores    
    Application("MonsterPracticeRoundNumTeams" & year) = counter
    Application.Unlock
End Sub



Sub DisplayCourseInfo(rsCourseInfo, displayMaleHandicap, displayFemaleHandicap)
    Dim count

    '' Display the men's handicap
    ''
    If (displayMaleHandicap) Then
        Response.Write(" <tr class=xl326913 height=15 style='height:11.25pt'>" & vbLf)
        Response.Write(" <td height=15 class=xl466913 style='height:11.25pt'>Men's Handicap</td>" & vbLf)
        For i = 1 to 18
            If (i = 1 Or i = 10) Then
                Response.Write(" <td class=xl436913>&nbsp;</td>" & vbLf)
            End If
            Response.Write("  <td class=xl326913>" & rsCourseInfo("MensHandicap" & i) & "</td>" & vbLf)
        Next
        Response.Write("  <td class=xl436913>&nbsp;</td>" & vbLf)
        Response.Write("  <td class=xl436913>&nbsp;</td>" & vbLf)
        Response.Write("  <td class=xl436913>&nbsp;</td>" & vbLf)
        Response.Write(" </tr>" & vbLf) 


        '' Display the mens par
        ''
        Response.Write(" <tr height=17 style='mso-height-source:userset;height:12.75pt'>" & vbLf)
        Response.Write("  <td height=17 class=xl246913 style='height:12.75pt'>Men's Par</td>" & vbLf)

        For i = 1 to 18
            If (i = 1 Or i = 10) Then
                Response.Write(" <td class=xl256913>" & count & "</td>" & vbLf)
                count = 0
            End If
            count = count + CInt(rsCourseInfo("MensPar" & i))
            Response.Write("  <td class=xl266913>" & rsCourseInfo("MensPar" & i) & "</td>" & vbLf)
        Next
        Response.Write(" <td class=xl256913>" & count & "</td>" & vbLf)
        Response.Write("  <td class=xl396913>" & rsCourseInfo("MensPar") & "</td>" & vbLf)
        Response.Write("  <td class=xl396913>&nbsp;</td>" & vbLf)
        Response.Write(" </tr>" & vbLf) 
    End If

    If (displayFemaleHandicap) Then
        '' Display the women's handicap
        ''
        Response.Write(" <tr class=xl326913 height=15 style='height:11.25pt'>" & vbLf)
        Response.Write(" <td height=15 class=xl466913 style='height:11.25pt'>Lady's Handicap</td>" & vbLf)
        For i = 1 to 18
            If (i = 1 Or i = 10) Then
                Response.Write(" <td class=xl436913>&nbsp;</td>" & vbLf)
            End If
            Response.Write("  <td class=xl326913>" & rsCourseInfo("WomensHandicap" & i) & "</td>" & vbLf)
        Next
        Response.Write("  <td class=xl436913>&nbsp;</td>" & vbLf)
        Response.Write("  <td class=xl436913>&nbsp;</td>" & vbLf)
        Response.Write("  <td class=xl436913>&nbsp;</td>" & vbLf)
        Response.Write(" </tr>" & vbLf) 


        '' Display the women's par
        ''
        Response.Write(" <tr height=17 style='mso-height-source:userset;height:12.75pt'>" & vbLf)
        Response.Write("  <td height=17 class=xl246913 style='height:12.75pt'>Lady's Par</td>" & vbLf)

        For i = 1 to 18
            If (i = 1 Or i = 10) Then
                Response.Write(" <td class=xl256913>" & count & "</td>" & vbLf)
                count = 0
            End If
            count = count + CInt(rsCourseInfo("WomensPar" & i))
            Response.Write("  <td class=xl266913>" & rsCourseInfo("WomensPar" & i) & "</td>" & vbLf)
        Next
        Response.Write(" <td class=xl256913>" & count & "</td>" & vbLf)
        Response.Write("  <td class=xl396913>" & rsCourseInfo("WomensPar") & "</td>" & vbLf)
        Response.Write("  <td class=xl396913>&nbsp;</td>" & vbLf)
        Response.Write(" </tr>" & vbLf)
    End If
End Sub


Sub MoveToDay(rsCoures, day, year)
    rsCourses.MoveFirst()
    
	Do Until rsCourses.EOF
	    If (StrComp(rsCourses("Day"), day, vbTextCompare) = 0 And _
	        StrComp(rsCourses("Year"), year, vbTextCompare) = 0) Then
	        Exit Do
	    End If
	
	    rsCourses.MoveNext()
	Loop
End Sub


Sub DisplayHeaderRowForRound()
    Dim j
    Response.Write(" <tr class=xl326913 height=15 style='height:11.25pt'>" & vbLf)
    Response.Write("  <td height=15 class=xl446913 width=103 style='height:11.25pt;width:77pt'>Hole</td>" & vbLf)
    Response.Write("  <td class=xl416913 width=35 style='width:26pt'>HC</td>" & vbLf)
 
    For j = 1 to 18
        Response.Write("  <td class=xl406913 width=23 style='width:17pt'>" & j & "</td>" & vbLf)
        If (j = 9) Then
            Response.Write("  <td class=xl406913 width=31 style='width:17pt'>OUT</td>" & vbLf)
        ElseIf (j = 18) Then
            Response.Write("  <td class=xl406913 width=29 style='width:17pt'>IN</td>" & vbLf)
        End If
    Next

    Response.Write("   <td class=xl416913 width=55 style='border-left:none;width:41pt'>Total</td>" & vbLf)
    Response.Write("   <td class=xl416913 width=95>Total Par Points</td>" & vbLf)
    Response.Write(" </tr>" & vbLf)
End Sub

%>
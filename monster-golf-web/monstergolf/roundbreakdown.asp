<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<!-- #include file='includes\funcs.asp' -->

<%	MainHeader "Results" %>
<link rel="STYLESHEET" type="text/css" href="styles.css"></link>

<body>
 <% 
    Dim oConn, rsCourseInfo, year, day, player1ID, player2ID
    Dim genderPlayer1, genderPlayer2, hasMale, hasFemale
    Dim rsMatchups, strType, sql
    Dim aNetScorePlayer1, aNetScorePlayer2, hasData, rsParPointsScoring, aContribs, bothContribution

    ReDim aContribs(2, 3)
    ReDim aNetScorePlayer1(18)
    ReDim aNetScorePlayer2(18)

    Set oConn = GetConnection()
	year = Request("year")
	player1ID = Request("Player1")
	player2ID = Request("Player2")
    genderPlayer1 = GetGenderFromDB(player1ID)
    genderPlayer2 = GetGenderFromDB(player2ID)
 
    hasMale = False
    If (StrComp(genderPlayer1, "M", vbTextCompare) = 0 Or StrComp(genderPlayer2, "M", vbTextCompare) = 0) Then
        hasMale = True
    End If
    hasFemale = False
    If (StrComp(genderPlayer1, "F", vbTextCompare) = 0 Or StrComp(genderPlayer2, "F", vbTextCompare) = 0) Then
        hasFemale = True
    End If
 

	'' Now display all of the results for this day
	''

    sql = "SELECT Users.* AS [User1], Users_1.* AS [User2], Teams.* " + _
        "FROM (Teams INNER JOIN Users ON Teams.Player1 = Users.UserID) INNER JOIN Users AS Users_1 ON Teams.Player2 = Users_1.UserID " + _
        "WHERE (((Teams.Year)=" & year & ") AND ((Teams.Player1)=[Users].[UserID]) AND ((Teams.Type)='" & _
        "Tournament" & "') AND ((Users.UserID)=" & player1ID & ") AND ((Users_1.UserID)=" & player2ID & ")) ORDER BY Teams.ID"

    Set rsMatchups = GetRecords(oConn, sql)
    If (rsMatchups.EOF) Then
        '' Swap player 1 and player 2
        ''
        sql = "SELECT Users.* AS [User1], Users_1.* AS [User2], Teams.* " + _
            "FROM (Teams INNER JOIN Users ON Teams.Player1 = Users.UserID) INNER JOIN Users AS Users_1 ON Teams.Player2 = Users_1.UserID " + _
            "WHERE (((Teams.Year)=" & year & ") AND ((Teams.Player1)=[Users].[UserID]) AND ((Teams.Type)='" & _
            "Tournament" & "') AND ((Users.UserID)=" & player2ID & ") AND ((Users_1.UserID)=" & player1ID & ")) ORDER BY Teams.ID"
        Set rsMatchups = GetRecords(oConn, sql)
    End If

    bothContribution = 0

    For q = 1 to 2
        If (q = 1) Then
            day = "Day1"
        Else
            day = "Day2"
        End If

        Response.Write("<br /><Center><b>Day " & q & "</b></Center><br/>" & vbLf)
        DisplayStartScoringTable()
        DisplayHeaderRowForRound()
 
        '' Display the par and handicaps for men/women
        ''
        Set rsCourseInfo = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & year & " AND Day='" & day & "'")
        DisplayCourseInfo rsCourseInfo, hasMale, hasFemale

        Set rsParPointsScoring = GetRecords(oConn, "SELECT * FROM ParPointsScoring WHERE Year=" & year & " and Type='Tournament'")

        Dim cell1Class, cellNetScore, cell2Class, restOfCells, cellParPoints, cellRestOfParPoints
        Dim player1Name, player2Name

        cell1Class   = "xl316913"
        cellNetScore = "xl366913"
        cell2Class   = "xl296913"
        restOfCells  = "xl306913"

        cellParPoints = "xl386913"
        cellPar2Class = "xl376913"
        cellRestOfParPoints = ""   '"xl386913"

        cellBigParPoints = "xl8411728"
    
        genderPlayer1 = rsMatchups("Users.Gender")
        genderPlayer2 = rsMatchups("Users_1.Gender")

        player1Name = rsMatchups("Users.FirstName") + " " + rsMatchups("Users.LastName")
        player2Name = rsMatchups("Users_1.FirstName") + " " + rsMatchups("Users_1.LastName")
        DisplayUsersScore oConn, year, day, rsMatchups("Users.UserID"), player1Name, _
            rsCourseInfo, genderPlayer1, aNetScorePlayer1, True, _
            cell1Class, cellNetScore, cell2Class, restOfCells, cellParPoints, cellBigParPoints
        DisplayUsersScore oConn, year, day, rsMatchups("Users_1.UserID"), player2Name, _
            rsCourseInfo, genderPlayer2, aNetScorePlayer2, False, _
            cell1Class, cellNetScore, cell2Class, restOfCells, cellParPoints, cellBigParPoints
    	
    	
	    '' Display the par points for the team
	    ''
        Response.Write(" <tr height=15 style='height:11.25pt'>" & vbLf)
        Response.Write("  <td height=15 class=" + cellNetScore + " style='height:11.25pt'>Team par points</td>" & vbLf)
        Response.Write("  <td class=" & cellPar2Class & ">&nbsp;</td>" & vbLf)

        Dim runningCount, nineHoles, parPointsForHole, player1ParPointsForHole, player2ParPointsForHole
        Dim player1Contribution, player2Contribution, runningPlayer1Contrib, runningPlayer2Contrib
        Dim day1HamEgg, day2HamEgg, parForHole
        runningCount = 0
        player1Contribution = 0
        player2Contribution = 0
        runningPlayer1Contrib = 0
        runningPlayer2Contrib = 0
        
        For i = 1 to 18
            player1ParPointsForHole = GetParPointsForHole(rsParPointsScoring, aNetScorePlayer1(i), i, rsCourseInfo, genderPlayer1)
            player2ParPointsForHole = GetParPointsForHole(rsParPointsScoring, aNetScorePlayer2(i), i, rsCourseInfo, genderPlayer2)

            If (StrComp(genderPlayer1, "M", vbTextCompare) = 0) Then
                parForHole = rsCourseInfo("MensPar" & i)
            Else
                parForHole = rsCourseInfo("WomensPar" & i)
            End If

            if year >= 2007 then
                parPointsForHole = CalculateParPointsForHoleForTeam2007(player1ParPointsForHole, player2ParPointsForHole, parForHole)
            else
                parPointsForHole = CalculateParPointsForHoleForTeam(day, player1ParPointsForHole, player2ParPointsForHole)
            end if
            
            runningCount = runningCount + parPointsForHole
            nineHoles = nineHoles + parPointsForHole
            Response.Write("  <td class=" & cellParPoints & ">" & parPointsForHole & "</td>" & vbLf)
            
            If (i = 9) Then
                Response.Write("  <td class=" & cellPar2Class & ">" & nineHoles & "</td>" & vbLf)
                nineHoles = 0
            End If
            
            If (i = 18) Then
                Response.Write("  <td class=" & cellPar2Class & ">" & nineHoles & "</td>" & vbLf)
            End If
            
            '' Contribution
            ''
            Response.Write("<script language=javascript>" & vbLf)
            if (year >= 2007 and parForHole = 4) Then
                If (player1ParPointsForHole = player2ParPointsForHole) Then
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users.UserID") & _
                        "_" & day & "_contribution').innerHTML = '<b>" & player1ParPointsForHole & "</b>';" & vbLf)
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users_1.UserID") & _
                        "_" & day & "_contribution').innerHTML = '<b>" & player2ParPointsForHole & "</b>';" & vbLf)
                    bothContribution = bothContribution + player1ParPointsForHole
                ElseIf (player1ParPointsForHole > player2ParPointsForHole) Then
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users.UserID") & _
                        "_" & day & "_contribution').innerText = " & player1ParPointsForHole & ";" & vbLf)
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users_1.UserID") & _
                        "_" & day & "_contribution').innerHTML = '<i>" & player2ParPointsForHole & "</i>';" & vbLf)
                    player1Contribution = player1Contribution + player1ParPointsForHole

                Else
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users.UserID") & _
                        "_" & day & "_contribution').innerHTML = '<i>" & player1ParPointsForHole & "</i>';" & vbLf)
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users_1.UserID") & _
                        "_" & day & "_contribution').innerText = " & player2ParPointsForHole & ";" & vbLf)
                    player2Contribution = player2Contribution + player2ParPointsForHole
                End If
            ElseIf (year >= 2007) Then
                Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users.UserID") & _
                    "_" & day & "_contribution').innerText = " & player1ParPointsForHole & ";" & vbLf)
                player1Contribution = player1Contribution + player1ParPointsForHole

                Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users_1.UserID") & _
                    "_" & day & "_contribution').innerText = " & player2ParPointsForHole & ";" & vbLf)
                player2Contribution = player2Contribution + player2ParPointsForHole
            ElseIf (StrComp(day, "Day1", vbTextCompare) = 0 ) Then
                If (player1ParPointsForHole = player2ParPointsForHole) Then
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users.UserID") & _
                        "_" & day & "_contribution').innerHTML = '<b>" & player1ParPointsForHole & "</b>';" & vbLf)
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users_1.UserID") & _
                        "_" & day & "_contribution').innerHTML = '<b>" & player2ParPointsForHole & "</b>';" & vbLf)
                    bothContribution = bothContribution + player1ParPointsForHole
                ElseIf (player1ParPointsForHole > player2ParPointsForHole) Then
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users.UserID") & _
                        "_" & day & "_contribution').innerText = " & player1ParPointsForHole & ";" & vbLf)
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users_1.UserID") & _
                        "_" & day & "_contribution').innerHTML = '<i>" & player2ParPointsForHole & "</i>';" & vbLf)
                    player1Contribution = player1Contribution + player1ParPointsForHole

                Else
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users.UserID") & _
                        "_" & day & "_contribution').innerHTML = '<i>" & player1ParPointsForHole & "</i>';" & vbLf)
                    Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users_1.UserID") & _
                        "_" & day & "_contribution').innerText = " & player2ParPointsForHole & ";" & vbLf)
                    player2Contribution = player2Contribution + player2ParPointsForHole
                End If
            Else
                Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users.UserID") & _
                    "_" & day & "_contribution').innerText = " & player1ParPointsForHole & ";" & vbLf)
                player1Contribution = player1Contribution + player1ParPointsForHole

                Response.Write("document.getElementById('hole" & i & "_" & rsMatchups("Users_1.UserID") & _
                    "_" & day & "_contribution').innerText = " & player2ParPointsForHole & ";" & vbLf)
                player2Contribution = player2Contribution + player2ParPointsForHole
            End If
            
            '' Is there some ham and egging going on?
            ''
            If ((player1ParPointsForHole - player2ParPointsForHole => 2 And player1ParPointsForHole > 0) Or _
                (player2ParPointsForHole - player1ParPointsForHole => 2 And player2ParPointsForHole > 0)) Then
                If (StrComp(day, "Day1", vbTextCompare) = 0 ) Then
                    day1HamEgg = day1HamEgg + 1
                Else
                    day2HamEgg = day2HamEgg + 1
                End If
            End If
            
            
            If (i = 9) Then
                Response.Write("document.getElementById('Out_" & rsMatchups("Users.UserID") & _
                    "_" & day & "_contribution').innerText = " & player1Contribution & ";" & vbLf)
                Response.Write("document.getElementById('Out_" & rsMatchups("Users_1.UserID") & _
                    "_" & day & "_contribution').innerText = " & player2Contribution & ";" & vbLf)
                runningPlayer1Contrib = player1Contribution
                runningPlayer2Contrib = player2Contribution
                
                player1Contribution = 0
                player2Contribution = 0
            End If
            If (i = 18) Then
                Response.Write("document.getElementById('In_" & rsMatchups("Users.UserID") & _
                    "_" & day & "_contribution').innerText = " & player1Contribution & ";" & vbLf)
                Response.Write("document.getElementById('In_" & rsMatchups("Users_1.UserID") & _
                    "_" & day & "_contribution').innerText = " & player2Contribution & ";" & vbLf)
                    
                runningPlayer1Contrib = runningPlayer1Contrib + player1Contribution
                runningPlayer2Contrib = runningPlayer2Contrib + player2Contribution
                
                Response.Write("document.getElementById('Total_" & rsMatchups("Users.UserID") & _
                    "_" & day & "_contribution').innerText = " & runningPlayer1Contrib & ";" & vbLf)
                Response.Write("document.getElementById('Total_" & rsMatchups("Users_1.UserID") & _
                    "_" & day & "_contribution').innerText = " & runningPlayer2Contrib & ";" & vbLf)
                    
                aContribs(q, 1) = CLng(runningPlayer1Contrib)
                aContribs(q, 2) = CLng(runningPlayer2Contrib)
            End If
            Response.Write("</script>" & vbLf)
        Next
        
        '' Total
        ''
        Response.Write("  <td class=" & cellPar2Class & ">" & runningCount & "</td>" & vbLf)
                
        Response.Write(" </tr>" & vbLf)
        Response.Write("</table>" & vbLf)
        
        '' Update the big square
        ''
        Response.Write("<script language=javascript>document.getElementById('field" & "_" & day & + "_" & _
            rsMatchups("Users.UserID") & "').innerHTML = '" & runningCount & "';</script>")
        
        
        If (q = 1) Then
            Response.Write("<BR><BR><BR>")
        End If
    Next
    
	Response.Write("<BR><BR>" & vbLf)

    '' Show the breakdown of the round
    ''
    Dim totalParPointsDayOne, totalParPointsDayTwo, parPointsDay1, parPointsDay2, pctDay1, pctDay2, avePct, pctBoth, bothAdd
    
    '' Deal with the situation if they both had contributed on teh first day
    If (bothContribution <> 0) Then
        bothAdd = bothContribution / 2
    End If
    
    totalParPointsDayOne = aContribs(1, 1) + aContribs(1, 2) + bothContribution
    totalParPointsDayTwo = aContribs(2, 1) + aContribs(2, 2)
  
	Response.Write("<table align=center border=0 class=tableheadnowidth>" & vbLf)
	Response.Write("  <tr>" & vbLf)
	Response.Write("    <td class=header colspan=4>Contribution breakdown - par points</td>"& vbLf)
  	Response.Write("  </tr>" & vbLf)
	Response.Write("  <tr>" & vbLf)
	Response.Write("    <td><b>Name</b></td>"& vbLf)
	Response.Write("    <td><b>Day 1</b></td>"& vbLf)
	Response.Write("    <td><b>Day 2</b></td>"& vbLf)
	Response.Write("    <td><b>Average</b></td>"& vbLf)
  	Response.Write("  </tr>" & vbLf)
	Response.Write("  <tr>" & vbLf)
	
	parPointsDay1 = aContribs(1, 1) + bothAdd
	parPointsDay2 = aContribs(2, 1)
	pctDay1 = (parPointsDay1 * 100) / totalParPointsDayOne
	pctDay2 = (parPointsDay2 * 100) / totalParPointsDayTwo
    avePct = ((pctDay1 + pctDay2) / 2)
    		
	Response.Write("    <td>" & player1Name & "</td>"& vbLf)
	Response.Write("    <td align=center>" & parPointsDay1 & " (" & Round(pctDay1, 2) & "%)</td>"& vbLf)
	Response.Write("    <td align=center>" & parPointsDay2 & " (" & Round(pctDay2, 2) & "%)</td>"& vbLf)
 	Response.Write("    <td align=center>" & Round(avePct, 2) & "</td>"& vbLf)
 	Response.Write("  </tr>" & vbLf)

	parPointsDay1 = aContribs(1, 2) + bothAdd
	parPointsDay2 = aContribs(2, 2)
	pctDay1 = (parPointsDay1 * 100) / totalParPointsDayOne
	pctDay2 = (parPointsDay2 * 100) / totalParPointsDayTwo
    avePct = ((pctDay1 + pctDay2) / 2)

    pctBoth = (bothContribution * 100) / totalParPointsDayOne

	Response.Write("  <tr>" & vbLf)
	Response.Write("    <td>Both (Day1 only)</td>"& vbLf)
	Response.Write("    <td colspan=3><i>" & bothContribution & " (" & Round(pctBoth, 2) & "%) -- split up evenly</i></td>"& vbLf)
 	Response.Write("  </tr>" & vbLf)

 	Response.Write("  <tr>" & vbLf)
	Response.Write("    <td>" & player2Name & "</td>"& vbLf)
	Response.Write("    <td align=center>" & parPointsDay1 & " (" & Round(pctDay1, 2) & "%)</td>"& vbLf)
	Response.Write("    <td align=center>" & parPointsDay2 & " (" & Round(pctDay2, 2) & "%)</td>"& vbLf)
 	Response.Write("    <td align=center>" & Round(avePct, 2) & "</td>"& vbLf)
  	Response.Write("  </tr>" & vbLf)
  	Response.Write("  <tr>" & vbLf)
	Response.Write("    <td align=center>Total</td>"& vbLf)
	Response.Write("    <td align=center>" & totalParPointsDayOne & "</td>"& vbLf)
	Response.Write("    <td align=center>" & totalParPointsDayTwo & "</td>"& vbLf)
	Response.Write("    <td align=center>" & totalParPointsDayOne + totalParPointsDayTwo & "</td>"& vbLf)
  	Response.Write("  </tr>" & vbLf)
  	
  	Response.Write("  <tr>" & vbLf)
	Response.Write("    <td colspan=4>&nbsp;</td>"& vbLf)
  	Response.Write("  </tr>" & vbLf)
  	
  	Response.Write("  <tr>" & vbLf)
	Response.Write("    <td align=center>Ham/Egg Factor *</td>"& vbLf)
	Response.Write("    <td align=center>" & Round((day1HamEgg / 18) * 100, 2) & "%</td>"& vbLf)
	Response.Write("    <td align=center>" & Round((day2HamEgg / 18) * 100, 2) & "%</td>"& vbLf)
	Response.Write("    <td align=center>&nbsp;</td>"& vbLf)
  	Response.Write("  </tr>" & vbLf)
  	
  	
	Response.Write("</table>" & vbLf)

    Set rsCourseInfo = Nothing
	Set rsMatchups = Nothing
	oConn.Close

    Response.Write("<BR><center>* Ham/Egg factor is derived from comparing the contribution of players, and on each<BR>")
    Response.Write("hole figuring out if any one player contributed 2 par points more than the other.</center>")

''
''
'' Functions used by the above
''
''

Function DisplayUsersScore(oConn, year, day, userID, fullName, rsCourseInfo, genderPlayer, aParPointsPlayer, _
                           showTotalScore, cell1Class, cellNetScore, cell2Class, restOfCells, restOfCellsParPoints, cellBigParPoints)
    Dim rsScores, handicap, sql, strType
    
    If (StrComp(day, "Day1", vbTextCompare) = 0 Or StrComp(day, "Day2", vbTextCompare) = 0) Then
        strType = "Tournament"
    Else
        strType = "Practice"
    End If
    
    sql = "SELECT * " + _
        "FROM TourneyScores INNER JOIN Teams ON (TourneyScores.UserId = Teams.Player2) OR (TourneyScores.UserId = Teams.Player1) " + _
        "WHERE (((TourneyScores.UserId)=" & userID & ") AND ((TourneyScores.Year)=" & year & ") " + _
        "  AND ((TourneyScores.Round)='" & day & "') AND ((Teams.Type)='" & strType & "'))"
    
    Set rsScores = GetRecords(oConn, sql)
    
    '' If there is no data, exit
    ''
    If (rsScores.EOF) Then
        DisplayUsersScore = False
        Exit Function
    End If
       
    handicap = rsScores("Handicap")
    
    If (Not rsScores.EOF) Then
	    Response.Write(" <tr>" & vbLf)
        Response.Write("  <td height=15 class=" + cell1Class + " style='height:11.25pt'>" & GetDisplayName(userID, fullName) & "</td>" & vbLf)
	    Response.Write("  <td class=" + cellPar2Class + ">" & handicap & "</td>" & vbLf)
	    
	    Dim countFor9, runningTotal, aScores, handicapOfHole, i, j, pointsToSubtractFromGrossScore, tempHandicap
	    
	    ReDim aScores(18)
    	
	    '' Display the first line of information, which are all fo the scores totalled up
	    ''
	    For i = 1 to 18
	        aScores(i) = rsScores("Hole" & i & "_Score")
 	        countFor9 = countFor9 + aScores(i)
 	        runningTotal = runningTotal + aScores(i)
	        Response.Write("  <td class=" & restOfCells & ">" & aScores(i) & "</td>" & vbLf)
	        
	        If (i = 9 Or i = 18) Then
 	            Response.Write("  <td class=" & cellPar2Class & ">" & countFor9 & "</td>" & vbLf)
 	            countFor9 = 0
 	        End If
	    Next

  	    Response.Write("  <td class=" & cellPar2Class & " style='border-left:none'>" & runningTotal & "</td>" & vbLf)

        If (showTotalScore) Then
            '' TODO the rowspan for the scoring page is 5
            Response.Write("  <td id=field_" & day & "_" & userID & " class=" & cellBigParPoints & _
                " rowspan=7 style='border-bottom:.5pt solid black'></td>" & vbLf)
        End If
        
	    Response.Write(" </tr>" & vbLf)

	    '' Display the net score, using the handicap
	    ''
        Response.Write(" <tr height=15 style='height:11.25pt'>" & vbLf)
        Response.Write("  <td height=15 class=" + cellNetScore + " style='height:11.25pt'>net score</td>" & vbLf)
        Response.Write("  <td class=" & cellPar2Class & ">&nbsp;</td>" & vbLf)

        countFor9 = 0
        runningTotal = 0
        For i = 1 to 18
            '' From the handicap, derive the net score
            ''
            If (StrComp(genderPlayer, "M", vbTextCompare) = 0) Then
                handicapOfHole = rsCourseInfo("MensHandicap" & i)       
            Else
                handicapOfHole = rsCourseInfo("WomensHandicap" & i)       
            End If

            pointsToSubtractFromGrossScore = 0
            tempHandicap = handicap
            While (tempHandicap >= handicapOfHole)
                pointsToSubtractFromGrossScore = pointsToSubtractFromGrossScore + 1
                tempHandicap = tempHandicap - 18
            Wend
            
            aParPointsPlayer(i) = aScores(i) - pointsToSubtractFromGrossScore
            countFor9 = countFor9 + aParPointsPlayer(i)
            runningTotal = runningTotal + aParPointsPlayer(i)
            Response.Write("  <td class=" & restOfCellsParPoints & ">" & aParPointsPlayer(i) & "</td>" & vbLf)
            
	        If (i = 9 Or i = 18) Then
 	            Response.Write("  <td class=" & cellPar2Class & ">" & countFor9 & "</td>" & vbLf)
 	            countFor9 = 0
 	        End If
        Next
        Response.Write("  <td class=" & cellPar2Class & " style='border-left:none'>" & runningTotal & "</td>" & vbLf)
        Response.Write("</tr>" & vbLf)
        
        
        '' Contribution
        ''
        Response.Write(" <tr height=15 style='height:11.25pt'>" & vbLf)
        Response.Write("  <td height=15 class=" + cellNetScore + " style='height:11.25pt'>Contribution</td>" & vbLf)
        Response.Write("  <td class=" & cellPar2Class & ">&nbsp;</td>" & vbLf)
        For i = 1 to 18
            Response.Write("  <td class=" & restOfCellsParPoints & " id=""hole" & i & "_" & userId & "_" & day & "_contribution""></td>" & vbLf)
	        If (i = 9) Then
 	            Response.Write("  <td class=" & cellPar2Class & " id=""Out_" & userId & "_" & day & "_contribution""></td>" & vbLf)
 	        End If
	        If (i = 18) Then
 	            Response.Write("  <td class=" & cellPar2Class & " id=""In_" & userId & "_" & day & "_contribution""></td>" & vbLf)
 	        End If
        Next
        Response.Write("  <td class=" & cellPar2Class & " style='border-left:none' id=""Total_" & userId & "_" & day & "_contribution""></td>" & vbLf)
       
        Response.Write("</tr>" & vbLf)        
    End If
    Set scores = Nothing

    DisplayUsersScore = True
End Function


Sub DisplayStartScoringTable()
%>
    <table align=center border=0 cellpadding=0 cellspacing=0 width=650 class=xl276913
     style='border-collapse:collapse;table-layout:fixed;width:495pt'>
     <col class=xl276913 width=103 style='mso-width-source:userset;mso-width-alt:
     3766;width:77pt'>
     <col class=xl276913 width=35 style='mso-width-source:userset;mso-width-alt:
     1280;width:26pt'>
     <col class=xl276913 width=23 style='mso-width-source:userset;mso-width-alt:
     841;width:17pt'>
     <col class=xl276913 width=22 span=8 style='mso-width-source:userset;
     mso-width-alt:804;width:17pt'>
     <col class=xl276913 width=31 style='mso-width-source:userset;mso-width-alt:
     1133;width:23pt'>
     <col class=xl276913 width=22 span=9 style='mso-width-source:userset;
     mso-width-alt:804;width:17pt'>
     <col class=xl276913 width=29 style='mso-width-source:userset;mso-width-alt:
     1060;width:22pt'>
     <col class=xl276913 width=55 style='mso-width-source:userset;mso-width-alt:
     2011;width:41pt'>
<%
End Sub

%>
</body>
</html>

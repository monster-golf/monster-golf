<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<!-- #include file='includes\funcs.asp' -->

<style>
table
	{mso-displayed-decimal-separator:"\.";
	mso-displayed-thousand-separator:"\,";}
</style>
<link rel="STYLESHEET" type="text/css" href="styles.css"></link>

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
 <tr class=xl326913 height=15 style='height:11.25pt'>
  <td height=15 class=xl446913 width=103 style='height:11.25pt;width:77pt'>Hole</td>
  <td class=xl416913 width=35 style='width:26pt'>HC</td>
  <td class=xl406913 width=23 style='width:17pt'>1</td>
  <td class=xl406913 width=22 style='width:17pt'>2</td>
  <td class=xl406913 width=22 style='width:17pt'>3</td>
  <td class=xl406913 width=22 style='width:17pt'>4</td>
  <td class=xl406913 width=22 style='width:17pt'>5</td>
  <td class=xl406913 width=22 style='width:17pt'>6</td>
  <td class=xl406913 width=22 style='width:17pt'>7</td>
  <td class=xl406913 width=22 style='width:17pt'>8</td>
  <td class=xl406913 width=22 style='width:17pt'>9</td>
  <td class=xl416913 width=31 style='width:23pt'>OUT</td>
  <td class=xl406913 width=22 style='width:17pt'>10</td>
  <td class=xl406913 width=22 style='width:17pt'>11</td>
  <td class=xl406913 width=22 style='width:17pt'>12</td>
  <td class=xl406913 width=22 style='width:17pt'>13</td>
  <td class=xl406913 width=22 style='width:17pt'>14</td>
  <td class=xl406913 width=22 style='width:17pt'>15</td>
  <td class=xl406913 width=22 style='width:17pt'>16</td>
  <td class=xl406913 width=22 style='width:17pt'>17</td>
  <td class=xl406913 width=22 style='width:17pt'>18</td>
  <td class=xl416913 width=29 style='width:22pt'>IN</td>
  <td class=xl416913 width=55 style='border-left:none;width:41pt'>Total</td>
  <td class=xl416913 width=95>Total Par Points</td>
 </tr>
 <% Dim oConn, rsCourseInfo, year, day

    Set oConn = GetConnection()
	year = Request("year")
	day = Request("HeaderSelect")

    Set rsCourseInfo = GetRecords(oConn, "SELECT * FROM Courses WHERE Year=" & year & " AND Day='" & day & "'")

    '' Display the par and handicaps for men/women
    ''
    DisplayCourseInfo rsCourseInfo, True, True

	'' Now display all of the results for this day
	''
	Dim rsMatchups, counter, strType, sql
    Dim aNetScorePlayer1, aNetScorePlayer2, hasData, rsParPointsScoring
    Dim player1Name, player2Name
    
    ReDim aNetScorePlayer1(18)
    ReDim aNetScorePlayer2(18)

    If (StrComp(Request("HeaderSelect"), "Day1", vbTextCompare) = 0 Or _
        StrComp(Request("HeaderSelect"), "Day2", vbTextCompare) = 0) Then
       strType = "Tournament" 
    Else
       strType = "Practice" 
    End If

    sql = "SELECT Users.* AS [User1], Users_1.* AS [User2], Teams.* " + _
        "FROM (Teams INNER JOIN Users ON Teams.Player1 = Users.UserID) INNER JOIN Users AS Users_1 ON Teams.Player2 = Users_1.UserID " + _
        "WHERE (((Teams.Year)=" & year & ") AND ((Teams.Player1)=[Users].[UserID]) AND ((Teams.Type)='" & _
        strType & "')) ORDER BY Teams.ID"

	Set rsMatchups = GetRecords(oConn, sql)
	
    Set rsParPointsScoring = GetRecords(oConn, "SELECT * FROM ParPointsScoring WHERE Year=" & year & " and Type='" & strType & "'")
	
	counter = 1
	Do Until rsMatchups.EOF
	    'Response.Write(rsMatchups("Player1") & " vs " & rsMatchups("Player2") & "<br>")
        Dim cell1Class, cellNetScore, cell2Class, restOfCells, cellParPoints, cellRestOfParPoints, parForHole
        
        If (counter Mod 2) Then
            cell1Class   = "xl286913"
            cellNetScore = "xl336913"
            cell2Class   = "xl296913"
            restOfCells  = "xl306913"
            
            cellParPoints = "xl356913"
            cellPar2Class = "xl346913"
            cellRestOfParPoints = "xl346913"
            
            cellBigParPoints = "xl6415123"
        Else
            cell1Class   = "xl316913"
            cellNetScore = "xl366913"
            cell2Class   = "xl296913"
            restOfCells  = "xl306913"
            
            cellParPoints = "xl386913"
            cellPar2Class = "xl376913"
            cellRestOfParPoints = ""   '"xl386913"
            
            cellBigParPoints = "xl8411728"
        End If
        
        genderPlayer1 = rsMatchups("Users.Gender")
        genderPlayer2 = rsMatchups("Users_1.Gender")
        player1Name = rsMatchups("Users.FirstName") + " " + rsMatchups("Users.LastName")
        player2Name = rsMatchups("Users_1.FirstName") + " " + rsMatchups("Users_1.LastName")
       
        hasData = DisplayUsersScore(oConn, year, day, rsMatchups("Users.UserID"), player1Name, _
            rsCourseInfo, genderPlayer1, aNetScorePlayer1, True, _
            cell1Class, cellNetScore, cell2Class, restOfCells, cellParPoints, cellBigParPoints)
	    DisplayUsersScore oConn, year, day, rsMatchups("Users_1.UserID"), player2Name, _
	        rsCourseInfo, genderPlayer2, aNetScorePlayer2, False, _
	        cell1Class, cellNetScore, cell2Class, restOfCells, cellParPoints, cellBigParPoints
		
		'' Display the par points for these guys user
		''
		If (hasData) Then
		    Response.Write(" <tr height=15 style='height:11.25pt'>" & vbLf)
            Response.Write("  <td height=15 class=" + cellNetScore + " style='height:11.25pt'>team par points</td>" & vbLf)
            Response.Write("  <td class=" & cellPar2Class & ">&nbsp;</td>" & vbLf)

            Dim runningCount, nineHoles, parPointsForHole, player1ParPointsForHole, player2ParPointsForHole
            runningCount = 0
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
 	            
 	            If (i Mod 9 = 0) Then
 	                Response.Write("  <td class=" & cellPar2Class & ">" & nineHoles & "</td>" & vbLf)
 	                nineHoles = 0
 	            End If
            Next
            
            '' Total
            ''
 	        Response.Write("  <td class=" & cellPar2Class & ">" & runningCount & "</td>" & vbLf)
            
            Dim strTotalLink
            If (StrComp(day, "Practice", vbTextCompare) <> 0) Then
                strTotalLink = "<a href=""roundbreakdown.asp?year=" & year & "&Player1=" & rsMatchups("Player1") & _
                    "&Player2=" & rsMatchups("Player2") & """>" & runningCount & "</a>"
            Else
                strTotalLink = runningCount
            End If
             
            Response.Write("<script language=javascript>document.getElementById('field" & _
                rsMatchups("Users.UserID") & "').innerHTML = '" & strTotalLink & "';</script>")
            
		    Response.Write(" </tr>" & vbLf)
		End If 
		
		rsMatchups.MoveNext
		counter = counter + 1
	Loop
	
	Set rsCourseInfo = Nothing
	Set rsMatchups = Nothing
	oConn.Close
	

''
''
'' Functions used by the above
''
''



Function DisplayUsersScore(oConn, year, round, userID, fullName, rsCourseInfo, genderPlayer, aParPointsPlayer, _
                           showTotalScore, cell1Class, cellNetScore, cell2Class, restOfCells, restOfCellsParPoints, cellBigParPoints)
    Dim rsScores, handicap, sql, strType
    
    If (StrComp(round, "Day1", vbTextCompare) = 0 Or StrComp(round, "Day2", vbTextCompare) = 0) Then
        strType = "Tournament"
    Else
        strType = "Practice"
    End If
    
    sql = "SELECT * " + _
        "FROM TourneyScores INNER JOIN Teams ON (TourneyScores.UserId = Teams.Player2) OR (TourneyScores.UserId = Teams.Player1) " + _
        "WHERE (((TourneyScores.UserId)=" & userID & ") AND ((TourneyScores.Year)=" & year & ") " + _
        "  AND ((TourneyScores.Round)='" & round & "') AND ((Teams.Type)='" & strType & "'))"
    
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
            Response.Write("  <td id=field" & userID & " class=" & cellBigParPoints & _
                " rowspan=5 style='border-bottom:.5pt solid black'>52</td>" & vbLf)
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
    End If
    Set scores = Nothing

    DisplayUsersScore = True
End Function

%>
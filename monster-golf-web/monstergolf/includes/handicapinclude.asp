<%
''Globals
Dim g_lNumScores, g_aAllVals
'' Constants
Dim cScore, cIndex, cSlope, cSDate, cHD, cHDUse, cEDate, cTourn, cCName, cScrID, cEntBy
cScore = 0
cIndex = 1
cSlope = 2
cSDate = 3
cHD    = 4
cHDUse = 5
cEDate = 6
cTourn = 7
cCName = 8
cScrID = 9
cEntBy = 10

Function ValsToFile(aVals, cConst, lNum)
	Dim i
	
	ValsToFile = ""
	
	For i = 0 To lNum - 1
		ValsToFile = ValsToFile & SetVal(ValsToFile, "", aVals(i, cConst), "][" & aVals(i, cConst))
	Next
End Function

Sub MoveValsOne(aVals, lNum)
	Dim i
	
	If lNum > 1 Then
		For i = lNum - 1 To 1 Step -1
			g_aAllVals(i, cScore) = aVals(i-1, cScore)
			g_aAllVals(i, cIndex) = aVals(i-1, cIndex)
			g_aAllVals(i, cSlope) = aVals(i-1, cSlope)
			g_aAllVals(i, cSDate) = aVals(i-1, cSDate)
			g_aAllVals(i, cHD)    = aVals(i-1, cHD)
			g_aAllVals(i, cHDUse) = aVals(i-1, cHDUse)
			g_aAllVals(i, cEDate) = aVals(i-1, cEDate)
			g_aAllVals(i, cTourn) = aVals(i-1, cTourn)
			g_aAllVals(i, cCName) = aVals(i-1, cCName)
			g_aAllVals(i, cScrID) = aVals(i-1, cScrID)
			g_aAllVals(i, cEntBy) = aVals(i-1, cEntBy)
		Next
	End If
End Sub

Sub SortByDate()
	Dim i, j, lHighDate, lMark
	Dim aTempAll()
	ReDim aTempAll(UBound(g_aAllVals, 1), UBound(g_aAllVals, 2))
	
	lHighDate = Date + 1
	
	For j = 0 To g_lNumScores - 1
		For i = 0 To g_lNumScores - 1
			If CDate(lHighDate) > CDate(g_aAllVals(i, cSDate)) And g_aAllVals(i, cHDUse) = False Then
				lHighDate = g_aAllVals(i, cSDate)
				lMark = i
			End If
		Next
		
		g_aAllVals(lMark, cHDUse) = True
		aTempAll(j, cScore) = g_aAllVals(lMark, cScore)
		aTempAll(j, cIndex) = g_aAllVals(lMark, cIndex)
		aTempAll(j, cSlope) = g_aAllVals(lMark, cSlope)
		aTempAll(j, cSDate) = g_aAllVals(lMark, cSDate)
		aTempAll(j, cHD)    = g_aAllVals(lMark, cHD)
		aTempAll(j, cHDUse) = g_aAllVals(lMark, cHDUse)
		aTempAll(j, cEDate) = g_aAllVals(lMark, cEDate)
		aTempAll(j, cTourn) = g_aAllVals(lMark, cTourn)
		aTempAll(j, cCName) = g_aAllVals(lMark, cCName)
		aTempAll(j, cScrID) = g_aAllVals(lMark, cScrID)
		aTempAll(j, cEntBy) = g_aAllVals(lMark, cEntBy)
		lHighDate = Date + 1
	Next
	
	j = 0
	
	For i = g_lNumScores - 1 To 0 Step - 1
		g_aAllVals(j, cScore) = aTempAll(i, cScore)
		g_aAllVals(j, cIndex) = aTempAll(i, cIndex)
		g_aAllVals(j, cSlope) = aTempAll(i, cSlope)
		g_aAllVals(j, cSDate) = aTempAll(i, cSDate)
		g_aAllVals(j, cHD)    = aTempAll(i, cHD)
		g_aAllVals(j, cHDUse) = False
		g_aAllVals(j, cEDate) = aTempAll(i, cEDate)
		g_aAllVals(j, cTourn) = aTempAll(i, cTourn)
		g_aAllVals(j, cCName) = aTempAll(i, cCName)
		g_aAllVals(j, cScrID) = aTempAll(i, cScrID)
		g_aAllVals(j, cEntBy) = aTempAll(i, cEntBy)
		j = j + 1
	Next
End Sub

Function GetReduction(lCurrHandi, lHD1, lHD2, lCount)
	If lHD1 < lHD2 Then
		If (lCurrHandi - lHD2) >= 3.0 Then
			GetReduction = LookUpReduction((lCurrHandi -	((lHD1 + lHD2)/2)), lCount)
		Else
			GetReduction = 0
		End If
	Else
		If (lCurrHandi - lHD1) >= 3.0 Then
			GetReduction = LookUpReduction((lCurrHandi -	((lHD1 + lHD2)/2)), lCount)
		Else
			GetReduction = 0
		End If
	End If
End Function

Function LookUpReduction(lAverage, lRounds)
	LookUpReduction = 0
	
	If lAverage >= 4.0 And lAverage <= 4.4 Then
		If lRounds = 2 Then
			LookUpReduction = 1
		End If
	ElseIf lAverage >= 4.5 And lAverage <= 4.9 Then
		If lRounds = 2 Then
			LookUpReduction = 1.8
		ElseIf lRounds = 3 Then
			LookUpReduction = 1
		End If
	ElseIf lAverage >= 5.0 And lAverage <= 5.4 Then
		If lRounds = 2 Then
			LookUpReduction = 2.6
		ElseIf lRounds = 3 Then
			LookUpReduction = 1.9
		ElseIf lRounds = 4 Then
			LookUpReduction = 1
		End If
	ElseIf lAverage >= 5.5 And lAverage <= 5.9 Then
		If lRounds = 2 Then
			LookUpReduction = 3.4
		ElseIf lRounds = 3 Then
			LookUpReduction = 2.7
		ElseIf lRounds = 4 Then
			LookUpReduction = 1.9
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 1
		End If
	ElseIf lAverage >= 6.0 And lAverage <= 6.4 Then
		If lRounds = 2 Then
			LookUpReduction = 4.1
		ElseIf lRounds = 3 Then
			LookUpReduction = 3.5
		ElseIf lRounds = 4 Then
			LookUpReduction = 2.8
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 1.9
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 1
		End If
	ElseIf lAverage >= 6.5 And lAverage <= 6.9 Then
		If lRounds = 2 Then
			LookUpReduction = 4.8
		ElseIf lRounds = 3 Then
			LookUpReduction = 4.3
		ElseIf lRounds = 4 Then
			LookUpReduction = 3.7
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 2.9
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 2
		ElseIf lRounds >= 20 Then
			LookUpReduction = 1
		End If
	ElseIf lAverage >= 7.0 And lAverage <= 7.4 Then
		If lRounds = 2 Then
			LookUpReduction = 5.5
		ElseIf lRounds = 3 Then
			LookUpReduction = 5
		ElseIf lRounds = 4 Then
			LookUpReduction = 4.5
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 3.8
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 3
		ElseIf lRounds >= 20 Then
			LookUpReduction = 2.1
		End If
	ElseIf lAverage >= 7.5 And lAverage <= 7.9 Then
		If lRounds = 2 Then
			LookUpReduction = 6.2
		ElseIf lRounds = 3 Then
			LookUpReduction = 5.7
		ElseIf lRounds = 4 Then
			LookUpReduction = 5.3
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 4.7
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 3.9
		ElseIf lRounds >= 20 Then
			LookUpReduction = 3.1
		End If
	ElseIf lAverage >= 8.0 And lAverage <= 8.4 Then
		If lRounds = 2 Then
			LookUpReduction = 6.8
		ElseIf lRounds = 3 Then
			LookUpReduction = 6.4
		ElseIf lRounds = 4 Then
			LookUpReduction = 6
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 5.5
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 4.8
		ElseIf lRounds >= 20 Then
			LookUpReduction = 4.1
		End If
	ElseIf lAverage >= 8.5 And lAverage <= 8.9 Then
		If lRounds = 2 Then
			LookUpReduction = 7.4
		ElseIf lRounds = 3 Then
			LookUpReduction = 7.1
		ElseIf lRounds = 4 Then
			LookUpReduction = 6.7
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 6.2
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 5.7
		ElseIf lRounds >= 20 Then
			LookUpReduction = 5
		End If
	ElseIf lAverage >= 9.0 And lAverage <= 9.4 Then
		If lRounds = 2 Then
			LookUpReduction = 8.1
		ElseIf lRounds = 3 Then
			LookUpReduction = 7.8
		ElseIf lRounds = 4 Then
			LookUpReduction = 7.4
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 7
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 6.5
		ElseIf lRounds >= 20 Then
			LookUpReduction = 5.9
		End If
	ElseIf lAverage >= 9.5 And lAverage <= 9.9 Then
		If lRounds = 2 Then
			LookUpReduction = 8.7
		ElseIf lRounds = 3 Then
			LookUpReduction = 8.4
		ElseIf lRounds = 4 Then
			LookUpReduction = 8.1
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 7.7
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 7.3
		ElseIf lRounds >= 20 Then
			LookUpReduction = 6.7
		End If
	ElseIf lAverage >= 10.0 And lAverage <= 10.4 Then
		If lRounds = 2 Then
			LookUpReduction = 9.2
		ElseIf lRounds = 3 Then
			LookUpReduction = 9
		ElseIf lRounds = 4 Then
			LookUpReduction = 8.8
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 8.4
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 8
		ElseIf lRounds >= 20 Then
			LookUpReduction = 7.6
		End If
	ElseIf lAverage >= 10.5 And lAverage <= 10.9 Then
		If lRounds = 2 Then
			LookUpReduction = 9.8
		ElseIf lRounds = 3 Then
			LookUpReduction = 9.5
		ElseIf lRounds = 4 Then
			LookUpReduction = 9.4
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 9.1
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 8.7
		ElseIf lRounds >= 20 Then
			LookUpReduction = 8.3
		End If
	ElseIf lAverage >= 11.0 And lAverage <= 11.4 Then
		If lRounds = 2 Then
			LookUpReduction = 10.4
		ElseIf lRounds = 3 Then
			LookUpReduction = 10.2
		ElseIf lRounds = 4 Then
			LookUpReduction = 10
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 9.7
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 9.4
		ElseIf lRounds >= 20 Then
			LookUpReduction = 9.1
		End If
	ElseIf lAverage >= 11.5 And lAverage <= 11.9 Then
		If lRounds = 2 Then
			LookUpReduction = 11
		ElseIf lRounds = 3 Then
			LookUpReduction = 10.8
		ElseIf lRounds = 4 Then
			LookUpReduction = 10.6
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 10.4
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 10.1
		ElseIf lRounds >= 20 Then
			LookUpReduction = 9.8
		End If
	ElseIf lAverage >= 12.0 And lAverage <= 12.4 Then
		If lRounds = 2 Then
			LookUpReduction = 11.5
		ElseIf lRounds = 3 Then
			LookUpReduction = 11.4
		ElseIf lRounds = 4 Then
			LookUpReduction = 11.2
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 11
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 10.7
		ElseIf lRounds >= 20 Then
			LookUpReduction = 10.5
		End If
	ElseIf lAverage >= 12.5 And lAverage <= 12.9 Then
		If lRounds = 2 Then
			LookUpReduction = 12.1
		ElseIf lRounds = 3 Then
			LookUpReduction = 11.9
		ElseIf lRounds = 4 Then
			LookUpReduction = 11.8
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 11.6
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 11.4
		ElseIf lRounds >= 20 Then
			LookUpReduction = 11.1
		End If
	ElseIf lAverage >= 13.0 And lAverage <= 13.4 Then
		If lRounds = 2 Then
			LookUpReduction = 12.6
		ElseIf lRounds = 3 Then
			LookUpReduction = 12.5
		ElseIf lRounds = 4 Then
			LookUpReduction = 12.4
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 12.2
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 12
		ElseIf lRounds >= 20 Then
			LookUpReduction = 11.8
		End If
	ElseIf lAverage >= 13.5 And lAverage <= 13.9 Then
		If lRounds = 2 Then
			LookUpReduction = 13.2
		ElseIf lRounds = 3 Then
			LookUpReduction = 13.1
		ElseIf lRounds = 4 Then
			LookUpReduction = 12.9
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 12.8
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 12.6
		ElseIf lRounds >= 20 Then
			LookUpReduction = 12.4
		End If
	ElseIf lAverage >= 14.0 And lAverage <= 14.4 Then
		If lRounds = 2 Then
			LookUpReduction = 13.7
		ElseIf lRounds = 3 Then
			LookUpReduction = 13.6
		ElseIf lRounds = 4 Then
			LookUpReduction = 13.5
		ElseIf lRounds >= 5 And lRounds <= 9 Then
			LookUpReduction = 13.4
		ElseIf lRounds >= 10 And lRounds <= 19 Then
			LookUpReduction = 13.2
		ElseIf lRounds >= 20 Then
			LookUpReduction = 13
		End If
	End If
End Function

Function GetHandicapRS(oConn, UserID)
	Dim aScore, aIndex, aSlope, aSDate, aEDate, aCName, aScrID, x, strCurrHandicap, lAcceptableScores, aHD, aAHD, aTourn, aTournDiffs, aEntBy, oScores
	Set oScores = GetRecords(oConn, "SELECT TOP 20 s.CourseName, s.DateEntered, s.DateOfRound, s.ID, s.Rating, s.Slope, s.Score, s.Tournament, s.UserID, s.EnteredBy, u.FirstName + ' ' + u.LastName AS EnteredByName FROM (MG_Scores s LEFT OUTER JOIN MG_Users u ON s.EnteredBy = u.UserID) WHERE s.UserID = " & UserID & " ORDER BY s.DateOfRound DESC, s.DateEntered DESC")
	g_lNumScores = 0
	
	If oScores.EOF Then
		If Request("Score") <> "" Then
			ReDim g_aAllVals(SetVal(g_lNumScores + 1, 21, 20, g_lNumScores + 1), 10)
			g_aAllVals(0, cScore) = Request("Score")
			g_aAllVals(0, cIndex) = Request("Index")
			g_aAllVals(0, cSlope) = Request("Slope")
			g_aAllVals(0, cSDate) = SetVal(Request("Date"), "", Date, Request("Date"))
			g_aAllVals(0, cHD)    = Round(((g_aAllVals(0, cScore)-g_aAllVals(0, cIndex))*113)/g_aAllVals(0, cSlope), 1)
			g_aAllVals(0, cHDUse) = False
			g_aAllVals(0, cEDate) = Now
			g_aAllVals(0, cTourn) = SetVal(Request("Tournament"), "on", True, False)
			g_aAllVals(0, cCName) = Request("CourseName")
			g_aAllVals(0, cScrID) = 0
			g_aAllVals(0, cEntBy) = "You"
			g_lNumScores = 1
			GetRecords oConn, GetInsert(UserID)
		End If
		
		strCurrHandicap = "N/A"
	Else
		x = 0
		Do Until oScores.EOF
			x = x + 1
			If x = 20 Then
				Exit Do
			End If
			oScores.MoveNext
		Loop
		
		g_lNumScores = x
		oScores.MoveFirst
		ReDim aScore(g_lNumScores - 1)
		ReDim aIndex(g_lNumScores - 1)
		ReDim aSlope(g_lNumScores - 1)
		ReDim aSDate(g_lNumScores - 1)
		ReDim aEDate(g_lNumScores - 1)
		ReDim aScore(g_lNumScores - 1)
		ReDim aTourn(g_lNumScores - 1)
		ReDim aCName(g_lNumScores - 1)
		ReDim aScrID(g_lNumScores - 1)
		ReDim aEntBy(g_lNumScores - 1)
		x = 0
		
		Do Until oScores.EOF
			aScore(x) = oScores("Score")
			aIndex(x) = oScores("Rating")
			aSlope(x) = oScores("Slope")
			aSDate(x) = oScores("DateOfRound")
			aEDate(x) = oScores("DateEntered")
			aTourn(x) = oScores("Tournament")
			aCName(x) = oScores("CourseName")
			aScrID(x) = oScores("ID")
			aEntBy(x) = SetVal(IsNull(oScores("EnteredByName")), True, "You", oScores("EnteredByName"))
			
			x = x + 1
			
			If x = 20 Then
				Exit Do
			End If
			
			oScores.MoveNext
		Loop
		
		If Request("Score") <> "" Then
			ReDim g_aAllVals(SetVal(g_lNumScores + 1, 21, 20, g_lNumScores + 1), 10)
			ReDim aTournDiffs(SetVal(g_lNumScores + 1, 21, 20, g_lNumScores + 1))
		Else
			ReDim g_aAllVals(g_lNumScores, 10)
			ReDim aTournDiffs(g_lNumScores)
		End If
		
		Dim lTCount : lTCount = 0
		
		For x = 0 To g_lNumScores - 1
			g_aAllVals(x, cScore) = aScore(x)
			g_aAllVals(x, cIndex) = aIndex(x)
			g_aAllVals(x, cSlope) = aSlope(x)
			g_aAllVals(x, cSDate) = aSDate(x)
			g_aAllVals(x, cHD)    = Round(((aScore(x)-aIndex(x))*113)/aSlope(x), 1)
			g_aAllVals(x, cHDUse) = False
			g_aAllVals(x, cEDate) = aEDate(x)
			g_aAllVals(x, cTourn) = CBool(aTourn(x))
			g_aAllVals(x, cCName) = aCName(x)
			g_aAllVals(x, cScrID) = aScrID(x)
			g_aAllVals(x, cEntBy) = aEntBy(x)
			
			If g_aAllVals(x, cTourn) Then
				aTournDiffs(lTCount) = g_aAllVals(x, cHD)
				lTCount = lTCount + 1
			End If
		Next
		
		If Request("Score") <> "" Then
			g_lNumScores = SetVal(g_lNumScores + 1, 21, 20, g_lNumScores + 1)
			MoveValsOne g_aAllVals, g_lNumScores
			g_aAllVals(0, cScore) = Request("Score")
			g_aAllVals(0, cIndex) = Request("Index")
			g_aAllVals(0, cSlope) = Request("Slope")
			g_aAllVals(0, cSDate) = SetVal(Request("Date"), "", Date, Request("Date"))
			g_aAllVals(0, cHD)    = Round(((g_aAllVals(0, cScore)-g_aAllVals(0, cIndex))*113)/g_aAllVals(0, cSlope), 1)
			g_aAllVals(0, cHDUse) = False
			g_aAllVals(0, cEDate) = Now
			g_aAllVals(0, cTourn) = SetVal(Request("Tournament"), "on", True, False)
			g_aAllVals(0, cCName) = Request("CourseName")
			g_aAllVals(0, cScrID) = 0
			g_aAllVals(0, cEntBy) = "You"
			
			If g_aAllVals(0, cTourn) Then
				aTournDiffs(lTCount) = g_aAllVals(0, cHD)
				lTCount = lTCount + 1
			End If
			
			GetRecords oConn, GetInsert(UserID)
			SortByDate
		End If
		
		Dim lLowest, lMark, y, lAveAdd
		lLowest = 100
		lAveAdd = 0
		
		Select Case g_lNumScores
			Case 2,3
				lAcceptableScores = 1
			Case 4,5
				lAcceptableScores = 2
			Case 6,7
				lAcceptableScores = 3
			Case 8,9
				lAcceptableScores = 4
			Case 10,11
				lAcceptableScores = 5
			Case 12,13
				lAcceptableScores = 6
			Case 14,15
				lAcceptableScores = 7
			Case 16,17
				lAcceptableScores = 8
			Case 18,19
				lAcceptableScores = 9
			Case 20
				lAcceptableScores = 10
			Case Else
				lAcceptableScores = 0
		End Select
		
		For y = 1 To lAcceptableScores
			For x = 0 To g_lNumScores - 1
				If lLowest > g_aAllVals(x, cHD) And g_aAllVals(x, cHDUse) = False Then
					lLowest = g_aAllVals(x, cHD)
					lMark = x
				End If
			Next
			
			g_aAllVals(lMark, cHDUse) = True
			lLowest = 100
		Next
		
		Dim HDtoUse
		
		For x = 0 To g_lNumScores - 1
			If g_aAllVals(x, cHDUse) Then
				HDtoUse = CSng(g_aAllVals(x, cHD))
				lAveAdd = Round((lAveAdd + HDtoUse), 1)
			End If
		Next
		
		Dim aT2Lowest(1), lHandiReduct
		
		If lAcceptableScores > 0 Then
			strCurrHandicap = (lAveAdd/lAcceptableScores) * 0.96
			
			If lTCount > 1 Then
				aT2Lowest(0) = aTournDiffs(0)
				aT2Lowest(1) = aTournDiffs(1)
				
				If lTCount > 2 Then
					For x = 2 To lTCount - 1
						If aT2Lowest(0) < aT2Lowest(1) Then
							aT2Lowest(1) = SetVal(aTournDiffs(x) < aT2Lowest(1), True, aTournDiffs(x), aT2Lowest(1))
						Else
							aT2Lowest(0) = SetVal(aTournDiffs(x) < aT2Lowest(0), True, aTournDiffs(x), aT2Lowest(0))
						End If
					Next
				End If
				
				lHandiReduct = GetReduction(strCurrHandicap, aT2Lowest(0), aT2Lowest(1), lTCount)
				strCurrHandicap = CStr(strCurrHandicap - lHandiReduct)
			End If
		Else
			strCurrHandicap = "N/A"
		End If
		
		If InStr(strCurrHandicap, ".") > 0 Then
			If InStr(CStr(strCurrHandicap), "E") > 0 Then
				If Mid(strCurrHandicap, InStr(strCurrHandicap, "E")) = "E-01" Then
					strCurrHandicap = "0." & Left(strCurrHandicap, 1)
				Else
					strCurrHandicap = "0.0"
				End If
			Else
				If InStr(strCurrHandicap, ".") + 1 < Len(strCurrHandicap) Then
					strCurrHandicap = Mid(strCurrHandicap, 1, InStr(strCurrHandicap, ".") + 1)
				End If
			End If
		End If
	End If
	
	oScores.Close
	GetHandicapRS = strCurrHandicap
End Function

Function GetInsert(UserID)
	GetInsert = "INSERT INTO MG_Scores (UserID, Rating, Slope, Score, CourseName, DateOfRound, DateEntered, Tournament) VALUES " & _
			"(" & UserID & "," & _
			g_aAllVals(0, cIndex) & "," & _
			g_aAllVals(0, cSlope) & "," & _
			g_aAllVals(0, cScore) & ",'" & _
			Replace(g_aAllVals(0, cCName), "'", "''") & "','" & _
			Replace(g_aAllVals(0, cSDate), "'", "''") & "','" & _
			Replace(g_aAllVals(0, cEDate), "'", "''") & "'," & _
			SetVal(g_aAllVals(0, cTourn), True, "1", "0") & ")"
End Function
%>

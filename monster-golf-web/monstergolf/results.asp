<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<!-- #include file='includes\funcs.asp' -->
<%	MainHeader "Results"
	
	If Request("Year") = "" Then %>
<br>
<center>
Select a Year<br>
<%  Dim oConn, rs, curYear
    Set oConn = GetConnection()
    Set rs = GetRecords(oConn, "SELECT Year from Courses ORDER BY Year DESC") 

    While (Not rs.EOF)
        If (curYear <> rs("Year")) Then
            curYear = rs("Year")
            Response.Write("<a class=PageLink href='results.asp?year=" & curYear & "'>" & curYear & "</a><br>" & vbLf)
        End If
        rs.MoveNext()
    Wend
%>
</center>
<%	Else
		ResultsHeader Request("Year"), Request("HeaderSelect")
		Response.Flush

		If (StrComp(Request("HeaderSelect"), "", vbTextCompare) = 0) Then
            Server.Execute("leaderboard.asp")
		ElseIf (StrComp(Request("HeaderSelect"), "Day1", vbTextCompare) = 0 Or _
		    StrComp(Request("HeaderSelect"), "Day2", vbTextCompare) = 0 Or _
		    StrComp(Request("HeaderSelect"), "Practice", vbTextCompare) = 0) Then
		    Server.Execute("scores.asp")
		ElseIf (StrComp(Request("HeaderSelect"), "Teams", vbTextCompare) = 0) Then
		    Server.Execute("teamsfromdb.asp")
		ElseIf (StrComp(Request("HeaderSelect"), "PracticeLeaders", vbTextCompare) = 0) Then
		    if (Request("Year") = "2009") then
		        Server.Execute("2009\Practice.html")
		    Else
		        Server.Execute("practiceresults.asp")
		    End if
		ElseIf (StrComp(Request("HeaderSelect"), "Individuals", vbTextCompare) = 0) Then
		    Server.Execute("individualgrossresults.asp")
		ElseIf (StrComp(Request("HeaderSelect"), "YearsStats", vbTextCompare) = 0) Then
		    Server.Execute("yearstats.asp")
		End If
	End If
	
	MainFooter("Results") 
%>

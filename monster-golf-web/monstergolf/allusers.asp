<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<!-- #include file='includes\funcs.asp' -->
<% MainHeader "Stats" %>
<link rel="STYLESHEET" type="text/css" href="styles.css"></link>

<% 
    Dim oConn, year, day, rsData, rsCourses
    
    Set oConn = GetConnection()
	year = Request("year")
	day = Request("HeaderSelect")

   sql = "SELECT distinct u.FirstName, u.LastName, u.UserID, t.Year " & _
         "from (users u inner join TourneyScores t on t.userid = u.userid) order by u.LastName, u.FirstName,t.Year"
    Set rs = GetRecords(oConn, sql)
 %>

<table align=center class=tableheadnowidth>
  <tr>
    <td class="colhead">&nbsp;</td>
    <td class="colhead">Name</td>
    <td class="colhead">Monsters</td>
   </tr>
   
<%  Dim curYear, curPlayerID, strYears, curPlayerName, counter

    curYear = rs("Year")
    curPlayerID = rs("UserID")
    curPlayerName = rs("FirstName") + " " + rs("LastName")
    counter = 1
    While (Not rs.EOF)
        If (curPlayerID <> rs("UserID")) Then
            DisplayUser counter, curPlayerId, curPlayerName, strYears
            
            curYear = rs("Year")
            curPlayerID = rs("UserID")
            curPlayerName = rs("FirstName") + " " + rs("LastName")

            strYears = ""
        End If
        
         If (strYears <> "") Then
               strYears = strYears & ", "
         End If
         strYears = strYears & "<a href='results.asp?year=" & rs("Year") & "'>" & rs("Year") & "</a>"
        rs.MoveNext()
    Wend
    
    DisplayUser counter, curPlayerId, curPlayerName, strYears
 %>
</table>

<%	MainFooter("Results") 

Sub DisplayUser(counter, curPlayerId, playerName, strYears)
   Dim sclass
   if (counter mod 2 = 0) then
      sclass=" class=""evenrow"""
   else
      sclass=" class=""oddrow"""
   end if
    Response.Write("  <tr>" & vbLf)
    Response.Write("    <td" & sclass & ">" & counter & "</td>" & vbLf)
    Response.Write("    <td" & sclass & ">" & GetDisplayName(curPlayerId, playerName) & "</td>" & vbLf)
    Response.Write("    <td" & sclass & ">" & strYears & "</td>" & vbLf)
    Response.Write("  </tr>" & vbLf)
    counter = counter + 1
End Sub

%>
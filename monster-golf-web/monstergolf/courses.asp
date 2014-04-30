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

    Set rsCourses = GetRecords(oConn, "SELECT * FROM Courses ORDER BY Year ASC")
 %>

<table align=center>
<tr>
<td colspan=2 align=center>
<table border=1 bordercolor=black cellpadding=1 cellspacing=0>
<%  Dim dayText, curYear
    While (NOT rsCourses.EOF) 
        curYear = rsCourses("Year")%>
 <tr><td colspan=5 class=Title align=center>Monster <%=curYear%>:  <%=rsCourses("Course") %></td></tr>
  <tr>
<%      For i = 1 to 3
            If (i = 1) Then
                MoveToDay rsCoures, "Practice", curYear
                dayText = "Practice"
            ElseIf (i = 2) Then
                MoveToDay rsCoures, "Day1", curYear
                dayText = "Day 1"
            Else
                MoveToDay rsCoures, "Day2", curYear
                dayText = "Day 2"
            End If
 %>
    <td class=Score><b><%=rsCourses("Description")%> (<%=dayText%>)</td>
    <td>Men's Rating/Slope</td>
    <td align=center>Men's Par</td>
    <td>Ladies Rating/Slope</td>
    <td align=center>Ladies Par</td>
   </tr>
   <tr>
    <td>&nbsp;</td>
    <td><%=Round(rsCourses("MensRating"), 1)%> / <%=rsCourses("MensSlope")%></td>
    <td align=center><%=rsCourses("MensPar")%></td>
    <td><%=Round(rsCourses("WomensRating"), 1)%> / <%=rsCourses("WomensSlope")%></td>
    <td align=center><%=rsCourses("WomensPar")%></td>
  </tr>
<%          Do While (Not rsCourses.EOF)
                If (rsCourses("Year") <> curYear) THen
                    Exit Do
                End If
                rsCourses.MoveNext
            Loop       
        Next 
    Wend %>
  </table>
</td>
</tr>
</table>

<%	MainFooter("Results") %>
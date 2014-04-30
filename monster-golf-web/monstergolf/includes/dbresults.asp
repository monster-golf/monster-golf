<%
Function GetConnection()
	Dim sConn
	sConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & Server.MapPath(".") & "\Users\Handicaps.mdb"
'Response.WRite("SCON is [" + sConn + "]")
	Set GetConnection = Server.CreateObject("ADODB.Connection")
	GetConnection.Provider = "Microsoft.Jet.OLEDB.4.0"
	GetConnection.Open Server.MapPath(".") & "\Users\Handicaps.mdb", "Admin", ""
	'GetConnection.Open sConn, "", "", -1
End Function

Function ExecUpdate(oConn, sSQL)
'Response.WRite("sSQL is [" + sSQL + "]<br>")
   Dim val
   val = -1
   oConn.Execute sSQL, val, adExecuteNoRecords
End Function

Function GetRecords(oConn, sSQL)
'Response.WRite("sSQL is [" + sSQL + "]<br>")
	Set GetRecords = oConn.Execute(sSQL)
End Function

Sub WriteParticipants(year, addremove)
   Dim rsteams, oConn, x, rsRookie
   
   Set oConn = GetConnection()
   
   Set rsteams = GetRecords(oConn, "SELECT u1.Paid as Paid1, u1.UserID as UserID1, u1.LastName + ', ' + u1.FirstName as Player1Name, " & _
   "u2.Paid as Paid2, u2.UserID as UserID2, u2.LastName + ', ' + u2.FirstName as Player2Name, t.* " & _
   "FROM (Teams t INNER JOIN Users u1 ON t.Player1=u1.UserID) " & _
   "left outer JOIN Users u2  ON t.Player2=u2.UserID " & _
   "WHERE (((t.Year)=" & year & ") And ((t.Player1)=u1.UserID)) order by u1.LastName ASC, u1.FirstName ASC;")
%>   
<table border="1" cellpadding="1" cellspacing="0">
<tr><td colspan="4" align="center"><%=year%> Field</td></tr>
<%
x = 1
If rsteams.eof then
response.Write "<td colspan='4'>To Be Determined...</td></tr>"
else
   Do Until rsteams.EOF
      response.Write "<td>" & x & "</td>"
      x = x + 1
      response.Write "<td>" & rsteams("Player1Name") & "&nbsp;"
      if (addremove) then 
         if (rsteams("Paid1")) then
            response.Write "<br><b>x</b> paid"
         else
            response.Write "<br><input type='checkbox' name='paid' value='" & rsteams("UserID1") & "'> paid?"
         end if
      Else
         'Set rsRookie = GetRecords(oConn, "Select Year From Teams where (Player1 = " & rsteams("UserID1") & " or Player2 = " & rsteams("UserID1") & ") order by Year asc")
         'if (rsRookie("Year") = year) Then
         '   response.Write "(rookie)"
         'End If
         'rsRookie.Close()
      End If
      response.Write "</td>"
      if (rsteams("Player2") = -1) then
         response.Write "<td>unknown&nbsp;"
      else
         response.Write "<td>" & rsteams("Player2Name") & "&nbsp;"
         if (addremove) then 
            if (rsteams("Paid2")) then
               response.Write "<br><b>x</b> paid"
            else
               response.Write "<br><input type='checkbox' name='paid' value='" & rsteams("UserID2") & "'> paid?"
            end if
         Else
         '   Set rsRookie = GetRecords(oConn, "Select Year From Teams where (Player1 = " & rsteams("UserID2") & " or Player2 = " & rsteams("UserID2") & ") order by Year asc")
         '   if (rsRookie("Year") = year) Then
         '      response.Write "(rookie)"
         '   End If
         '   rsRookie.Close()
         End If
      end if
      response.Write "</td>"
      if (addremove) then
      response.Write "<td><a href='javascript:doremove(" & rsteams("ID") & ");' class='PageLink'>remove</a></td></tr>"
      end if
      response.Write "</tr>"
      rsteams.MoveNext
   Loop
end if
%>
</table>
<% oConn.Close()
End Sub
%>
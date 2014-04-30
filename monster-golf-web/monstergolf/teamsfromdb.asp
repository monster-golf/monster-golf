<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<!-- #include file='includes\funcs.asp' -->
<%
    Dim oConn, year
    Set oConn = GetConnection()
	year = Request("year")
%>
<style> 
.skin { background-color:red; font-weight:bold;} 
.rowheader { background-color:#394fbc; color:white; font-weight:bold; font-size:10pt; } 
.FlightBlank { padding-top:1px; padding-right:1px; padding-left:1px; mso-ignore:padding; color:windowtext; font-size:9.0pt; font-weight:700; font-style:normal;
	text-decoration:none; font-family:Tahoma, sans-serif; mso-font-charset:0; mso-number-format:General; text-align:center; vertical-align:bottom; border-top:.5pt solid windowtext;
	border-right:1.0pt solid windowtext; border-bottom:.5pt solid windowtext; border-left:1.0pt solid windowtext; background:#FFCC00; mso-pattern:auto none; white-space:nowrap;}
.Flight1st { color:white; font-size:10.0pt; font-family: verdana,arial,san serif; background:red; }
.Flight1stBold { color:white; font-size:10.0pt; font-weight:bold; font-family: verdana,arial,san serif; background:red; }
.Flight2nd { color:white; font-size:10.0pt; font-family: verdana,arial,san serif; background:blue; }
.Flight2ndBold { color:white; font-size:10.0pt; font-weight:bold; font-family: verdana,arial,san serif; background:blue;}	
.Flight3rd { color:white; font-size:10.0pt; font-family: verdana,arial,san serif; background:#FF6600;}
.Flight3rdBold { color:white; font-size:10.0pt; font-weight:bold; font-family: verdana,arial,san serif; background:#FF6600;}
.Flight4th {color:white; font-size:10.0pt; font-family: verdana,arial,san serif; background:#339966;}
.Flight4thBold { color:white; font-size:10.0pt; font-weight:bold; font-family: verdana,arial,san serif; background:#339966;}
table { background-color:black; } 
.userlink { color : FFFFFF }
</style>

<center><b>Monster <%=year%></b><br><br>
<table cellpadding=2 cellspacing=1 border=0>
<%
	Dim sql, player1Name, player2Name, flightStyle
	sql = "SELECT Users.* AS [User1], Users_1.* AS [User2], Teams.* " + _
        "FROM (Teams INNER JOIN Users ON Teams.Player1=Users.UserID) INNER JOIN Users AS Users_1 ON Teams.Player2=Users_1.UserID " + _
        "WHERE (((Teams.Year)=" & year & ") And ((Teams.Player1)=Users.UserID)) AND Type='Tournament' ORDER BY ID"

	Set rsMatchups = GetRecords(oConn, sql)
	counter = 1
	
	'' Are there flights that we have to worry about?
	''
	Dim flightsExist : flightsExist = True
	If (IsNull(rsMatchups("Flight"))) Then
        flightsExist = False
    End If

	'' Header row
	''
	Response.Write(" <tr class=rowheader>" & vbLf)
	Response.Write("  <td class=rowheader>#</td>" & vbLf)
	Response.Write("  <td class=rowheader align=center><b>Teams</b></td>" & vbLf)

	'' Are there flights that we have to worry about?
	''
	If (flightsExist) Then
	    Response.Write("  <td class=rowheader align=center><b>Flight</b></td>")
	End If
	Response.Write(" </tr>" & vbLf)
	
	Do Until rsMatchups.EOF
         if (flightsExist) then
            flightStyle = rsMatchups("StyleForFlight")
         else
            flightStyle = "FlightBlank"
         end if
        Response.Write(" <tr class=" + flightStyle + ">" & vbLf)
        Response.Write("  <td align=center class=" + flightStyle + ">" & counter & "</td>" & vbLf)
        player1Name = GetDisplayName(rsMatchups("Users.UserId"), rsMatchups("Users.FirstName") & " " & rsMatchups("Users.LastName"))
        player2Name = GetDisplayName(rsMatchups("Users_1.UserId"), rsMatchups("Users_1.FirstName") & " " & rsMatchups("Users_1.LastName"))
       
        Response.Write("  <td align=center class=" + flightStyle + ">" & player1Name & " / " & player2Name & "</td>" & vbLf)
        
        If (flightsExist) Then
            '' TODO -- we must highlihgt the flight appropriately
            ''
            Response.Write("  <td align=center class=" + flightStyle + ">" & rsMatchups("Flight") & "</td>" & vbLf)
        End If
        
        Response.Write(" </tr>" & vbLf)
        rsMatchups.MoveNext()
        counter = counter + 1
    Loop
%>

</table></center>


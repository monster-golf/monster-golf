<%	Option Explicit %>
<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\handicapinclude.asp' -->
<!-- #include file="includes\db.asp" -->
<%	Response.Buffer  = True
	Response.ExpiresAbsolute=#March 20,2000 00:00:00#
	MainHeader "Handicap" %>
<style>
	body  {font-family: verdana,arial,san serif; font-size:9.0pt; }
	td {font-family: verdana,arial,san serif; font-size:8.0pt; background:#ffffff;}
	input {font-family: verdana,arial,san serif; font-size:8.0pt; }
</style>
<% Dim oConn, oUsers, lNum, oLastDate, sSortBy, sql

If Request("user") <> "" Then
 If Request("user") = "waldadmin" Or Request("user") = "waldadminmonster" Then
  Session("AllUser") = Request("user")
 End If
End If

If Session("AllUser") = "" Then %>
<form name='alluser' method='post' action='allhandicap.asp'>
<center><br><br>User Name <input type='text' name='user' value=''></center>
</form>
</body></html>
<%
Response.End
End If	
	Set oConn = GetConnection
	
	If Request("RemoveUser") <> "" Then
		Response.Write "Removing User: " & Request("RemoveUser")
		GetRecords oConn, "DELETE FROM MG_Users WHERE [UserID] = " & Request("RemoveUser")
	End If
	
	If Request("SortBy") = "" Then
		sSortBy = "FirstName"
	Else
		sSortBy = Request("SortBy")
	End If
	
	sql = "SELECT * from mg_AllUsers " & _
		" ORDER BY " & sSortBy & " ASC"
	Set oUsers = GetRecords(oConn, sql) %>
<br>
<table border=1 bordercolor=gray cellspacing=0 cellpadding=2 align=center>
	<tr>
	<td colspan=14>
	   &nbsp;&nbsp; &nbsp;
	   <a href='uploadscores.asp' class='MenuLink'>Upload Multiple Scores</a> &nbsp; | &nbsp;
	   <a href='tourneyaddteam.asp' class='MenuLink'>Add Players to this Years tournament</a> &nbsp; | &nbsp;
	   <a href='updateuserhcps.asp' class='MenuLink'>Update Handicaps</a>
	</td>
	</tr>
	<tr>
		<td nowrap>&nbsp;</td>
		<td nowrap align=center><a href='allhandicap.asp?SortBy=UserID' class='<%=SetVal(sSortBy, "UserID", "MenuLink", "PageLink")%>'>ID</a>
		<td nowrap align=center><a href='allhandicap.asp?SortBy=FirstName' class='<%=SetVal(sSortBy, "FirstName", "MenuLink", "PageLink")%>'>Name</a></td>
		<td nowrap align=center><a href='allhandicap.asp?SortBy=UserName' class='<%=SetVal(sSortBy, "UserName", "MenuLink", "PageLink")%>'>User Name</a></td>
		<td nowrap align=center><a href='allhandicap.asp?SortBy=Email' class='<%=SetVal(sSortBy, "Email", "MenuLink", "PageLink")%>'>Email</a></td>
		<td nowrap align=center><a href='allhandicap.asp?SortBy=UserType' class='<%=SetVal(sSortBy, "UserType", "MenuLink", "PageLink")%>'>User Type</a></td>
		<td nowrap align=center><a href='allhandicap.asp?SortBy=Handicap' class='<%=SetVal(sSortBy, "Handicap", "MenuLink", "PageLink")%>'>Handicap</a></td>
		<td nowrap align=center><a href='allhandicap.asp?SortBy=NumRounds' class='<%=SetVal(sSortBy, "NumRounds", "MenuLink", "PageLink")%>'># of Rounds</a></td>
		<td nowrap align=center><a href='allhandicap.asp?SortBy=LastEntered' class='<%=SetVal(sSortBy, "LastEntered", "MenuLink", "PageLink")%>'>Last Entered</a></td>
		<td nowrap align=center><a href='allhandicap.asp?SortBy=GHIN' class='<%=SetVal(sSortBy, "GHIN", "MenuLink", "PageLink")%>'>GHIN</a></td>
		<td nowrap>&nbsp;</td>
	</tr>
<%	lNum = 0
	
	Do Until oUsers.EOF
		lNum = lNum + 1
		g_lNumScores = 0 %>
	<tr>
		<td nowrap><%=lNum%></td>
		<td nowrap><%=oUsers("UserID")%></td>
		<td nowrap><a class=MenuLink href='handicap.asp?UserID=<%=oUsers("UserID")%>'><%=oUsers("FirstName") & " " & oUsers("LastName")%></a></td>
		<td nowrap><a class=MenuLink href='handicap.asp?UserID=<%=oUsers("UserID")%>'><%=oUsers("UserName")%></a></td>
		<td nowrap><a class=MenuLink href='mailto:<%=oUsers("Email")%>'><%=oUsers("Email")%></a></td>
		<td nowrap><%=oUsers("UserType")%></td>
		<td nowrap align=center><%=oUsers("Handicap")%></td>
		<td nowrap align=center><%=oUsers("NumRounds")%>&nbsp;</td>
		<td nowrap><%=oUsers("LastEntered")%>&nbsp;</td>
		<td nowrap><%=oUsers("GHIN")%>&nbsp;</td>
		<td nowrap><a class=MenuLink href='javascript:removeuser(<%=oUsers("UserID")%>)'>remove</a></td>
	</tr>
<%		oUsers.MoveNext
	Loop
	
	oUsers.Close
	oConn.Close %>
<script>
function removeuser(userid) {
 if (confirm("Are you sure you want to remove user: " + userid))
  document.location.href = 'allhandicap.asp?RemoveUser=' + userid;
}
</script>
</table>
<%	MainFooter "Handicap" %>




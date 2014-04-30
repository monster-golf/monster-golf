<%	Option Explicit %>
<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\handicapinclude.asp' -->
<!-- #include file="includes\db.asp" -->
<html>
<head>
<style>
	body  {font-family: verdana,arial,san serif; font-size:9.0pt; }
	td {font-family: verdana,arial,san serif; font-size:8.0pt; background:#ffffff;}
	input {font-family: verdana,arial,san serif; font-size:8.0pt; }
</style>
</head>
<%
	Response.Buffer  = True
	Response.ExpiresAbsolute=#March 20,2000 00:00:00# 

If Request("user") <> "" Then
 If Request("user") = "waldadmin" Then
  Session("AllUser") = Request("user")
 End If
End If

If Session("AllUser") = "" Then %>
<body>
<form name='alluser' method='post' action='updateuserhcps.asp'>
<center><br><br>User Name <input type='text' name='user' value=''></center>
</form>
</body></html>
<%
Response.End
End If	

If Request("showpage") = "" Then %>
<frameset border="1" cols="50%,50%">
<frame name="updatefrm" src="updateuserhcps.asp?showpage=true" scrolling="auto">
<frame name="allfrm" src="allhandicap.asp" scrolling="auto">
</frameset>
<%
End If

If (Request("doupdate") = "true") Then
   Server.ScriptTimeout = 10000
   Dim oConnSQL, oRS, strSQL, oldhandi, newhandi
   Set oConnSQL = GetConnection
   
   if (Request("updateuser") <> "") Then
      strSQL = "select * from mg_Users where UserID = " & Request("updateuser")
   else
      strSQL = "Select * from mg_Users order by userid"
   end if
   
   Set oRS = GetRecords(oConnSQL, strSQL)
   dim x : x = 0
   
   do until oRS.eof
      g_lNumScores = 0
      oldhandi = oRS("Handicap")
      newhandi = Replace(GetHandicapRS(oConnSQL, oRS("UserID")), "N/A", "0.0")
      strSQL = "Update mg_users set Handicap = " & newhandi & " where userid = " & oRS("UserID")
      GetRecords oConnSQL, strSQL
      Response.Write strSQL & " - " & oRS("FirstName") & " " & oRS("LastName") & " from " & oldhandi & " to " & newhandi & "<br><br>"
      Response.Flush
      oRS.MoveNext
   loop
End If
%>
<script>
function submitpage()
{
document.updateusers.doupdate.value = "true";
document.updateusers.submit();
}
</script>
<body>
<form method="post" name="updateusers" action="updateuserhcps.asp?showpage=true">
<a href="allhandicap.asp" target="_top" class="PageLink">close</a><br>
<input type="hidden" name="doupdate" value="">
user id: <input type="text" name="updateuser" size="5" maxlength="5" value=""> (leave blank to do all users) <input type="button" name="dosubmit" onclick="submitpage()" value="submit">
</form>
</body>
</html>

<%	Option Explicit %>
<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\handicapinclude.asp' -->
<!-- #include file="includes\db.asp" -->
<%
   Server.ScriptTimeout = 15000

   Dim oConnSQL, newhandi, oUsers, currDate, currDate16
   on error resume next
   Set oConnSQL = GetConnection
   if err.number > 0 then
      response.Write (err.Description)
      response.End
   end if

   'currDate = CDate("3/1/2006")
   'do
   '   currDate16 = Month(currDate) & "/16/" & Year(currDate)
   '   GetRecords oConnSQL, GetHandicapSQLForStorage(oConnSQL, 155, currDate)
   '   GetRecords oConnSQL, GetHandicapSQLForStorage(oConnSQL, 155, currDate16)
   '   currDate = DateAdd("m", 1, currDate)
   '   if (currDate >= CDate("5/1/2007")) Then
   '      exit do
   '   end if
   'Loop
      
   Set oUsers = GetRecords(oConnSQL, "Select top 2 u.UserID FROM MG_Users u left join MG_Handicaps h on h.userid = u.userid where u.UserID not in (46,61,76,93,105,112,120,139,150,158,165,166,167,170,227,232,237) and h.HandicapID is null order by u.UserID")
   Do Until oUsers.EOF
      currDate = CDate("1/1/2004")
      do
         currDate16 = Month(currDate) & "/16/" & Year(currDate)
         on error resume next
         GetRecords oConnSQL, GetHandicapSQLForStorage(oConnSQL, oUsers("UserID"), currDate)
         if err.number > 0 then
            response.Write (err.Description)
            exit do
         end if
         response.Write currDate & " - " & oUsers("UserID") & "<br>"
         on error resume next
         GetRecords oConnSQL, GetHandicapSQLForStorage(oConnSQL, oUsers("UserID"), currDate16)
         if err.number > 0 then
            response.Write (err.Description)
            exit do
         end if
         response.Write currDate16 & " - " & oUsers("UserID") & "<br>"
         response.Flush()
         currDate = DateAdd("m", 1, currDate)
         if (currDate >= CDate("5/1/2007")) Then
            exit do
         end if
      Loop
      oUsers.MoveNext
	Loop
   oConnSQL.Close
	
	Response.Write "<br>Done"
%>
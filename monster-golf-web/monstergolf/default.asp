<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<% MainHeader "Home" %>
<form name='SignUp' method='Post' action='default.asp?DoSignUp=true'>
</form>
<table align='center' cellspacing='0' cellpadding=3 border=1 bordercolor=black>
<%
'<tr><td colspan="2" align="center">
'<b>!! Wanted Money for the Monster !!</b><br />If you have seen these procrastinators get a hold of their wallets.<br /><br />
'<table>
'   Set oConn = GetConnection()
'  
'   Set rsteams = GetRecords(oConn, "SELECT u1.Paid as Paid1, u1.UserID as UserID1, u1.Image as Image1, u1.FirstName + ' ' + u1.LastName as Player1Name, " & _
'   "u2.Paid as Paid2, u2.UserID as UserID2, u2.Image as Image2, u2.FirstName + ' ' + u2.LastName as Player2Name, t.* " & _
'   "FROM (Teams t INNER JOIN Users u1 ON t.Player1=u1.UserID) " & _
'   "left outer JOIN Users u2  ON t.Player2=u2.UserID " & _
'   "WHERE (((t.Year)=2007) And ((t.Player1)=u1.UserID)) order by u1.FirstName ASC, u1.LastName ASC;")
'   Dim image
'   Dim fso : Set fso = Server.CreateObject("Scripting.FileSystemObject")
'   Dim count : count = 0
'   
'   Do Until rsteams.EOF
'      if (Not rsteams("Paid1")) then
'         If (fso.FileExists(Server.MapPath(".") + "\images\" + Replace(rsteams("Player1Name"), " ", "") + ".jpg")) Then
'            image = Replace(rsteams("Player1Name"), " ", "") + ".jpg"
'         ElseIf (IsNull(rsteams("Image1"))) Then
'            image = "noperson.jpg"
'         Else
'            image = rsteams("Image1")
'         End If
'         if (count = 0) then
'            response.Write ("<tr>")
'         end if
'         Response.Write "<td valign='top' align='center' style='font-weight:bold'><img style=""border:groove 6px #bc1124;"" src=""images/" & image & """ alt=""" & rsteams("Player1Name") & """/><br />" & rsteams("Player1Name") & "</td>"
'         count = count + 1
'         if (count = 6) then
'            response.Write "</tr>"
'            count = 0
'         end if
'      end if
'      if (Not rsteams("Paid2")) then
'         If (fso.FileExists(Server.MapPath(".") + "\images\" + Replace(rsteams("Player2Name"), " ", "") + ".jpg")) Then
'            image = Replace(rsteams("Player2Name"), " ", "") + ".jpg"
'         ElseIf (IsNull(rsteams("Image2"))) Then
'            image = "noperson.jpg"
'         Else
'            image = rsteams("Image2")
'         End If
'         if (count = 0) then
'            response.Write ("<tr>")
'         end if
'         Response.Write "<td valign='top' align='center' style='font-weight:bold'><img style=""border:groove 6px #bc1124;"" src=""images/" & image & """ alt=""" & rsteams("Player2Name") & """/><br />" & rsteams("Player2Name") & "</td>"
'         count = count + 1
'         if (count = 6) then
'            response.Write "</tr>"
'            count = 0
'         end if
'      end if
'      rsteams.MoveNext
'   Loop
'
'   if (count <> 0) then
'      response.Write "</tr>"
'      count = 0
'   end if
'</table>
'</td></tr>
%>
<tr><td align="center">
<div style="padding:3px 5px 7px 5px;width:500px;text-align:center;font-weight:bold;">
    <b>Monster XXIV (2014) - Wild Horse Pass</b>
    <br />May 15th, 16th and 17th<br />
    <a target="wildhorse" href="http://www.wingilariver.com/wild-horse-pass">Wild Horse Resort</a> Chandler, Arizona<br />
    <a target="whirlwind" href="http://www.whirlwindgolf.com/index.php">Whirlwind Golf Club</a><br />


</div>
<div style="padding:3px 5px 7px 5px;width:500px;text-align:center;"><%=DateDiff("D", Date, "5/15/2014")%> days to go to.</div>

<div style="padding:3px 5px 7px 5px;width:500px;"><form action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_top">
<input type="hidden" name="cmd" value="_s-xclick">
<input type="hidden" name="hosted_button_id" value="E9SLAX2GZDTRQ">
<table>
<tr><td><input type="hidden" name="on0" value="Monster Entry (PayPal fee included)">Monster Entry (PayPal fee included)</td></tr><tr><td><select name="os0">
	<option value="1 Player">1 Player $258.00 USD</option>
	<option value="2 Players">2 Players $516.00 USD</option>
	<option value="3 Players">3 Players $774.00 USD</option>
	<option value="4 Players">4 Players $1,032.00 USD</option>
</select> </td></tr>
</table>
<input type="hidden" name="currency_code" value="USD">
<input type="image" src="https://www.paypalobjects.com/en_US/i/btn/btn_buynowCC_LG.gif" border="0" name="submit" alt="PayPal - The safer, easier way to pay online!">
<img alt="" border="0" src="https://www.paypalobjects.com/en_US/i/scr/pixel.gif" width="1" height="1">
</form>
</div>

<div style="padding:10px 5px 7px 5px;width:500px;text-align:justify;font-weight:normal;">
<table>
<tr><td align="right"><b>Practice Round:</b></td><td>Devils Claw - Thursday, May 15th, Tee times starting at 9:45 AM</td></tr>
<tr><td></td><td>5:00 PM Horserace on Cattail</td></tr>
<tr><td align="right"><b>Opening Round:</b></td><td>Cattail - Friday, May 16th, Shotgun, 1:00 PM</td></tr>
<tr><td></td><td>Charity Auction to follow</td></tr>
<tr><td align="right"><b>Final Round:</b></td><td>Saturday, Devils Claw, May 17th, Shotgun, 1:00 PM</td></tr>
<tr><td></td><td>Banquet and Awards to follow</td></tr>
</table>
</div>
<br />
<img src="whirlwind.jpg" style="width:600px;" />
<br /><br />
Congratulations to the 2013 Champions<br /><br />
<font style="font-weight:bold;font-size:16px;">Steve Wald &amp; Paul Plemel</font>
<br /><br />
<img src="2013Champs.png" alt="Steve &amp; Paul" />
<!--
<br /><br />
<b>2012 Awards Ceremony</b><br />
<img src="2012Participants.png" alt="Awards 2012" />

<br /><br />
<div style="padding:3px 5px 7px 5px;width:500px;text-align:center;">
Bill always enjoyed life, he will be missed this year.<br /><br />
<img src="BillyatSummitLake.jpg" />
</div>
-->
</td>
<td valign=top rowspan="2">
<% WriteParticipants 2014, false %>
</td>
</tr>
<!--
<tr>
<td align=center>
<b><a href="results.asp?year=2008" class="PageLink">Monster XVIII (2008)</a><br/>was in Mesquite, NV on 6/9/2008 & 6/10/2008</b><br />
Congrats to last Year's Champions <b>Jeff Herberger & Paul Olson</b><br />
<img src="Herby_Ole.jpg" style="border:solid 3px #bc1124;" /><br /><br />
<img src="Champ_Handover.jpg" style="border:solid 3px #bc1124;" /><br /><br />
and the Rookie of the Year <b>Sean Coughlin</b><br />
<img src="Roy_Coughlin.jpg" style="border:solid 3px #bc1124;" />
<br/><br/>
<a target="story" href="http://www.golf.com/golf/tours_news/article/0,28136,1868330,00.html" class="PageLink">Scientist's confirm golf is a sport!</a>
<br />
<span style="font-weight: bold; font-size=14pt">Athletes???</span><br />
<img src="athletes.jpg" style="border:solid 3px #bc1124;" />
<br />
</td>
</tr>
-->
</table>
</td>
</tr>
</table>
<% MainFooter "Home" %>
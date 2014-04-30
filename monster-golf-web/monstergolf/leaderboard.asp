<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<!-- #include file='includes\funcs.asp' -->
<style>
<!--table
	{mso-displayed-decimal-separator:"\.";
	mso-displayed-thousand-separator:"\,";}
.FlightBlank { padding-top:1px; padding-right:1px; padding-left:1px; mso-ignore:padding; color:windowtext; font-size:9.0pt; font-weight:700; font-style:normal;
	text-decoration:none; font-family:Tahoma, sans-serif; mso-font-charset:0; mso-number-format:General; text-align:center; vertical-align:bottom; border-top:.5pt solid windowtext;
	border-right:1.0pt solid windowtext; border-bottom:.5pt solid windowtext; border-left:1.0pt solid windowtext; background:#FFCC00; mso-pattern:auto none; white-space:nowrap;}
.ColumnHeader { padding-top:1px; padding-right:1px; padding-left:1px; mso-ignore:padding; color:windowtext; font-size:9.0pt; font-weight:700; font-style:normal;
	text-decoration:none; font-family:Tahoma, sans-serif; mso-font-charset:0; mso-number-format:General; text-align:center; vertical-align:bottom; border:.5pt solid windowtext;
	background:#99CCFF; mso-pattern:auto none; white-space:nowrap; }
.TourneyHeader { padding-top:1px; padding-right:1px; padding-left:1px; mso-ignore:padding; color:#003300; font-size:11.0pt; font-weight:700; font-style:normal;
	text-decoration:none; font-family:verdana,arial,san serif; mso-font-charset:0; mso-number-format:General; text-align:center; vertical-align:bottom;
	border-top:1.0pt solid windowtext; border-right:none; border-bottom:1.0pt solid windowtext; border-left:1.0pt solid windowtext; background:#FFFF99; mso-pattern:auto none; white-space:nowrap; }
.Flight1st { color:white; font-size:10.0pt; font-family: verdana,arial,san serif; background:red; }
.Flight1stBold { color:white; font-size:10.0pt; font-weight:bold; font-family: verdana,arial,san serif; background:red; }
.Flight2nd { color:white; font-size:10.0pt; font-family: verdana,arial,san serif; background:blue; }
.Flight2ndBold { color:white; font-size:10.0pt; font-weight:bold; font-family: verdana,arial,san serif; background:blue;}	
.Flight3rd { color:white; font-size:10.0pt; font-family: verdana,arial,san serif; background:#FF6600;}
.Flight3rdBold { color:white; font-size:10.0pt; font-weight:bold; font-family: verdana,arial,san serif; background:#FF6600;}
.Flight4th {color:white; font-size:10.0pt; font-family: verdana,arial,san serif; background:#339966;}
.Flight4thBold { color:white; font-size:10.0pt; font-weight:bold; font-family: verdana,arial,san serif; background:#339966;}
.userlink { color : FFFFFF }
	-->
</style>
</head>
<%
    Dim oConn, year, day
    
    Set oConn = GetConnection()
	year = Request("year")
	day = Request("HeaderSelect")
    Set rsSlogan = GetRecords(oConn, "SELECT * FROM Details WHERE Year=" & year)
%>
<center>
<table cellpadding="8" cellspacing="0">
<tr><td valign=top>
<table border=1 bordercolor=black  cellpadding=0 cellspacing=0>
 <tr height=20 style='mso-height-source:userset;height:15.0pt'>
  <td colspan=6 height=20 class='TourneyHeader' style='border-right:1.0pt solid black;height:15.0pt;'>MONSTER GOLF <%=year%> - <%=rsSlogan("Slogan")%></td>
 </tr>
<% 
    Dim usesFlights, numTeams, aItems, playoffDetails, aFlights, flightIndex, flightCount, flightFound
    ReDim aFlights(10)
    flightCount = 0
    flightFound = false
    
    CalculateTourneyScoresForYear year

    aItems = Application("MonsterTourneyScores" & year)
    usesFlights = Application("MonsterTourneyUsesFlights" & year)
    numTeams = Application("MonsterTourneyNumTeams" & year)
    playoffDetails = Application("MonsterTourneyPlayoffDetails" & year)

    If (numTeams > 0) Then
        Response.Write(" <tr height=22 style='height:16.5pt'>" & vbLf)
        Response.Write("  <td height=22 class='ColumnHeader' width=51 style='height:16.5pt;width:38pt'>Place</td>" & vbLf)
        If (usesFlights) Then
            Response.Write("  <td class='ColumnHeader' width=54 style='width:41pt'>Flight</td>" & vbLf)
        End If
        Response.Write("  <td class='ColumnHeader' width=178 style='border-left:none;'>Team</td>" & vbLf)
        Response.Write("  <td class='ColumnHeader' width=54 style='width:41pt'>Day 1</td>" & vbLf)
        Response.Write("  <td class='ColumnHeader' width=54 style='width:41pt'>Day 2</td>" & vbLf)
        Response.Write("  <td class='ColumnHeader' width=54 style='width:41pt'>Total</td>" & vbLf)
        Response.Write("</tr>" & vbLf)
    End If
    
    Dim team, styleNormal, styleBold, playerIDs, href
    
    For i = 0 to numTeams - 1
        Response.Write(" <tr height=21 style='height:15.75pt'>" & vbLf)

        team = aItems(i, 2)
        If (i = 0 And aItems(i, 6)) Then
            team = "* " + team
        End If

        If (usesFlights) Then
            styleNormal = aItems(i, 8)
            styleBold = styleNormal + "Bold"
        Else
            styleNormal = "FlightBlank"
            styleBold = "FlightBlank"
        End If
        
        playerIDs = Split(aItems(i, 5), "/")
        href = "<a class='" + styleBold + "' href='roundbreakdown.asp?year=" & year & "&Player1=" & playerIDs(0) & "&Player2=" & playerIDs(1) & "'>"
    
        Response.Write("  <td height=21 class='ColumnHeader' style='height:15.75pt;border-top:none'>" & aItems(i, 9) & "</td>" & vbLf)
        If (usesFlights) Then
            flightFound = false
            For flightIndex = 0 to UBound(aFlights)
               If (aItems(i, 7) = aFlights(flightIndex)) Then
                  flightFound = true
                  Exit For
               End If
            Next
            If (Not flightFound) Then
               aFlights(flightCount) = aItems(i, 7)
               flightCount = flightCount + 1
            End If
            Response.Write("  <td class=" & styleBold & " style='border-top:none;border-left:none;text-align:center;'>" & aItems(i, 7) & "</td>" & vbLf)
        End If
        Response.Write("  <td class=" & styleBold & " style='border-top:none;border-left:none' nowrap>" & team & "</td>" & vbLf)
        Response.Write("  <td class=" & styleNormal & " style='border-top:none;text-align:center;'>" & href & aItems(i, 3) & "</a></td>" & vbLf)
        Response.Write("  <td class=" & styleNormal & " style='border-top:none;text-align:center;'>" & href & aItems(i, 4) & "</a></td>" & vbLf)
        Response.Write("  <td class=" & styleBold & " style='border-top:none;text-align:center;'>" & href & aItems(i, 1) & "</a></td>" & vbLf)
        Response.Write(" </tr>" & vbLf)
    Next
   Response.Write(" </table></td><td valign=top><table border=1 bordercolor=black  cellpadding=0 cellspacing=0>" & vbLf)
        
    If (usesFlights) Then
      For flightIndex = 0 To flightCount - 1
        Response.Write(" <tr height=20 style='mso-height-source:userset;height:15.0pt'>" & vbLf)
        Response.Write(" <td colspan=3 height=20 class='TourneyHeader' style='border-right:1.0pt solid black;height:15.0pt;'>FLIGHT " + aFlights(flightIndex) + "</td>" & vbLf)
        Response.Write(" </tr>" & vbLF)
        Response.Write(" <tr height=22 style='height:16.5pt'>" & vbLf)
        Response.Write("  <td height=22 class='ColumnHeader' width=51 style='height:16.5pt;width:38pt'>Place</td>" & vbLf)
        Response.Write("  <td class='ColumnHeader' width=178 style='border-left:none;'>Team</td>" & vbLf)
        Response.Write("  <td class='ColumnHeader' width=54 style='width:41pt'>Total</td>" & vbLf)
        Response.Write("</tr>" & vbLf)
         For i = 0 to numTeams - 1
            if (aFlights(flightIndex) = aItems(i, 7)) Then
               Response.Write(" <tr height=21 style='height:15.75pt'>" & vbLf)
               team = aItems(i, 2)
               If (i = 0 And aItems(i, 6)) Then
                     team = "* " + team
               End If

               If (usesFlights) Then
                     styleNormal = aItems(i, 8)
                     styleBold = styleNormal + "Bold"
               Else
                     styleNormal = "FlightBlank"
                     styleBold = "FlightBlank"
               End If
                 
               playerIDs = Split(aItems(i, 5), "/")
               href = "<a class='" + styleBold + "' href='roundbreakdown.asp?year=" & year & "&Player1=" & playerIDs(0) & "&Player2=" & playerIDs(1) & "'>"
             
               Response.Write("  <td height=21 class='ColumnHeader' style='height:15.75pt;border-top:none'>" & aItems(i, 9) & "</td>" & vbLf)
               Response.Write("  <td class=" & styleBold & " style='border-top:none;border-left:none' nowrap>" & team & "</td>" & vbLf)
               Response.Write("  <td class=" & styleBold & " style='border-top:none;text-align:center;'>" & href & aItems(i, 1) & "</a></td>" & vbLf)
               Response.Write(" </tr>" & vbLf)
            End If
         Next
      Next
    End If %>
</table></td></tr></table>
<%   If (playoffDetails <> "") Then
        Response.Write("<BR>* " + playoffDetails)
   End If %>
</center>
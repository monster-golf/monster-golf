<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<!-- #include file='includes\funcs.asp' -->


</head>

<center>
<table x:str border=0 cellpadding=0 cellspacing=0>
 <tr height=22 style='height:16.5pt'>
  <td height=22 class='Place' width=51 style='height:16.5pt;width:38pt'>Place</td>
  <td class='Team' width=178 style='border-left:none;'>Team</td>
  <td class='Total' width=54 style='width:41pt'>Total</td>
 </tr>
<% 
    Dim oConn, aItems, year, numTeams
 
    Set oConn = GetConnection()
	year = Request("year")

    CalculatePracticeRoundScoresForYear year

    aItems = Application("MonsterPracticeRoundScores" & year)
    numTeams = Application("MonsterPracticeRoundNumTeams" & year)

    For i = 0 to numTeams - 1
        Response.Write(" <tr height=21 style='height:15.75pt'>" & vbLf)
        
        Response.Write("  <td height=21 class='ColumnHeader' style='height:15.75pt;border-top:none'>" & aItems(i, 4) & "</td>" & vbLf)
        Response.Write("  <td class='TeamName' style='border-top:none;border-left:none'>" & aItems(i, 2) & "</td>" & vbLf)
        Response.Write("  <td class='TeamScore' style='border-top:none'>" & aItems(i, 1) & "</td>" & vbLf)
        Response.Write(" </tr>" & vbLf)
        lastScore = aItems(i, 1)
    Next

%>
 
</table>
</center>
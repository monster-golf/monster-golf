<%	Function SetVal(val1, val2, trueVal, falseVal)
		If val1 = val2 Then
			SetVal = trueVal
		Else
			SetVal = falseVal
		End If
	End Function

	Sub Header(sSection) %>
<table width='100%' cellspacing=0 cellpadding=4 class=HeaderTable>
<tr>
	<td align="center" nowrap class="<%=SetVal(sSection, "home", "SelHeaderText", "HeaderText")%>"><a href="default.asp?section=home" class="<%=SetVal(sSection, "home", "SelHeaderText", "HeaderText")%>">Wedding News</a></td>
	<td align="center" nowrap class="<%=SetVal(sSection, "minneapolis", "SelHeaderText", "HeaderText")%>"><a href="default.asp?section=minneapolis" class="<%=SetVal(sSection, "minneapolis", "SelHeaderText", "HeaderText")%>">Minneapolis Info</a></td>
	<td align="center" nowrap class="<%=SetVal(sSection, "travel", "SelHeaderText", "HeaderText")%>"><a href="default.asp?section=travel" class="<%=SetVal(sSection, "travel", "SelHeaderText", "HeaderText")%>">Travel Info</a></td>
	<td align="center" nowrap class="<%=SetVal(sSection, "songs", "SelHeaderText", "HeaderText")%>"><a href="default.asp?section=songs" class="<%=SetVal(sSection, "songs", "SelHeaderText", "HeaderText")%>">Song Requests</a></td>
	<td align="center" nowrap class="<%=SetVal(sSection, "comments", "SelHeaderText", "HeaderText")%>"><a href="default.asp?section=comments" class="<%=SetVal(sSection, "comments", "SelHeaderText", "HeaderText")%>">Guest Comments</a></td>
	<td align="center" nowrap class="<%=SetVal(sSection, "gallery", "SelHeaderText", "HeaderText")%>"><a href="default.asp?section=gallery" class="<%=SetVal(sSection, "gallery", "SelHeaderText", "HeaderText")%>">Gallery</a></td>
</tr>
</table>
<br>
<%	End Sub

	Dim aFileData, x
	Dim oFS, oFile, sText, bWroteNew

	If Request("attending") <> "" Then
		Set oFS = Server.CreateObject("Scripting.FileSystemObject")
		Set oFile = oFS.OpenTextFile(Server.MapPath(".\attending.txt"), , True)

		If Not oFile.AtEndOfStream Then
			sText = oFile.ReadAll
		Else
			sText = ""
		End If

		oFile.Close
		aFileData = Split(sText, vbCrLf)
		sText = ""
		Set oFile = oFS.OpenTextFile(Server.MapPath(".\attending.txt"), 2, True)

		oFile.WriteLine Trim(Request("attending") & " - " & Request("numberattending")) & " - " & Request("shuttle") & " (" & Now & ")"

		For x = 0 To UBound(aFileData)
			If Trim(aFileData(x)) <> "" Then
				oFile.WriteLine Trim(aFileData(x))
			End If
		Next

		oFile.Close
	End If

	If Request("section") = "travel" Then
		Set oFS = Server.CreateObject("Scripting.FileSystemObject")
		Set oFile = oFS.OpenTextFile(Server.MapPath(".\travel.txt"), , True)

		If Not oFile.AtEndOfStream Then
			sText = oFile.ReadAll
		Else
			sText = ""
		End If

		If Request("from") <> "" Then
			oFile.Close
			Set oFile = oFS.OpenTextFile(Server.MapPath(".\travel.txt"), 2, True)
			sText = "<tr class=HeaderText><td>" & Trim(Request("from")) & "</td><td>" & Trim(Request("to")) & "</td><td>" & Trim(Request("airline")) & "</td><td>$" & Trim(Request("price")) & "</td><td>" & Trim(Request("date")) & "</td></tr>" & vbCrLf & sText
			oFile.Write sText
		End If

		oFile.Close
	End If

	If Request("section") = "songs" Then
		Set oFS = Server.CreateObject("Scripting.FileSystemObject")
		Set oFile = oFS.OpenTextFile(Server.MapPath(".\songrequests.txt"), , True)

		If Not oFile.AtEndOfStream Then
			sText = oFile.ReadAll
		Else
			sText = ""
		End If

		oFile.Close

		If Request("song") <> "" Then
			aFileData = Split(sText, vbCrLf)
			sText = ""
			Set oFile = oFS.OpenTextFile(Server.MapPath(".\songrequests.txt"), 2, True)
			bWroteNew = False

			For x = 0 To UBound(aFileData)
				If Trim(aFileData(x)) <> "" Then
					If Left(Request("song"), 1) < Left(aFileData(x), 1) Then
						If Not bWroteNew Then
							bWroteNew = True
							oFile.WriteLine Trim(Request("song") & "||" & Request("requestor"))
							sText = sText & Trim(Request("song") & "||" & Request("requestor")) & vbCrLf
						End If
					End If

					oFile.WriteLine Trim(aFileData(x))
					sText = sText & Trim(aFileData(x)) & vbCrLf
				End If
			Next

			If Not bWroteNew Then
				oFile.WriteLine Trim(Request("song") & "||" & Request("requestor"))
				sText = sText & Trim(Request("song") & "||" & Request("requestor")) & vbCrLf
			End If

			oFile.Close
		End If

		aFileData = Split(sText, vbCrLf)
	End If

	If Request("section") = "comments" Then
		Set oFS = Server.CreateObject("Scripting.FileSystemObject")
		Set oFile = oFS.OpenTextFile(Server.MapPath(".\guestcomments.txt"), , True)

		If Not oFile.AtEndOfStream Then
			sText = oFile.ReadAll
		Else
			sText = ""
		End If

		oFile.Close

		If Request("guestcomment") <> "" Then
			aFileData = Split(sText, vbCrLf)
			sText = ""
			Set oFile = oFS.OpenTextFile(Server.MapPath(".\guestcomments.txt"), 2, True)

			oFile.WriteLine Request("guest") & "||" & Now & "||" & Replace(Request("guestcomment"), vbCrLf, " ")
			sText = sText & Request("guest") & "||" & Now & "||" & Replace(Request("guestcomment"), vbCrLf, " ") & vbCrLf

			For x = 0 To UBound(aFileData)
				If Trim(aFileData(x)) <> "" Then
					oFile.WriteLine Trim(aFileData(x))
					sText = sText & Trim(aFileData(x)) & vbCrLf
				End If
			Next

			oFile.Close
		End If

		aFileData = Split(sText, vbCrLf)
	End If %>
<html>
<style>
.coral { background-color: #E97781; }
.espresso { background-color: #5D4E4B; }
.berry { background-color: #A93238; }
.tang { background-color: #F36348; }

body { font-family:verdana; color:white; font-size:12pt; font-weight:normal; background: #5D4E4B; margin-left:5px; margin-right:5px;}

td { font-family:verdana; font-size:10pt; font-weight:bold; color:white; }

.CommentText { font-family:verdana; font-size:10pt; font-weight:normal; }

.CommentTextGreen { background: #A93238; font-family:verdana; font-size:10pt; font-weight:normal; }

.CommentBreak { background: #A93238; font-family:verdana; font-size:1pt; }

.HeaderTable { border: #FFCC99 2px solid; }
.MainImg { border: #FFCC99 2px solid; cursor:pointer; }

.HeaderText { background: #FFCC99; color:black; font-weight:bold; font-family:verdana; font-size:10pt; }
.SelHeaderText { background: #A93238; color:white; font-weight:bold; font-family:verdana; font-size:10pt; }

.InputText { border: #3C2F40 1px solid; background: #FFCC99; color:black; font-weight:normal; font-family:verdana; font-size:9pt; }
.RegularLink { color:#FFCC99; font-weight:bold; font-family:verdana; font-size:9pt; }
.SubButton { background: #A93238; color:white; font-weight:normal; font-family:verdana; font-size:8pt; }
</style>

<body topmargin="0" leftmargin="0" rightmargin="0">
<form name="WedForm" action="default.asp" method="post">
<%	If Request("section") <> "" Then
			Header Request("section")
	End If

	Select Case Request("section")
		Case "" %>
<center>
<br>
<big>Welcome to Tara and Brian's wedding-site.</big><br>
<big>Save the date - July 22nd, 2006</big> &nbsp; &nbsp; <%=DateDiff("d", Date, "7/22/2006")%> days from today.<br><br>
Do you plan on attending? &nbsp;
Your Name <input class=InputText type=text name="attending" size="30"> &nbsp;How Many <input class=InputText type=text name="numberattending" size="2"><br>
Do You plan on riding the shuttle from the hotel or church to the reception? <input type="radio" name="shuttle" value="Yes" checked> Yes <input type="radio" name="shuttle" value="No"> No<br><input class="SubButton" type="submit" value="Submit" onClick="document.WedForm.action='default.asp?section=home'" NAME="Submit1"><br>
<a href="wedding_brochure.pdf" class="RegularLink" target="brochure">Click here to read the wedding brochure (pdf format)</a>
<br><br>Click on the picture to enter.<br>
<br><img class="MainImg" src='main.jpg' onClick="document.WedForm.action='default.asp?section=home'; document.WedForm.submit();"><br><br>
</center>
<%		Case "home" %>
<table width="100%">
<tr><td align=right>Who : </td><td>Tara Demars and Brian Giesinger</td></tr>
<tr><td align=right>What : </td><td>Wedding Shindig</td></tr>
<tr><td align=right>Where : </td><td>Wedding: Our Lady of the Lake Catholic Church in Mound, MN</td></tr>
<tr><td align=right></td><td>Reception: Lakeside at the Mark and Becky (Giesinger) Lee's Residence in Mound, MN</td></tr>
<tr><td align=right>When : </td><td>July 22nd, 2006 at 2:30 pm - <%=DateDiff("d", Date, "7/22/2006")%> days from today.</td></tr>
<tr><td align=right>Maps : </td>
<td>
<a class="RegularLink" target="map" href="http://local.live.com/default.aspx?v=2&cp=44.913414~-93.552616&style=r&lvl=13&sp=aN.44.942081_-93.642397_Lee%20Residence_1989%20Lakeside%20Lane%0d%0aMound%2c%20MN%2055364~aN.44.935534_-93.666489_Our%20Lady%20of%20the%20Lake%20Catholic%20Church_2385%20Commerce%20Blvd%0d%0aMound%2c%20MN%2055364~aN.44.862014_-93.536439_Country%20Suites_591%20West%2078th%20St%0d%0aChanhassen%2c%20MN%2055317">Map of Lake Minnetonka Area with Wedding Locations</a><br>
<a class="RegularLink" target="route" href="http://maps.yahoo.com/beta/#maxp=location&q5=I-494,+plymouth,+mn&q4=1989+LAKESIDE+LN,+Mound,+MN,+55364&q3bizid=24294452&q3=2385+Commerce+Blvd,+55364&q2bizid=16981389&q2=591+W+78TH+ST,+Chanhassen,+MN,+55317&q1=US-212,+eden+prairie,+Mn&trf=0&mvt=m&lon=-93.5465240478516&lat=44.9475485338514&mag=6">Driving Routes</a><br>
</td></tr>
<tr><td colspan=2><br>We are registered with: <a href='http://www.target.com/clubwedd/' target='clubwedd' class='RegularLink'>Target</a> and <a href="http://www.crateandbarrel.com/gr/guest/viewRegistry.aspx?grid=375012" target="crateandbarrel" class="RegularLink">Crate & Barrel</a></td></tr>
<tr><td colspan=2><br>Shuttle Information:<br><blockquote>A shuttle will be provided to transport guests from the Country Suites in Chanhassen to the ceremony and reception in Mound, as the reception is in a private neighborhood with street parking only.  The Lee residence can be difficult to find and we would like to minimize cars parked on the neighborhood streets, so guests are encouraged, but are not required, to utilize the shuttle service to avoid getting lost and encountering parking issues in the neighborhood (the city is planning road construction this summer!).  Following the ceremony at Our Lady of the Lake, the shuttles will take guests to the outdoor reception at the Lee residence.  Guests may also choose to drive their own personal vehicles to the ceremony and ride the shuttle from the church to the reception site.  Shuttles will return guests to the church and the Country Suites periodically throughout 
the night.  Street parking at the reception will be available for guests, but the shuttle has been provided for your convenience.  Please indicate if you plan to use the shuttle on the response card.
<br><br>The shuttle(s) will depart Country Suites at approximately 1:45PM, arriving at the church at approximately 2:15PM.
<br><br>Return trips from the reception will commence at approximately 8:00PM and continue hourly throughout the night.</blockquote></td></tr>
<tr><td colspan="2">Weekend Schedule:
<blockquote>Thursday, July 20th:<BR>All day -- Lakeside Lounging<BR>10:00 PM -- Bon fire<BR> <BR>Friday, July 21st:<BR>9:00 AM -- Golf (TBD) or Float at Big Island<BR>4:00 PM -- Rehearsal at Our Lady of the Lake<BR>6:00 PM -- Rehearsal dinner<BR>9:00 PM -- EVERYONE meet at Maynard's!  (see Minneapolis Info Page for <BR>directions)<BR> <BR>Saturday, July 22nd:<BR>1:45 PM -- Shuttle(s) departs Country Suites for Ceremony<BR>2:30 PM -- Ceremony at Our Lady of the Lake<BR>3:30 PM -- Shuttle(s) bring guests to Reception at Lee Residence<BR>4:00 PM -- Cocktail hour (or two)<BR>5:30 PM -- Dinner<BR>7:00 PM -- DANCE!<BR> <BR>Sunday, July 23rd:<BR>10:00 AM -- Lakeside Lounging<BR>
</blockquote></td></tr>
<tr><td colspan=2><br>REMEMBER: This reception is OUTDOORS, so please dress accordingly!</td></tr>
<tr><td colspan=2><a href="wedding_brochure.pdf" class="RegularLink" target="brochure">Click here to read the wedding brochure (pdf format)</a></td></tr>
</table>
<br><br><br>
<%		Case "minneapolis" %>
<table border=0 cellpadding=0 cellspacing=2>
<tr><td colspan="2">Minneapolis is... well, pretty BIG!!  It is one of the so-called Twin Cities of Minneapolis and St. Paul.  Mound is a western suburb of Minneapolis located around Lake Minnetonka.  Chanhassen is a southwestern suburb and is easily accessible from Minneapolis and Mound.  There is PLENTY to do in your spare time in a city of this size.  Here is a brief list of places we go to do what we love best -- EAT AND DRINK!</td></tr>
<tr><td colspan="2"><br>Dining & Nightlife:</td></tr>
<tr><td colspan="2" class="CommentBreak">&nbsp;</td></tr>
<tr><td colspan="2">On the Lake:</td></tr>
<tr><td valign="top"><img src="fletchers.jpg" align=left></td>
<td valign="top">
<li><a class="RegularLink" href="http://www.maynardsonline.com/maynardsexcelsior.htm" target="maynards">Maynards/The Wharf</a> -- only 3.5 miles north of the Country Suites
Waterfront dining, casual outdoor atmosphere.</li>
<li><a class="RegularLink" href="http://www.lordfletchers.com/" target="lord">Lord Fletchers</a> -- we've spent many a summer day here!!!
Waterfront dining and drinks (served in plastic souvenir cups)
Beach volleyball courts; but alas, no more rolly-bowly.
Shirt and shoes barely required for deck patrons, fancier indoor dining also
available.</li>
<li><a class="RegularLink" href="http://www.bayviewevent.com/grille.html" target="bayside">Bayside Grille</a> -- next to Maynards
Waterfront dining and drinks, a little fancier than Maynards.</li></td></tr>
</td></tr>
<tr><td colspan="2" class="CommentBreak">&nbsp;</td></tr>
<tr><td colspan="2">In Chanhassen:</td></tr>
<tr><td valign="top"><img src="chammps.jpg"></td>
<td valign="top">
<li>High Timber Lounge -- they have beer, liquor, and karaoke on Thursdays and Fridays, plus they are 200 feet from your bed... what else could you ask for?</li>
<li>Buffalo Wild Wings -- wear a bib -- finger food at its finest -- Tara suggests Hot BBQ, Goose says Spicy Garlic</li>
<li>Applebees -- no matter what city your in, you can order the Bourbon Street steak and know it's going to be good.  Ask Jeff Wald...</li>
<li>Perkins Restaurant & Bakery -- Just 3 blocks from the hotel and only 3 words required when ordering:<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Chicken Tender Melt<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;OR<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Chicken Teriyaki Bread Bowl Salad, No Tomatos, Fat Free French and Fat Free Ranch on the Side.</li>
<li>Axel's River Grille -- we've never been here, but it was on the hotel's website... Located across the street from the hotel, this casual fine dinning restaurant serves lunch and dinner daily. They specialize in charcoal grilled steaks and seafood. They also have a good selection of pasta, salads and sandwiches. Full service bar.</li>
<li><a class="RegularLink" href="http://www.chanhassentheatres.com/" target="dinnertheatre">Chanhassen Dinner Theatre</a> - check the website for upcoming show times and prices.</li>
</td></tr>
<tr><td colspan="2" class="CommentBreak">&nbsp;</td></tr>
<tr><td colspan="2">In Minnetonka/Plymouth Area:</td></tr>
<tr><td valign="top"><img src="applebees.jpg"></td>
<td valign="top">
<li>Chammps -- Ridgedale Mall, best $9 beers in town.</li>
<li>Applebees -- If the hotel seems too far away, no matter what city your in, you can order the Bourbon Street steak and know it's going to be good.  Ask Jeff Wald...</li>
</td></tr>
<tr><td colspan="2" class="CommentBreak">&nbsp;</td></tr>
<tr><td colspan="2">Other Attractions:</td></tr>
<tr><td valign="top"><img src="mall_of_america.JPG"></td>
<td valign="top">
<li><a class="RegularLink" href="http://www.mallofamerica.com/" target="mallamerica">Mall of America</a> -- Bloomington -- Camp Snoopy for the kids, retail madness for the adults.</li>
<li><a class="RegularLink" href="http://www.valleyfair.com/" target="valleyfair">Valley Fair</a> -- Shakopee -- Disneyland of Minnesota</li>
<li>Downtown Minneapolis -- Nicolette Mall -- outdoor shopping, local music, Missippi River.</li>
</td></tr>
<tr><td colspan="2" class=CommentBreak>&nbsp;</td></tr>
<tr><td colspan="2">
<br>It's a LARGE METROPOLIS with LOTS to do... try:
<a class="RegularLink" href="http://twincities.citysearch.com/" target="msp">http://twincities.citysearch.com/</a><br><br>
</td></tr>
</table>
<%		Case "travel" %>
<table>
<tr><td colspan=2>
Most major airlines fly into the Minneapolis-St Paul International Airport (MSP).<br><br>
</td></tr>
<tr><td colspan=2><br>Hotel Information: (3+ options)</td></tr>
<tr><td colspan=2>A block of rooms has been set aside at the Country Suites in Chanhassen.  Please call the hotel to reserve your room, requesting the Demars/Giesinger Wedding Group.  Rooms will be held until June 30th, 2006.  Lodging alternatives in Chanhassen include the Americ Inn and Holiday Inn Express.</td></tr>
<tr><td colspan=2 class="CommentBreak"></td></tr>
<tr><td colspan=2>1. Country Suites</td></tr>
<tr><td colspan=2>&nbsp;&nbsp;&nbsp;&nbsp;Phone reservations - (952) 937-2424</td></tr>
<tr><td class=CommentText valign=top align=right nowrap></td>
<td class=CommentText>
<a class="RegularLink" href="http://www.chwcms.com/chi/images/hotels/CHANHASS/1RMrevised-012706.gif" target="countryqueen">1 Room Suite with Queen bed and Pull-out Sofa Sleeper</a> Nightly Rate: $89 + tax
</td></tr>
<tr><td class=CommentText valign=top align=right nowrap></td>
<td class=CommentText>
<a class="RegularLink" href="http://www.chwcms.com/chi/images/hotels/CHANHASS/QQ-012706.gif" target="country2queen">1 Room Suite with 2 Queen beds</a> Nightly Rate: $99 + tax
</td></tr>
<tr><td class=CommentText valign=top align=right nowrap></td>
<td class=CommentText>
<a class="RegularLink" href="http://www.chwcms.com/chi/images/hotels/CHANHASS/2RM.gif" target="countryking">1 Room Suite with 1 King bed and separate Living Room with Pull-out Sofa Sleeper</a> Nightly Rate: $109 + tax
</td></tr>
<tr><td class=CommentText valign=top align=right nowrap>Ammenities:</td><td class=CommentText>Complimentary full hot breakfast buffet served daily
Wet Bar with Refrigerator, Microwave and Coffee maker
Indoor heated swimming pool
On-site fitness center
Complimentary Wireless High-Speed Internet Access
Complimentary local telephone calls
Check-in time: 3PM, Check-out time: Noon </td></tr>
<tr><td class=CommentText valign=top align=right nowrap>Summary:</td><td class=CommentText>All rooms are roomy nice suites.</td></tr>
<tr><td colspan=2><br>2. Holiday Inn Express:</td></tr>
<tr><td colspan=2>&nbsp;&nbsp;&nbsp;&nbsp;Please call or visit:
   <a class="RegularLink" href="http://www.ichotelsgroup.com/h/d/ex/1/en/home" target="holidayinn">http://www.ichotelsgroup.com/h/d/ex/1/en/home</a></td></tr>
<tr><td colspan=2>&nbsp;&nbsp;&nbsp;&nbsp;Phone reservations - (952) 401-8849</td></tr>
<tr><td colspan=2><br>3. AmericInn:</td></tr>
<tr><td colspan=2>&nbsp;&nbsp;&nbsp;&nbsp;Please call or visit:
<a class="RegularLink" href="http://www.americinn.com/" target="americinn">http://www.americinn.com/</td></tr>
<tr><td colspan=2>&nbsp;&nbsp;&nbsp;&nbsp;Phone reservations - (952) 934-3888</td></tr>
</table>
<%		Case "songs" %>
<table cellspacing=0 width='100%'>
<tr><td nowrap>&nbsp;Let us know what song you would like to hear/dance to at the rececption:</td></tr>
<tr><td nowrap>&nbsp;Song Name <input class=InputText type=text name="song" size="30"> Your Name <input class=InputText type=text name="requestor" size="30"> <input class=SubButton type=submit value="Submit" onClick="document.WedForm.action='default.asp?section=songs'"></td></tr>
<tr><td nowrap>&nbsp;</td></tr>
<tr><td nowrap class=CommentBreak>&nbsp;</td></tr>
<tr><td align=center>
<table width='100%'>
<tr><td align=center class=SelHeaderText>Song Requested</td><td align=center class=SelHeaderText>Requested By</td>
<td bgcolor='#A93238'>&nbsp;&nbsp;</td>
<td align=center class=SelHeaderText>Song Requested</td><td align=center class=SelHeaderText>Requested By</td></tr>
<%			x = 0

			Do While x <= UBound(aFileData)
				If Trim(aFileData(x)) <> "" Then %>
<tr>
<td class=HeaderText><%=Left(aFileData(x), InStr(aFileData(x), "||") - 1)%></td>
<td class=HeaderText><%=Mid(aFileData(x), InStr(aFileData(x), "||") + 2)%></td>
<td bgcolor='#A93238'>&nbsp;&nbsp;</td>
<%				End If

				x = x + 1

				If x > UBound(aFileData) Then
					If Trim(aFileData(x-1)) <> "" Then %>
<td class=HeaderText>&nbsp;</td>
<td class=HeaderText>&nbsp;</td>
</tr>
<%					End If

					Exit Do
				End If

				If Trim(aFileData(x)) <> "" Then %>
<td class=HeaderText><%=Left(aFileData(x), InStr(aFileData(x), "||") - 1)%></td>
<td class=HeaderText><%=Mid(aFileData(x), InStr(aFileData(x), "||") + 2)%></td>
</tr>
<%				Else %>
<td class=HeaderText>&nbsp;</td>
<td class=HeaderText>&nbsp;</td>
</tr>
<%				End If

				x = x + 1
			Loop %>
</table>
</td></tr>
</table>
<%		Case "comments" %>
<table cellspacing=0 width='100%'>
<tr><td>&nbsp;</td><td nowrap colspan=2>&nbsp;Any comments/advice/words of wisdom/jokes for us?</td></tr>
<tr><td>&nbsp;</td><td nowrap valign=top align=right>Your Name&nbsp;</td><td><input class=InputText type=text name="guest" size="30"></td></tr>
<tr><td>&nbsp;</td><td nowrap valign=top align=right>Comments&nbsp;</td><td nowrap><textarea class=InputText name="guestcomment" rows=5 cols=60></textarea> <input class=SubButton type=submit value="Submit" onClick="document.WedForm.action='default.asp?section=comments'"></td></tr>
<tr><td nowrap colspan=3 class=CommentBreak>&nbsp;</td></tr>
<%			For x = 0 To UBound(aFileData)
				If Trim(aFileData(x)) <> "" Then %>
<tr><td class=CommentText<%=SetVal(x mod 2, 0, "", "Green")%>>&nbsp;</td>
<td class=CommentText<%=SetVal(x mod 2, 0, "", "Green")%>>
<u><%=Left(aFileData(x), InStr(aFileData(x), "||") - 1)%></u></td>
<td class=CommentText<%=SetVal(x mod 2, 0, "", "Green")%> align=right><u><small><%=Mid(aFileData(x), InStr(aFileData(x), "||") + 2, InStrRev(aFileData(x), "||") - InStr(aFileData(x), "||") - 2)%></small></u>&nbsp;&nbsp;&nbsp;
</td></tr>
<td class=CommentText<%=SetVal(x mod 2, 0, "", "Green")%>>&nbsp;</td>
<td colspan=2 class=CommentText<%=SetVal(x mod 2, 0, "", "Green")%>>
<%=Mid(aFileData(x), InStrRev(aFileData(x), "||") + 2)%><br>
</td></tr>
<tr><td nowrap colspan=3 class=CommentBreak>&nbsp;</td></tr>
<%				End If
			Next %>
</table>
<%		Case "gallery"
			Dim sImage, sNext, sPrev
			sImage = Request("image")

			If sImage = "" Then
				sImage = "01"
				sNext = "02"
				sPrev = ""
			Else
				sNext = CInt(sImage) + 1
				sPrev = CInt(sImage) - 1

				If sNext = 54 Then
					sNext = ""
				ElseIf sNext < 10 Then
					sNext = "0" & CStr(sNext)
				End If

				If sPrev = 0 Then
					sPrev = ""
				ElseIf sPrev < 10 Then
					sPrev = "0" & CStr(sPrev)
				End If
			End If %>
<center>
<%			If sPrev <> "" Then %>
<a class=RegularLink href="default.asp?section=gallery&image=<%=sPrev%>">&lt;&lt;Prev</a>&nbsp;&nbsp;&nbsp;&nbsp;
<%			Else %>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<%			End If

			If sNext <> "" Then %>
<a class=RegularLink href="default.asp?section=gallery&image=<%=sNext%>">Next&gt;&gt;</a>
<img height=0 width=0 src="gallery/<%=sNext%>.jpg">
<%			Else %>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<%			End If %>
<br><br><img src="gallery/<%=sImage%>.jpg" style="border:double 8px #A93238"><br>
</center>
<%		Case "listattendees"
			Set oFS = Server.CreateObject("Scripting.FileSystemObject")
			Set oFile = oFS.OpenTextFile(Server.MapPath(".\attending.txt"), , True)

			If Not oFile.AtEndOfStream Then
				sText = oFile.ReadAll
			Else
				sText = ""
			End If

			Response.Write "<pre>" & sText & "</pre>"
	End Select %>
</form>
</body>
</html>



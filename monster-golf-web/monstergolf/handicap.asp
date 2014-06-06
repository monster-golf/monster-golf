<%	Option Explicit
	Response.Buffer  = True
	Response.ExpiresAbsolute=#March 20,2000 00:00:00# %>
<!-- #include file='includes\db.asp' -->
<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\handicapinclude.asp' -->
<!-- #include file='includes\handicapui.asp' -->
<!-- #include file='includes\groupui.asp' -->
<%
MainHeader "Handicap"

If Request("Logout") = "True" Then
	Session("UserID") = ""
	
	If Session("AllUser") <> "" Then %>
<script>document.location.href='allhandicap.asp';</script>
</body>
</html>
<%	End If
End If

If Request.QueryString("NewUser") = "True" Then
	Response.Redirect("handicap.asp")
End If

Dim oConn, oRS, strAlreadyAUser, strCurrHandicap
Set oConn = GetConnection
Set g_oConn = oConn
strAlreadyAUser = ""

SubmitEdits Request("SubmitEdits"), Request("RemoveScore"), Request("EditScore"), Session("UserID"), Request("EditHandicap")
SubmitFriendScore Request("SubmitFriendScore"), Request("PostFriend")

If Request("AddUser") = "True" Then
	If Session("UserID") = "" Then
		Set oRS = GetRecords(oConn, "SELECT * FROM MG_Users WHERE UserName LIKE '" & Trim(Request("UserName")) & "' AND Email LIKE '" & Trim(Request("Email")) & "'")
		
		If Not oRS.EOF Then
		   strAlreadyAUser = "You already have an account so i have logged you in.<br>"
			Session("UserID") = oRS("UserID")
			strCurrHandicap = oRS("Handicap")
		Else
			GetRecords oConn, "INSERT INTO MG_Users (UserName, Email, FirstName, LastName) VALUES ('" & Request("UserName") & "', '" & Request("Email") & "', '" & Request("FirstName") & "', '" & Request("LastName") & "')"
			Set oRS = GetRecords(oConn, "SELECT * FROM MG_Users WHERE UserID = (SELECT Max(UserID) FROM MG_Users)")
			Session("UserID") = oRS("UserID")
         strCurrHandicap = oRS("Handicap")
		End If
	Else
		GetRecords oConn, "Update MG_Users Set UserName = '" & Request("UserName") & "', Email = '" & Request("Email") & "', FirstName = '" & Request("FirstName") & "', LastName = '" & Request("LastName") & "', GHIN = '" & Request("GHIN") & "', MobileEmail = '" & Request("MobileEmail") & "' WHERE UserID = " & Session("UserID")
		Set oRS = GetRecords(oConn, "SELECT * FROM MG_Users WHERE UserID = " & Session("UserID"))
	End If
ElseIf Request("FileName") <> "" Then
	Set oRS = GetRecords(oConn, "SELECT * FROM MG_Users WHERE UserName LIKE '" & Trim(Request("FileName")) & "' AND Email LIKE '" & Trim(Request("Email")) & "'")
	
	If Not oRS.EOF Then
		Session("UserID") = oRS("UserID")
      strCurrHandicap = oRS("Handicap")
	End If
ElseIf Session("AllUser") <> "" And Request("UserID") <> "" Then
	Set oRS = GetRecords(oConn, "SELECT * FROM MG_Users WHERE UserID = " & Trim(Request("UserID")))
	
	If Not oRS.EOF Then
		Session("UserID") = oRS("UserID")
      strCurrHandicap = oRS("Handicap")
	End If
ElseIf Session("UserID") <> "" Then
	Set oRS = GetRecords(oConn, "SELECT * FROM MG_Users WHERE UserID = " & Trim(Session("UserID")))
	strCurrHandicap = oRS("Handicap")
End If

If Session("UserID") = "" Or Request.Form("NewUser") = "True" Then %>
<script>
	function CheckInput() {
		var strErr = '';
		var re = / /gi;

		if (document.GetUser.FileName.value.replace(re,'') == '')
			strErr = strErr + '\n\tEnter your User Name.';

		if (document.GetUser.Email.value.replace(re,'') == '')
			strErr = strErr + '\n\tEnter your Email Address.';
		
		if (strErr == '') {
			return true;
		} else {
			alert('Please fix the following problems:' + strErr);
			return false;
		}	
	}

	function CheckNewUser() {
		var strErr = '';
		var re = / /gi;

		if (document.GetUser.FirstName.value.replace(re,'') == '')
			strErr = strErr + '\n\tEnter your First Name.';

		if (document.GetUser.LastName.value.replace(re,'') == '')
			strErr = strErr + '\n\tEnter your Last Name.';

		if (document.GetUser.UserName.value.replace(re,'') == '')
			strErr = strErr + '\n\tEnter your User Name.';

		if (document.GetUser.Email.value.replace(re,'') == '')
			strErr = strErr + '\n\tEnter your Email Address.';
		
		if (strErr == '') {
			return true;
		} else {
			alert('Please fix the following problems:' + strErr);
			return false;
		}	
	}
	
	function GoNew() {
		document.GetUser.NewUser.value = 'True';
		document.GetUser.submit();
	}
</script>
<form name=GetUser method=Post action='handicap.asp'>
<input type=hidden name=NewUser value=''>
<center>
<%	If Request.Form("NewUser") = "True" Then %>
<table>
<%		If Session("UserID") <> "" Then %>
<tr><td align=right>First Name</td><td><input type=text name=FirstName value='<%=oRS("FirstName")%>'></td><td align=right>Last Name</td><td><input style='width:170px' type=text name=LastName value='<%=oRS("LastName")%>'></td></tr>
<tr><td align=right>User Name</td><td><input type=text name=UserName value='<%=oRS("UserName")%>'></td><td align=right>Email</td><td><input style='width:170px' type=text name=Email value='<%=oRS("Email")%>'></td></tr>
<tr><td align=right>Your GHIN #</td><td><input type=text name=GHIN value='<%=oRS("GHIN")%>'></td><td align=right>Mobile Email</td><td><input style='width:170px' type=text name=MobileEmail value='<%=oRS("MobileEmail")%>'></td></tr>
<%		Else %>
<tr>
<td align=center colspan=4>
This form is to create a new account.<br>If you already have an account please click <a href='handicap.asp'>here</a> and log in.<br>
If you forgot your login information please send an email to:<br><a href='mailto:monster@monstergolf.org'>monster@monstergolf.org</a><br><br>
</td></tr>
<tr><td align=right>First Name</td><td><input type=text name=FirstName value=''></td><td align=right>Last Name</td><td><input type=text name=LastName value=''></td></tr>
<tr><td align=right>User Name</td><td><input type=text name=UserName value=''></td><td align=right>Email</td><td><input type=text name=Email value=''>
<%		End If %>
</td></tr></table>
<input type=hidden name=AddUser value='True'>
<input type=submit name=SubUser value='Enter' onClick='return CheckNewUser()'><br><br><br>
<%	Else
		If Request("FileName") <> "" Then %>
Invalid user name or email.<br><br>
<%		End If %>
User Name: <input type=text name=FileName value=''><br>
Email Address: <input type=text name=Email value=''><br>
<input type=submit name=SubUser value='Enter' onClick='return CheckInput()'><br><br><br>
<a class='PageLink' href='javascript:GoNew()'>Click here to sign up for Monster handicapping.</a><br><br>
<center>
</form>
<%	End If
Else
	Dim x, strEmail, strPath, strHandicapTrend
	
	g_lNumScores = 0
	strHandicapTrend = GetHandicapRS(oConn, Session("UserID"))
	
	Dim strLastIndex, strLastSlope, strLastCourse
	
	strLastIndex = ""
	strLastSlope = ""
	
	If g_lNumScores > 0 Then
		strLastIndex = g_aAllVals(0, cIndex)
		strLastSlope = g_aAllVals(0, cSlope)
		strLastCourse = g_aAllVals(0, cCName)
	End If %>
<script>
	function ValidateDate(dateStr) {
		if (dateStr=='') {
			return true;
		}
		
		if ( dateStr.indexOf(' ') > 0 )
		{
			dateStr = dateStr.substring(0,dateStr.indexOf(' '));
		}

		var ParseStr = /^(\d{1,2})(\/|-)(\d{1,2})\2(\d{4})$/;
		var matchArray = dateStr.match(ParseStr);
		if (matchArray == null) {
			return false;
		}
		month = matchArray[1];
		day = matchArray[3];
		year = matchArray[4];
		currentdate=new Date();
		currentyear=currentdate.getFullYear();

		if (isNaN(month)) return false;
		if (isNaN(day)) return false;
		if (isNaN(year)) return false;

		if (month < 1 || month > 12) {
			return false;
		}
		if ((year==0000) || (year < (currentyear - 100)) || (year > (currentyear + 100))) {
			return false;
		}
		if (day < 1 || day > 31) {
			return false;
		}
		if ((month==4 || month==6 || month==9 || month==11) && day==31) {
			return false;
		}
		if (month == 2) {
			var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
			if (day>29 || (day==29 && !isleap)) {
				return false;
			}
		}
		return true;
	}

	function CheckInput() {
		var strErr = '';
		var hasonescore = false;
		var score;

      if (document.GetScore.PostFriend != null) {
         if (document.GetScore.PostFriend.length != null) {
            for (x=0;x<document.GetScore.PostFriend.length;x++) {
               score = eval('document.GetScore.PostScore' + document.GetScore.PostFriend[x].value);
               if (score.value != '') {
                  hasonescore = true;
		            if (isNaN(score.value))
			            strErr = strErr + '\n\tInvalid Score, you must post at lease one valid score.';
		            else if (score.value < 56 || score.value > 180)
			            strErr = strErr + '\n\tInvalid Score, you must post at lease one valid score.';
			      }
            }
         }
         else {
            score = eval('document.GetScore.PostScore' + document.GetScore.PostFriend.value);
            if (score.value != '') {
               hasonescore = true;
		         if (isNaN(score.value))
			         strErr = strErr + '\n\tInvalid Score, you must post at lease one valid score.';
		         else if (score.value < 56 || score.value > 180)
			         strErr = strErr + '\n\tInvalid Score, you must post at lease one valid score.';
			   }
         }
      }
      
      score = document.GetScore.Score;
      
      if (score.value != '') {
         hasonescore = true;
		   if (isNaN(score.value))
			   strErr = strErr + '\n\tInvalid Score, you must post at lease one valid score.';
		   else if (score.value < 56 || score.value > 180)
			   strErr = strErr + '\n\tInvalid Score, you must post at lease one valid score.';
		} else if (!hasonescore)
			   strErr = strErr + '\n\tInvalid Score, you must post at lease one valid score.';
		
		if (isNaN(document.GetScore.Index.value) || document.GetScore.Index.value == '')
			strErr = strErr + '\n\tInvalid Course Rating';
		else if (document.GetScore.Index.value < 56 || document.GetScore.Index.value > 100)
			strErr = strErr + '\n\tInvalid Course Rating';
		
		if (isNaN(document.GetScore.Slope.value) || document.GetScore.Slope.value == '')
			strErr = strErr + '\n\tInvalid Course Slope';
		else if (document.GetScore.Slope.value < 56 || document.GetScore.Slope.value > 160)
			strErr = strErr + '\n\tInvalid Course Slope';

		if (!ValidateDate(document.GetScore.Date.value) || document.GetScore.Date.value == '')
			strErr = strErr + '\n\tInvalid Date of Round';
		else {
			theDate = new Date("<%=Date%>")
			myDate = new Date(document.GetScore.Date.value)
			if (myDate > theDate)
				strErr = strErr + '\n\tInvalid Date of Round, date must be today or on a previous day.';
		}

		if (strErr == '') {
			return true;
		} else {
			alert('Please fix the following problems:' + strErr);
			return false;
		}	
	}

	function CheckEdit(sID) {
		if (document.GetScore.EditScore != null) {
			if (document.GetScore.EditScore.length != null) {
				for (var x=0; x < document.GetScore.EditScore.length; x++) {
					if (document.GetScore.EditScore[x].value == sID) {
						document.GetScore.EditScore[x].checked = true;
						break;
					}
				}
			} else {
				if (document.GetScore.EditScore.value == sID)
					document.GetScore.EditScore.checked = true;
			}
		}
	}
	function formulaHandi(userID, slope)
	{
	   var handival = new Number(eval("document.GetScore.currhandi" + userID + ".value"));
	   var formcoursehandi = eval("document.GetScore.coursehandi" + userID);
      var coursehandi = Math.round((handival*slope)/113);
      formcoursehandi.value = coursehandi;
	}
	function doHandi() {
	  if (document.GetScore.courseslope.value.length > 1) {
		   if (document.GetScore.courseslope.value.length > 3)
			   document.GetScore.courseslope.value = document.GetScore.courseslope.value.substr(0, 3);
		   
		   var slope = new Number(document.GetScore.courseslope.value);
		   
		   if (document.GetScore.handiuserid.length != null) {
		      for (var x=0; x<document.GetScore.handiuserid.length; x++)
		         formulaHandi(document.GetScore.handiuserid[x].value, slope);
		   }
		   else
            formulaHandi(document.GetScore.handiuserid.value, slope);
		}
	}
</script>
<form name='GetScore' action='handicap.asp' method='post'>
<input type=hidden name=Email value='<%=strEmail%>'>
<input type=hidden name=LogOut value=''>
<input type=hidden name=NewUser value=''>
<table align=center border=1 bordercolor=black cellspacing=0 cellpadding=5>
<tr><td colspan=3>
<% HeaderDisplay strCurrHandicap, strHandicapTrend, strAlreadyAUser, oRS("FirstName"), oRS("LastName"), oRS("UserName"), oRS("Email"), Session("AllUser") %>
</td></tr>
<tr>
<td valign="top">
<% HandicapForm strLastIndex, strLastSlope, SetVal(Request("Date"), "", Date, Request("Date")), strLastCourse %>
<% HandicapInfo %>
</td>
<td valign="top">
<% ScoreList g_lNumScores, g_aAllVals, Session("AllUser")
   Statistics Session("UserID") %>
</td>
<td valign="top">
<% RemoveFriend Request("removefriend"), Session("UserID")
   AddFriend Request("FriendID"), Session("UserID")
   FriendList Session("UserID") %>
</td>
</tr>
</table>
</form>
<%
End If

MainFooter "Home"
oConn.Close %>

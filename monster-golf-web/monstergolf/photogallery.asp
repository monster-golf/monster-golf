<!-- #include file='includes\db.asp' -->
<!-- #include file='includes\include.asp' -->
<%	MainHeader "Photos"
	
	If Request("FileName") <> "" Then
      Dim oConn, oRS
      Set oConn = GetConnection
	   Set oRS = GetRecords(oConn, "SELECT * FROM MG_Users WHERE UserName LIKE '" & Trim(Request("FileName")) & "' AND Email LIKE '" & Trim(Request("Email")) & "'")
   	
	   If Not oRS.EOF Then
		   Session("UserID") = oRS("UserID")
		Else
		   Response.Write("<center><b><br/>Invalid login please try again.</b></center>")
	   End If
	End If
	
	If Session("UserID") = "" Then %>
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
</script>
<form name=GetUser method=Post action='photogallery.asp'>
<center>
<br/>Please login with your Monster handicap login to view the Monster Gallery<br/>
User Name: <input type=text name=FileName value=''><br>
Email Address: <input type=text name=Email value=''><br>
<input type=submit name=SubUser value='Enter' onClick='return CheckInput()'><br><br><br>
</center>
</form>
<% Else
	If Request("Year") = "" Then %>
<center>
<br>Select a Year<br>
<a class=PageLink href='photogallery.asp?Year=2008'>2008 new photos!</a><br>
<a class=PageLink href='photogallery.asp?Year=2007'>2007</a><br>
<a class=PageLink href='photogallery.asp?Year=2006'>2006</a><br>
<a class=PageLink href='photogallery.asp?Year=2005'>2005</a><br>
<a class=PageLink href='photogallery.asp?Year=2004'>2004</a><br>
<a class=PageLink href='photogallery.asp?Year=2003'>2003</a><br>
<a class=PageLink href='photogallery.asp?Year=2002'>2002</a><br>
<a class=PageLink href='photogallery.asp?Year=2001'>2001 new photos!</a><br>
<a class=PageLink href='photogallery.asp?Year=2000'>2000</a><br>
<a class=PageLink href='photogallery.asp?Year=1999'>1999 new photos!</a><br>
</center>
<%	Else %>
<script>
function GoNext() {
	document.imgForm.submit();
}
function GoPrev() {
	document.imgForm.PrevImage.value = 'True';
	document.imgForm.submit();
}
</script>
<form name='imgForm' method=Post action='photogallery.asp'>
<input type=hidden name='PrevImage' value=''>
<input type=hidden name='Year' value='<%=Request("Year")%>'>
<center>
Photo Gallery <%=Request("Year")%><br><br> 	
<%		Set objFSO = Server.CreateObject("Scripting.FileSystemObject")
		Set objFolder = objFSO.GetFolder(Server.MapPath(".") & "\" & Request("Year") & "\")
		Set objFiles = objFolder.Files
		
		Dim imgTotalCount, imgCount
		
		If Request("imgTotalCount") = "" Then
			imgTotalCount = 0
			
			For Each objFile In objFiles
			   If LCase(Right(objFile.Name, 3)) = "jpg" Then
					imgTotalCount = imgTotalCount + 1
				End If
			Next
		Else
			imgTotalCount = Request("imgTotalCount")
		End If
		
		If Request("imgCount") = "" Then
			imgCount = 1
		Else
			imgCount = Request("imgCount")
			
			If Request("PrevImage") = "True" Then
				imgCount = imgCount - 1
			Else
				imgCount = imgCount + 1
			End If
		End If
      
		x = 0
		prevImage = ""
		nextImage = ""
		bFound = False
		bWroteImg = False %>
<%=imgCount%> of <%=imgTotalCount%><br>
<input type=hidden name='imgCount' value='<%=imgCount%>'>
<input type=hidden name='imgTotalCount' value='<%=imgTotalCount%>'>
<%		If CInt(imgCount) = 1 Then %>
&lt;&lt;Prev &nbsp; <a class='PageLink' href='javascript:GoNext()'>Next&gt;&gt;</a><br><br>
<%		ElseIf CInt(imgCount) = CInt(imgTotalCount) Then %>
<a class='PageLink' href='javascript:GoPrev()'>&lt;&lt;Prev</a> &nbsp; Next&gt;&gt;<br><br>
<%		Else %>
<a class='PageLink' href='javascript:GoPrev()'>&lt;&lt;Prev</a> &nbsp; <a class='PageLink' href='javascript:GoNext()'>Next&gt;&gt;</a><br><br>
<%		End If
		
		For Each objFile In objFiles
		   If LCase(Right(objFile.Name, 3)) = "jpg" Then
			If bWroteImg Then
				nextImage = objFile.Name
				Exit For
			End If

			If Request("LastImage") = "" Then
				Response.Write("<img style='border:2px black solid' src='" & Request("Year") & "/" & objFile.Name & "'><input type=hidden name='LastImage' value='" & objFile.Name & "'>")
				bWroteImg = True
			ElseIf bFound Then
				Response.Write("<img style='border:2px black solid' src='" & Request("Year") & "/" & objFile.Name & "'><input type=hidden name='LastImage' value='" & objFile.Name & "'>")
				bWroteImg = True
			ElseIf objFile.Name = Request("LastImage") Or bFound Then
				bFound = True
				If Request("PrevImage") <> "" Then
					If prevImage <> "" Then
						bWroteImg = True
						Response.Write("<img style='border:2px black solid' src='" & Request("Year") & "/" & prevImage & "'><input type=hidden name='LastImage' value='" & prevImage & "'>")
					Else
						bWroteImg = True
						Response.Write("<img style='border:2px black solid' src='" & Request("Year") & "/" & objFile.Name & "'><input type=hidden name='LastImage' value='" & objFile.Name & "'>")
					End If
				End If
			End If

			prevImage = objFile.Name
		   End If
		Next 
		
		If Not bWroteImg Then
			Response.Write("<img style='border:2px black solid' src='" & Request("Year") & "/" & prevImage & "'><input type=hidden name='LastImage' value='" & prevImage & "'>")
		End If %>
</center>
<%	If nextImage <> "" Then %>
<img height="0" width="0" src='<%=Request("Year") & "/" & nextImage%>'>
<%	End If  %>
</form>
<% End If

 End If
 MainFooter "Photos" %>

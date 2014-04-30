<%
Function GetConnection()
	Dim sConn
	'sConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=c:\Websites\data\monsterco.mdb"
	'sConn = "Provider=SQLOLEDB;Persist Security Info=False;User ID=aaronwa_monster;Password=aaron1;Initial Catalog=aaronwa_monsterco;Data Source=mssql7.hsphere.cc;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=WALDO;Use Encryption for Data=False;Tag with column collation when possible=False"
	sConn = "Provider=SQLOLEDB;Persist Security Info=False;User ID=monstergolf;Password=M0nS+0n@!n;Initial Catalog=monstergolf;Data Source=monstergolf.db.6071781.hostedresource.com;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=WALDO;Use Encryption for Data=False;Tag with column collation when possible=False"
	'sConn = "Data Source=.\SQLEXPRESS;AttachDbFilename=C:\WebSites\data\monsterco.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True"
	Set GetConnection = Server.CreateObject("ADODB.Connection")
	GetConnection.Open sConn, "", "", -1
End Function

Function GetRecords(oConn, sSQL)
	Set GetRecords = oConn.Execute(sSQL, -1, -1)
End Function
%>
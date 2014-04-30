<!-- #include file='includes\include.asp' -->
<!-- #include file='includes\dbresults.asp' -->
<!-- #include file='includes\funcs.asp' -->

<br>
<center>
<br />

<a class=PageLink href='lowscore.asp?HeaderSelect=YearsStats&type=avegross&year=<%=Request("year")%>'><%=Request("year")%> Ave Low Gross</a><br>
<a class=PageLink href='lowscore.asp?HeaderSelect=YearsStats&type=avenet&year=<%=Request("year")%>'><%=Request("year")%> Ave Low Net</a><br>
<a class=PageLink href='lowscore.asp?HeaderSelect=YearsStats&type=gross&year=<%=Request("year")%>'><%=Request("year")%> Low Gross</a><br>
<a class=PageLink href='lowscore.asp?HeaderSelect=YearsStats&type=net&year=<%=Request("year")%>'><%=Request("year")%> Low Net</a><br>
<br>
</center>


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="contact.aspx.cs" Inherits="Contact" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>Contact Aaron Wald</title>
<link rel="stylesheet" type="text/css" href="styles.css" />
<script>
function SaveContact() {
   document.getElementById("bodyContact").style.cursor = "wait";
   document.getElementById("saveMessage").style.display = "";
   AjaxContact.SaveContact(document.aaronwald.company.value, 
      document.aaronwald.firstName.value, 
      document.aaronwald.lastName.value, 
      document.aaronwald.phone.value, 
      document.aaronwald.email.value, 
      document.aaronwald.referredBy.value, 
      document.aaronwald.timeToCall.value, 
      document.aaronwald.description.value,
      function (res) {
         if (res.error) alert(res.error);
         document.getElementById("saveMessage").innerHTML = res.value;
         document.getElementById("bodyContact").style.cursor = "default";
      });
}
function CloseSaveMessage() {
   document.getElementById("saveMessage").innerHTML = "Saving contact information please wait...";
   document.getElementById("saveMessage").style.display = "none";
}
function CheckLength(formvar, totallength) {
   if (formvar.value.length > totallength) {
      alert(document.getElementById(formvar.id).getAttribute('title') + ' can only be ' + totallength + ' characters long.');
      formvar.value = formvar.value.substr(0,totallength);
      formvar.focus();
   }
   document.getElementById(formvar.id + 'Count').innerHTML = formvar.value.length;
}
</script>
</head>
<body topmargin="0" onload="document.aaronwald.company.focus();" id="bodyContact">
<form id="aaronwald" method="post" runat="server">
<div class="promise" id="saveMessage" style="position:absolute;padding:20px 20px 20px 20px;top:130px;left:110px;height:450px;width:500px;background:#9bd7d3;border:solid 5px #95673f;display:none;">
Saving contact information please wait...
</div>
<table cellpadding="0" cellspacing="0">
<tr>
<td style="background:url(images/leftbarmiddle.jpg) repeat-y top left;width:80px;height:100%;">&nbsp;</td>
<td>
<br />
<table cellpadding="0" cellspacing="0" width="97%" bgcolor="white">
<tr>
   <td valign="top" style="border-top:solid 5px #95673f;" class="blank">&nbsp;</td>
   <td valign="top"><img src="images/topright.gif"></td>
</tr>
<tr>
   <td valign="top" style="padding-left:30px;" nowrap class="description">
      <font class="sections">-Your Contact Information-</font><br /><br />
      Please fill out as much or as little information as you please. I will contact<br />you at your convenience.
      What you provide will only be available to me<br />and I will not pass it on to anyone else unless requested by you.
      <table cellpadding="0" cellspacing="0">
      <tr><td colspan="3" class="blank">&nbsp;</td></tr>
      <tr>
         <td class="description" nowrap valign="top" style="text-align:right;padding-top:3px;">Company&nbsp;</td>
         <td class="inputsback"><input type="text" name="company" class="inputs" maxlength="75" size="30" title="First Name" /></td>
      </tr>
      <tr><td colspan="3" class="blank">&nbsp;</td></tr>
      <tr>
         <td class="description" nowrap valign="top" style="text-align:right;padding-top:3px;">First Name&nbsp;</td>
         <td class="inputsback"><input type="text" name="firstName" class="inputs" maxlength="50" size="30" title="First Name" /></td>
      </tr>
      <tr><td colspan="3" class="blank">&nbsp;</td></tr>
      <tr>
         <td class="description" nowrap valign="top" style="text-align:right;padding-top:3px;">Last Name&nbsp;</td>
         <td class="inputsback"><input type="text" name="lastName" class="inputs" maxlength="50" size="30" title="Last Name" /></td>
      </tr>
      <tr><td colspan="3" class="blank">&nbsp;</td></tr>
      <tr>
         <td class="description" nowrap valign="top" style="text-align:right;padding-top:3px;">Phone Number&nbsp;</td>
         <td class="inputsback"><input type="text" name="phone" class="inputs" maxlength="25" size="30" title="Phone Number" /></td>
         <td class="descriptionsmall" nowrap valign="top" style="padding-top:3px;padding-left:5px;">(999) 555-5555 x1234</td>
      </tr>
      <tr><td colspan="3" class="blank">&nbsp;</td></tr>
      <tr>
         <td class="description" nowrap valign="top" style="text-align:right;padding-top:3px;">Email&nbsp;</td>
         <td class="inputsback"><input type="text" name="email" class="inputs" maxlength="50" size="30" title="Email" /></td>
         <td class="descriptionsmall" nowrap valign="top" style="padding-top:3px;padding-left:5px;">email@domain.ext</td>
      </tr>
      <tr><td colspan="3" class="blank">&nbsp;</td></tr>
      <tr>
         <td class="description" nowrap valign="top" style="text-align:right;padding-top:3px;">Referred By&nbsp;</td>
         <td class="inputsback"><input type="text" name="referredBy" class="inputs" maxlength="75" size="30" title="Referred By" /></td>
         <td class="descriptionsmall" nowrap valign="top" style="padding-top:3px;padding-left:5px;">Who told you about me or where did you find me?</td>
      </tr>
      <tr><td colspan="3" class="blank">&nbsp;</td></tr>
      <tr><td class="description" nowrap colspan="3" valign="top">Best Time to Contact You</td></tr>
      <tr>
         <td class="inputsback" colspan="2"><textarea name="timeToCall" id="timeToCall" class="inputs" style="width:321px;height:65px;" title="Best Time to Contact You" onkeyup="CheckLength(this, 250);"></textarea></td>
         <td class="descriptionsmall" nowrap valign="top" style="padding-top:3px;padding-left:5px;">
            Date(s) and Time(s) when you will be available<br />for an approximately 1 hour conversation.
            <br /><br />
            <span id="timeToCallCount" style="display:inline">0</span> characters of 250 maximum.
         </td>
      </tr>
      <tr><td colspan="3" class="blank">&nbsp;</td></tr>
      <tr><td class="description" nowrap colspan="3" valign="top">Brief Project Description</td></tr>
      <tr>
         <td class="inputsback" colspan="2"><textarea name="description" id="description" class="inputs" style="width:321px;height:100px;" title="Brief Project Description" onkeyup="CheckLength(this, 500);"></textarea></td>
         <td class="descriptionsmall" nowrap valign="top" style="padding-top:3px;padding-left:5px;">
            Supply some information about what business<br />functions you are having problems with.<br />Are you thinking about an internet (customer facing)<br /> or intranet (employee management/internal tools)?
            <br /><br />
            <span id="descriptionCount" style="display:inline">0</span> characters of 500 maximum.
         </td>
      </tr>
      <tr><td colspan="3" class="blank">&nbsp;</td></tr>
      <tr>
         <td class="description" nowrap valign="top" style="text-align:right;padding-top:3px;">&nbsp;</td>
         <td class="inputsback"><input type="button" name="btnSave" class="inputs" title="Submit Your Information" style="cursor:pointer;width:225px;" value="Submit Information" onclick="SaveContact();" /></td>
      </tr>
      </table>
      <br /><br />Or, you can call (425) 591-6584, email: &nbsp;<a class="contact" href="mailto:aaron@aaronwald.net">aaron@aaronwald.net</a><br /><br />
      <a href="index.htm" class="promise">-Click to go Back-</a><br />
   </td>
   <td valign="top" style="border-right:solid 5px #95673f;width:30px;">&nbsp;</td>
</tr>
<tr>
   <td valign="top" style="border-bottom:solid 5px #95673f;" class="blank" align="center">&nbsp;</td>
   <td valign="top"><img src="images/bottomright.gif"></td>
</tr>
</table>
<br />
</td>
</tr>
</table>
</form>
</body>
</html>

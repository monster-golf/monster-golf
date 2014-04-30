<%@ page language="C#" autoeventwireup="true" inherits="TheDefault, App_Web_default.aspx.cdcab7d2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Patty Giesinger</title>
    <style>
    body,html {
      font-size:18px;
      font-family:helvetica;
      background:#d1f2fc;
    }
    .thanks {
      font-size:25px;
    }
    a {
      font-size:16px;
      color:blue;
    }
    </style>    
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel style="padding-bottom:20px;" ID="upform" runat="server">
    We need your Patty Giesinger 60th Birthday Greetings.<br/><br/>
    Click 'Browse' to select your file, then click 'Upload Movie'.<br/><br/>
    <asp:FileUpload ID="moveUp" runat="server" /> : Maximum size 25MB<br/>
    <asp:Button ID="btnMoveUp" runat="server" Text="Upload Movie" OnClientClick="this.disabled=true;document.form1.submit();" />
    </asp:Panel>
    <asp:Panel style="padding-bottom:20px;" ID="upthanks" runat="server" CssClass="thanks">
    Thank you for your contribution!<br/>
    <asp:LinkButton ID="another" runat="server" Text="I have another file to give." OnClick="another_click" />
    </asp:Panel>
    <img src="mamag.jpg" />
    <img src="mamag1.jpg" />
    <img src="mamag2.jpg"/>
    </form>
</body>
</html>

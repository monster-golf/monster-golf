<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Results.aspx.cs" Inherits="Results" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Scoring View</title>
<style type="text/css">
body{font-family:sans-serif, arial; margin:10px;font-size:12px;background-color:#1F58AE; color:#ffffff;}
a {color:#ffffff; margin:5px;}
a:hover { color:#FF3F19;}
.ScoreCell {white-space:nowrap; padding: 4px;}
.HeaderScoreCell {white-space:nowrap; padding: 4px;background-color:Yellow;font-weight:bold;}
.GridSortLink,.GridSortLink:Hover,.GridSortLink:Visited{color:black;text-decoration:underline;}
</style>
<script type="text/javascript" language="javascript" src="XmlHttp.js"></script>
<script language="javascript" type="text/javascript">
    xmlurl = "TourneyXml.aspx";
    function EmailTo() {
        var o = document.getElementById("divEmail");
        if (o) o.style.display = (o.style.display == "") ? "none" : "";
    }
    function EmailResults() {
        var o = document.getElementById("hdnTourneyId");
        var resUrl = "";
        if (o) resUrl = "&t=" + o.value;
        o = document.getElementById("ddScoring");
        if (o && o.selectedIndex > 0) resUrl += "&so=" + o[o.selectedIndex].value;
        o = document.getElementById("ddRound");
        if (o && o.selectedIndex > 0) resUrl += "&r=" + o[o.selectedIndex].value;
        o = document.getElementById("chkTotalsOnly");
        if (o && o.checked) resUrl += "&to=1";
        o = document.getElementById("chkIncludeIndividuals");
        if (o && o.checked) resUrl += "&ii=1";
        o = document.getElementById("chkHighlightBest");
        if (o && o.checked) resUrl += "&hb=1";
        o = document.getElementById("chkColorFlights");
        if (o && o.checked) resUrl += "&cf=1";
        var form = "";
        o = document.getElementById("chkAddPlayers");
        if (o && o.checked) form += "addplayers=1";
        o = document.getElementById("txtEmailAlso");
        if (o) form += "&emails=" + o.value;
        o = document.getElementById("txtSubj");
        if (o) form += "&sbj=" + o.value;
        o = document.getElementById("txtMesg");
        if (o) form += "&msg=" + o.value;
        SendForm(form, "emailresults=1" + resUrl, AlterResult);
    }
    function Sort(which) {
        document.getElementById("hdnSort").value = which;
        document.forms[0].submit();
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Panel ID="pnlOptions" runat="server">
    View:&nbsp;&nbsp;&nbsp;&nbsp; 
    <asp:CheckBox ID="chkTotalsOnly" Text="Totals Only" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:CheckBox ID="chkIncludeIndividuals" Text="Include Individuals" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:CheckBox ID="chkHighlightBest" Text="Highlight Best" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:CheckBox ID="chkColorFlights" Text="Color Flights" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
    <br />
    Round <asp:DropDownList ID="ddRound" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
    Scoring <asp:DropDownList ID="ddScoring" runat="server" />
    <asp:Button Text="View" runat="server" OnClick="ScoringOption_Click" /><br/>
    <a href="javascript:EmailTo();">Email Results</a>
    <div id="divEmail" style="display:none;">
    To:<br /><asp:TextBox ID="txtEmailAlso" runat="server" Style="width: 500px" Text="dustin@par4golfmanagement.com;ryan.thomas@docusign.com" /><br/>
    Subject:<br/><asp:TextBox ID="txtSubj" runat="server" Style="width: 500px" Text="Monster Results" /><br />
    Message:<br /><asp:TextBox ID="txtMesg" runat="server" Style="width: 500px;height:200px;" TextMode="MultiLine" Text="Results from todays round" /><br />
    <asp:CheckBox ID="chkAddPlayers" runat="server" Text="Add Tournament Golfers" /><br />
    <button id="btnEmail" onclick="EmailResults();return false;">Send</button>
    </div>
    </asp:Panel>
    <asp:Label ID="txtGridHeader" runat="server" Text="Results" /><br />
    <asp:DataGrid ID="dataGridViewResults" runat="server" AutoGenerateColumns="false" BackColor="White" ForeColor="Black" />
    </div>
    <input type="hidden" name="hdnSort" id="hdnSort" value="" />
    <asp:HiddenField ID="hdnTourneyId" runat="server" />
    </form>
</body>
</html>

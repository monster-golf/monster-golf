<%@ page language="C#" autoeventwireup="true" inherits="Tourney, App_Web_tourney.aspx.cdcab7d2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title id="titleTag" runat="server">Tourney View</title>
<style type="text/css" id="stylDef" runat="server">
body{font-family:sans-serif, arial; margin:10px;font-size:12px;background-color:#1F58AE; color:#ffffff;}
a {color:#ffffff; margin:5px;}
a:hover { color:#FF3F19;}
.Detail { position:relative;float:left;min-width:25px;min-height:16px; padding:4px;text-align:center;border:solid 1px #cccccc;}
.Detail1 { position:relative;float:left;min-width:200px;min-height:16px; padding:4px;border:solid 1px #cccccc;font-size:12px;}

.Detail_2 { position:relative;float:left;min-width:25px;min-height:16px; padding:4px;text-align:center;border:solid 1px #cccccc;}
.Detail1_2 { position:relative;float:left;min-width:200px;min-height:16px; padding:4px;border:solid 1px #cccccc;font-size:12px; background-color:#008EFF; }

.Detail_Head { position:relative;float:left;min-width:25px;min-height:16px; padding:4px;text-align:center;border:solid 1px #cccccc;background-color:#0229AE;}
.Detail1_Head { position:relative;float:left;min-width:200px;min-height:16px; padding:4px;border:solid 1px #cccccc;font-size:12px;background-color:#0229AE; }

.Loading { position:absolute;top:100px; left:100px; font-size:30px; color:#EFE486; font-weight:bold; text-decoration:blink; z-index:100;}
.cb { padding: 0px 3px; margin:0px; }
.UserUpdate { float:left; margin: 3px 30px 0px;}
.TourneyInfo { clear: both; padding-top: 20px; }
</style>
<script type="text/javascript" language="javascript" src="XmlHttp.js"></script>
<script type="text/javascript" language="javascript">
    xmlurl = "TourneyXml.aspx";
    function TourneyId() {
        var dd = document.getElementById("ddTourney");
        if (dd && dd.selectedIndex > 0) return dd[dd.selectedIndex].value;
        else {
            var loc = new String(document.location.href);
            var idxOf = loc.indexOf("&t=");
            if (idxOf < 0) idxOf = loc.indexOf("?t=");
            if (idxOf < 0) return -1;
            else {
                var tsubstr = loc.substring(idxOf + 3, loc.indexOf("&", idxOf + 3));
                return tsubstr;
            }
        }
    }
    function UpdateTourneyUsers() {
        var tid = TourneyId();
        if (tid > -1) SendForm("tourneyid=" + tid, "updatehcp=1", function() { alert('update complete'); });
    }
    function ScrollTop() {
        var top = 0;
        top = window.pageYOffset;
        if (!top) top = document.documentElement.scrollTop;
        if (!top) top = document.body.scrollTop;
        if (!top) top = 0;
        return top;
    }
    function Loading(hide) {
        var loadingit = document.getElementById("loadingScreen");
        if (loadingit) {
            loadingit.style.display = (hide) ? "none" : "";
            if (!hide) loadingit.style.top = (100 + ScrollTop()) + "px";
        }
    }
    function TourneyDetails() {
        GetHTMLAsync("gettourney=1&tourneyid=" + TourneyId(), TourneyDetailsDone);
    }
    function TourneyDetailsDone(html) {
        var ti = document.getElementById("tourneyInfo");
        if (ti) ti.innerHTML = html;
        ti = document.getElementById("rounddate");
        var afterrounddate = false;
        if (ti) {
            var currDate = new Date();
            var roundDate = new Date(ti.innerHTML);
            if (roundDate.getDate() < currDate.getDate()) afterrounddate = true;
        }
        ti = document.getElementById("updateUsers");
        if (ti) ti.style.display = (afterrounddate || TourneyId() == -1) ? "none" : "";
        ti = document.getElementById("pnlViewResults");
        if (ti) ti.style.display = (TourneyId() == -1) ? "none" : "";
    }
    function SetRound(roundid) {
        GetHTMLAsync("settourneyround=" + roundid + "&tourneyid=" + TourneyId(), function() { alert('Round scores setup complete'); });
    }
    var playerscores;
    function ViewRound(roundid) {
        playerscores = document.getElementById("playerscores" + roundid);
        GetHTMLAsync("viewtourneyround=" + roundid + "&tourneyid=" + TourneyId(), ViewRoundDone);
    }
    function ViewRoundDone(html) {
        if (playerscores) playerscores.innerHTML = html;
    }
    function EmailGroups(roundid) {
        GetHTMLAsync("emailtourneyround=" + roundid + "&tourneyid=" + TourneyId(), AlterResult);
    }
    var players = new Array();
    function ChkPlayer(chk, id, r) {
        if (chk.checked) players.push(id);
        else {
            var rem;
            for (var x = 0; x < players.length; x++) {
                if (players[x] == id) {
                    rem = x;
                    break;
                }
            }
            players.splice(rem, 1);
        }
        var groupbtn = document.getElementById("setgroup" + r);
        if (players.length == 4) {
            groupbtn.style.display = "";
            groupbtn.style.top = chk.parentNode.offsetTop + "px";
            groupbtn.style.left = (chk.offsetLeft + chk.offsetWidth + 10) + "px";
        } else groupbtn.display = "none";
    }
    function SetGroup(roundid) {
        var formdata = "setgroup=1";
        while (players.length > 0) formdata += "&player=" + players.pop();
        SendForm(formdata, "viewtourneyround=" + roundid + "&tourneyid=" + TourneyId(), ViewRoundDone);
    }
    function BreakGroup(groupid, roundid) {
        var formdata = "breakgroup=" + groupid;
        SendForm(formdata, "viewtourneyround=" + roundid + "&tourneyid=" + TourneyId(), ViewRoundDone);
    }
    function EnterScores(roundid) {
        document.location.href = "Tourney.aspx?t=" + TourneyId() + "&r=" + roundid;
    }
    function EmailScores(roundid) {
        var formdata = "emailscores=" + roundid + "&tourneyid=" + TourneyId();
        SendForm(formdata, "", AlterResult);
    }
    function ViewResults() {
        var tid = TourneyId();
        if (tid != -1) document.location.href = "Results.aspx?t=" + tid;
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="loadingScreen" class="Loading" style="display:none;">Loading...</div>
    <div style="min-width:1020px;">
        <asp:DropDownList ID="ddTourney" runat="server" onchange="TourneyDetails()" style="float:left;" />
        <asp:Panel ID="updateUsers" style="display:none;" CssClass="UserUpdate" runat="server"><a href="javascript:UpdateTourneyUsers()">Update Tournament Users HCP</a></asp:Panel>
        <asp:Panel ID="pnlViewResults" style="display:none;cursor:pointer;text-decoration:underline;" CssClass="UserUpdate" runat="server" onclick="ViewResults()">Results</asp:Panel>
        <asp:Panel ID="emailScores" Style="display:none;" runat="server" />
        <asp:Panel ID="tourneyInfo" CssClass="TourneyInfo" runat="server" />
        <asp:Panel ID="tourneyResults" runat="server" />
    </div>
    <asp:Panel ID="monsterLogo" runat="server" style="margin-top:20px"><img src="MonsterLogo.png" alt="Monster Golf" /></asp:Panel>
    </form>
</body>
</html>

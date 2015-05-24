<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tourney.aspx.cs" Inherits="Tourney" EnableViewState="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title id="titleTag" runat="server">Tourney View</title>
<style type="text/css" id="stylDef" runat="server">
body{font-family:sans-serif, arial; margin:10px;font-size:12px;background-color:#1F58AE; color:#ffffff;}
a {color:#ffffff;}
a:hover { color:#FF3F19;}
a.header {margin-right:15px;}
.Detail { min-width:25px;min-height:16px; padding:4px;text-align:center;border:solid 1px #cccccc;}
.Detail1 { min-width:200px;min-height:16px; padding:4px;border:solid 1px #cccccc;font-size:12px;}
.DetailStart { min-width:70px; }
.DetailStart div {padding-top:2px;}
.Detail_2 { min-width:25px;min-height:16px; padding:4px;text-align:center;border:solid 1px #cccccc; background-color:#008EFF;}
.Detail1_2 { min-width:200px;min-height:16px; padding:4px;border:solid 1px #cccccc;font-size:12px; background-color:#008EFF; }
.Detail_Head { min-width:25px;min-height:16px; padding:4px;text-align:center;border:solid 1px #cccccc;background-color:#0229AE;}
.Detail1_Head { min-width:200px;min-height:16px; padding:4px;border:solid 1px #cccccc;font-size:12px;background-color:#0229AE; }
.Loading { position:fixed;top:200px; left:100px; font-size:30px; color:#EFE486; font-weight:bold; z-index:100;}
.cb { padding: 0px 3px; margin:0px; }
.starthole { height:20px;font-size:10pt;padding:1px;text-align:center; }
input.starthole {width:65px;}
.UserUpdate { float:left; margin: 3px 30px 0px;}
.TourneyInfo { clear: both; padding-top: 20px; }
.PageBreak {page-break-before:always;}
#playersList { clear:both; vertical-align:top; margin-top:30px; }
#teamsList { clear:both; vertical-align:top; margin:30px; }
.PlayersTable table { border-collapse: collapse; cursor:pointer; }
.PlayersTable td { border: solid 1px white; padding:6px; }
.TeamsTable table { border-collapse: collapse; cursor:pointer;}
.TeamsTable td { border: solid 1px white; padding:6px; }
#firstName { width: 180px; margin-left:6px; }
#lastName { width: 180px; margin-left:7px; }
#emailName { width: 180px; }
#addPlayer { margin-left:33px;}
.flight { width:20px; font-size:9pt;}
.owner { width:80px; font-size:9pt;}
#createTournament { clear:both; margin-top:30px; }
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
        if (tid > -1) SendForm("tourneyid=" + tid, "updatehcp=1");
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
            //if (!hide) loadingit.style.top = (100 + ScrollTop()) + "px";
        }
    }
    function TourneyDetails() {
        var tourneyId = TourneyId();
        if (tourneyId == "") document.location.href = "Tourney.aspx";
        else {
            TourneyView();
            var createTournament = document.getElementById("createTournament");
            if (createTournament) createTournament.style.display = "none";
            GetHTMLAsync("gettourney=1&tourneyid=" + TourneyId(), TourneyDetailsDone);
        }
    }
    function TourneyDetailsDone(html) {
        var ti = document.getElementById("tourneyInfo");
        if (ti) ti.innerHTML = html;
        ti = document.getElementById("rounddate_1");
        var afterrounddate = false;
        if (ti) {
            var currDate = new Date();
            currDate.setHours(currDate.getHours() - 24);
            var roundDate = new Date(ti.innerHTML);
            if (roundDate < currDate) afterrounddate = true;
        }
        ti = document.getElementById("updateUsers");
        if (ti) ti.style.display = (afterrounddate || TourneyId() == -1) ? "none" : "";
        ti = document.getElementById("viewResults");
        if (ti) ti.style.display = (TourneyId() == -1) ? "none" : "";
        ti = document.getElementById("createTeams");
        if (ti) ti.style.display = (afterrounddate || TourneyId() == -1 == -1) ? "none" : "";
    }
    function SetRound(roundid) {
        GetHTMLAsync("settourneyround=" + roundid + "&tourneyid=" + TourneyId(), function() { ViewRound(roundid); });
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
        if (players.length >= 3) {
            groupbtn.style.display = "";
            groupbtn.style.top = chk.parentNode.offsetTop + "px";
            groupbtn.style.left = (chk.parentNode.offsetLeft + groupbtn.offsetWidth) + "px";
        } else groupbtn.display = "none";
    }
    function SetGroup(roundid) {
        var formdata = "setgroup=1";
        while (players.length > 0) formdata += "&player=" + players.pop();
        playerscores = document.getElementById("playerscores" + roundid);
        SendForm(formdata, "viewtourneyround=" + roundid + "&tourneyid=" + TourneyId(), ViewRoundDone);
    }
    function BreakGroup(groupid, roundid) {
        var formdata = "breakgroup=" + groupid;
        playerscores = document.getElementById("playerscores" + roundid);
        SendForm(formdata, "viewtourneyround=" + roundid + "&tourneyid=" + TourneyId(), ViewRoundDone);
    }
    function SetTourneyInfo(txt) {
        var setinfo = "" + txt.id + "=" + txt.value;
        SendForm(setinfo, "updatetourneyinfo=1&t=" + TourneyId());
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
    function StartingHole(hole, groupid) {
        var tid = TourneyId();
        if (tid > -1) SendForm("tourneyid=" + tid + "&group=" + groupid + "&startinghole=" + hole.value, "");
    }
    function StartingTime(time, groupid, roundnum) {
        var tid = TourneyId();
        if (tid > -1) {
            var dofr= "";
            var rd = document.getElementById("dateofround_" + roundnum);
            if (!rd) {
                rd = document.getElementById("rounddate_" + roundnum);
                dofr = rd.innerHTML;
            } else {
                dofr = rd.value;
            }
            SendForm("tourneyid=" + tid + "&group=" + groupid + "&startingtime=" + time.value + "&rd=" + dofr, "");
        }
    }
    function TourneyView(setupteams) {
        var ti = document.getElementById("tourneyInfo");
        if (ti) ti.style.display = (setupteams) ? "none" : "";
        ti = document.getElementById("tourneyResults");
        if (ti) ti.style.display = (setupteams) ? "none" : "";
        if (!setupteams) {
            ti = document.getElementById("playersList");
            if (ti) ti.style.display = "none";
            ti = document.getElementById("teamsList");
            if (ti) ti.style.display = "none";
        }
        ti = document.getElementById("createTeams");
        if (ti) ti.style.display = (setupteams) ? "none" : "";
        ti = document.getElementById("tourneyView");
        if (ti) ti.style.display = (setupteams) ? "" : "none";
    }
    function SetupTeams() {
        TourneyView(true);
        GetHTMLAsync("playerslist=1&t=" + TourneyId(), PlayersDone);
        GetHTMLAsync("teamslist=1&t=" + TourneyId(), TeamsDone);
    }
    function SortTeams(sort) {
        GetHTMLAsync("teamslist=1&t=" + TourneyId() + "&sort=" + sort, TeamsDone);
    }
    var teamPlayers;
    function PlayersDone(html) {
        teamPlayers = new Array();
        var playersList = document.getElementById("playersList");
        if (playersList) {
            playersList.innerHTML = html;
            playersList.style.display = "inline-block";
        }
    }
    function TeamsDone(html) {
        var teamsList = document.getElementById("teamsList");
        if (teamsList) {
            teamsList.innerHTML = html;
            teamsList.style.display = "inline-block";
        }
    }
    function SelectTeamPlayer(userId) {
        var findPlayer = teamPlayers.indexOf(userId);
        if (findPlayer == -1) {
            teamPlayers.push(userId);
            var obj = document.getElementById("player" + userId);
            if (obj) obj.style.backgroundColor = "black";
            if (teamPlayers.length > 0) {
                obj = document.getElementById("setteam" + userId);
                if (obj) obj.style.display = "";
            }
        } else {
            UnSelectTeamPlayer(userId);
            teamPlayers.splice(findPlayer, 1);
        }
        for (var x = 0; x < teamPlayers.length; x++) {
            obj = document.getElementById("setteam" + teamPlayers[x]);
            if (obj) {
                if (teamPlayers.length == 1) {
                    obj.value = "Add Player"
                } else {
                    obj.value = "Set Team"
                }
            }
        }
    }
    function UnSelectTeamPlayer(userId) {
        var obj = document.getElementById("setteam" + userId);
        if (obj) obj.style.display = "none";
        obj = document.getElementById("player" + userId);
        if (obj) obj.style.backgroundColor = "";
    }
    function SetTeam(e) {
        var setteamplayers = "teamslist=1&t=" + TourneyId();
        var userId;
        while (userId = teamPlayers.pop()) {
            setteamplayers += "&player=" + userId;
            UnSelectTeamPlayer(userId);
        }
        GetHTMLAsync(setteamplayers, function (html) { TeamsDone(html); GetHTMLAsync("playerslist=1&t=" + TourneyId(), PlayersDone); });
        return CE(e);
    }
    function RemoveTeam(teamId) {
        var setteamplayers = "teamslist=1&t=" + TourneyId() + "&removeteam=" + teamId;
        GetHTMLAsync(setteamplayers, function (html) { TeamsDone(html); GetHTMLAsync("playerslist=1&t=" + TourneyId(), PlayersDone); } );
    }
    function TeamFlight(flighttxt, teamid) {
        var setteam = "flight=" + flighttxt.value + "&t=" + TourneyId() + "&teamid=" + teamid;
        GetHTMLAsync(setteam);
    }
    function TeamOwner(ownertxt, teamid) {
        var setteam = "owner=" + ownertxt.value + "&t=" + TourneyId() + "&teamid=" + teamid;
        GetHTMLAsync(setteam);
    }
    function AddPlayer() {
        var addplayer = "playerslist=1&t=" + TourneyId();
        var obj = document.getElementById("firstName");
        if (obj) addplayer += "&firstname=" + obj.value;
        obj = document.getElementById("lastName");
        if (obj) addplayer += "&lastname=" + obj.value;
        obj = document.getElementById("emailName");
        if (obj) addplayer += "&emailname=" + obj.value;
        GetHTMLAsync(addplayer, PlayersDone);
    }
    function RemovePlayer(userId) {
        var removeplayer = "playerslist=1&t=" + TourneyId() + "&removeplayer=" + userId;
        GetHTMLAsync(removeplayer, PlayersDone);
    }
    function SetTournamentCheck(checkobj, teamid) {
        var setteamplayers = "tournamentcheck=" + checkobj.checked + "&t=" + TourneyId() + "&" + checkobj.id + "=" + teamid;
        GetHTMLAsync(setteamplayers);
    }
    function DefaultTxt(txt) {
        if (txt.getAttribute("defaulttxt") == txt.value) {
            txt.value = "";
        } else if (txt.value == "") {
            txt.value = txt.getAttribute("defaulttxt");
        }
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="loadingScreen" class="Loading" style="display:none;">Loading...</div>
    <div style="min-width:1020px;">
        <asp:DropDownList ID="ddTourney" runat="server" onchange="TourneyDetails()" style="float:left;" />
        <asp:Panel ID="updateUsers" style="display:none;" CssClass="UserUpdate" runat="server"><a href="javascript:UpdateTourneyUsers()">Update Tournament Players HCP</a></asp:Panel>
        <div id="viewResults" style="display:none;" class="UserUpdate" onclick="ViewResults()"><a href="javascript:ViewResults()">Results</a></div>
        <div id="createTeams" style="display:none;" class="UserUpdate"><a href="javascript:SetupTeams()">Setup Teams</a></div>
        <div id="tourneyView" style="display:none;" class="UserUpdate"><a href="javascript:TourneyView()">Tournament</a></div>
        <p id="createTournament">
            <input name="newslogan" onfocus="DefaultTxt(this)" onblur="DefaultTxt(this)" defaulttxt="tournament name" value="tournament name" /><br />
            <input name="newdescription" onfocus="DefaultTxt(this)" onblur="DefaultTxt(this)" defaulttxt="description" value="description" /><br />
            <input name="newlocation" onfocus="DefaultTxt(this)" onblur="DefaultTxt(this)" defaulttxt="location" value="location" /><br />
            <input name="newtournament" type="submit" value="New Tournament" />
        </p>
        <asp:Panel ID="emailScores" Style="display:none;" runat="server" />
        <asp:Panel ID="tourneyInfo" CssClass="TourneyInfo" runat="server" />
        <asp:Panel ID="tourneyResults" runat="server" />
        <div style="clear:both;"></div>
        <div id="playersList" style="display:none;"></div>
        <div id="teamsList" style="display:none;"></div>
    </div>
    <asp:Panel ID="monsterLogo" runat="server" style="margin-top:20px"><img src="MonsterLogo.png" alt="Monster Golf" /></asp:Panel>
    </form>
</body>
</html>

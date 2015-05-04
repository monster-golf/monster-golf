<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableEventValidation="false" EnableViewStateMac="false" %>

<!DOCTYPE html>

<html>
<head runat="server">
<title>Monster Online Scoring</title>
<script type="text/javascript" language="javascript" src="XmlHttp.js"></script>
<script type="text/javascript" language="javascript">
function TxtNum(obj) {
    var val = parseInt(obj.value);
    if (isNaN(val)) val = 0;
    return val;
}
function ErrShow(show) {
    var savefailed = document.getElementById("pnlSaveFailed");
    if (savefailed) {
        // last save was successful so make sure all rows have been saved.
        if (savefailed.style.display == "" && !show) ScoresSaveAll();
        savefailed.style.display = (show) ? "" : "none";
    }
}
function ErrDisplay() { ErrShow(true); }
function ErrHide() { ErrShow(false); }
var insaveall = false;
function ScoresSaveAll() {
    if (insaveall) return;
    var playerrow = document.getElementById("pnlScoresList");
    if (playerrow) {
        insaveall = true;
        playerrow = GetFirstItem(playerrow.firstChild, "player_");
        while (playerrow) {
            var obj = GetFirstItem(playerrow.firstChild, "score_name_");
            if (obj) ScoresSave(obj);
            playerrow = GetFirstItem(playerrow.nextSibling, "player_");
        }
    }
    insaveall = false;
}
function ScoresSave(obj, e) {
    var invalid = false;
    if (!IsE(obj, "score_name_")) {
        var isnum = 0;
        if (obj.value != "") {
            var val = parseInt(obj.value);
            var isnum = parseInt(val);
            if (isNaN(isnum)) invalid = true;
            else obj.value = isnum;
            if (isnum == 1) invalid = !confirm("HOLE IN ONE....FOR REALS!!!!");
            if (isnum >= 15) invalid = !confirm("It really took " + isnum + " strokes...ouch.");
        }
    }
    if (invalid) {
        obj.value = "";
        TxtFocus(obj);
        return CE(e);
    } else {
        var x = 1;
        var scorerow = obj.parentNode;
        var scoreobj = GetFirstItem(scorerow.firstChild, "score_" + x++ + "_");
        var holedata = "userid=" + EAttr(scorerow, "userid");
        holedata += "&userlookup=" + EAttr(scorerow, "userlookup");
        var obj = GetFirstItem(scorerow.firstChild, "score_name_");
        if (obj) holedata += "&name=" + escape(obj.value);
        obj = GetFirstItem(scorerow.firstChild, "score_hcp_");
        if (obj) holedata += "&hcp=" + escape(obj.value);
        obj = document.getElementById("courseName");
        if (obj) holedata += "&coursename=" + escape(obj.value);
        obj = document.getElementById("courseSlope");
        if (obj) holedata += "&courseslope=" + escape(obj.value);
        obj = document.getElementById("courseRating");
        if (obj) holedata += "&courserating=" + escape(obj.value);
        
        while (scoreobj) {
            val = parseInt(scoreobj.value);
            if (!isNaN(val) && val > 0) holedata += "&" + scoreobj.id + "=" + scoreobj.value;
            scoreobj = GetFirstItem(scoreobj.nextSibling, "score_" + x++ + "_");
        }
        SendForm(holedata, "savescore=1", ErrHide, ErrDisplay);
    }
    if (obj) return ScoresAdd(obj, e);
}
var scoresTO;
function ScoresAdd(obj, e) {
    if (!obj.parentNode) return KeyEntKill(e);
    var scoreobj = obj.parentNode.firstChild;
    var f9 = 0;
    var b9 = 0;
    var hcp = 0;
    while (scoreobj) {
        if (EAttr(scoreobj, "score") == "f") f9 += TxtNum(scoreobj);
        else if (EAttr(scoreobj, "score") == "b") b9 += TxtNum(scoreobj);
        else if (IsE(scoreobj, "_out_") && f9 > 0) scoreobj.value = f9.toString();
        else if (IsE(scoreobj, "_in_") && b9 > 0) scoreobj.value = b9.toString();
        else if (IsE(scoreobj, "_total_") && (f9 + b9) > 0) scoreobj.value = (f9 + b9).toString();
        else if (IsE(scoreobj, "_hcp_")) hcp = TxtNum(scoreobj);
        else if (IsE(scoreobj, "_net_") && (f9 + b9) > 0) scoreobj.value = ((f9 + b9) - hcp).toString();
        scoreobj = scoreobj.nextSibling;
    }
    if (scoresTO != null) window.clearTimeout(scoresTO);
    scoresTO = window.setTimeout(ScoresComplete, 2000);
    return KeyEntKill(e);
}
var scoreInitsList = new Array();
function ScoresFocus(obj, e) {
    if (!obj) return KeyEntKill(e);
    if (IsE(obj, "score_1_") || IsE(obj, "score_2_") || IsE(obj, "score_3_") || IsE(obj, "score_10_") || IsE(obj, "score_11_") || IsE(obj, "score_12_")) return KeyEntKill(e);
    var scorehole = obj.id.substring(0, obj.id.lastIndexOf("_") + 1);
    var scorelist = document.getElementById("pnlScoresList");
    var scorerow = GetFirstItem(obj.parentNode.parentNode.firstChild, "player_");
    var x = 0;
    if (scoreInitsList.length == 0) {
        while (scorerow) {
            var scoreinits = GetFirstItem(scorerow.firstChild, "score_initials_");
            if (scoreinits) {
                x = scoreInitsList.length;
                scoreInitsList[x] = document.createElement("input");
                scoreInitsList[x].className = scoreinits.className;
                scoreInitsList[x].value = scoreinits.value;
                scoreInitsList[x].tabIndex = scoreinits.tabIndex;
                scoreInitsList[x].size = scoreinits.size;
                scoreInitsList[x].style.position = "absolute";
                scoreInitsList[x].style.backgroundColor = "#ffffff";
                scoreInitsList[x].style.display = "none";
                scorerow.appendChild(scoreInitsList[x]);
            }
            scorerow = GetFirstItem(scorerow.nextSibling, "player_");
        }
        scorerow = GetFirstItem(obj.parentNode.parentNode.firstChild, "player_");
    }
    x = 0;
    while (scorerow) {
        var score = GetFirstItem(scorerow.firstChild, scorehole);
        if (score) {
            scoreInitsList[x].style.display = "";
            scoreInitsList[x].style.left = (score.offsetLeft - (scoreInitsList[x].offsetWidth*2)) + "px";
            x++;
        }
        scorerow = GetFirstItem(scorerow.nextSibling, "player_");
    }
    return KeyEntKill(e);
}
function ScoresBlur(obj, e) {
    ScoresSave(obj, event);
    for (var x = 0; x < scoreInitsList.length; x++) {
        scoreInitsList[x].style.display = "none";
    }
    return KeyEntKill(e);
}
function ScoresComplete() {
    var scorelist = document.getElementById("pnlScoresList");
    var score;
    var complete = true;
    if (scorelist) {
        var scorerow = GetFirstItem(scorelist.firstChild, "player");
        while (scorerow) {
            var scoreobj = scorerow.firstChild;
            var hasname = false;
            while (scoreobj) {
                if (IsE(scoreobj, "score_name_") && scoreobj.value != "") hasname = true;
                else if (EAttr(scoreobj, "score") == "f" || EAttr(scoreobj, "score") == "b") score = TxtNum(scoreobj);
                if (score == 0 && hasname) {
                    complete = false;
                    break;
                }
                scoreobj = scoreobj.nextSibling;
            }
            scorerow = GetFirstItem(scorerow.nextSibling, "player");
        }
    }
    var scoresign = document.getElementById("pnlScorerSign");
    if (complete && scoresign) scoresign.style.display = "";
    else if (scoresign) scoresign.style.display = "none";
    return complete;
}
function PlayerFocus(txt) {
    var playersch = GetFirstItem(txt.parentNode.firstChild, "score_search_");
    if (playersch) playersch.style.display = "";
}
function PlayerBlur(txt, e) {
    var playersch = GetFirstItem(txt.parentNode.firstChild, "score_search_");
    if (playersch) playersch.style.display = "none";
    var val = txt.value;
    var ddS = document.getElementById("ddScorer");
    var ddA = document.getElementById("ddAttester");
    var userid = parseInt(EAttr(txt.parentNode, "userid"));
    if (!isNaN(userid)) {
        if (ddS && ddS.options[userid] != null) PlayerDD(ddS, userid, txt.parentNode.id, userid, "", val);
        if (ddA && ddA.options[userid] != null) PlayerDD(ddA, userid, txt.parentNode.id, userid, "", val);
        var valspl = val.split(" ");
        val = "";
        for (var x = 0; x < valspl.length; x++) {
            if (val.length == 2) break;
            if (valspl[x] != "") val += valspl[x].substr(0, 1)
        }
        txt = GetFirstItem(txt.parentNode.firstChild, "score_initials_");
        if (txt) txt.value = val;
    }
}
var playerdiv;
function PlayerSearch(img) {
    var txt = GetFirstItem(img.parentNode.firstChild, "score_name_");
    if (txt.value != EAttr(txt, "lastsrch")) {
        img.style.display = "none";
        if (playerdiv) playerdiv.style.display = "none";
        var playerrow = img.parentNode;
        if (EAttr(playerrow, "userid") != "" && EAttr(playerrow, "name") != "" && EAttr(playerrow, "name") != txt.value) {
            if (!confirm("Click OK to remove " + EAttr(playerrow, "name") + " from this group.")) {
                return;
            } else {
                playerrow.setAttribute("userid", "");
                playerrow.setAttribute("email", "");
                playerrow.setAttribute("hcp", "");
                playerrow.setAttribute("name", "");
            }
        }
        playerdiv = GetFirstItem(txt.nextSibling, "score_player_"); ;
        if (!IsE(playerdiv, "score_player")) {
            playerdiv = document.createElement("div");
            playerdiv.className = "PlayerList";
            playerdiv.id = txt.id.replace("score_name_", "score_player_");
            txt.parentNode.appendChild(playerdiv);
        }
        playerdiv.innerHTML = "Searching...";
        txt.setAttribute("lastsrch", txt.value);
        GetHTMLAsync("player=" + escape(txt.value), PlayerFind);
    }
}
function SlopeChange(txt) {
    txt = document.getElementById("courseSlope");
    var slope = 113
    if (txt) slope = parseInt(txt.value);
    var scorelist = document.getElementById("pnlScoresList");
    if (scorelist) {
        var scorerow = scorelist.firstChild;
        while (scorerow) {
            if (IsE(scorerow, "player")) {
                var hcp = parseFloat(EAttr(scorerow, "hcp"));
                if (!isNaN(hcp)) {
                    var coursehcp = Math.round((hcp * slope) / 113);
                    txt = GetFirstItem(scorerow.firstChild, "score_hcp_");
                    txt.value = coursehcp;
                }
            }
            scorerow = scorerow.nextSibling;
        }
    }
}
function PlayerSearchClose(obj) {
    if (obj) obj.style.display = "none";
}
function PlayerFind(html) {
    var img = GetFirstItem(playerdiv.parentNode.firstChild, "score_search_");
    if (img) img.style.display = "";
    playerdiv.style.display = "";
    playerdiv.innerHTML = html;
}
function PlayerDD(dd, idx, id, userid, email, name) {
    if (id == "player_" + idx || id == "player_" + userid) {
        if (dd.options[idx].value.indexOf(id) > -1) {
            dd.options[idx].value = id + ":" + userid + ":" + email;
            dd.options[idx].text = name;
        }
    }
}
function PlayerSelect(userid, first, last, email, hcp) {
    var playerrow = playerdiv.parentNode;
    var id = playerrow.id;
    var name = first + " " + last;
    var txt = GetFirstItem(playerrow.firstChild, "score_name_");
    if (txt) txt.value = name;
    txt = GetFirstItem(playerrow.firstChild, "score_search_");
    if (txt) txt.style.display = "none";
    txt = GetFirstItem(playerrow.firstChild, "score_initials_");
    if (txt) txt.value = first.substr(0, 1) + last.substr(0, 1);
    playerrow.setAttribute("userid", userid);
    playerrow.setAttribute("email", email);
    playerrow.setAttribute("hcp", hcp);
    playerrow.setAttribute("name", name);
    hcp = parseFloat(hcp);
    if (!isNaN(hcp)) {
        txt = document.getElementById("courseSlope");
        var slope = 113
        if (txt) slope = parseInt(txt.value);
        var coursehcp = Math.round((hcp * slope) / 113);
        txt = GetFirstItem(playerrow.firstChild, "score_hcp_");
        txt.value = coursehcp;
    }
    var ddS = document.getElementById("ddScorer");
    var ddA = document.getElementById("ddAttester");
    for (var x = 1; x <= 5; x++) {
        PlayerDD(ddS, x, id, userid, email, first + " " + last);
        PlayerDD(ddA, x, id, userid, email, first + " " + last);
    }
    playerdiv.style.display = "none";
    if (!roundcheckcancel) {
        GetHTMLAsync("checkforround=" + userid, RoundCurrentCheck);
    }
    obj = document.getElementById("pnlPlayerHelp");
    if (obj) obj.style.display = "none";
}
function RoundResume(url) {
    document.location.href = url;
}
var roundcheckcancel = false;
function RoundCurrentCheck(html) {
    var div = document.createElement("div");
    div.innerHTML = html;
    div = div.firstChild;
    var tourney = EAttr(div, "tourney");
    var roundnum = EAttr(div, "roundnum");
    var lookup = EAttr(div, "lookup");
    var groupid = EAttr(div, "groupid");
    if (tourney == "" && roundnum == "" && lookup == "" && groupid == "") {
        div = div.firstChild;
        tourney = EAttr(div, "tourney");
        roundnum = EAttr(div, "roundnum");
        lookup = EAttr(div, "lookup");
        groupid = EAttr(div, "groupid");
    }
    if (tourney != "" || roundnum != "" || lookup != "" || groupid != "") {
        if (document.forms[0].tourneyId.value == tourney && document.forms[0].roundNum.value == roundnum || document.forms[0].roundNum.value == groupid) return;
        if (confirm("Found a round you were playing today.  Would you like to continue that round?")) {
            var url = "Default.aspx?r=" + groupid;
            if (tourney != "" && roundnum != "") url = "Default.aspx?t=" + tourney + "&r=" + roundnum + "&u=" + lookup;
            SendForm("deletegroup=" + document.forms[0].roundNum.value, "", Function("RoundResume('" + url + "');"), Function("RoundResume('" + url + "');"));
        } else {
            roundcheckcancel = true;
        }
    }
}
function CourseSearch(txt) {
    var pnl = document.getElementById("pnlCourseList");
    if (pnl) pnl.innerHTML = "Searching...";
    GetHTMLAsync("course=" + escape(txt.value), CourseFind);
}
function CourseFind(html) {
    var pnl = document.getElementById("pnlCourseList");
    if (pnl) pnl.innerHTML = html;
}
function CourseEnter() {
    var obj = document.getElementById("pnlCourseList");
    if (obj) obj.innerHTML = "";
    obj = document.getElementById("courseHeader");
    if (obj) obj.style.display = "";
    obj = document.getElementById("courseName");
    if (obj) {
        obj.style.display = "";
        TxtFocus(obj);
    }
    obj = document.getElementById("courseSlope");
    if (obj) obj.style.display = "";
    obj = document.getElementById("courseRating");
    if (obj) obj.style.display = "";
    obj = document.getElementById("showF9");
    if (obj) obj.parentNode.style.display = "";
    obj = document.getElementById("showB9");
    if (obj) obj.parentNode.style.display = "";
    obj = document.getElementById("pnlPlayerHelp");
    if (obj) obj.style.display = "";
    obj = document.getElementById("pnlScoresList");
    if (obj) obj.style.display = "";
}
function CourseSelect(name, slope, rating) {
    var obj = document.getElementById("pnlCourseList");
    if (obj) obj.innerHTML = "";
    obj = document.getElementById("txtFindCourse");
    if (obj) obj.style.display = "none";
    obj = document.getElementById("btnFindCourse");
    if (obj) obj.style.display = "none";
    obj = document.getElementById("btnChangeCourse");
    if (obj) obj.style.display = "";
    obj = document.getElementById("courseHeader");
    if (obj) obj.style.display = "";
    obj = document.getElementById("courseName");
    if (obj) {
        obj.value = name;
        obj.style.display = "";
    }
    obj = document.getElementById("courseSlope");
    if (obj) {
        obj.value = slope;
        SlopeChange(slope);
        obj.style.display = "";
    }
    obj = document.getElementById("courseRating");
    if (obj) {
        obj.value = rating;
        obj.style.display = "";
    }
    obj = document.getElementById("pnlScoresList");
    if (obj) obj.style.display = "";
    obj = document.getElementById("showF9");
    if (obj) obj.parentNode.style.display = "";
    obj = document.getElementById("showB9");
    if (obj) obj.parentNode.style.display = "";
    obj = document.getElementById("pnlPlayerHelp");
    if (obj) obj.style.display = "";
}
function CourseChange() {
    var obj = document.getElementById("txtFindCourse");
    if (obj) obj.style.display = "";
    obj = document.getElementById("btnFindCourse");
    if (obj) obj.style.display = "";
    obj = document.getElementById("btnChangeCourse");
    if (obj) obj.style.display = "none";
}
function SignDDValid(dd) {
    if (!dd || dd.disabled) return true;
    if (dd.selectedIndex == 0) return false;
    else if (dd[dd.selectedIndex].text == "") return false;
    return true;
}
function SignDDs(e) {
    if (!ScoresComplete()) return false;
    if (!SignDDValid(document.getElementById("ddScorer"))) { alert("Please select the scorer"); return false; }
    else if (!SignDDValid(document.getElementById("ddAttester"))) { alert("Please select the attester"); return false; }
    else return true;
}
function Show9(chk,type,e) {
    var scorelist = document.getElementById("pnlScoresList");
    var score;
    var disp = (chk.checked) ? "" : "none";
    var otherchk = document.getElementById(((type == "b") ? "showF9" : "showB9"));
    var dispinits = (chk.checked && otherchk.checked) ? "" : "none"; 
    if (scorelist) {
        var scorerow = scorelist.firstChild;
        while (scorerow) {
            var scoreobj = scorerow.firstChild;
            while (scoreobj) {
                if (EAttr(scoreobj, "score") == type) scoreobj.style.display = disp;
                else if (IsE(scoreobj, "score_initials_")) scoreobj.style.display = dispinits;
                scoreobj = scoreobj.nextSibling;
            }
            scorerow = scorerow.nextSibling;
        }
    }
    return KeyEntKill(e);
}
function SearchExternal() {
    GetHTMLAsync("externalcourse=1", SearchExtDone);
}
function SearchExtDone(html) {
    document.getElementById("externalresult").innerHTML = html;
}
function CheckScoresPost(e) {
    var scorelist = document.getElementById("pnlScoresList");
    var msg = "";
    if (scorelist) {
        var scorerow = scorelist.firstChild;
        while (scorerow) {
            if (IsE(scorerow, "player_")) {
                var checkemail = GetFirstItem(scorerow.firstChild, "email_");
                if (checkemail && checkemail.value == "") {
                    msg = "Without an email address the score will not be posted to Monster Golf handicapping.  Continue?";
                    break;
                }
            }
            scorerow = scorerow.nextSibling;
        }
    }
    if (msg != "" && !confirm(msg)) return CE(e);
    else return true;
}
var scoresscroll = null;
var scoresleft = 200;
var scorespause = false;
var liveresults;
var livescores;
var livescoresto = null;
function LiveScores() {
    var t = document.forms[0].tourneyId.value;
    if (t != "") {
        var r = document.forms[0].roundNum.value;
        var o = "";
        if (document.forms[0].orderby && document.forms[0].orderby.length > 1) {
            if (document.forms[0].orderby[0].checked) {
                o = document.forms[0].orderby[0].value;
            }
            else if (document.forms[0].orderby[1].checked) {
                o = document.forms[0].orderby[1].value;
            }
        }
        var q = "livescores=1&t=" + t + "&r=" + r + "&o=" + o;
        GetHTMLAsync(q, function (html) {
            if (!liveresults) liveresults = document.getElementById("liveresults");
            if (liveresults) {
                if (!livescores) {
                    livescores = document.getElementById("livescorescontrols");
                    if (livescores) livescores.style.display = "block";
                    livescores = document.getElementById("livescores");
                }
                if (livescores) livescores.style.display = "block";
                liveresults.innerHTML = html;
                if (!scorespause) LiveScoresPlay();
            }
        });
        livescoresto = window.setTimeout(LiveScores, 120000);
    }
}
window.setTimeout(LiveScores, 1000);
function LiveScoresScroll(scoresmove) {
    scoresleft -= scoresmove;
    if (scoresleft < livescores.offsetWidth && Math.abs(scoresleft) < liveresults.offsetWidth) {
        liveresults.style.left = (scoresleft) + "px";
    } else {
        if (Math.abs(scoresleft) >= liveresults.offsetWidth) {
            if (!scorespause) scoresleft = 100;
        } else {
            scoresleft += scoresmove;
        }
    }
}
function LiveScoresBack() {
    var livescores = document.getElementById("livescores");
    LiveScoresScroll(-(livescores.offsetWidth - 50));
}
function LiveScoresForward() {
    var livescores = document.getElementById("livescores");
    LiveScoresScroll(livescores.offsetWidth - 50);
}
function LiveScoresPause() {
    var playpause = document.getElementById("liveplay");
    playpause.style.display = "";
    playpause = document.getElementById("livepause");
    playpause.style.display = "none";

    if (scoresscroll) window.clearInterval(scoresscroll);
    scorespause = true;
}
function LiveScoresPlay() {
    var playpause = document.getElementById("liveplay");
    playpause.style.display = "none";
    playpause = document.getElementById("livepause");
    playpause.style.display = "";

    if (scoresscroll) window.clearInterval(scoresscroll);
    scoresscroll = window.setInterval(function () { LiveScoresScroll(5); }, 150);
    scorespause = false;
}
function LiveScoresSort() {
    scoresleft = 200;
    LiveScoresScroll(0);
    if (livescoresto) window.clearTimeout(livescoresto);
    LiveScores();
}
</script>
<style type="text/css">
    body{font-family:sans-serif, arial; margin:<%=(tourneyId.Value == "") ? "10px" : "10px 0px"%>;font-size:14px;background-color:#1F58AE; color:#ffffff;}
    a {color:#ffffff;}
    a:hover { color:#FF3F19;}
    .ScoresList{position:relative;width:930px;margin-top:10px;}
    .ScoresRow{position:relative;float:none;clear:both;border:none;}
    .Score {color:#222222;position:relative;font-size:14px;border-top:solid 1px #cccccc; border-left:solid 1px #cccccc; border-right:none;border-bottom:none;text-align:center;padding:3px;border-radius:0px;margin:0px 0px 0px 0px;}
    .ScoreName{width:140px;overflow:hidden;text-align:right !important;}
    .ScoreCourseName{width:140px;overflow:hidden;text-align:left !important;}
    .Score:disabled { background-color:#ffffff; color:#000000; }
    .ScoreHighlight { background-color:#FFEA68; }
    .ScoreHole { width:22px; }
    .ScoreHCP { width: 30px; }
    .ScoreTotal { width:30px; color:#222222;}
    .ScoreNet {width:30px; color:#222222; border-right:solid 1px #cccccc !important;}
    .ScoreLast{border-bottom:solid 1px #cccccc !important;}
    .PlayerSearch{position:absolute;top:0px;left:-25px;}
    .PlayerList {position:relative;float:left;border-left:solid 1px #cccccc;min-width:140px;padding:3px;}
    .CourseInfo {font-size:14px;border:solid 1px #cccccc;padding:3px;width:30px;z-index:1;}
    .CourseName {font-size:14px;border:solid 1px #cccccc;padding:3px;width:140px;z-index:1;}
    .ScorePost {width: 50px;}
    .ScoreRating {width: 50px;border-right:solid 1px #cccccc !important;}
    .ScoreTourney{padding-left:10px;}
    .PlayerHelp { margin:10px; }
    .CourseFind { margin-top:10px; }
    .SaveFailed { width:450px;position:relative;font-size:16px;border:1px solid #FFEA68;margin:2px;padding:3px;}
    .RoundInfo { position:relative;white-space:nowrap; padding-left:10px; }
    .ScorerSign { position:relative;padding-left:10px;float:none;clear:both; }
    #livescores { width:870px; border-left:solid 1px #cccccc; border-right:solid 1px #cccccc; border-bottom:solid 1px #cccccc; display:none; background-color: yellow; white-space: nowrap; overflow:hidden; color:#333; position:relative;padding:10px; height:16px; }
    #liveresults { position: absolute; white-space: nowrap; left:200px; }
    #livescorescontrols { width:878px; border-left:solid 1px #cccccc; border-right:solid 1px #cccccc; display:none; background-color: yellow; color:#333; padding:6px; padding-bottom:0px; font-size:12px; white-space:nowrap; }
    #livescorescontrols a { font-size:20px; color:#333; }
</style>
<meta name="viewport" content="initial-scale=1.25" />
</head>
<body onload="TxtFocus(document.getElementById('txtFindCourse'))">
<form id="form1" runat="server">
<asp:HiddenField ID="tourneyId" runat="server" />
<asp:HiddenField ID="roundNum" runat="server" />
<asp:HiddenField ID="scorerId" runat="server" />
<asp:HiddenField ID="scorerUserId" runat="server" />
<asp:HiddenField ID="scorerName" runat="server" />
<div class="RoundInfo">
<asp:Label ID="lblRoundInfo" runat="server">Monster Scoring</asp:Label> <asp:Label ID="lblDateInfo" runat="server" />
</div>
<asp:Panel ID="pnlFindCourse" runat="server" Visible="false" CssClass="CourseFind">
    Course: <asp:TextBox ID="txtFindCourse" runat="server" CssClass="CourseName" onkeypress="return KeyEntToSearch(this,event);" onkeyup="return KeyEntToSearch(this,event);" /> 
    <asp:Button ID="btnFindCourse" runat="server" UseSubmitBehavior="false" OnClientClick="return CourseSearch(document.getElementById('txtFindCourse'))" Text="Find" />
    <asp:Button ID="btnChangeCourse" runat="server" UseSubmitBehavior="false" OnClientClick="return CourseChange()" Text="Change" />
    <asp:Panel ID="pnlCourseList" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlPlayerHelp" runat="server" CssClass="PlayerHelp" style="display:none;">
Enter player names, hit the search icon <img src="find.png" alt="find player" style="vertical-align:middle;" /> when it appears to the left of the name or hit the enter/go key on your keyboard. Also, it will auto search after you type 3 characters.  If a name is found in the Monster handicap system it will show up in a list below the name box, make sure to click on the name.
</asp:Panel>
<asp:Panel ID="pnlSaveFailed" CssClass="SaveFailed" style="display:none;" runat="server" onclick="ErrHide(); ScoresSaveAll();">Your last score did not save successfully, you may be offline.  Will try to post after the next hole, or click here to try again.</asp:Panel>
<asp:Panel ID="pnlScoresList" CssClass="ScoresList" runat="server" />
<asp:Panel ID="pnlScorerSign" CssClass="ScorerSign" runat="server">
    <p>Your round is complete.<br />
    Please review your scores with your playing partners.
    When you are satisfied select the scorer and attester and click Sign Card.<br /><br />
    Who kept score? <asp:DropDownList ID="ddScorer" runat="server" /><br />
    Who will attest the score card? <asp:DropDownList ID="ddAttester" runat="server" /> <asp:Button ID="btnScorerSign" runat="server" OnClientClick="return SignDDs(event)" OnClick="btnSign_Click" Text="Sign Card" />
    </p>
</asp:Panel>
<asp:Button ID="btnPost" runat="server" Text="Post Scores" OnClick="btnPost_Click" OnClientClick="return CheckScoresPost(event);" Visible="false" />
<div id="livescorescontrols">
    <a href="javascript:LiveScoresBack()"> &lt;&lt; </a> &nbsp;
    <a href="javascript:LiveScoresPause()" id="livepause"> || </a> 
    <a href="javascript:LiveScoresPlay()" id="liveplay"> &gt; </a> &nbsp;
    <a href="javascript:LiveScoresForward()"> &gt;&gt; </a> &nbsp;
    Today: name (hcp) net over/under par [holes played]
    order by <input type="radio" name="orderby" id="ordername" value="Name" onchange="LiveScoresSort()" checked="checked" /> <label for="ordername">name</label>
    <input type="radio" name="orderby" id="orderscore" value="NetTotal" onchange="LiveScoresSort()" /> <label for="orderscore">score</label>
</div>
<div id="livescores"><div id="liveresults"></div></div>
<div style="margin-top: 20px;"><img src="MonsterLogo.png" alt="Monster Golf" /></div>
<%--<div onclick="return SearchExternal()" style="display:none;">External Course Search</div>
<div id="externalresult" style="display: none;"></div>
--%>
</form>
</body>
</html>

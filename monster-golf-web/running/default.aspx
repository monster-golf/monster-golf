<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="Running.RunningPage" %>

<!DOCTYPE html >

<html>
<head runat="server">
    <title>Year In Workout</title>
    <style>
        body { font-family:Arial; font-size:14px; background-color:#e9e9e9; color:#232323; margin:0px; }
        .TB { font-family:Arial; font-size:13px;padding:2px 4px; border: solid 1px #666666; border-bottom: solid 2px #444444; border-right: solid 2px #444444; -webkit-border-radius: 6px; border-radius: 6px; margin:2px; }
        .Row { position:relative; padding:4px; clear:both; }
        .RowSmall { font-size:12px; position:relative; padding:4px; clear:both; }
        .Panel { margin:8px; padding:4px;border: solid 1px #999999; -webkit-border-radius: 10px; border-radius: 10px; max-width:500px;} 
        .Runner { position:relative; padding:3px; float:left; border: solid 1px transparent; }
        .Runner:hover { border: solid 1px #666666; -webkit-border-radius: 6px; border-radius: 6px; }
        a { text-decoration:none; color:#39C; }
        a:hover { text-decoration:underline; color:#069; }
	    .Btn { font-size: 18px; color:#069; font-weight:bold; padding:3px 15px; margin:5px;}
	    .RunnerAnim { position:relative; width:300px; height:30px; margin-top:10px;border:solid 1px #999999; -webkit-border-radius: 6px; border-radius: 6px; }
	    .RunnerStatus { position:absolute; left:0px; top:14px; width:16px; height:14px; background-color:#67F34E; border-top:solid 2px #0B3A00; -webkit-border-radius:0px 0px 0px 6px; border-radius:0px 0px 0px 6px; }
	    .RunnerIcon { position:absolute;left:0px; top:-1px; }
	    .MonthWorkouts { margin: 5px 5px 5px 10px }
	    .WorkoutRow { margin: 2px; }
    </style>
    <script type="text/javascript" language="javascript" src="XmlHttp.js"></script>
    <script language="javascript" type="text/javascript">
    function CheckSignUpInfo() {
        return true;
    }
    function CheckWorkoutInfo() {
    }

    function ObjTop(obj) {
        var top = 0;
        while(obj) {
            if (obj.offsetTop) top += obj.offsetTop;
            obj = obj.parentNode; 
        }
        return top;
    }
    var workouts;
    function OpenCloseWorkouts() {
        if (!workouts.getAttribute("open")) {
            workouts.style.display = "";
            workouts.setAttribute("open", "1");
            var y = ObjTop(workouts);
            window.scrollTo(0,y);
        } else {
            workouts.style.display = "none";
            workouts.setAttribute("open", "");
        }
    }
    function FillWorkoutDate(dt, wid) {
        workouts = document.getElementById(wid);
        if (workouts) {
            if (!workouts.getAttribute("filled")) GetHTMLAsync("workouts=" + escape(dt), FillInWorkouts);
            else OpenCloseWorkouts();
        }
    }
    function FillInWorkouts(html) {
        if (workouts) {
            workouts.innerHTML = html;
            workouts.setAttribute("filled", "1");
            OpenCloseWorkouts();
        }
    }
    function FillWorkout(workoutid, workoutdate, workouttypeid, workoutminutes, workoutseconds, workoutmiles, workoutcalories) {
        var obj = document.getElementById("hdnWorkoutId");
        if (obj) obj.value = workoutid;
        obj = document.getElementById("txtDate");
        if (obj) obj.value = workoutdate;
        obj = document.getElementById("ddlExercise");
        if (obj) {
            for (var i=0; i<obj.length; i++) {
                if (obj[i].value == workouttypeid) {
                    obj.selectedIndex = i;
                    break;
                }
            }
        }
        obj = document.getElementById("txtMinutes");
        if (obj) obj.value = workoutminutes;
        obj = document.getElementById("txtSeconds");
        if (obj) obj.value = workoutseconds;
        obj = document.getElementById("txtMiles");
        if (obj) obj.value = workoutmiles;
        obj = document.getElementById("txtCalories");
        if (obj) obj.value = workoutcalories;
    }
    var divStatus,icoStatus,finStatus,statusTO;
    function RunIt() {
        var l = parseInt(icoStatus.style.left) + 1;
        if (l > finStatus) finStatus = 1;
        divStatus.style.width = (l+16) + "px";
        icoStatus.style.left = l + "px";
        if (l < finStatus) statusTO = window.setTimeout(RunIt, ((finStatus > 75) ? ((finStatus > 175) ? 25 : 50) : 100));
        else statusTO = null;
    }
    function RunIcon(id,pct) {
        if (statusTO == null) {
            var maxwidth = 300;
            finStatus = parseInt(maxwidth*pct)-16;
            divStatus = document.getElementById("runnerStatus" + id);
            icoStatus = document.getElementById("runnerIcon" + id);
            icoStatus.style.left = "0px";
            RunIt();
        } else window.setTimeout(function() { RunIcon(id,pct); }, 500);
    }
    </script>
</head>
<body id="runningBdy" runat="server">
<form id="running" runat="server">
<asp:HiddenField ID="hdnRunnerId" runat="server" />
<asp:HiddenField ID="hdnWorkoutId" runat="server" />
<asp:Panel id="pnlAllInfo" runat="server" CssClass="Panel" />
<asp:Panel id="pnlSignUp" runat="server" Visible="false" CssClass="Panel">
    <asp:Button id="btnSaveSignUp" runat="server" Text="Save" OnClick="btnSaveSignUp_Click" OnClientClick="return CheckSignUpInfo()" tabindex="1" CssClass="Btn" />
</asp:Panel>
<asp:Panel id="pnlAdd" runat="server" Visible="false" CssClass="Panel">
    <asp:Button id="btnSaveWorkout" runat="server" Text="Add" onClick="btnSaveWorkout_Click" OnClientClick="return CheckWorkoutInfo()" tabindex="1" CssClass="Btn" />
</asp:Panel>
<asp:Panel id="pnlRunner" runat="server" Visible="false" CssClass="Panel">
</asp:Panel>
</form>
</body>
</html>

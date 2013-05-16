using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

namespace Running
{
    public partial class RunningPage : System.Web.UI.Page
    {
        int runnerId = 0;
        string mileagetype = "", workouttype = "";
        int mileageexercise = 0, workoutexercise = 0;
        float mileage = 0, workouts = 0;

        private DropDownList SignUpDDL(string which, string val) { return SignUpDDL(which, val, false); }
        private DropDownList SignUpDDL(string which, string val, bool addall)
        {
            DropDownList ddl = new DropDownList();
            ddl.TabIndex = 1;
            switch (which)
            {
                case "mileage":
                    #region mileage
                    ddl.ID = "ddlProgressMileage";
                    ddl.Items.Add(new ListItem("Mileage per year broken down by month and week", "mileage_year_month_week"));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Mileage per month broken down week", "mileage_month_week"));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Mileage per week", "mileage_week"));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    break;
                    #endregion
                case "workout":
                    #region workout
                    ddl.ID = "ddlProgressWorkout";
                    ddl.Items.Add(new ListItem("Workouts per year broken down by month and week", "workouts_year_month_week"));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Workouts per month broken down week", "workouts_month_week"));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Workouts per week", "workouts_week"));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    break;
                    #endregion
                case "mobile":
                    #region mobile
                    ddl.ID = "ddlMobileService";
                    ddl.Items.Add(new ListItem("AT&T", EMAIL.c_ATT));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Alltel", EMAIL.c_Alltel));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Boost Mobile", EMAIL.c_BoostMobile));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Cellular One", EMAIL.c_CellularOne));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Cingular Wireless", EMAIL.c_CingularWireless));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Cricket", EMAIL.c_Cricket));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Edge Wireless", EMAIL.c_EdgeWireless));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("GTE", EMAIL.c_GTE));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Metrocall", EMAIL.c_Metrocall));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Microcell", EMAIL.c_Microcell));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Midwest Wireless", EMAIL.c_MidwestWireless));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("MobileOne", EMAIL.c_MobileOne));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Mobilfone", EMAIL.c_Mobilfone));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Nextel", EMAIL.c_Nextel));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("PacificBell", EMAIL.c_PacificBell));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Qwest", EMAIL.c_Qwest));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Sprint", EMAIL.c_SprintPCS));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Telus", EMAIL.c_Telus));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("T-Mobile", EMAIL.c_TMobile));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("US Cellular", EMAIL.c_USCellular));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("US West", EMAIL.c_USWest));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Verizon", EMAIL.c_Verizon));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Virgin Mobile", EMAIL.c_VirginMobile));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Vodafone", EMAIL.c_Vodafone));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    ddl.Items.Add(new ListItem("Voice Stream", EMAIL.c_VoiceStream));
                    if (ddl.Items[ddl.Items.Count - 1].Value == val) ddl.Items[ddl.Items.Count - 1].Selected = true;
                    break;
                    #endregion
                case "exercise":
                    #region exercise
                    ddl.ID = "ddlExercise";
                    DB db = new DB();
                    if (addall) ddl.Items.Add(new ListItem("All", "0"));
                    SqlDataReader sdr = db.Get("select WorkoutTypeId, WorkoutType from AW_RunnerWorkoutType order by WorkoutTypeId");
                    while (sdr.Read())
                    {
                        ListItem li = new ListItem(db.GetStr(sdr, "WorkoutType"), db.GetStr(sdr, "WorkoutTypeId"));
                        if (!ddl.Items.Contains(li))
                        {
                            if (li.Value == val) li.Selected = true;
                            ddl.Items.Add(li);
                        }
                    }
                    db.Close(sdr);
                    #endregion
                    break;
            }
            return ddl;
        }
        private void FillAll(DB db)
        {
            //pnlAllInfo.Controls.Clear();
            SqlDataReader athletes = db.Get("select RunnerId,Name,ProgressMileage,Miles,ProgressWorkout,Workouts,ProgressMileageExercise,ProgressWorkoutExercise from AW_Runner order by Name");
            float mileageforyear = 0, workoutsforyear = 0, tempfl = 0;
            while (athletes.Read())
            {
                pnlAllInfo.Controls.Add(WEB.PNL("runner" + db.GetInt(athletes, "RunnerId"), "<a href='default.aspx?id=" + db.GetInt(athletes, "RunnerId") + "'>" + db.GetStr(athletes,"Name") + "</a> | ", "Runner", false));
                string type = db.GetStr(athletes, "ProgressMileage");
                tempfl = db.GetFloat(athletes, "Miles");
                switch (type)
                {
                    case "mileage_month_week": tempfl = tempfl * 12; break;
                    case "mileage_week": tempfl = tempfl * 52; break;
                }
                mileageforyear += tempfl;
                
                type = db.GetStr(athletes, "ProgressWorkout");
                tempfl = db.GetFloat(athletes, "Workouts");
                switch (type)
                {
                    case "workouts_month_week": tempfl = tempfl * 12; break;
                    case "workouts_week": tempfl = tempfl * 52; break;
                }
                workoutsforyear += tempfl;
            }
            pnlAllInfo.Controls.Add(WEB.PNL("runnersignup", "<a href='default.aspx?signup=true'>Join the Group...</a>", "Runner", false));
            db.Close(athletes, false);
            float progressmileageforyear = 0;
            int progressworkoutminutes = 0, progressworkoutseconds = 0, progressworkoutsforyear = 0;
            string sql = "select SUM(CASE WHEN r.ProgressWorkoutExercise IS NULL OR r.ProgressWorkoutExercise = 0 OR r.ProgressWorkoutExercise = w.WorkoutTypeId THEN 1 ELSE 0 END) as Workouts, Sum(CASE WHEN r.ProgressMileageExercise IS NULL OR r.ProgressMileageExercise = 0 OR r.ProgressMileageExercise = w.WorkoutTypeId THEN WorkoutMiles ELSE 0 END) as WorkoutMiles, Sum(WorkoutMinutes) as WorkoutMinutes, Sum(WorkoutSeconds) as WorkoutSeconds from AW_Runner r JOIN AW_RunnerWorkout w ON w.RunnerID = r.RunnerId";
            SqlDataReader workoutsums = db.Get(sql);
            if (workoutsums.Read())
            {
                progressworkoutsforyear = db.GetInt(workoutsums, "Workouts");
                progressmileageforyear = db.GetFloat(workoutsums, "WorkoutMiles");
                progressworkoutminutes = db.GetInt(workoutsums, "WorkoutMinutes");
                progressworkoutseconds = db.GetInt(workoutsums, "WorkoutSeconds");
            }
            db.Close(workoutsums, false);
            WEB.WorkoutTotals(db, pnlAllInfo, "Group", 0, workoutsforyear, mileageforyear, progressworkoutsforyear, progressmileageforyear, progressworkoutminutes, progressworkoutseconds, pnlAllInfo.Controls.Count, true);
            runningBdy.Attributes["onload"] = "RunIcon(0," + (((decimal)progressmileageforyear / (decimal)mileageforyear)).ToString() + ");";
        }
        private void FillMonthRow(int year, int month, int monthcount)
        {
            DateTime workoutdate = new DateTime(year, month, 1);
            string yearmon = year.ToString() + month.ToString();
            Panel pnl = WEB.PNL("monthhldr" + yearmon, "", "RowSmall", false);
            pnlRunner.Controls.Add(pnl);
            pnl.Controls.Add(WEB.BR(string.Format("<a href=\"javascript:FillWorkoutDate('{0}','month{1}')\">{2}</a> - {3} workouts", workoutdate.ToShortDateString(), yearmon, workoutdate.ToString("MMMM/yyyy"), monthcount)));
            pnl.Controls.Add(WEB.BR(string.Format("<div id='month{0}' class='MonthWorkouts'> </div>", yearmon)));
        }
        private void RunnerExercise(DB db)
        {
            pnlRunner.Controls.Clear();
            SqlDataReader exercise = db.Get("SELECT Count(WorkoutId) as WorkoutCount, DATEPART(month,WorkoutDate) as WorkoutMonth, DATEPART(year,WorkoutDate) as WorkoutYear FROM AW_RunnerWorkout w WHERE RunnerId = " + runnerId + " GROUP BY DATEPART(month,WorkoutDate), DATEPART(year,WorkoutDate) ORDER BY DATEPART(month,WorkoutDate) DESC, DATEPART(year,WorkoutDate) DESC");
            while (exercise.Read())
            {
                FillMonthRow(db.GetInt(exercise, "WorkoutYear"), db.GetInt(exercise, "WorkoutMonth"), db.GetInt(exercise, "WorkoutCount"));
            }
            db.Close(exercise, false);
            exercise = db.Get("SELECT t.WorkoutType, t.WorkoutTypeId, Count(*) as Workouts, Sum(WorkoutMiles) as WorkoutMiles, Sum(WorkoutMinutes) as WorkoutMinutes, Sum(WorkoutSeconds) as WorkoutSeconds, Sum(WorkoutCalories) as WorkoutCalories FROM AW_RunnerWorkout w JOIN AW_RunnerWorkoutType t on w.WorkoutTypeId = t.WorkoutTypeId WHERE RunnerId = " + runnerId + " GROUP BY t.WorkoutType, t.WorkoutTypeId ORDER BY t.WorkoutType");
            int workoutseconds = 0, workoutminutes = 0, workoutcalories = 0;
            int workoutcount = 0, progressworkoutsforyear = 0, progressworkoutminutes = 0, progressworkoutseconds = 0;
            float workoutmiles = 0;
            float progressmileageforyear = 0;
            while (exercise.Read())
            {
                workoutcount = db.GetInt(exercise, "Workouts");
                string exerciseworkouttype = db.GetStr(exercise, "WorkoutType");
                string allprogress = string.Format("{0}: {1}", exerciseworkouttype, workoutcount);
                workoutmiles = db.GetFloat(exercise, "WorkoutMiles");
                int workouttypeid = db.GetInt(exercise, "WorkoutTypeId");
                if (workoutexercise == 0 || workoutexercise == workouttypeid) progressworkoutsforyear += workoutcount;
                if (mileageexercise == 0 || mileageexercise == workouttypeid) progressmileageforyear += workoutmiles;
                if (workoutmiles > 0) allprogress += string.Format(" - Mileage: {0}", workoutmiles);
                workoutminutes = 0;
                workoutminutes = db.GetInt(exercise, "WorkoutMinutes");
                if (workoutminutes > 0)
                {
                    workoutseconds = db.GetInt(exercise, "WorkoutSeconds");
                    progressworkoutminutes += workoutminutes;
                    progressworkoutseconds += workoutseconds;
                    workoutminutes += workoutseconds / 60;
                    workoutseconds = workoutseconds % 60;
                    int workouthours = workoutminutes / 60;
                    workoutminutes = workoutminutes % 60;
                    allprogress += string.Format(" Time: {0} hours {1} minutes", workouthours, workoutminutes);
                    if (workoutseconds > 0) allprogress += string.Format(" {0} seconds", workoutseconds);
                }
                workoutcalories = db.GetInt(exercise, "WorkoutCalories");
                if (workoutcalories > 0) allprogress += string.Format(" Calories: {0}", workoutcalories);
                pnlRunner.Controls.AddAt(0, WEB.PNL("runnerworkouts" + exerciseworkouttype.ToLower().Replace(" ", ""), allprogress, "RowSmall", false));
            }
            float workoutsforyear = 0, mileageforyear = 0;
            switch (mileagetype)
            {
                case "mileage_month_week": mileageforyear = mileage * 12; break;
                case "mileage_week": mileageforyear = mileage * 52; break;
                default: mileageforyear = mileage; break;
            }
            switch (workouttype)
            {
                case "workouts_month_week": workoutsforyear = workouts * 12; break;
                case "workouts_week": workoutsforyear = workouts * 52; break;
                default: workoutsforyear = workouts; break;
            }
            WEB.WorkoutTotals(db, pnlRunner, "", runnerId, workoutsforyear, mileageforyear, progressworkoutsforyear, progressmileageforyear, progressworkoutminutes, progressworkoutseconds, 0, true);
            if (mileageforyear > 0) runningBdy.Attributes["onload"] += "RunIcon(" + runnerId + "," + (((decimal)progressmileageforyear / (decimal)mileageforyear)).ToString() + ");";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string name = "", email = "", mobilenum = "", mobileservice = "";
            bool emailweekly = true, emailmonthly = true, txtweekly = false, txtmonthly = false;
            DB db = new DB();
            if (Request.QueryString["id"] != null) hdnRunnerId.Value = Request.QueryString["id"];
            if (!int.TryParse(hdnRunnerId.Value, out runnerId)) runnerId = 0;
            if (runnerId > 0)
            {
                SqlDataReader runner = db.Get("SELECT RunnerId,Name,Email,EmailWeekly,EmailMonthly,TxtPhone,TxtWeekly,TxtMonthly,ProgressMileage,Miles,ProgressWorkout,Workouts,ProgressMileageExercise,ProgressWorkoutExercise FROM AW_Runner WHERE RunnerId = " + runnerId);
                if (runner.Read())
                {
                    name = db.GetStr(runner, "Name");
                    email = db.GetStr(runner, "Email");
                    emailweekly = db.GetBool(runner, "EmailWeekly");
                    emailmonthly = db.GetBool(runner, "EmailMonthly");
                    mobilenum = db.GetStr(runner, "TxtPhone");
                    if (mobilenum.Contains("@"))
                    {
                        mobileservice = mobilenum.Substring(mobilenum.IndexOf("@"));
                        mobilenum = mobilenum.Substring(0, mobilenum.IndexOf("@"));
                    }
                    else mobilenum = "";
                    txtweekly = db.GetBool(runner, "TxtWeekly");
                    txtmonthly = db.GetBool(runner, "TxtMonthly");
                    mileagetype = db.GetStr(runner, "ProgressMileage");
                    mileage = db.GetFloat(runner, "Miles");
                    mileageexercise = db.GetInt(runner, "ProgressMileageExercise");
                    workouttype = db.GetStr(runner, "ProgressWorkout");
                    workouts = db.GetInt(runner, "Workouts");
                    workoutexercise = db.GetInt(runner, "ProgressWorkoutExercise");
                    db.Close(runner, false);
                }
            }

            bool showall = true, showsignup = false, showworkoutform = false, showrunner = false;
            Panel pnl;
            if (Request.QueryString["signup"] != null)
            {
                showsignup = true;
                showall = false;
            }
            if (runnerId > 0 && !showsignup)
            {
                showrunner = true;
                showworkoutform = true;
            }
            if (showall && !IsPostBack) FillAll(db);
            pnlAllInfo.Visible = showall;
            #region signup form
            if (showsignup)
            {
                pnl = WEB.PNL("namerow", "Name: ", "Row", false);
                pnl.Controls.Add(WEB.TB("txtName", name, "TB", 100, "", "", true, false, 200));
                pnlSignUp.Controls.Add(pnl);
                pnl = WEB.PNL("emailrow", "Email: ", "Row", false);
                pnl.Controls.Add(WEB.TBEM("txtEmail", email, "TB", 100, "", "", true, false, 200));
                pnl.Controls.Add(WEB.BR(""));
                pnl.Controls.Add(WEB.CB("chkEmailWeekly", "Email weekly progress?", emailweekly, "", "", false));
                pnl.Controls.Add(WEB.BR(""));
                pnl.Controls.Add(WEB.CB("chkEmailMonthly", "Email monthly progress?", emailmonthly, "", "", false));
                pnlSignUp.Controls.Add(pnl);
                pnl = WEB.PNL("textmessagerow", "If you would like to receive text progress please provide your mobile phone number, select your mobile service provider, and check whether you want weekly or monthly progress.", "Row", false);
                pnlSignUp.Controls.Add(pnl);
                pnl = WEB.PNL("mobilenumrow", "Mobile Number: ", "Row", false);
                pnl.Controls.Add(WEB.TB("txtMobileNum", mobilenum, "TB", 20, "", "", true, false, 200));
                pnlSignUp.Controls.Add(pnl);
                pnl = WEB.PNL("mobileservicerow", "Mobile Service: ", "Row", false);
                pnl.Controls.Add(SignUpDDL("mobile", mobileservice));
                pnl.Controls.Add(WEB.BR(""));
                pnl.Controls.Add(WEB.CB("chkTxtWeekly", "Text weekly progress?", txtweekly, "", "", false));
                pnl.Controls.Add(WEB.BR(""));
                pnl.Controls.Add(WEB.CB("chkTxtMonthly", "Text monthly progress?", txtmonthly, "", "", false));
                pnlSignUp.Controls.Add(pnl);
                pnl = WEB.PNL("progressmessagerow", "Select how you would like to track work workouts.", "Row", false);
                pnlSignUp.Controls.Add(pnl);
                pnl = WEB.PNL("mileagerow", "Mileage Progress: ", "Row", false);
                pnl.Controls.Add(SignUpDDL("mileage", mileagetype));
                pnl.Controls.Add(WEB.BR(""));
                pnl.Controls.Add(WEB.BR("Miles: "));
                pnl.Controls.Add(WEB.TBNUM("txtProgressMiles", mileage, "TB", 10, "", "", true, false, 100));
                pnl.Controls.Add(WEB.BR(" For Exercise: "));
                pnl.Controls.Add(SignUpDDL("exercise", mileageexercise.ToString(), true));
                pnl.Controls[pnl.Controls.Count - 1].ID = "ddlProgressMileageExercise";
                pnlSignUp.Controls.Add(pnl);
                pnl = WEB.PNL("workoutrow", "Workout Progress: ", "Row", false);
                pnl.Controls.Add(SignUpDDL("workout", workouttype));
                pnl.Controls.Add(WEB.BR(""));
                pnl.Controls.Add(WEB.BR("Workouts: "));
                pnl.Controls.Add(WEB.TBNUM("txtProgressWorkouts", workouts, "TB", 10, "", "", true, false, 100));
                pnl.Controls.Add(WEB.BR(" For Exercise: "));
                pnl.Controls.Add(SignUpDDL("exercise", workoutexercise.ToString(), true));
                pnl.Controls[pnl.Controls.Count - 1].ID = "ddlProgressWorkoutsExercise";
                pnlSignUp.Controls.Add(pnl);
                pnlSignUp.Controls.Add(btnSaveSignUp);
            }
            pnlSignUp.Visible = showsignup;
            #endregion
            #region worktout form
            if (showworkoutform)
            {
                pnlAdd.Controls.Add(WEB.PNL("exercisename", name + " <a href='default.aspx?signup=1&id=" + runnerId + "'>modify</a>", "Row", false));
                pnl = WEB.PNL("daterow", "Date: ", "Row", false);
                pnl.Controls.Add(WEB.TBDT("txtDate", DateTime.MinValue, "TB", "", "", true, false, 75));
                pnlAdd.Controls.Add(pnl);
                pnl = WEB.PNL("exerciserow", "Exercise: ", "Row", false);
                pnl.Controls.Add(SignUpDDL("exercise", ""));
                pnlAdd.Controls.Add(pnl);
                pnl = WEB.PNL("milesrow", "Miles: ", "Row", false);
                pnl.Controls.Add(WEB.TBNUM("txtMiles", 0, "TB", 10, "", "", true, false, 60));
                pnlAdd.Controls.Add(pnl);
                pnl = WEB.PNL("minutesrow", "Minutes: ", "Row", false);
                pnl.Controls.Add(WEB.TBNUM("txtMinutes", 0, "TB", 10, "", "", true, false, 60));
                pnlAdd.Controls.Add(pnl);
                pnl = WEB.PNL("secondsrow", "Seconds: ", "Row", false);
                pnl.Controls.Add(WEB.TBNUM("txtSeconds", 0, "TB", 10, "", "", true, false, 60));
                pnlAdd.Controls.Add(pnl);
                pnl = WEB.PNL("caloriesrow", "Calories: ", "Row", false);
                pnl.Controls.Add(WEB.TBNUM("txtCalories", 0, "TB", 10, "", "", true, false, 60));
                pnlAdd.Controls.Add(pnl);
                pnlAdd.Controls.Add(btnSaveWorkout);
            }
            pnlAdd.Visible = showworkoutform;
            #endregion
            if (showrunner) RunnerExercise(db);
            pnlRunner.Visible = showrunner;
            db.Close();
        }
        protected void btnSaveSignUp_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            string cmd;
            float progress;
            int exercise;
            string txtnum = Request.Form["txtMobileNum"];
            if (txtnum != null)
            {
                txtnum = txtnum.Replace("-", "");
                txtnum = txtnum.Replace("(", "");
                txtnum = txtnum.Replace(")", "");
                txtnum = txtnum.Replace(" ", "");
                txtnum = txtnum.Replace(".", "");
                if (txtnum != "") txtnum += Request.Form["ddlMobileService"];
            }
            if (runnerId == 0)
            {
                cmd = "INSERT INTO AW_Runner (Name, Email, EmailWeekly, EmailMonthly, TxtPhone, TxtWeekly, TxtMonthly, ProgressMileage, Miles, ProgressWorkout, Workouts, ProgressMileageExercise, ProgressWorkoutExercise) VALUES (";
                cmd += DB.stringSql(Request.Form["txtName"]) + ",";
                cmd += DB.stringSql(Request.Form["txtEmail"]) + ",";
                cmd += ((Request.Form["chkEmailWeekly"] == null) ? "0" : "1") + ",";
                cmd += ((Request.Form["chkEmailMonthly"] == null) ? "0" : "1") + ",";
                cmd += DB.stringSql(txtnum) + ",";
                cmd += ((Request.Form["chkTxtWeekly"] == null) ? "0" : "1") + ",";
                cmd += ((Request.Form["chkTxtMonthly"] == null) ? "0" : "1") + ",";
                cmd += DB.stringSql(Request.Form["ddlProgressMileage"]) + ",";
                if (!float.TryParse(Request.Form["txtProgressMiles"], out progress)) progress = 0;
                cmd += progress + ",";
                cmd += DB.stringSql(Request.Form["ddlProgressWorkout"]) + ",";
                if (!float.TryParse(Request.Form["txtProgressWorkouts"], out progress)) progress = 0;
                cmd += progress + ",";
                if (!int.TryParse(Request.Form["ddlProgressMileageExercise"], out exercise)) exercise = 0;
                cmd += exercise + ",";
                if (!int.TryParse(Request.Form["ddlProgressWorkoutExercise"], out exercise)) exercise = 0;
                cmd += exercise + ")";
            }
            else
            {
                cmd = "UPDATE AW_Runner SET ";
                cmd += " Name = " + DB.stringSql(Request.Form["txtName"]);
                cmd += ",Email = " + DB.stringSql(Request.Form["txtEmail"]);
                cmd += ",EmailWeekly = " + ((Request.Form["chkEmailWeekly"] == null) ? "0" : "1");
                cmd += ",EmailMonthly = " + ((Request.Form["chkEmailMonthly"] == null) ? "0" : "1");
                cmd += ",TxtPhone = " + DB.stringSql(txtnum);
                cmd += ",TxtWeekly = " + ((Request.Form["chkTxtWeekly"] == null) ? "0" : "1");
                cmd += ",TxtMonthly = " + ((Request.Form["chkTxtMonthly"] == null) ? "0" : "1");
                cmd += ",ProgressMileage = " + DB.stringSql(Request.Form["ddlProgressMileage"]);
                if (!float.TryParse(Request.Form["txtProgressMiles"], out progress)) progress = 0;
                cmd += ",Miles = " + progress;
                cmd += ",ProgressWorkout = " + DB.stringSql(Request.Form["ddlProgressWorkout"]);
                if (!float.TryParse(Request.Form["txtProgressWorkouts"], out progress)) progress = 0;
                cmd += ",Workouts = " + progress;
                if (!int.TryParse(Request.Form["ddlProgressMileageExercise"], out exercise)) exercise = 0;
                cmd += ",ProgressMileageExercise = " + exercise;
                if (!int.TryParse(Request.Form["ddlProgressWorkoutExercise"], out exercise)) exercise = 0;
                cmd += ",ProgressWorkoutExercise = " + exercise;
                cmd += " WHERE RunnerId = " + runnerId;
            }
            db.Exec(cmd);
            if (runnerId == 0)
            {
                cmd = "SELECT MAX(RunnerId) AS RunnerId FROM AW_Runner";
                SqlDataReader sdr = db.Get(cmd);
                if (sdr.Read()) runnerId = db.GetInt(sdr, "RunnerId");
            }
            else db.Close();
            Response.Redirect("Default.aspx?id=" + runnerId);
        }
        protected void btnSaveWorkout_Click(object sender, EventArgs e)
        {
            int workoutId;
            if (!int.TryParse(hdnWorkoutId.Value, out workoutId)) workoutId = 0;

            DB db = new DB();
            string cmd;
            float miles;
            if (!float.TryParse(Request.Form["txtMiles"], out miles)) miles = 0;
            int minutes, seconds, calories;
            if (!int.TryParse(Request.Form["txtMinutes"], out minutes)) minutes = 0;
            if (!int.TryParse(Request.Form["txtSeconds"], out seconds)) seconds = 0;
            if (!int.TryParse(Request.Form["txtCalories"], out calories)) calories = 0;
            if (workoutId == 0)
            {
                cmd = "INSERT INTO AW_RunnerWorkout (RunnerId, WorkoutDate, WorkoutTypeId, WorkoutMiles, WorkoutMinutes, WorkoutSeconds, WorkoutCalories) VALUES (";
                cmd += runnerId + ",";
                cmd += DB.stringSql(Request.Form["txtDate"]) + ",";
                cmd += Request.Form["ddlExercise"] + ",";
                cmd += miles + ",";
                cmd += minutes + ",";
                cmd += seconds + ",";
                cmd += calories + ")";
            }
            else
            {
                cmd = "UPDATE AW_RunnerWorkout SET ";
                cmd += "WorkoutDate = " + DB.stringSql(Request.Form["txtDate"]) + ",";
                cmd += "WorkoutTypeId = " + Request.Form["ddlExercise"] + ",";
                cmd += "WorkoutMiles = " + miles + ",";
                cmd += "WorkoutMinutes = " + minutes + ",";
                cmd += "WorkoutSeconds = " + seconds + ",";
                cmd += "WorkoutCalories = " + calories + " WHERE WorkoutId= " + workoutId;
            }
            db.Exec(cmd);
            //FillAll(db);
            //RunnerExercise(db);
            db.Close();
            Response.Redirect("Default.aspx?id=" + runnerId);
        }
    }
}
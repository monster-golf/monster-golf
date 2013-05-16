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
    public partial class Notify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime checkDate = DateTime.Now;
            bool issunday = checkDate.DayOfWeek == DayOfWeek.Sunday;
            bool isfirstdayofmonth = checkDate.Day == 1;
            if (issunday || isfirstdayofmonth)
            {
                DB db = new DB();
                string sql = @"SELECT r.RunnerId,r.Name, r.Email, r.EmailWeekly, r.EmailMonthly, r.TxtPhone, r.TxtWeekly, r.TxtMonthly, r.ProgressMileage, r.Miles, r.ProgressWorkout, r.Workouts, 
                                SUM(CASE WHEN r.ProgressWorkoutExercise IS NULL OR r.ProgressWorkoutExercise = 0 OR r.ProgressWorkoutExercise = w.WorkoutTypeId THEN 1 ELSE 0 END) AS WorkoutCount
                               ,SUM(CASE WHEN r.ProgressMileageExercise IS NULL OR r.ProgressMileageExercise = 0 OR r.ProgressMileageExercise = w.WorkoutTypeId THEN WorkoutMiles ELSE 0 END) AS WorkoutMiles
                               ,SUM(w.WorkoutMinutes) AS WorkoutMinutes
                               ,SUM(w.WorkoutSeconds) AS WorkoutSeconds
                               ,SUM(w.WorkoutCalories) AS WorkoutCalories
                           FROM AW_Runner r
                                LEFT JOIN AW_RunnerWorkout w ON w.RunnerId = r.RunnerId AND WorkoutDate between ";
                DateTime startdate, enddate;
                enddate = checkDate.Subtract(TimeSpan.FromDays(1));
                if (isfirstdayofmonth) startdate = new DateTime(enddate.Year, enddate.Month, 1);
                else startdate = enddate.Subtract(TimeSpan.FromDays(6));
                sql += DB.stringSql(startdate.ToShortDateString());
                sql += " AND ";
                sql += DB.stringSql(enddate.ToShortDateString() + " 23:59:59");
                //sql += " WHERE r.RunnerId = 1 ";
                sql += " GROUP BY r.RunnerId,r.Name, r.Email, r.EmailWeekly, r.EmailMonthly, r.TxtPhone, r.TxtWeekly, r.TxtMonthly,r.ProgressMileage,r.Miles,r.ProgressWorkout,r.Workouts";
                SqlDataReader sdr = db.Get(sql);
                while (sdr.Read())
                {
                    int runnerid = db.GetInt(sdr, "RunnerId");
                    string name = db.GetStr(sdr, "Name");
                    string email = db.GetStr(sdr, "Email");
                    bool emailweekly = db.GetBool(sdr, "EmailWeekly");
                    bool emailmonthly = db.GetBool(sdr, "EmailMonthly");
                    string mobiletxt = db.GetStr(sdr, "TxtPhone");
                    if (!mobiletxt.Contains("@")) mobiletxt = "";
                    bool txtweekly = db.GetBool(sdr, "TxtWeekly");
                    bool txtmonthly = db.GetBool(sdr, "TxtMonthly");
                    float mileagepledge = db.GetFloat(sdr, "Miles"), mileageforweek = 0, mileageformonth = 0, mileageforyear = 0;
                    float workoutspledge = db.GetFloat(sdr, "Workouts"), workoutsforweek = 0, workoutsformonth = 0, workoutsforyear = 0;
                    string mileagetype = db.GetStr(sdr, "ProgressMileage");
                    string workouttype = db.GetStr(sdr, "ProgressWorkout");
                    switch (mileagetype)
                    {
                        case "mileage_month_week":
                            mileageforweek = mileagepledge / (float)4.3;
                            mileageformonth = mileagepledge;
                            mileageforyear = mileagepledge * 12;
                            break;
                        case "mileage_week":
                            mileageforweek = mileagepledge;
                            mileageformonth = mileagepledge * (float)4.3;
                            mileageforyear = mileagepledge * 52;
                            break;
                        default:
                            mileageforweek = mileagepledge / 52;
                            mileageformonth = mileagepledge / 12;
                            mileageforyear = mileagepledge;
                            break;
                    }
                    switch (workouttype)
                    {
                        case "workouts_month_week":
                            workoutsforweek = workoutspledge / (float)4.3;
                            workoutsformonth = workoutspledge;
                            workoutsforyear = workoutspledge * 12;
                            break;
                        case "workouts_week":
                            workoutsforweek = workoutspledge;
                            workoutsformonth = workoutspledge * (float)4.3;
                            workoutsforyear = workoutspledge * 52;
                            break;
                        default:
                            workoutsforweek = workoutspledge / 52;
                            workoutsformonth = workoutspledge / 12;
                            workoutsforyear = workoutspledge;
                            break;
                    }
                    float progressworkoutmiles = db.GetFloat(sdr, "WorkoutMiles");
                    int progressworkoutscount = db.GetInt(sdr, "WorkoutCount");
                    int progressworkoutminutes = db.GetInt(sdr, "WorkoutMinutes");
                    int progressworkoutseconds = db.GetInt(sdr, "WorkoutSeconds");
                    Panel emailPnl = new Panel();
                    if (isfirstdayofmonth)
                    {
                        string progress = WEB.WorkoutTotals(db, emailPnl, ((isfirstdayofmonth) ? "Month Totals" : "Week Totals"), runnerid, workoutsformonth, mileageformonth, progressworkoutscount, progressworkoutmiles, progressworkoutminutes, progressworkoutseconds, 0, false);
                        //progress += "\nhttp://aaronwald.net/running/?id=" + runnerid;
                        emailPnl.Controls.Add(WEB.BR("<a href='http://aaronwald.net/running/?id=" + runnerid + "'>Click to View Online</a>"));
                        if (txtweekly && mobiletxt != "") EMAIL.SendMessage(mobiletxt, "", progress, false, null, Server);
                        if (emailweekly && email != "") EMAIL.SendMessage(email + ":" + name, "Workout Monthly Update", WEB.WriteControl(emailPnl, "body"), true, null, Server);
                        Response.Write("Monthly's Sent to " + name + "<br/>");
                    }
                    else
                    {
                        string progress = WEB.WorkoutTotals(db, emailPnl, ((isfirstdayofmonth) ? "Month Totals" : "Week Totals"), runnerid, workoutsforweek, mileageforweek, progressworkoutscount, progressworkoutmiles, progressworkoutminutes, progressworkoutseconds, 0, false);
                        //progress += "\nhttp://aaronwald.net/running/?id=" + runnerid;
                        emailPnl.Controls.Add(WEB.BR("<a href='http://aaronwald.net/running/?id=" + runnerid + "'>Click to View Online</a>"));
                        if (txtweekly && mobiletxt != "") EMAIL.SendMessage(mobiletxt, "", progress, false, null, Server);
                        if (emailweekly && email != "") EMAIL.SendMessage(email + ":" + name, "Workout Weekly Update", WEB.WriteControl(emailPnl, "body"), true, null, Server);
                        Response.Write("Weekly's Sent to " + name + "<br/>");
                    }
                }
            }
            Response.Write("<br/><a href='default.aspx'>Done</a>");
        }
    }
}
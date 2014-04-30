using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;
namespace Running
{
    public partial class RunningXml : System.Web.UI.Page
    {
        private void RunnerExerciesByMonth(DB db)
        {
            if (Request["workouts"] != null)
            {
                DateTime month;
                DateTime.TryParse(Request["workouts"], out month);
                Panel pnlRunner = new Panel();
                DateTime monthDay1 = new DateTime(month.Year, month.Month, 1);
                DateTime monthDayLast = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month));
                SqlDataReader exercise = db.Get("SELECT WorkoutId, WorkoutDate, w.WorkoutTypeId, t.WorkoutType, WorkoutMiles, WorkoutMinutes, WorkoutSeconds, WorkoutCalories FROM AW_RunnerWorkout w JOIN AW_RunnerWorkoutType t on w.WorkoutTypeId = t.WorkoutTypeId WHERE RunnerId = " + runnerId + " AND WorkoutDate BETWEEN '" + monthDay1.ToShortDateString() + "' AND '" + monthDayLast.ToShortDateString() + " 23:59:59' ORDER BY WorkoutDate DESC, WorkoutId DESC");
                int workoutid = 0, workoutseconds = 0, workoutminutes = 0, workoutcalories = 0;
                float workoutmiles = 0;
                StringBuilder sb = new StringBuilder("<div>");
                while (exercise.Read())
                {
                    workoutid = db.GetInt(exercise, "WorkoutId");
                    string exerciseid = "exercise" + workoutid;
                    Panel pnl = WEB.PNL(exerciseid, "", "RowSmall", false);
                    pnlRunner.Controls.AddAt(0, pnl);
                    DateTime workoutdate = db.GetDate(exercise, "WorkoutDate");
                    int exercisetypeid = db.GetInt(exercise, "WorkoutTypeId");
                    string exercisetype = db.GetStr(exercise, "WorkoutType");
                    workoutmiles = db.GetFloat(exercise, "WorkoutMiles");
                    workoutminutes = db.GetInt(exercise, "WorkoutMinutes");
                    workoutseconds = db.GetInt(exercise, "WorkoutSeconds");
                    workoutcalories = db.GetInt(exercise, "WorkoutCalories");
                    sb.AppendFormat("<div class='WorkoutRow'><a href=\"javascript:FillWorkout({0},'{1}','{2}',{3},{4},{5},{6})\">{7}</a> - ", workoutid, workoutdate.ToShortDateString(), exercisetypeid, workoutminutes, workoutseconds, workoutmiles, workoutcalories, workoutdate.ToString("MM-dd-yy"));
                    sb.Append(exercisetype);
                    if (workoutmiles > 0) sb.AppendFormat(" {0} miles", workoutmiles);
                    if (workoutminutes > 0) sb.AppendFormat(", {0} minutes", workoutminutes);
                    if (workoutseconds > 0) sb.AppendFormat(", {0} seconds", workoutseconds);
                    if (workoutcalories > 0) sb.AppendFormat(" {0} calories", workoutcalories);
                    sb.Append("</div>");
                }
                db.Close(exercise);
                sb.Append("</div>");
                WEB.WriteEndResponse(Response, sb); 
            }
        }
        DB db;
        int runnerId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["runnerid"] != null)
            {
                if (!int.TryParse(Request.QueryString["runnerid"], out runnerId)) runnerId = 0;
            }
            db = new DB();
            RunnerExerciesByMonth(db);
            Response.Write("<complete>true</complete>");
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            db.Close();
        }
    }
}
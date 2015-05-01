using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Tourney : System.Web.UI.Page
{
    DB db = new DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        int roundnum, tourneyid;
        SqlDataReader sdr;
        bool tourneyadded = false;

        if (Page.IsPostBack)
        {
            if (Request.Form["newslogan"] != "")
            {
                string sql = "INSERT into mg_Tourney (TournamentID, Slogan, Description, Location, NumRounds) ";
                sql += "SELECT Max(TournamentID) + 1, ";
                sql += DB.stringSql(Request.Form["newslogan"], true) + ",";
                sql += DB.stringSql(Request.Form["newdescription"], true) + ",";
                sql += DB.stringSql(Request.Form["newlocation"], true) + ",";
                sql += "1 FROM mg_Tourney ";
                db.Exec(sql);
                tourneyadded = true;
            }
        }
        if (int.TryParse(Request["r"], out roundnum) &&
            int.TryParse(Request["t"], out tourneyid))
        {
            WEB w = new WEB();
            ddTourney.Visible = false;
            updateUsers.Visible = false;
            Literal lit = new Literal();
            string tourneyName, courseName, dateStr, actualDateStr;
            List<ScoreInfo> courseInfo = ScoreInfo.LoadCourseInfo(tourneyid.ToString(), roundnum.ToString(), true, out tourneyName, out dateStr, out actualDateStr, out courseName);
            lit.Text = tourneyName + "  -  Round: " + roundnum + " on " + dateStr + " at " + courseName;
            titleTag.Text = lit.Text;
            tourneyInfo.Controls.Add(lit);
            lit = new Literal();
            lit.Text = w.TourneyScores(db, tourneyid, roundnum, false, Request["p"] == null && DateTime.Now < DateTime.Parse(actualDateStr).AddDays(2), "Name").ToString();
            tourneyResults.Controls.Add(lit);
            if (Request["p"] == null)
            {
                emailScores.Visible = true;
                lit = new Literal();
                lit.Text = "<a href='javascript:EmailScores(" + roundnum + ")'>Email Results</a>";
                emailScores.Controls.Add(lit);
            }
            else
            {
                monsterLogo.Visible = false;
                stylDef.InnerHtml += "\nbody{background-color:#ffffff;color:#000000;}\n.Detail_Head { color:#000000;background-color:#dddddd; }\n.Detail1_Head { color:#000000;background-color:#dddddd; }";
            }
        }
        else
        {
            if (tourneyadded)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "viewtourney", "TourneyDetails();", true);
            }
            emailScores.Visible = false;
            sdr = db.Get("select TournamentId,Slogan from mg_Tourney order by TournamentId desc");
            ddTourney.Items.Add(new ListItem("--select--", ""));
            while (sdr.Read())
            {
                ddTourney.Items.Add(new ListItem(sdr[1].ToString(), sdr[0].ToString()));
                if (tourneyadded)
                {
                    ddTourney.SelectedIndex = ddTourney.Items.Count - 1;
                    tourneyadded = false;
                }
            }
        }
    }

    protected override void OnUnload(EventArgs e)
    {
        base.OnUnload(e);
        db.Close();
    }
}

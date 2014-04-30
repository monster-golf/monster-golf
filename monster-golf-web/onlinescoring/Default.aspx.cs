using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using docusign;
using System.IO;
using System.Text;


public partial class _Default : System.Web.UI.Page
{
    private string PlayerInitials(string name)
    {
        string inits = "";
        if (name != "")
        {
            foreach (string sname in name.Split(' '))
            {
                if (sname != "") inits += sname.Substring(0, 1);
                if (inits.Length == 2) break;
            }
        }
        return inits;
    }
    private Panel Scores(string rowid, ScoreInfo si, bool live, bool last)
    {
        return Scores(rowid, si, live, last, 0, 0);
    }
    private Panel Scores(string rowid, ScoreInfo si, bool live, bool last, int row, int numplayers)
    {
        Panel pnl = new Panel();
        pnl.ID = (rowid == "") ? "" : rowid + "_" + si.ID;
        pnl.Attributes.Add("userid", si.ID);
        pnl.Attributes.Add("name", si.Name);
        pnl.Attributes.Add("hcp", si.HCP);
        pnl.Attributes.Add("userlookup", si.LookupID);
        pnl.CssClass = "ScoresRow";
        string css = last ? " ScoreLast" : "";
        string namekey = "";
        if (tourneyId.Value == "" && live)
        {
            namekey = "return KeyEntToSearch(this,event);";
            pnl.Controls.Add(WEB.IMG("score_search_" + si.ID, "~/find.png", "PlayerSearch", "PlayerSearch(this)", true));
        }
        pnl.Controls.Add(WEB.TB("score_name_" + si.ID, si.Name, "Score ScoreName" + css, 100, namekey, "", tourneyId.Value == "" && live, false, 0));
        int tabidsub = 1;
        if (tourneyId.Value == "" && live)
        {
            ((WebControl)pnl.Controls[pnl.Controls.Count - 1]).Attributes.Add("onfocus", "PlayerFocus(this,event)");
            ((WebControl)pnl.Controls[pnl.Controls.Count - 1]).Attributes.Add("onblur", "PlayerBlur(this,event)");
            ((TextBox)pnl.Controls[pnl.Controls.Count - 1]).TabIndex = (short)row;
            tabidsub = 0;
        }
        int tabindex = 0;
        for (int x = 1; x <= 18; x++)
        {
            if (Request.QueryString["overwrite"] != "1") tabindex = (live && row > 0) ? ((numplayers * (x - tabidsub)) + row) : 0;
            string cssadd = css;
            if (Request.QueryString["h"] == x.ToString()) cssadd += " ScoreHighlight";
            pnl.Controls.Add(WEB.TB("score_" + x + "_" + si.ID, si.Scores[x.ToString()], "Score ScoreHole" + cssadd, 2, "return ScoresAdd(this,event)", "", "return ScoresFocus(this,event)", "return ScoresBlur(this,event)", live, false, true, ((x < 10) ? "f" : "b"), tabindex));
            if (x == 9)
            {
                pnl.Controls.Add(WEB.TB("score_out_" + si.ID, si.Out, "Score ScoreTotal" + css, 3, "", "", false, false, 1));
                pnl.Controls.Add(WEB.TB("score_initials_" + si.ID, PlayerInitials(si.Name), "Score ScoreTotal" + css, 3, "", "", (tourneyId.Value == "" && live), false, 2));
                ((TextBox)pnl.Controls[pnl.Controls.Count - 1]).TabIndex = -1;
            }
        }
        pnl.Controls.Add(WEB.TB("score_in_" + si.ID, si.In, "Score ScoreTotal" + css, 3, "", "", false, false, 1));
        pnl.Controls.Add(WEB.TB("score_total_" + si.ID, si.Total, "Score ScoreTotal " + css, 3, "", "", false, false, 2));
        if (Request.QueryString["overwrite"] != "1" && tourneyId.Value == "" && live && row > 0) tabindex = (live && row > 0) ? ((numplayers * (19 - tabidsub)) + row) : 0;
        else tabindex = 0;
        pnl.Controls.Add(WEB.TB("score_hcp_" + si.ID, si.HCP, "Score ScoreHCP" + css, 2, "", "return ScoresAdd(this,event)", "return ScoresFocus(this,event)", "return ScoresBlur(this,event)", tourneyId.Value == "" && live, false, true, "", tabindex));
        pnl.Controls.Add(WEB.TB("score_net_" + si.ID, si.Net, "Score ScoreNet" + css, 3, "", "", false, false, 2));
        return pnl;
    }
    List<ScoreInfo> players = new List<ScoreInfo>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["signed"] == "1")
        {
            pnlScorerSign.Visible = false;
            Literal lit = new Literal();
            lit.Text = "Post your scores to Monster Golf Handicap?<br/><br/>";
            pnlScoresList.Controls.Add(lit);

            tourneyId.Value = Request.QueryString["t"];
            roundNum.Value = Request.QueryString["r"];
            scorerId.Value = Request.QueryString["u"];

            pnlPlayerHelp.Visible = tourneyId.Value == "";

            if (tourneyId.Value == "") players = ScoreInfo.LoadGroup(roundNum.Value);
            else players = ScoreInfo.LoadGroup(tourneyId.Value, roundNum.Value, scorerId.Value);

            pnlScoresList.Controls.Add(WEB.PNL("", "Validate the Slope and Rating before you post your scores", "", false));
            Panel pnl = new Panel();
            pnlScoresList.Controls.Add(pnl);
            pnl.CssClass = "ScoresRow";
            pnl.Controls.Add(WEB.TB("namelbl", "Name", "Score ScoreName", 100, "", "", false, false, 0));
            pnl.Controls.Add(WEB.TB("scorelbl", "Tot", "Score ScorePost", 3, "", "", false, false, 3));
            pnl.Controls.Add(WEB.TB("courselbl", "Course", "Score ScoreCourseName", 100, "", "", false, false, 0));
            pnl.Controls.Add(WEB.TB("slopelbl", "Slope", "Score ScorePost", 3, "", "", false, false, 3));
            pnl.Controls.Add(WEB.TB("ratinglbl", "Rating", "Score ScoreRating", 4, "", "", false, false, 3));

            int x = 1, userid = 0;
            foreach (ScoreInfo player in players)
            {
                userid = 0;
                int.TryParse(player.ID, out userid);
                if (userid <= 5)
                {
                    pnl.Controls.Add(WEB.TB("emaillbl", "Email", "Score ScoreCourseName", 100, "", "", false, false, 0));
                    break;
                }
            }

            foreach (ScoreInfo player in players)
            {
                userid = 0;
                int.TryParse(player.ID, out userid);
                string css = x == players.Count ? " ScoreLast" : "";

                string idform = "{0}_" + x++ + "_" + userid;
                pnl = new Panel();
                pnlScoresList.Controls.Add(pnl);
                pnl.ID = string.Format(idform, "player");
                pnl.Attributes.Add("userid", pnl.ID);
                pnl.CssClass = "ScoresRow";
                pnl.Controls.Add(WEB.TB(string.Format(idform, "name"), player.Name, "Score ScoreName" + css, 100, "", "", false, false, 0));
                pnl.Controls.Add(WEB.TB(string.Format(idform, "score"), player.Total, "Score ScorePost" + css, 3, "", "", true, false, 3));
                pnl.Controls.Add(WEB.TB(string.Format(idform, "course"), player.CourseName, "Score ScoreCourseName" + css, 100, "", "", true, false, 0));
                pnl.Controls.Add(WEB.TB(string.Format(idform, "slope"), player.CourseSlope, "Score ScorePost" + css, 3, "", "", true, false, 3));
                pnl.Controls.Add(WEB.TB(string.Format(idform, "rating"), player.CourseRating, "Score ScoreRating" + css, 4, "", "", true, false, 3));
                if (userid <= 5)
                {
                    pnl.Controls.Add(WEB.TB(string.Format(idform, "email"), "", "Score ScoreCourseName" + css, 100, "", "", true, false, 0));
                }
                pnl.Controls.Add(WEB.CB(string.Format(idform, "tournament"), "Tourney?", tourneyId.Value != "", "ScoreTourney", "", tourneyId.Value != ""));
            }
            btnPost.Visible = true;
        }
        else
        {
            pnlScorerSign.Style.Add(HtmlTextWriterStyle.Display, "none");
            if (!Page.IsPostBack)
            {
                tourneyId.Value = Request.QueryString["t"];
                roundNum.Value = Request.QueryString["r"];
                scorerId.Value = Request.QueryString["u"];

                if (tourneyId.Value == "")
                {
                    if (roundNum.Value != "") players = ScoreInfo.LoadGroup(roundNum.Value);
                    int i = 1;
                    for (i = players.Count + 1; i <= 5; i++) players.Add(new ScoreInfo(i.ToString()));
                    i = 1;
                    foreach (ScoreInfo player in players)
                    {
                        if (player.ID == "0" || player.ID == "") player.ID = i.ToString();
                        i++;
                    }
                    pnlFindCourse.Controls.Add(WEB.PNL("courseHeader", "Course Name &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp; &nbsp; &nbsp;&nbsp; Slope  Rating", "", roundNum.Value == ""));
                    pnlFindCourse.Controls.Add(WEB.TB("courseName", players[0].CourseName, "CourseInfo CourseName", 100, "", "", true, roundNum.Value == "", 0));
                    pnlFindCourse.Controls.Add(WEB.TB("courseSlope", players[0].CourseSlope, "CourseInfo", 3, "", "SlopeChange(this)", true, roundNum.Value == "", 2));
                    pnlFindCourse.Controls.Add(WEB.TB("courseRating", players[0].CourseRating, "CourseInfo", 4, "", "", true, roundNum.Value == "", 2));
                    pnlFindCourse.Controls.Add(WEB.CB("showF9", "Show Front 9", true, "CourseInfo", "return Show9(this,'f',event);", roundNum.Value == ""));
                    pnlFindCourse.Controls.Add(WEB.CB("showB9", "Show Back 9", true, "CourseInfo", "return Show9(this,'b',event);", roundNum.Value == ""));
                    pnlFindCourse.Visible = true;
                    if (roundNum.Value == "")
                    {
                        roundNum.Value = ScoreInfo.GetId();
                        pnlScoresList.Style.Add(HtmlTextWriterStyle.Display, "none");
                        btnChangeCourse.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                    else
                    {
                        txtFindCourse.Style.Add(HtmlTextWriterStyle.Display, "none");
                        btnFindCourse.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                }
                else players = ScoreInfo.LoadGroup(tourneyId.Value, roundNum.Value, scorerId.Value);
            }
            else
            {
                if (tourneyId.Value == "")
                {
                    if (roundNum.Value != "")
                    {
                        players = ScoreInfo.LoadGroup(roundNum.Value);
                        //foreach (ScoreInfo player in players) FillReqPlayer(player, player.ID);
                    }
                    else
                    {
                        players.Add(FillReqPlayer("1"));
                        players.Add(FillReqPlayer("2"));
                        players.Add(FillReqPlayer("3"));
                        players.Add(FillReqPlayer("4"));
                        players.Add(FillReqPlayer("5"));
                    }
                }
                else players = ScoreInfo.LoadGroup(tourneyId.Value, roundNum.Value, scorerId.Value);
            }
            ScoreInfo si = new ScoreInfo("label", "", "", ScoreInfo.empty18List(true), "out", "in", "total", "hcp", "net");
            pnlScoresList.Controls.Add(Scores("", si, false, false));
            bool allcomplete = false;
            foreach (ScoreInfo player in players) { if (player.Name != "") { allcomplete = true; break; } }
            bool allsigned = true;
            ddScorer.Items.Add(new ListItem("-- Select Scorer --", ""));
            ddAttester.Items.Add(new ListItem("-- Select Attester --", ""));
            int x = 1;
            foreach (ScoreInfo player in players)
            {
                string ddval = "player_" + x;
                ddval += ":" + player.ID + ":";
                ddScorer.Items.Add(new ListItem(player.Name, ddval));
                if (player.LookupID == scorerId.Value && scorerId.Value != "")
                {
                    scorerUserId.Value = player.ID;
                    scorerName.Value = player.Name;
                    ddScorer.SelectedIndex = ddScorer.Items.Count - 1;
                    ddScorer.Enabled = false;
                }
                else
                {
                    ddAttester.Items.Add(new ListItem(player.Name, ddval));
                }
                if (player.Name != "" && !player.RoundComplete) allcomplete = false;
                if (!player.CardSigned || !player.CardAttested || Request.QueryString["overwrite"] == "1") allsigned = false;
                pnlScoresList.Controls.Add(Scores("player", player, !allsigned, false, x, players.Count));
                x++;
            }
            if (allcomplete && !allsigned)
            {
                pnlScorerSign.Style.Remove(HtmlTextWriterStyle.Display);
            }

            if (tourneyId.Value != "")
            {
                string tourneyName = "", dateofRound = "", courseName;
                List<ScoreInfo> courseInfo = ScoreInfo.LoadCourseInfo(tourneyId.Value, roundNum.Value, out tourneyName, out dateofRound, out courseName);
                foreach (ScoreInfo c in courseInfo) pnlScoresList.Controls.Add(Scores(c.ID, c, false, false));
                lblDateInfo.Text = "&nbsp;&nbsp;  - &nbsp;&nbsp;  Round " + roundNum.Value + ": " + dateofRound;
                lblRoundInfo.Text = tourneyName + " " + courseName;
            }
            else
            {
                lblDateInfo.Text = "Date of Round: " + DateTime.Now.ToShortDateString();
            }
            si.ID = "label2";
            pnlScoresList.Controls.Add(Scores("", si, false, true));
        }
    }
    //protected void FillReqPlayer(ScoreInfo player)
    //{
    //    int scorenum = 0, f9 = 0, b9 = 0;
    //    List<string> scores = new List<string>();
    //    for (int x = 1; x <= 18; x++)
    //    {
    //        scorenum = 0;
    //        if (int.TryParse(Request.Form["score_" + x + "_" + player.ID], out scorenum))
    //        {
    //            if (x < 10) f9 += scorenum;
    //            else b9 += scorenum;
    //        }
    //        if (player.Scores.ContainsKey(x.ToString())) player.Scores[x.ToString()] = scorenum.ToString();
    //        else player.Scores.Add(x.ToString(), scorenum.ToString());
    //        scores.Add(scorenum.ToString());
    //    }
    //    string key = ScoreInfo.ScoreKey.Out.ToString();
    //    if (player.Scores.ContainsKey(key)) player.Scores[key] = f9.ToString();
    //    else player.Scores.Add(key, f9.ToString());
    //    key = ScoreInfo.ScoreKey.In.ToString();
    //    if (player.Scores.ContainsKey(key)) player.Scores[key] = b9.ToString();
    //    else player.Scores.Add(key, b9.ToString());
    //    key = ScoreInfo.ScoreKey.Total.ToString();
    //    if (player.Scores.ContainsKey(key)) player.Scores[key] = (f9 + b9).ToString();
    //    else player.Scores.Add(key, (f9 + b9).ToString());

    //    int hcp = 0;
    //    int.TryParse(Request.Form["score_hcp_" + player.ID], out hcp);
    //    key = ScoreInfo.ScoreKey.HCP.ToString();
    //    if (player.Scores.ContainsKey(key)) player.Scores[key] = hcp.ToString();
    //    else player.Scores.Add(key, hcp.ToString());
        
    //    key = ScoreInfo.ScoreKey.Net.ToString();
    //    if (player.Scores.ContainsKey(key)) player.Scores[key] = (f9 + b9 - hcp).ToString();
    //    else player.Scores.Add(key, (f9 + b9 - hcp).ToString());
    //}
    protected ScoreInfo FillReqPlayer(string id)
    {
        int scorenum = 0, f9 = 0, b9 = 0;
        List<string> scores = new List<string>();
        for (int x = 1; x <= 18; x++)
        {
            scorenum = 0;
            if (int.TryParse(Request.Form["score_" + x + "_" + id], out scorenum))
            {
                if (x < 10) f9 += scorenum;
                else b9 += scorenum;
            }
            scores.Add(scorenum.ToString());
        }
        int hcp = 0;
        int.TryParse(Request.Form["score_hcp_" + id], out hcp);
        return new ScoreInfo(id, "", Request.Form["score_name_" + id], scores, f9.ToString(), b9.ToString(), (f9 + b9).ToString(), hcp.ToString(), (f9 + b9 - hcp).ToString(), Request["courseName"], Request["courseSlope"], Request["courseRating"], false, "", 1);
    }
    protected void FillReq(string reqid, out string id, out string userid)
    {
        id = "";
        userid = "";
        string[] req = new string[1];
        if (Request.Form[reqid] != null) req = Request.Form[reqid].Split(':');
        if (req.Length > 1)
        {
            id = req[0];
            userid = req[1];
        }
    }
    protected void NameEmailList(SqlDataReader sdr, Dictionary<string, string> nameemails)
    {
        string email = (sdr.IsDBNull(2)) ? "" : sdr[2].ToString();
        string name = (sdr.IsDBNull(0)) ? "" : sdr[0].ToString();
        name += " ";
        name += (sdr.IsDBNull(1)) ? "" : sdr[1].ToString();
        if (email != "" && !nameemails.ContainsKey(email)) nameemails.Add(email, name);
    }
    protected void btnSign_Click(object sender, System.EventArgs e)
    {
        string url = "Default.aspx";
        DS ds = new DS("");
        string scorerid = "", scoreruserid = "", scorername = "", scoreremail = "", attesterid = "", attesteruserid = "", attestername = "", attesteremail = "";
        if (tourneyId.Value == "") FillReq(ddScorer.ClientID, out scorerid, out scoreruserid);
        else
        {
            scorerid = scorerId.Value;
            scoreruserid = scorerUserId.Value;
            scorername = scorerName.Value;
        }
        FillReq(ddAttester.ClientID, out attesterid, out attesteruserid);
        DB db = new DB();
        SqlDataReader sdr = db.Get("select UserId,FirstName,LastName,Email,Handicap from mg_users where UserId = " + scoreruserid);
        if (sdr.Read())
        {
            scorername = sdr[1].ToString() + " " + sdr[2].ToString();
            scoreremail = sdr[3].ToString();
        }
        else
        {
            foreach (ScoreInfo player in players) { if (player.ID == scoreruserid) { scorername = player.Name; break; } }
        }
        db.Close(sdr, false);
        sdr = db.Get("select UserId,FirstName,LastName,Email,Handicap from mg_users where UserId = " + attesteruserid);
        if (sdr.Read())
        {
            attestername = sdr[1].ToString() + " " + sdr[2].ToString();
            attesteremail = sdr[3].ToString();
        }
        else
        {
            foreach (ScoreInfo player in players) { if (player.ID == attesteruserid) { attestername = player.Name; break; } }
        }
        db.Close(sdr, false);

        Dictionary<string, string> nameemails = new Dictionary<string, string>();
        if (tourneyId.Value == "")
        {
            sdr = db.Get(string.Format("select u.FirstName, u.LastName, u.Email from mg_tourneyscores t join mg_users u on u.UserId = t.UserId where GroupId = '{0}'", DB.stringSql(roundNum.Value)));
            while (sdr.Read()) NameEmailList(sdr, nameemails);
        }
        else
        {
            foreach (ScoreInfo player in players)
            {
                sdr = db.Get("select FirstName, LastName, Email from mg_users where UserId = " + player.ID);
                if (sdr.Read()) NameEmailList(sdr, nameemails);
                db.Close(sdr, false);
            }
        }
        db.Close(sdr, true);

        if (scorername == "" && scorerid == "") Response.Write("Select a scorer");
        else
        {
            if (ds.Send(players, scorername, scoreruserid, attestername, attesteruserid, Request["courseName"], Request["courseRating"], Request["courseSlope"], nameemails))
            {
                url = ds.SignURL(1, tourneyId.Value, roundNum.Value, scorerId.Value);
            }
            Response.Clear();
            Response.Redirect(url);
        }
    }
    protected void btnRS_Click(object sender, System.EventArgs e)
    {
        DateTime start = new DateTime(2012, 1, 1);
        while (start < DateTime.Now)
        {
            DateTime end = start.AddDays(1.0);
            DS ds = new DS("");
            FilteredEnvelopeStatuses fs = ds.RequestStatuses(start, end);
            foreach (EnvelopeStatus es in fs.EnvelopeStatuses)
            {
                Response.Write(es.EnvelopeID + " " + es.Status + "<br/>");
            }
            start = end;
        }
    }
    protected void btnPost_Click(object sender, System.EventArgs e)
    {
        int x = 1;
        DB db = new DB();
        string newusers = "";
        foreach (ScoreInfo player in players)
        {
            int userid = 0;
            int.TryParse(player.ID, out userid);
            string idform = "{0}_" + x++ + "_" + userid;
            string email = Request.Form[string.Format(idform, "email")];
            if (userid > 5 || (email != null && email != "" && player.Name != null && player.Name != ""))
            {
                string sql = "";
                if (userid <= 5)
                {
                    string[] namespl = player.Name.Split(' ');
                    string firstname = player.Name;
                    string lastname = "";
                    string username = player.Name.Replace(" ", "");
                    if (namespl.Length > 1)
                    {
                        firstname = namespl[0];
                        for (int i = 1; i < namespl.Length; i++)
                        {
                            if (lastname != "") lastname += " ";
                            lastname += namespl[i];
                        }
                    }

                    sql = "INSERT INTO MG_Users (UserName, Email, FirstName, LastName) VALUES ('" + DB.stringSql(username) + "', '" + DB.stringSql(email) + "', '" + DB.stringSql(firstname) + "', '" + DB.stringSql(lastname) + "');";
                    db.Exec(sql);
                    sql = "SELECT Max(UserID) FROM MG_Users;";
                    SqlDataReader sdr = db.Get(sql);
                    if (sdr.Read() && !sdr.IsDBNull(0))
                    {
                        if (int.TryParse(sdr[0].ToString(), out userid))
                        {
                            newusers += "<div style='margin:10px;'>" + player.Name + " has been added to the Monster Handicap system. Login information, User Name is " + username + " Email is " + email  + "</div>";
                        }
                    }
                    db.Close(sdr, false);
                }
                int score = 0;
                string course = Request.Form[string.Format(idform, "course")];
                if (course == null) course = "";
                int slope = 0;
                float rating = 0;
                bool tourney = true;
                int.TryParse(Request.Form[string.Format(idform, "score")], out score);
                int.TryParse(Request.Form[string.Format(idform, "slope")], out slope);
                float.TryParse(Request.Form[string.Format(idform, "rating")], out rating);
                if (tourneyId.Value == "" && Request.Form[string.Format(idform, "tournament")] == null) tourney = false;
                sql = "INSERT INTO MG_Scores (UserID, Rating, Slope, Score, CourseName, DateOfRound, DateEntered, Tournament, EnteredBy) VALUES (";
                sql += userid + "," + rating + "," + slope + "," + score + ",'" + DB.stringSql(course) + "','" + DateTime.Now.ToShortDateString() + "',getDate(),";
                if (tourney) sql += "1";
                else sql += "0";
                sql += ",null)";
                db.Exec(sql);
            }
        }
        db.Close();
        btnPost.Visible = false;
        pnlScoresList.Controls.Clear();
        Literal lit = new Literal();
        lit.Text = newusers + "<br/><br/>Scores have been posted. Thank you for using Monster Scoring.<br/><br/><a href='http://www.monstergolf.org'>Monster Golf</a>";
        pnlScoresList.Controls.Add(lit);
    }
}

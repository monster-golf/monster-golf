using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;

public partial class TourneyXml : System.Web.UI.Page
{
    DB db;
    protected void UpdateTourneyUsers()
    {
        if (Request["updatehcp"] == "1")
        {
            int tourneyid;
            if (int.TryParse(Request["tourneyid"], out tourneyid))
            {
                StringBuilder update = new StringBuilder();
                SqlDataReader sdr = db.Get("select ttp.ID, ttu.UserId, mu.Handicap, ttu.WebId, Max(mc.DateOfRound) as DateOfRound from mg_TourneyTeamPlayers ttp join mg_TourneyUsers ttu on ttu.UserId = ttp.UserId join mg_users mu on mu.UserId = ttu.WebId join mg_tourneyCourses mc on mc.TournamentId = ttp.TournamentId where ttp.TournamentId = " + tourneyid + " GROUP BY ttp.ID, ttu.UserId, mu.Handicap, ttu.WebId");
                DateTime startofround = DateTime.MinValue;
                while (sdr.Read())
                {
                    update.AppendFormat("update mg_TourneyUsers set HcpIndex = {0} where UserID = {1};\n", sdr["Handicap"], sdr["UserId"]);

                    if (startofround == DateTime.MinValue)
                    {
                        if (!sdr.IsDBNull(sdr.GetOrdinal("DateOfRound"))) DateTime.TryParse(sdr["DateOfRound"].ToString(), out startofround);
                        else startofround = DateTime.MaxValue;
                    }
                    if (startofround > DateTime.Now)
                    {
                        update.AppendFormat("update mg_TourneyTeamPlayers set Handicap = {0} where ID = {1};\n", sdr["Handicap"], sdr["ID"]);
                        update.AppendFormat("update mg_TourneyScores set HCP = Round((CourseSlope*{0})/113,0),DateOfRound='{3}' where UserId = {1} and TourneyId = {2} and CardSigned = 0 and CardAttested = 0;\n", sdr["Handicap"], sdr["WebId"], tourneyid, startofround);
                    }
                }
                db.Close(sdr, false);
                db.Exec(update.ToString());
            }
        }
    }
    protected void SetTourneyScores()
    {
        int roundnum, tourneyid;
        if (int.TryParse(Request["settourneyround"], out roundnum) &&
            int.TryParse(Request["tourneyid"], out tourneyid))
        {
            StringBuilder update = new StringBuilder();
            SqlDataReader sdr = db.Get("select ttu.WebId, ttu.FirstName + ' ' + ttu.LastName as PlayerName, ttp.Handicap, ttp.TeeNumber from mg_TourneyTeamPlayers ttp join mg_TourneyUsers ttu on ttu.UserId = ttp.UserId where ttp.TournamentId = " + tourneyid);
            while (sdr.Read())
            {
                //TODO: cannot compare agains round in the sql string because round could be a comma seperated list of round numbers
                update.AppendFormat("insert into mg_TourneyScores (TourneyId, RoundNum, UserId, Name, UserLookup, CourseName, CourseSlope, CourseRating) " +
                        "select c.TournamentId, c.[Round], {0}, '{1}', '{2}', c.Course, d.Slope, Round(d.Rating,1) from mg_tourneycourses c " +
                        "join mg_TourneyCourseDetails d on d.CourseId = c.CourseId AND d.TeeNumber = {5} " +
                        "where c.TournamentId = {3} and c.[Round] = {4} and  NOT Exists(select * from mg_TourneyScores WHERE TourneyId = {3} and RoundNum = {4} and userID = {0});\n",
                        sdr[0], DB.stringSql(sdr[1].ToString()), ScoreInfo.GetId(), tourneyid, roundnum, sdr[3]);
                update.AppendFormat("update mg_TourneyScores set HCP = Round((CourseSlope*{1})/113,0) WHERE TourneyId = {2} and RoundNum = {3} and userID = {0};\n",
                    sdr[0], sdr[2], tourneyid, roundnum);
            }
            db.Close(sdr, false);
            db.Exec(update.ToString());
        }
    }
    protected void GetTourneyScores()
    {
        int roundnum, tourneyid;
        if (int.TryParse(Request["viewtourneyround"], out roundnum) &&
            int.TryParse(Request["tourneyid"], out tourneyid))
        {
            string update = "";
            if (Request["setgroup"] != null && Request["player"] != null)
            {
                string groupid = ScoreInfo.GetId();
                foreach (string player in Request["player"].ToString().Split(','))
                {
                    update += string.Format("update mg_tourneyScores set GroupId = '{0}' where TourneyId = {1} and RoundNum = {2} and UserId = {3};\n",
                        DB.stringSql(groupid), tourneyid, roundnum, player);
                }
            }
            else if (Request["breakgroup"] != null)
            {
                update += string.Format("update mg_tourneyScores set GroupId = '',EmailSent=0,StartingHole=NULL where GroupId = '{0}';", DB.stringSql(Request["breakgroup"]));
            }
            if (update != "") db.Exec(update);
            SqlDataReader sdr = db.Get("select DateOfRound, [Round] from mg_tourneyCourses where tournamentId = " + tourneyid);
            DateTime startofround = DateTime.MinValue;
            while (sdr.Read())
            {
                if (!sdr.IsDBNull(sdr.GetOrdinal("DateOfRound")) && ScoreInfo.IsCurrentRound(sdr, roundnum.ToString()))
                {
                    if (DateTime.TryParse(sdr["DateOfRound"].ToString(), out startofround)) break;
                }
            }
            db.Close(sdr, false);
            WEB w = new WEB();
            WEB.WriteEndResponse(Response, w.TourneyScores(db, tourneyid, roundnum, startofround > DateTime.Now, startofround < DateTime.Now, "StartingHole, GroupId, Name"));
        }
    }
    protected void GetTourneyDetails()
    {
        if (Request["gettourney"] == "1")
        {
            StringBuilder sb = new StringBuilder("<div>");
            int tourneyid;
            if (int.TryParse(Request["tourneyid"], out tourneyid))
            {

                string select = "select NumRounds from mg_Tourney where TournamentId = " + tourneyid;
                SqlDataReader sdr = db.Get(select);
                int numrounds = 1;
                if (sdr.Read() && !int.TryParse(sdr[0].ToString(), out numrounds)) numrounds = 1;
                db.Close(sdr, false);
                select = "select Distinct(RoundNum) as RoundNum from mg_TourneyScores where TourneyId = " + tourneyid;
                sdr = db.Get(select);
                List<int> needssetup = new List<int>();
                List<int> foundrounds = new List<int>();

                while (sdr.Read())
                {
                    int roundnum;
                    if (int.TryParse(sdr[0].ToString(), out roundnum)) foundrounds.Add(roundnum);
                }
                db.Close(sdr, false);
                for (int x = 1; x <= numrounds; x++)
                {
                    if (!foundrounds.Contains(x)) needssetup.Add(x);
                }
                
                select = "select Distinct(tts.RoundNum) from mg_TourneyTeamPlayers ttp " +
                "join mg_TourneyUsers ttu on ttu.UserId = ttp.UserId " +
                "join mg_TourneyScores tts on tts.TourneyId = ttp.TournamentId and tts.UserId = ttu.WebId and tts.CardSigned = 0 " +
                "where ttp.TournamentId = " + tourneyid + " and tts.UserId IS NULL";

                sdr = db.Get(select);
                List<int> needsscores = new List<int>();
                while (sdr.Read())
                {
                    int roundnum;
                    if (int.TryParse(sdr[0].ToString(), out roundnum)) needsscores.Add(roundnum);
                }
                db.Close(sdr, false);


                List<List<ScoreInfo>> coursesInfo = new List<List<ScoreInfo>>();
                List<string> tourneyNames = new List<string>();
                List<string> dateOfRounds = new List<string>();
                List<string> courseNames = new List<string>();
                for (int x = 1; x <= numrounds; x++)
                {
                    string tourneyName, dateofRound, courseName;
                    coursesInfo.Add(ScoreInfo.LoadCourseInfo(tourneyid.ToString(), x.ToString(), out tourneyName, out dateofRound, out courseName));
                    tourneyNames.Add(tourneyName);
                    dateOfRounds.Add(dateofRound);
                    courseNames.Add(courseName);
                }
                int detailround = -1;
                WEB w = new WEB();

                for (int x = 0; x < coursesInfo.Count; x++)
                {
                    if (x==0) sb.AppendFormat("<div>{0}</div>", tourneyNames[x]);
                    detailround = x + 1;
                    sb.AppendFormat("<div style='margin-top:20px;'>Round: {0} on <span id='rounddate'>{1}</span> at {2} ", detailround, dateOfRounds[x], courseNames[x]);
                    if (needssetup.Contains(detailround)) sb.AppendFormat(" <a href='javascript:SetRound({0});'>Set Up Round</a>", detailround);
                    else
                    {
                        sb.AppendFormat(" <a href='javascript:ViewRound({0});'>View Player Scores</a>", detailround);
                        DateTime dOfRound = DateTime.Parse(dateOfRounds[x]);
                        if (dOfRound.Day == DateTime.Now.Day && dOfRound > DateTime.Now) sb.AppendFormat(" <a href='javascript:EmailGroups({0});'>Send Groups Email</a>", detailround);
                    }
                    if (needsscores.Contains(detailround)) sb.AppendFormat(" <a href='javascript:EnterScores({0});'>Enter Scores</a>", detailround);
                    sb.AppendFormat("</div><div style='clear:both' id='playerscores{0}'> </div>", detailround);
                    w.TourneyRow(db, sb, new ScoreInfo("label", "", "Hole", ScoreInfo.empty18List(true), "out", "in", "total", "hcp", "net"));
                    foreach (ScoreInfo si in coursesInfo[x])
                    {
                        w.TourneyRow(db, sb, si);
                    }
                    sb.Append("<div style='clear:both'> </div>");
                }
            }
            sb.Append("\n</div>");
            WEB.WriteEndResponse(Response, sb);
        }
    }
    private void EmailGroups()
    {
        int roundnum, tourneyid;
        if (int.TryParse(Request["emailtourneyround"], out roundnum) &&
            int.TryParse(Request["tourneyid"], out tourneyid))
        {
            string emailfails = "";
            SqlDataReader sdr = db.Get("select Distinct t.Location, t.Slogan, t.[Description], t.NumRounds, tc.Course, tc.[Round], tc.DateOfRound, ts.GroupId from mg_Tourney t " +
                                       "join mg_tourneyCourses tc on tc.tournamentid = t.tournamentid " +
                                       "join mg_tourneyScores ts on ts.tourneyid = t.tournamentid and " +
                                       "	(convert(nvarchar(2), ts.RoundNum) = tc.[Round] OR " +
                                       "	 tc.[Round] like convert(nvarchar(2), ts.RoundNum) + ',%' OR " +
                                       "	 tc.[Round] like '%,' + convert(nvarchar(2), ts.RoundNum) OR " +
                                       "	 tc.[Round] like '%,' + convert(nvarchar(2), ts.RoundNum) + ',%') " +
                                       "where t.tournamentId = " + tourneyid + " and ts.EmailSent <> 1 and ts.GroupId IS NOT NULL and ts.GroupId <> ''");
            while (sdr.Read())
            {
                if (ScoreInfo.IsCurrentRound(sdr, roundnum.ToString()))
                {
                    int numrounds = 1;
                    string addday = "";
                    if (roundnum > 1) numrounds = 2;
                    else if (!sdr.IsDBNull(sdr.GetOrdinal("NumRounds"))) int.TryParse(sdr["NumRounds"].ToString(), out numrounds);
                    if (numrounds > 1) addday = " Day " + sdr["Round"];
                    DateTime startofround = DateTime.MinValue;
                    if (!sdr.IsDBNull(sdr.GetOrdinal("DateOfRound")))
                    {
                        DateTime.TryParse(sdr["DateOfRound"].ToString(), out startofround);
                    }
                    string starttime = "";
                    if (startofround != DateTime.MinValue) starttime = startofround.ToString("M/d/yyyy h:mm tt");
                    string subject = sdr["Slogan"] + addday + " at " + sdr["Course"];
                    string te = "Welcome to " + subject + ".<br/>Start of round " + starttime + "<br/>{1}<br/>To keep score for your group, click on the link<br/><br/>{0}<br/>Also, turn in your paper card, to Aaron Wald or Brian Giesinger, for validation.<br/><br/>When your card is fully filled out validate all scores, scroll down to below the score card and select a golfer from the other team to attest the scores. Then, you sign the card, and when you are done click Confirm Signing.  Hand your mobile device to the attester from the other team and they should validate the scores, then sign the card.<br/><br/>Once the scores have been attested you can no longer make changes.<br/><br/>Any questions? See Aaron Wald or Brian Giesinger at the end of the round.";

                    List<ScoreInfo> si = ScoreInfo.LoadGroup(sdr["GroupId"].ToString());
                    string groupnames = "<br/>Starting Hole " + si[0].StartingHole + "<table><tr><td style='padding-top:10px;padding-left:10px;'>Player</td><td style='padding-top:10px;padding-left:5px;'>HCP</td></tr>";
                    foreach (ScoreInfo s in si) groupnames += "<tr><td style='padding-left:10px;'>" + s.Name + "</td><td style='padding-left:5px;text-align:center;'>" + s.HCP + "</td>";
                    groupnames += "</table>";

                    foreach (ScoreInfo s in si)
                    {
                        if (!s.EmailSent)
                        {
                            DB db1 = new DB();
                            SqlDataReader sdrEm = db1.Get("select MobileEmail, Email from mg_users where userid = " + s.ID);
                            if (sdrEm.Read())
                            {
                                string email = (sdrEm.IsDBNull(sdrEm.GetOrdinal("MobileEmail"))) ? (sdrEm.IsDBNull(sdrEm.GetOrdinal("Email"))) ? "" : sdrEm["Email"].ToString() : sdrEm["MobileEmail"].ToString();
                                db1.Close(sdrEm, false);
                                if (email != "")
                                {
                                    string scoringlink = string.Format("<div style='font-size:15px;font-weight:bold;'><a href='http://monstergolf.org/monsterscoring/?t={0}&r={1}&u={2}&h={3}'>Start Scoring</a></div>", tourneyid, roundnum, s.LookupID, s.StartingHole);
                                    if (WEB.SendMessage(email + ":" + s.Name, subject, string.Format(te, scoringlink, groupnames), true, null, Server))
                                    {
                                        db1.Exec("update mg_tourneyscores set EmailSent = 1 WHERE TourneyScoreID = " + s.TourneyScoreID);
                                    }
                                    else
                                    {
                                        emailfails += "\n" + email;
                                    }
                                }
                            }
                            db1.Close();
                        }
                    }
                }
            }
            db.Close(sdr, false);
            string response = "Email send complete: ";
            if (emailfails != "") response += "\nEmail Failures:" + emailfails;
            Response.Write("<r>\n" + response + "\n</r>");
            Response.End();
        }
    }
    private void EmailScores()
    {
        int roundnum, tourneyid;
        if (int.TryParse(Request["emailresults"], out roundnum) &&
            int.TryParse(Request["t"], out tourneyid))
        {
            string murl = "http://monstergolf.org/monsterscoring/results.aspx?p=1&" + Request.QueryString;
            murl = murl.Replace("&", "%26").Replace("?", "%3f");
            string pdfurl = "http://www.pdfmyurl.com/?url=" + murl;
            System.Net.WebClient wc = new System.Net.WebClient();
            List<System.Net.Mail.Attachment> attachments = new List<System.Net.Mail.Attachment>();
            System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(wc.OpenRead(pdfurl), "results.pdf", "application/pdf");
            attachments.Add(attach);

            string response = "Email send complete: ";
            string emaillist = Request.Form["emails"];
            if (emaillist == null) emaillist = "";
            if (Request.Form["addplayers"] == "1")
            {
                SqlDataReader sdrEm = db.Get("select MobileEmail, Email, ttu.FirstName + ' ' + ttu.LastName as Name from mg_TourneyTeams tt join mg_TourneyTeamPlayers ttp on tt.TeamId = ttp.TeamId join mg_TourneyUsers ttu on ttu.UserId = ttp.UserId join mg_users u on ttu.WebId = u.UserId where tt.TournamentId = " + tourneyid);
                if (emaillist != "" && !emaillist.EndsWith(";")) emaillist += ";";
                while (sdrEm.Read())
                {
                    emaillist += (sdrEm.IsDBNull(sdrEm.GetOrdinal("MobileEmail"))) ? (sdrEm.IsDBNull(sdrEm.GetOrdinal("Email"))) ? "" : sdrEm["Email"].ToString() : sdrEm["MobileEmail"].ToString();
                    if (!sdrEm.IsDBNull(sdrEm.GetOrdinal("Name"))) emaillist += ":" + sdrEm["Name"];
                    emaillist += ";";
                }
                db.Close(sdrEm, true);
                response += " also sent to all tournament golfers";
            }

            string msg = Request.Form["msg"];
            if (msg == null) msg = "";
            msg = msg.Replace("\n", "<br/>");
            string sbj = Request.Form["sbj"];
            if (sbj == null) sbj = "Monster Results";
            if (!WEB.SendMessage(emaillist, sbj, msg, true, attachments, Server)) response += "\nEmail Failed:";
            Response.Write("<r>\n" + response + "\n</r>");
            Response.End();
        }
    }
    private void StartingHoleForGroup()
    {
        if (Request.Form["startingholeforgroup"] != null)
        {
            StringBuilder update = new StringBuilder();
            update.AppendFormat("update mg_TourneyScores set StartingHole = {0} WHERE GroupId = '{1}'", Request.Form["hole"], DB.stringSql(Request["startingholeforgroup"]));
            db.Exec(update.ToString());
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        db = new DB();
        UpdateTourneyUsers();
        SetTourneyScores();
        GetTourneyDetails();
        GetTourneyScores();
        StartingHoleForGroup();
        EmailGroups();
        EmailScores();
        Response.Write("<complete>true</complete>");
    }
    protected override void OnUnload(EventArgs e)
    {
        base.OnUnload(e);
        if (db != null) db.Close();
    }
}

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
                        update.AppendFormat("update mg_TourneyScores set HCP = Round((CourseSlope*{0})/113,0) where UserId = {1} and TourneyId = {2} and CardSigned = 0 and CardAttested = 0;\n", sdr["Handicap"], sdr["WebId"], tourneyid, startofround);
                    }
                }
                db.Close(sdr, false);
                db.Exec(update.ToString());
            }
        }
    }
    protected void InitTourneyScores()
    {
        int roundnum, tourneyid;
        if (int.TryParse(Request["settourneyround"], out roundnum) &&
            int.TryParse(Request["tourneyid"], out tourneyid))
        {
            StringBuilder update = new StringBuilder();
            SqlDataReader sdr = db.Get("select CourseID, TournamentID, Round, Course, DateOfRound from mg_TourneyCourses where TournamentId = " + tourneyid);
            while (sdr.Read())
            {
                if (ScoreInfo.IsCurrentRound(sdr, roundnum.ToString()))
                {
                    string courseId = sdr[0].ToString();
                    string coursename = DB.stringSql(sdr,3);
                    string dateofround = DB.stringSql(sdr,4);
                    db.Close(sdr, false);
                    sdr = db.Get("select ttu.WebId, ttu.FirstName + ' ' + ttu.LastName as PlayerName, ttp.Handicap, ttp.TeeNumber from mg_TourneyTeamPlayers ttp join mg_TourneyUsers ttu on ttu.UserId = ttp.UserId where ttp.TournamentId = " + tourneyid);
                    while (sdr.Read())
                    {
                        update.AppendFormat(
                            "insert into mg_TourneyScores (TourneyId, RoundNum, UserId, Name, UserLookup, CourseName, DateOfRound, CourseSlope, CourseRating) " +
                            "select {3}, {4}, {0}, {1}, {2}, {7}, {8}, d.Slope, Round(d.Rating,1) " +
                            "from mg_TourneyCourseDetails d " +
                            "where d.TeeNumber = {5} and " +
                            "	d.CourseId = {6} and " +
                            "	NOT Exists(select * from mg_TourneyScores WHERE TourneyId = {3} and RoundNum = {4} and userID = {0});\n",
                            sdr[0], DB.stringSql(sdr, 1), DB.stringSql(ScoreInfo.GetId(), true), tourneyid, roundnum, sdr[3], courseId, coursename, dateofround);
                        update.AppendFormat("update mg_TourneyScores set HCP = Round((CourseSlope*{1})/113,0), DateOfRound={4} WHERE TourneyId = {2} and RoundNum = {3} and userID = {0};\n",
                            sdr[0], sdr[2], tourneyid, roundnum, dateofround);
                    }
                }
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
            WEB.WriteEndResponse(Response, w.TourneyScores(db, tourneyid, roundnum, startofround > DateTime.Now, startofround < DateTime.Now, "StartingHole, DateOfRound, GroupId, TeamId, Name"));
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
                    //if (x==0) sb.AppendFormat("<div>{0}</div>", tourneyNames[x]);
                    detailround = x + 1;
                    DateTime dOfRound = DateTime.MinValue;
                    sb.AppendFormat("<div class='PageBreak' style='margin:20px 5px 10px;'>{0}<br/>Round: {1} on <span id='rounddate'>{2}</span> at {3} ", tourneyNames[x], detailround, dateOfRounds[x], courseNames[x]);
                    if (needssetup.Contains(detailround)) sb.AppendFormat("<br/><a class='header' href='javascript:SetRound({0});'>Set Up Round</a>", detailround);
                    else
                    {
                        sb.AppendFormat("<br/><a class='header' href='javascript:ViewRound({0});'>View Players</a>", detailround);
                        dOfRound = DateTime.Parse(dateOfRounds[x]);
                        if (dOfRound > DateTime.Now) sb.AppendFormat(" <a class='header' href='javascript:SetRound({0});'>Set Up Round</a>", detailround);
                        if ((dOfRound.Day - 1 == DateTime.Now.Day || dOfRound.Day == DateTime.Now.Day) && dOfRound > DateTime.Now) sb.AppendFormat(" <a class='header' href='javascript:EmailGroups({0});'>Send Groups Email</a>", detailround);
                    }
                    if (needsscores.Contains(detailround)) sb.AppendFormat("<br/><a class='header' href='javascript:EnterScores({0});'>Enter Scores</a>", detailround);
                    sb.AppendFormat("</div><div style='clear:both' id='playerscores{0}'> </div>", detailround);
                    sb.Append("<table cellpadding='0' cellspacing='0'>");
                    w.TourneyRow(db, sb, new ScoreInfo("label", "", "Hole", ScoreInfo.empty18List(true), "out", "in", "total", "hcp", "net"), 0, 0, dOfRound > DateTime.Now, false, "", true, 0, false);
                    foreach (ScoreInfo si in coursesInfo[x])
                    {
                        w.TourneyRow(db, sb, si, 0, 0, dOfRound > DateTime.Now, false, "", true, 0, false);
                    }
                    sb.Append("</table><div style='clear:both'> </div>");
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
            SqlDataReader sdr = db.Get("select Distinct t.Location, t.Slogan, t.[Description], t.NumRounds, tc.Course, tc.[Round], ts.DateOfRound, ts.GroupId " +
                                       "from mg_Tourney t " +
                                       "join mg_tourneyCourses tc on tc.tournamentid = t.tournamentid " +
                                       "join mg_tourneyScores ts on ts.tourneyid = t.tournamentid and " +
                                       "    (convert(nvarchar(2), ts.RoundNum) = tc.[Round] OR " +
                                       "     tc.[Round] like convert(nvarchar(2), ts.RoundNum) + ',%' OR " +
                                       "     tc.[Round] like '%,' + convert(nvarchar(2), ts.RoundNum) OR " +
                                       "     tc.[Round] like '%,' + convert(nvarchar(2), ts.RoundNum) + ',%') " +
                                       "where t.tournamentId = " + tourneyid + 
                                       "    and ts.EmailSent <> 1 " +
                                       "    and ts.GroupId IS NOT NULL " +
                                       "    and ts.GroupId <> '' " +
                                       "    and ts.RoundNum = " + roundnum);
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
                                string email = (sdrEm.IsDBNull(sdrEm.GetOrdinal("MobileEmail"))) ? "" : sdrEm["MobileEmail"].ToString();
                                email = email.Trim();
                                if (string.IsNullOrEmpty(email))
                                {
                                    email = (sdrEm.IsDBNull(sdrEm.GetOrdinal("Email"))) ? "" : sdrEm["Email"].ToString();
                                    email = email.Trim();
                                }
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
    private void PlayersList()
    {
        int tourneyid;
        if (Request.QueryString["playerslist"] == "1" &&
            int.TryParse(Request["t"], out tourneyid))
        {
            StringBuilder playersTable = new StringBuilder("<table class='PlayersTable' cellspacing='0'><tr><th>Name</th><th>HCP</th><th>In</th></tr>");
            SqlDataReader sdr = db.Get("select Distinct ttu.UserId, ttu.WebId, ttu.FirstName, ttu.LastName, ttu.HcpIndex, InTourney = CASE WHEN ttp.TeamID IS NULL THEN 0 ELSE 1 END from mg_TourneyUsers ttu left join mg_TourneyTeamPlayers ttp on ttu.UserId = ttp.UserId and ttp.TournamentId = " + tourneyid + " order by ttu.LastName, ttu.FirstName");
            while (sdr.Read())
            {
                if (!sdr.IsDBNull(sdr.GetOrdinal("UserId")))
                {
                    playersTable.AppendFormat("<tr id=\"player{0}\" onclick=\"SelectTeamPlayer('{0}');\"><td>", sdr["UserId"]);
                    playersTable.AppendFormat("<input type='button' id='setteam{0}' onclick='SetTeam(event)' style='display:none' value='Set Team' />", sdr["UserId"]);
                    if (!sdr.IsDBNull(sdr.GetOrdinal("LastName")))
                    {
                        playersTable.Append(sdr["LastName"]);
                        if (!sdr.IsDBNull(sdr.GetOrdinal("FirstName")))
                        {
                            playersTable.Append(", " + sdr["FirstName"]);
                        }
                    }
                    else if (!sdr.IsDBNull(sdr.GetOrdinal("FirstName")))
                    {
                        playersTable.Append(sdr["FirstName"]);
                    }
                    playersTable.Append("</td><td>");
                    if (!sdr.IsDBNull(sdr.GetOrdinal("HcpIndex")))
                    {
                        playersTable.Append(sdr["HcpIndex"]);
                    }
                    playersTable.Append("</td><td>");
                    if (!sdr.IsDBNull(sdr.GetOrdinal("InTourney")))
                    {
                        if (sdr["InTourney"].ToString() == "1")
                        {
                            playersTable.Append("yes");
                        }
                    }
                    playersTable.Append("</td></tr>");
                }
            }
            playersTable.Append("</table>");
            WEB.WriteEndResponse(Response, playersTable);
        }
    }
    private class player {
        public string firstName = "";
        public string lastName = "";
        public string hcp = "0";
        public string userId;
        public string fullName() {
            string name = lastName;
            if (string.IsNullOrEmpty(lastName)) {
                name = firstName;
            } else if (!string.IsNullOrEmpty(firstName)) {
                name += ", " + firstName;
            }
            return name;
        }
    }
    private void TeamsList()
    {
        int tourneyid;
        if (Request.QueryString["teamslist"] == "1" &&
            int.TryParse(Request["t"], out tourneyid))
        {
            SqlDataReader sdr;
            if (Request["player"] != null)
            {
                string insertteam = "";
                string teamname = "";
                SortedList<string, player> sortName = new SortedList<string, player>();
                foreach (string userId in Request["player"].ToString().Split(','))
                {
                    sdr = db.Get("select FirstName, LastName, HcpIndex from mg_tourneyUsers where UserID = " + userId);
                    while (sdr.Read())
                    {
                        player p = new player();
                        p.userId = userId;
                        if (!sdr.IsDBNull(sdr.GetOrdinal("LastName")))
                        {
                            p.lastName = sdr["LastName"].ToString();
                        }
                        if (!sdr.IsDBNull(sdr.GetOrdinal("FirstName")))
                        {
                            p.firstName = sdr["FirstName"].ToString();
                        }
                        if (!sdr.IsDBNull(sdr.GetOrdinal("HcpIndex")))
                        {
                            p.hcp = sdr["HcpIndex"].ToString();
                        }

                        sortName.Add(p.fullName(), p);
                    }
                    db.Close(sdr, false);
                }
                foreach (string key in sortName.Keys)
                {
                    player p = sortName[key];
                    if (teamname != "")
                    {
                        teamname += " - ";
                    }
                    teamname += key;

                    insertteam += "INSERT INTO mg_TourneyTeamPlayers (TeamID, UserID, TeeNumber, TournamentID, Handicap) ";
                    insertteam += "select MAX(TeamID)," + p.userId + ",0," + tourneyid + "," + p.hcp + " FROM MG_TourneyTeams; ";
                }
                insertteam = "INSERT INTO MG_TourneyTeams (TeamName, RoundNumber, TournamentID) VALUES (" + DB.stringSql(teamname, true) + ",0," + tourneyid + ");" + insertteam;
                db.Exec(insertteam);
            }
            int teamId;
            if (int.TryParse(Request["removeteam"], out teamId))
            {
                string removeteam = "delete from mg_TourneyTeamPlayers where teamId = " + teamId + ";delete from MG_TourneyTeams where teamId = " + teamId + ";";
                db.Exec(removeteam);
            }
            StringBuilder teamsTable = new StringBuilder("<table class='TeamsTable' cellspacing='0'><tr><th>Team</th><th>Flight</th></tr>");
            sdr = db.Get("select TeamId, TeamName, Flight from mg_tourneyTeams where tournamentId = " + tourneyid + " ORDER BY TeamName");
            while (sdr.Read())
            {
                if (!sdr.IsDBNull(sdr.GetOrdinal("TeamId")))
                {
                    teamsTable.Append("<tr><td>");
                    teamsTable.AppendFormat("<input type='button' id='removeteam{0}' onclick='RemoveTeam({0},event)' value='Remove' /> ", sdr["TeamId"]);
                    teamsTable.Append(sdr["TeamName"]);
                    teamsTable.Append("</td><td>");
                    teamsTable.Append(sdr["Flight"]);
                    teamsTable.Append("</td></tr>");
                }
            }
            db.Close(sdr, false);
            teamsTable.Append("</table>");
            WEB.WriteEndResponse(Response, teamsTable);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        db = new DB();
        UpdateTourneyUsers();
        InitTourneyScores();
        GetTourneyDetails();
        GetTourneyScores();
        StartingHoleForGroup();
        EmailGroups();
        EmailScores();
        PlayersList();
        TeamsList();
        Response.Write("<complete>true</complete>");
    }
    protected override void OnUnload(EventArgs e)
    {
        base.OnUnload(e);
        if (db != null) db.Close();
    }
}

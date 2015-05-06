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
            WEB.WriteEndResponse(Response, w.TourneyScores(db, tourneyid, roundnum, startofround > DateTime.Now.AddDays(-1), startofround < DateTime.Now, "StartingHole, DateOfRound, GroupId, TeamId, Name"));
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
                List<string> actualDateOfRounds = new List<string>();
                List<string> courseNames = new List<string>();
                for (int x = 1; x <= numrounds; x++)
                {
                    string tourneyName, dateofRound, actualDateOfRound, courseName;
                    coursesInfo.Add(ScoreInfo.LoadCourseInfo(tourneyid.ToString(), x.ToString(), true, out tourneyName, out dateofRound, out actualDateOfRound, out courseName));
                    tourneyNames.Add(tourneyName);
                    dateOfRounds.Add(dateofRound);
                    actualDateOfRounds.Add(actualDateOfRound);
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
                        dOfRound = DateTime.Parse(actualDateOfRounds[x]);
                        if (dOfRound > DateTime.Now) sb.AppendFormat(" <a class='header' href='javascript:SetRound({0});'>Set Up Round</a>", detailround);
                        if ((dOfRound.Day - 1 == DateTime.Now.Day || dOfRound.Day == DateTime.Now.Day) && dOfRound > DateTime.Now) sb.AppendFormat(" <a class='header' href='javascript:EmailGroups({0});'>Send Groups Email</a>", detailround);
                    }
                    if (needsscores.Contains(detailround)) sb.AppendFormat("<br/><a class='header' href='javascript:EnterScores({0});'>Enter Scores</a>", detailround);
                    sb.AppendFormat("</div><div style='clear:both' id='playerscores{0}'> </div>", detailround);
                    sb.Append("<table cellpadding='0' cellspacing='0'>");
                    w.TourneyRow(db, sb, new ScoreInfo("label", "", "Hole", ScoreInfo.empty18List(true), "out", "in", "total", "hcp", "net"), 0, 0, dOfRound > DateTime.Now.AddDays(-1), false, "", true, 0, false);
                    foreach (ScoreInfo si in coursesInfo[x])
                    {
                        w.TourneyRow(db, sb, si, 0, 0, dOfRound > DateTime.Now.AddDays(-1), false, "", true, 0, false);
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
            string firstName = Request["firstname"];
            string lastName = Request["lastname"];
            string emailName = Request["emailname"];
            SqlDataReader sdr;
            string sql;
            string webId = "0";
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                sql = "SELECT UserId as WebId FROM mg_Users WHERE FirstName = " + DB.stringSql(firstName, true) + " and LastName = " + DB.stringSql(lastName, true);
                sdr = db.Get(sql);
                if (!sdr.HasRows)
                {
                    db.Close(sdr, false);
                    db.Exec("INSERT INTO MG_Users (UserName, Email, FirstName, LastName, UserTypeID, Handicap) VALUES ('" + DB.stringSql(firstName, false) + DB.stringSql(lastName, false) + "', " + DB.stringSql(emailName, true) + ", " + DB.stringSql(firstName, true) + ", " + DB.stringSql(lastName, true) + ",1,0)");
                    //use the select stmt from above to get the id
                    sdr = db.Get(sql);
                }
                if (sdr.Read())
                {
                    webId = sdr["WebId"].ToString();
                    db.Close(sdr, false);
                    sql = "SELECT UserId FROM mg_TourneyUsers WHERE FirstName = " + DB.stringSql(firstName, true) + " and LastName = " + DB.stringSql(lastName, true);
                    sdr = db.Get(sql);
                    if (sdr.Read())
                    {
                        sql = "UPDATE mg_TourneyUsers Set WebId = " + webId + " WHERE UserId = " + sdr["UserId"].ToString();
                    }
                    else 
                    {
                        sql = "INSERT INTO mg_TourneyUsers (FirstName, LastName, HCPIndex, WebID) VALUEs (";
                        sql += DB.stringSql(firstName, true) + "," + DB.stringSql(lastName, true) + ",0," + webId + ")";
                    }
                    db.Close(sdr, false);
                    db.Exec(sql);
                }
            }
            int removeplayer;
            if (int.TryParse(Request["removeplayer"], out removeplayer))
            {
                sql = "SELECT WebId FROM mg_TourneyUsers WHERE UserId = " + removeplayer;
                sdr = db.Get(sql);
                if (sdr.Read())
                {
                    webId = sdr["WebId"].ToString();
                    // this query checks if they have no scores entered so they are safe to delete from mg_users
                    sql = "select u.UserId from mg_Users u left join mg_scores s on s.userId = u.UserId where s.ID is NULL and u.UserId = " + webId;
                    db.Close(sdr, false);
                    sdr = db.Get(sql);
                    if (sdr.Read())
                    {
                        db.Close(sdr, false);
                        sql = "DELETE mg_Users WHERE UserId = " + webId;
                        db.Exec(sql);
                    }
                    else
                    {
                        db.Close(sdr, false);
                    }
                }
                sql = "DELETE mg_TourneyUsers WHERE UserId = " + removeplayer;
                db.Exec(sql);
            }
            sql = "select ttu.UserId, ttu.WebId, ttu.FirstName, ttu.LastName, ttu.HcpIndex, InTourneyCount = COUNT(DISTINCT ttp.TeamID), InTourneyAll = COUNT(DISTINCT ttpall.TeamID) from mg_TourneyUsers ttu ";
            sql += " left join mg_TourneyTeamPlayers ttp on ttu.UserId = ttp.UserId and ttp.TournamentId = " + tourneyid;
            sql += " left join mg_TourneyTeamPlayers ttpall on ttu.UserId = ttpall.UserId ";
            sql += " group by ttu.UserId, ttu.WebId, ttu.FirstName, ttu.LastName, ttu.HcpIndex ";
            sql += " order by InTourneyCount DESC, ttu.LastName, ttu.FirstName";
            sdr = db.Get(sql);

            StringBuilder playersTable = new StringBuilder("<table class='PlayersTable' cellspacing='0'><tr><th>Name</th><th>HCP</th><th>In</th></tr>");
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
                    int intourney;
                    if (!sdr.IsDBNull(sdr.GetOrdinal("InTourneyCount")))
                    {
                        if (int.TryParse(sdr["InTourneyCount"].ToString(), out intourney))
                        {
                            if (intourney > 0)
                            {
                                playersTable.Append(intourney);
                            }
                        }
                    }
                    playersTable.Append("</td><td>");
                    if (!sdr.IsDBNull(sdr.GetOrdinal("InTourneyAll")))
                    {
                        if (int.TryParse(sdr["InTourneyAll"].ToString(), out intourney))
                        {
                            if (intourney == 0)
                            {
                                playersTable.AppendFormat("<a href='javascript:RemovePlayer({0});'>remove</a>", sdr["UserId"]);
                            }
                        }
                    }
                    playersTable.Append("</td></tr>");
                }
            }
            playersTable.Append("<tr><td colspan='4'>");
            playersTable.Append("First <input id='firstName' name='firstName' value=''/><br/>");
            playersTable.Append("Last <input id='lastName' name='lastName' value=''/><br/>");
            playersTable.Append("Email <input id='emailName' name='emailName' value='monster@monstergolf.org'/><br/>");
            playersTable.Append("<input type='button' id='addPlayer' onclick='AddPlayer()' value='Add Player' />");
            playersTable.Append("</td></tr>");
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
    private player GetPlayer(string userId)
    {
        DB db = new DB();
        SqlDataReader sdr = db.Get("select FirstName, LastName, HcpIndex from mg_tourneyUsers where UserID = " + userId);
        player p = new player();
        if (sdr.Read())
        {
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
        }
        db.Close();
        return p;
    }
    private SortedList<string, player> GetTeam(string teamID)
    {
        DB db = new DB();
        SortedList<string, player> sortName = new SortedList<string, player>();
        SqlDataReader sdr = db.Get("select userID from mg_TourneyTeamPlayers where teamId = " + teamID);
        while (sdr.Read())
        {
            player p = GetPlayer(sdr["userID"].ToString());
            sortName.Add(p.fullName(), p);
        }
        db.Close();
        return sortName;
    }
    private void TeamsList()
    {
        int tourneyid;
        if (Request.QueryString["teamslist"] == "1" &&
            int.TryParse(Request["t"], out tourneyid))
        {
            SqlDataReader sdr;
            SortedList<string, player> sortName = new SortedList<string, player>();
            if (Request["player"] != null)
            {
                string insertteam = "";
                string insertteamindividuals = "";
                string teamname = "";
                string existsselect = "";
                string notexistsselect = "";
                foreach (string userId in Request["player"].ToString().Split(','))
                {
                    player p = GetPlayer(userId);
                    if (!string.IsNullOrEmpty(p.userId))
                    {
                        sortName.Add(p.fullName(), p);
                    }
                }
                foreach (string key in sortName.Keys)
                {
                    player p = sortName[key];
                    if (teamname != "")
                    {
                        teamname += " - ";
                    }
                    teamname += key;

                    insertteamindividuals += "INSERT INTO mg_TourneyTeamPlayers (TeamID, UserID, TeeNumber, TournamentID, Handicap) ";
                    insertteamindividuals += "select MAX(TeamID)," + p.userId + ",0," + tourneyid + "," + p.hcp + " FROM MG_TourneyTeams; ";
                    string finduser = "select 1 from mg_TourneyTeamPlayers WHERE UserID = " + p.userId + " and TournamentID = " + tourneyid;
                    if (!string.IsNullOrEmpty(existsselect)) { existsselect += " AND "; }
                    if (!string.IsNullOrEmpty(notexistsselect)) { notexistsselect += " OR "; }
                    existsselect += " EXISTS(" + finduser + ") ";
                    notexistsselect += " NOT EXISTS(" + finduser + ") ";
                }

                sdr = db.Get("SELECT TeamID FROM MG_TourneyTeams WHERE TournamentID = " + tourneyid + " AND TeamName = " + DB.stringSql(teamname, true));
                if (sdr.Read())
                {
                    insertteam = "UPDATE MG_TourneyTeams SET SideBet = 1 WHERE TeamID = " + sdr["TeamID"].ToString();
                }
                else
                {
                    insertteam = "INSERT INTO MG_TourneyTeams (TeamName, RoundNumber, TournamentID, SideBet, TourneyTeam) ";
                    insertteam += " SELECT " + DB.stringSql(teamname, true) + ",0," + tourneyid + ",1,0 WHERE ";
                    insertteam += existsselect + ";";
                    insertteam += "INSERT INTO MG_TourneyTeams (TeamName, RoundNumber, TournamentID, SideBet, TourneyTeam) ";
                    insertteam += " SELECT " + DB.stringSql(teamname, true) + ",0," + tourneyid + ",0,1 WHERE ";
                    insertteam += notexistsselect + ";";
                    insertteam += insertteamindividuals;
                }
                db.Close(sdr, false);
                db.Exec(insertteam);
            }
            int teamId;
            if (int.TryParse(Request["removeteam"], out teamId))
            {
                string removeteam = "delete from mg_TourneyTeamPlayers where teamId = " + teamId + ";delete from MG_TourneyTeams where teamId = " + teamId + ";";
                db.Exec(removeteam);
            }
            string sql = "select tt.TeamId, tt.TeamName, tt.Flight, tt.SideBet, tt.TourneyTeam, ROUND(SUM(ttp.Handicap),2) AS TeamHCP, ttp1.Handicap AS HCP1, ttp2.Handicap AS HCP2 ";
            sql += " from mg_tourneyTeams tt ";
            sql += " JOIN mg_tourneyTeamPlayers ttp ON tt.TeamID = ttp.TeamID ";
            sql += " join mg_TourneyTeamPlayers ttp1 on tt.TeamId = ttp1.TeamId AND ttp1.UserID = (SELECT TOP 1 innerm1.UserId FROM mg_TourneyTeamPlayers innerm1 JOIN mg_TourneyUsers inneru1 on inneru1.UserId = innerm1.UserId WHERE innerm1.TeamId = tt.TeamId ORDER BY inneru1.LastName + ', ' + inneru1.FirstName ASC) ";
            sql += " join mg_TourneyTeamPlayers ttp2 on tt.TeamId = ttp2.TeamId AND ttp2.UserID = (SELECT TOP 1 innerm1.UserId FROM mg_TourneyTeamPlayers innerm1 JOIN mg_TourneyUsers inneru1 on inneru1.UserId = innerm1.UserId WHERE innerm1.TeamId = tt.TeamId ORDER BY inneru1.LastName + ', ' + inneru1.FirstName DESC) ";
            sql += " where tt.tournamentId = " + tourneyid;
            sql += " group BY tt.TeamId, tt.TeamName, tt.Flight, tt.SideBet, tt.TourneyTeam, ttp1.Handicap, ttp2.Handicap ";
            sql += " ORDER BY TourneyTeam,TeamHCP,TeamName ";
            sdr = db.Get(sql);
            StringBuilder teamsTable = new StringBuilder("<table class='TeamsTable' cellspacing='0'><tr><th>#</th><th>Tournament Team</th><th>Team HCP</th><th>HCP 1</th><th>HCP 2</th><th>Flight</th><th>Side Bet</th><th></th></tr>");
            int x = 0;
            while (sdr.Read())
            {
                if (!sdr.IsDBNull(sdr.GetOrdinal("TeamId")))
                {
                    teamsTable.Append("<tr><td>");
                    teamsTable.Append(++x);
                    teamsTable.Append("</td><td>");
                    teamsTable.AppendFormat("<input type='checkbox' id='intournament' onchange='SetTournamentCheck(this,{0})' {1}/>", sdr["TeamId"], sdr["TourneyTeam"].ToString() == "True" ? "checked='checked'" : "");
                    teamsTable.Append(sdr["TeamName"]);
                    teamsTable.Append("</td><td>");
                    teamsTable.Append(sdr["TeamHCP"]);
                    teamsTable.Append("</td><td>");
                    teamsTable.Append(sdr["HCP1"]);
                    teamsTable.Append("</td><td>");
                    teamsTable.Append(sdr["HCP2"]);
                    teamsTable.Append("</td><td>");
                    teamsTable.AppendFormat("<input id='flight{0}' class='flight' onchange='TeamFlight(this,{0})' value='{1}' /> ", sdr["TeamId"], sdr["Flight"]);
                    teamsTable.Append("</td><td>");
                    teamsTable.AppendFormat("<input type='checkbox' id='sidebet' onchange='SetTournamentCheck(this,{0})' {1}/>", sdr["TeamId"], sdr["SideBet"].ToString() == "True" ? "checked='checked'" : "");
                    teamsTable.Append("</td><td>");
                    teamsTable.AppendFormat("<a id='removeteam{0}' href='javascript:RemoveTeam({0})'>remove</a>", sdr["TeamId"]);
                    teamsTable.Append("</td></tr>");
                }
            }
            db.Close(sdr, false);
            teamsTable.Append("</table>");
            WEB.WriteEndResponse(Response, teamsTable);
        }
    }
    private void TeamSettings()
    {
        int tourneyid = 0;
        bool tournamentcheck;
        int teamid;
        if (bool.TryParse(Request["tournamentcheck"], out tournamentcheck) &&
            int.TryParse(Request["t"], out tourneyid))
        {
            string sql = null;
            if (int.TryParse(Request["intournament"], out teamid))
            {
                sql = "update mg_tourneyteams set TourneyTeam =" + (tournamentcheck ? "1" : "0") + " where teamid = " + teamid;
            }
            else if (int.TryParse(Request["sidebet"], out teamid))
            {
                sql = "update mg_tourneyteams set SideBet =" + (tournamentcheck ? "1" : "0") + " where teamid = " + teamid;
            }
            if (!string.IsNullOrEmpty(sql))
            {
                db.Exec(sql);
            }
        }
        if (Request["flight"] != null &&
            tourneyid > 0 &&
            int.TryParse(Request["teamid"], out teamid))
        {
            string sql = "update mg_tourneyteams set Flight =" + DB.stringSql(Request["flight"], true) + " where teamid = " + teamid;
            db.Exec(sql);
        }
    }
    private void SetTourneyInfo()
    {
        int tourneyid = 0;
        if (Request["updatetourneyinfo"] == "1" &&
            int.TryParse(Request["t"], out tourneyid))
        {
            foreach (string key in Request.Form.AllKeys)
            {
                if (key.Contains("_"))
                {
                    DateTime dateofRound;
                    if (DateTime.TryParse(Request.Form[key], out dateofRound))
                    {
                        string roundnum = key.Substring(key.IndexOf("_") + 1);
                        string sql = "update mg_tourneycourses set DateOfRound = '" + dateofRound.ToString("yyyy-M-d h:mm tt") + "' where TournamentID = " + tourneyid + " and [Round] = " + roundnum + ";";
                        sql += "update mg_tourneyscores set DateOfRound = '" + dateofRound.ToString("yyyy-M-d h:mm tt") + "' where TourneyID = " + tourneyid + " and [RoundNum] = " + roundnum;
                        db.Exec(sql);
                    }
                }
                else
                {
                    string sql = "update mg_tourney set " + key + "=" + DB.stringSql(Request.Form[key], true) + " where TournamentID = " + tourneyid;
                    db.Exec(sql);
                }
            }
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
        TeamSettings();
        SetTourneyInfo();
        Response.Write("<complete>true</complete>");
    }
    protected override void OnUnload(EventArgs e)
    {
        base.OnUnload(e);
        if (db != null) db.Close();
    }
}

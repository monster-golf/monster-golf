using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using docusign;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Data.SqlClient;

public class DB
{
    SqlCommand _command = null;
    public DB()
    {
        string connstr = ConfigurationManager.ConnectionStrings["monster"].ConnectionString;
        _command = new SqlCommand();
        _command.CommandType = System.Data.CommandType.Text;
        _command.Connection = new SqlConnection(connstr);
        _command.Connection.Open();
    }
    public SqlDataReader Get(string select)
    {
        _command.CommandText = select;
        return _command.ExecuteReader();
    }
    public System.Data.DataSet GetDataSet(string select)
    {
        System.Data.DataSet ds = new System.Data.DataSet();
        _command.CommandText = select;
        SqlDataAdapter da = new SqlDataAdapter(_command);
        da.Fill(ds);
        da.Dispose();
        return ds;
    }
    public void Close()
    {
        if (_command != null && _command.Connection != null)
        {
            _command.Connection.Close();
            _command.Connection.Dispose();
        }
    }
    public void Close(SqlDataReader sdr) { Close(sdr, true); }
    public void Close(SqlDataReader sdr, bool closeconn)
    {
        if (sdr != null)
        {
            sdr.Close();
            sdr.Dispose();
        }
        if (closeconn) Close();
    }

    public void Exec(string sql)
    {
        if (_command != null)
        {
            _command.CommandText = sql;
            _command.ExecuteNonQuery();
        }
    }
    private static string stripWord(string val, string word)
    {
        while (val.ToLower().Contains(word))
        {
            val = val.Remove(val.ToLower().IndexOf(word.ToLower()), word.Length);
        }
        return val;
    }
    public static string stringSql(string val)
    {
        val = stripWord(val, "delete");
        val = stripWord(val, "insert");
        val = stripWord(val, "update");
        val = stripWord(val, "alter");
        val = val.Replace("'", "''");
        return val;
    }
}
public class ScoreInfo
{
    public static string GetId()
    {
        return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
    }
    public static List<ScoreInfo> LoadGroup(string groupid)
    {
        DB db = new DB();
        SqlDataReader sdr = db.Get(string.Format("select UserId, UserLookup from mg_tourneyscores where GroupId='{0}' order by dateofround;", DB.stringSql(groupid)));
        List<ScoreInfo> si = new List<ScoreInfo>();
        while (sdr.Read())
        {
            string userid = sdr.IsDBNull(0) ? "0" : sdr[0].ToString();
            string lookup = sdr.IsDBNull(1) ? "" : sdr[1].ToString();
            si.Add(new ScoreInfo(userid, lookup, groupid, false));
        }
        db.Close(sdr);
        return si;
    }
    public static List<ScoreInfo> LoadGroup(string tourneyid, string round, string lookupid)
    {
        DB db = new DB();
        SqlDataReader sdr = db.Get(string.Format("select m1.UserId from mg_tourneyscores m1 join mg_tourneyscores m2 on m2.GroupId = m1.GroupId where m2.TourneyId={0} and m2.RoundNum={1} and m2.UserLookup='{2}';", tourneyid, round, DB.stringSql(lookupid)));
        List<ScoreInfo> si = new List<ScoreInfo>();
        while (sdr.Read()) si.Add(new ScoreInfo(sdr[0].ToString(), tourneyid, round, true));
        db.Close(sdr);
        return si;
    }
    public static List<ScoreInfo> LoadTourneyRound(string tourneyid, string round, string order)
    {
        DB db = new DB();
        SqlDataReader sdr = db.Get(string.Format("select distinct s.UserId,s.GroupId,s.Name,s.StartingHole,t.TeamId from mg_tourneyscores s join mg_tourneyUsers u on u.WebId = s.UserId join mg_tourneyTeamplayers t on t.UserId = u.UserId and t.TournamentId = TourneyId where TourneyId={0} and RoundNum={1} order by " + order + ";", tourneyid, round));
        List<ScoreInfo> si = new List<ScoreInfo>();
        while (sdr.Read()) si.Add(new ScoreInfo(sdr[0].ToString(), tourneyid, round, true));
        db.Close(sdr);
        return si;
    }
    public static bool IsCurrentRound(SqlDataReader sdr, string round) {
        string roundNums = sdr["Round"].ToString().Trim();
        bool isround = false;
        if (roundNums.Contains(","))
        {
            foreach (string r in roundNums.Split(','))
            {
                if (r == round)
                {
                    isround = true;
                    break;
                }
            }
        }
        else isround = roundNums == round;
        return isround;
    }
    public static List<ScoreInfo> LoadCourseInfo(string tourneyid, string round, out string tourneyName, out string dateOfRound, out string courseName)
    {
        DB db = new DB();
        string select = "select t.TournamentID, Location, Slogan, Description, tc.CourseID, tc.[Round], Course, DateOfRound, ID, TeeNumber, Slope, Rating, Par1, Par2, Par3, Par4, Par5, Par6, Par7, Par8, Par9, Par10, Par11, Par12, Par13, Par14, Par15, Par16, Par17, Par18, Handicap1, Handicap2, Handicap3, Handicap4, Handicap5, Handicap6, Handicap7, Handicap8, Handicap9, Handicap10, Handicap11, Handicap12, Handicap13, Handicap14, Handicap15, Handicap16, Handicap17, Handicap18 " +
            "from mg_Tourney t " +
            "join mg_tourneycourses tc on tc.tournamentid = t.tournamentid " +
            "join mg_TourneyCourseDetails tcd on tcd.CourseID = tc.CourseId " +
            "where t.TournamentID = " + tourneyid +
            " order by tc.[Round], TeeNumber";
        SqlDataReader sdr = db.Get(select);
        tourneyName = "";
        dateOfRound = "";
        courseName = "";
        List<ScoreInfo> si = new List<ScoreInfo>();
        while (sdr.Read())
        {
            if (!sdr.IsDBNull(sdr.GetOrdinal("Round")))
            {
                if (IsCurrentRound(sdr, round))
                {
                    if (tourneyName == "")
                    {
                        if (!sdr.IsDBNull(sdr.GetOrdinal("Slogan"))) tourneyName += sdr["Slogan"].ToString();
                        if (!sdr.IsDBNull(sdr.GetOrdinal("Description"))) tourneyName += ", " + sdr["Description"].ToString();
                        if (!sdr.IsDBNull(sdr.GetOrdinal("Location"))) tourneyName += ", " + sdr["Location"].ToString();
                    }
                    if (courseName == "")
                    {
                        if (!sdr.IsDBNull(sdr.GetOrdinal("Course"))) courseName += " " + sdr["Course"].ToString();
                    }
                    if (dateOfRound == "")
                    {
                        DateTime startofround = DateTime.MinValue;
                        if (!sdr.IsDBNull(sdr.GetOrdinal("DateOfRound")))
                        {
                            DateTime.TryParse(sdr["DateOfRound"].ToString(), out startofround);
                        }
                        if (startofround != DateTime.MinValue) dateOfRound = startofround.ToString("M/d/yyyy h:mm tt");
                    }
                    List<string> pars = new List<string>();
                    int parsf9 = 0, parsb9 = 0;
                    List<string> hcps = new List<string>();
                    for (int x = 1; x <= 18; x++)
                    {
                        int par;
                        if (!sdr.IsDBNull(sdr.GetOrdinal("Par" + x)) &&
                            int.TryParse(sdr["Par" + x].ToString(), out par))
                        {
                            pars.Add(par.ToString());
                            if (x > 9) parsb9 += par;
                            else parsf9 += par;
                        }
                        else pars.Add("");
                        if (!sdr.IsDBNull(sdr.GetOrdinal("Handicap" + x))) hcps.Add(sdr["Handicap" + x].ToString());
                        else hcps.Add("");
                    }
                    string teeNum = "0";
                    if (!sdr.IsDBNull(sdr.GetOrdinal("TeeNumber"))) teeNum = sdr["TeeNumber"].ToString();
                    string gender = (teeNum == "0") ? "Men's" : "Women's";
                    si.Add(new ScoreInfo("Pars" + teeNum, "", gender + " Par", pars, parsf9.ToString(), parsb9.ToString(), (parsf9 + parsb9).ToString(), "", ""));
                    string slope = "";
                    if (!sdr.IsDBNull(sdr.GetOrdinal("Slope"))) slope = sdr["Slope"].ToString();
                    string rating = "";
                    if (!sdr.IsDBNull(sdr.GetOrdinal("Rating"))) rating = sdr["Rating"].ToString();

                    si.Add(new ScoreInfo("Hcps" + teeNum, "", gender + " HCP", hcps, "", "", "", slope, rating));
                }
            }
        }
        db.Close(sdr);
        return si;
    }
    public enum ScoreKey
    {
        ID,
        LookupID,
        Name,
        Out,
        In,
        Total,
        HCP,
        Net,
        RoundComplete,
        CardSigned,
        CardAttested,
        CourseName,
        CourseSlope,
        CourseRating,
        DateOfRound,
        GroupId,
        EmailSent,
        TourneyScoreID,
        StartingHole
    }
    public static List<string> empty18List(bool useNum)
    {
        List<string> list18 = new List<string>();
        for (int x = 1; x <= 18; x++) list18.Add((useNum) ? x.ToString() : "");
        return list18;
    }
    public static List<string> empty18List()
    {
        return empty18List(false);
    }
    public Dictionary<string, string> Scores = new Dictionary<string, string>();
    public ScoreInfo(string id)
    {
        Load(id, GetId(), "", empty18List(), "", "", "", "", "", false, false, "", "", "113", "72", false, "", 1);
    }
    public ScoreInfo(string userid, string id1, string id2, bool usetourney)
    {
        string sql = "select Name, Hole1, Hole2, Hole3, Hole4, Hole5, Hole6, Hole7, Hole8, Hole9, Hole10, Hole11, Hole12, Hole13, Hole14, Hole15, Hole16, Hole17, Hole18, HCP, CardSigned, CardAttested, UserLookup, GroupId, CourseName, CourseSlope, CourseRating, EmailSent, TourneyScoreID, StartingHole from mg_tourneyscores where UserId={0} and UserLookup='{1}' and GroupId='{2}';";
        if (usetourney) sql = "select Name, Hole1, Hole2, Hole3, Hole4, Hole5, Hole6, Hole7, Hole8, Hole9, Hole10, Hole11, Hole12, Hole13, Hole14, Hole15, Hole16, Hole17, Hole18, HCP, CardSigned, CardAttested, UserLookup, GroupId, CourseName, CourseSlope, CourseRating, EmailSent, TourneyScoreID, StartingHole from mg_tourneyscores where UserId={0} and TourneyId={1} and RoundNum={2};";
        LoadDB(userid, string.Format(sql, userid, DB.stringSql(id1), DB.stringSql(id2)));
    }
    private void LoadDB(string id, string select)
    {
        DB db = new DB();
        SqlDataReader sdr = db.Get(select);
        if (sdr.Read())
        {
            string name = sdr[0].ToString();
            List<string> scores = new List<string>();
            int score;
            int f9total = 0;
            int b9total = 0;
            for (int x = 1; x <= 18; x++)
            {
                score = 0;
                if (!sdr.IsDBNull(x)) int.TryParse(sdr[x].ToString(), out score);
                if (score == 0) scores.Add("");
                else scores.Add(score.ToString());
                if (x < 10) f9total += score;
                else b9total += score;
            }
            int total = f9total + b9total;
            string f9 = (f9total > 0) ? f9total.ToString() : "";
            string b9 = (b9total > 0) ? b9total.ToString() : "";
            string tot = (total > 0) ? total.ToString() : "";
            string shcp = "";
            if (!sdr.IsDBNull(19)) shcp = sdr[19].ToString();
            int hcp;
            int.TryParse(shcp, out hcp);
            int net = (total > 0) ? total - hcp : 0;
            string snet = (net > 0) ? net.ToString() : "";
            bool cardsigned = false;
            bool.TryParse(sdr[20].ToString(), out cardsigned);
            bool cardattested = false;
            bool.TryParse(sdr[21].ToString(), out cardattested);
            string userlookup = (!sdr.IsDBNull(22)) ? sdr[22].ToString() : "";
            string groupid = (!sdr.IsDBNull(23)) ? sdr[23].ToString() : "";
            string course = (!sdr.IsDBNull(24)) ? sdr[24].ToString() : "";
            string slope = (!sdr.IsDBNull(25)) ? sdr[25].ToString() : "";
            string rating = (!sdr.IsDBNull(26)) ? sdr[26].ToString() : "";
            bool emailsent = false;
            bool.TryParse(sdr[27].ToString(), out emailsent);
            string tourneyscoreid = (!sdr.IsDBNull(28)) ? sdr[28].ToString() : "";
            string shole = (!sdr.IsDBNull(29)) ? sdr[29].ToString() : "1";
            int starthole;
            int.TryParse(shole, out starthole);

            Load(id, userlookup, name, scores, f9, b9, tot, shcp, snet, cardsigned, cardattested, groupid, course, slope, rating, emailsent, tourneyscoreid, starthole);
        }
        else
        {
            Load(id, "", "", empty18List(), "", "", "", "", "", false, false, "", "", "113", "72", false, "", 1);
        }
        db.Close(sdr);
    }
    private void Load(string id, string lookupid, string name, List<string> scores, string f9total, string b9total, string total, string hcp, string net, bool cardsigned, bool cardattested, string groupid, string coursename, string slope, string rating, bool emailsent, string tourneyscoreid, int startinghole)
    {
        int x = 1;
        bool roundcomplete = true;
        foreach (string f in scores)
        {
            if (f == "") roundcomplete = false;
            Scores.Add(x.ToString(), f);
            x++;
        }
        Scores.Add(ScoreKey.ID.ToString(), id);
        Scores.Add(ScoreKey.LookupID.ToString(), lookupid);
        Scores.Add(ScoreKey.Name.ToString(), name);
        Scores.Add(ScoreKey.Out.ToString(), f9total);
        Scores.Add(ScoreKey.In.ToString(), b9total);
        Scores.Add(ScoreKey.Total.ToString(), total);
        Scores.Add(ScoreKey.HCP.ToString(), hcp);
        Scores.Add(ScoreKey.Net.ToString(), net);
        Scores.Add(ScoreKey.RoundComplete.ToString(), roundcomplete.ToString());
        Scores.Add(ScoreKey.CardSigned.ToString(), cardsigned.ToString());
        Scores.Add(ScoreKey.CardAttested.ToString(), cardattested.ToString());
        Scores.Add(ScoreKey.GroupId.ToString(), groupid);
        Scores.Add(ScoreKey.CourseName.ToString(), coursename);
        Scores.Add(ScoreKey.CourseSlope.ToString(), slope);
        Scores.Add(ScoreKey.CourseRating.ToString(), rating);
        Scores.Add(ScoreKey.EmailSent.ToString(), emailsent.ToString());
        Scores.Add(ScoreKey.TourneyScoreID.ToString(), tourneyscoreid);
        Scores.Add(ScoreKey.StartingHole.ToString(), startinghole.ToString());
    }
    public ScoreInfo(string id, string lookupid, string name, List<string> scores, string f9total, string b9total, string total, string hcp, string net)
    {
        Load(id, lookupid, name, scores, f9total, b9total, total, hcp, net, false, false, "", "", "113", "72", false, "", 1);
    }
    public ScoreInfo(string id, string lookupid, string name, List<string> scores, string f9total, string b9total, string total, string hcp, string net, string course, string slope, string rating, bool emailsent, string tourneyscoreid, int startinghole)
    {
        Load(id, lookupid, name, scores, f9total, b9total, total, hcp, net, false, false, "", course, slope, rating, emailsent, tourneyscoreid, startinghole);
    }
    public string ID
    { 
        get { return Scores[ScoreKey.ID.ToString()]; }
        set { Scores[ScoreKey.ID.ToString()] = value; }
    }
    public string LookupID { get { return Scores[ScoreKey.LookupID.ToString()]; } }
    public string Name { get { return Scores[ScoreKey.Name.ToString()]; } }
    public string Out { get { return Scores[ScoreKey.Out.ToString()]; } }
    public string In { get { return Scores[ScoreKey.In.ToString()]; } }
    public string Total { get { return Scores[ScoreKey.Total.ToString()]; } }
    public string HCP { get { return Scores[ScoreKey.HCP.ToString()]; } }
    public string Net { get { return Scores[ScoreKey.Net.ToString()]; } }
    private bool BoolCheck(ScoreKey sk)
    {
        bool complete = false;
        bool.TryParse(Scores[sk.ToString()], out complete);
        return complete;
    }
    public bool RoundComplete { get { return BoolCheck(ScoreKey.RoundComplete); } }
    public bool CardSigned { get { return BoolCheck(ScoreKey.CardSigned); } }
    public bool CardAttested { get { return BoolCheck(ScoreKey.CardAttested); } }
    public string GroupID { get { return Scores[ScoreKey.GroupId.ToString()]; } }
    public string CourseName { get { return Scores[ScoreKey.CourseName.ToString()]; } }
    public string CourseSlope { get { return Scores[ScoreKey.CourseSlope.ToString()]; } }
    public string CourseRating { get { return Scores[ScoreKey.CourseRating.ToString()]; } }
    public bool EmailSent { get { return BoolCheck(ScoreKey.EmailSent); } }
    public string TourneyScoreID { get { return Scores[ScoreKey.TourneyScoreID.ToString()]; } }
    public string StartingHole { get { return Scores[ScoreKey.StartingHole.ToString()]; } }
}

/// <summary>
/// Summary description for DS
/// </summary>
public class DS
{
    string _envId = "";
    APIClient _dsapi;
    EnvelopeStatus _envstatus;

	public DS(string envId)
	{
        _envId = envId;
        if (envId != "")
        {
            _envstatus = DSApi().RequestStatus(envId);
        }
	}
    public FilteredEnvelopeStatuses RequestStatuses(DateTime begindate, DateTime enddate)
    {
        EnvelopeStatusFilter esf = new EnvelopeStatusFilter();
        esf.AccountId = ConfigurationManager.AppSettings["ds_accountid"];
        EnvelopeStatusFilterBeginDateTime begin = new EnvelopeStatusFilterBeginDateTime();
        begin.statusQualifier = "Created";
        begin.Value = begindate;
        esf.BeginDateTime = begin;
        esf.Statuses = new EnvelopeStatusCode[] { EnvelopeStatusCode.Any };
        esf.EndDateTime = enddate;
        esf.EndDateTimeSpecified = true;
        FilteredEnvelopeStatuses statuses = DSApi().RequestStatusesEx(esf);
        return statuses;
    }
    public APIClient DSApi()
    {
        if (_dsapi == null)
        {
            _dsapi = new APIClient();
            _dsapi.Url = ConfigurationManager.AppSettings["ds_url"];
        }
        return _dsapi;
    }
    public EnvelopeStatus EnvStatus { get { return _envstatus; } }
    public bool Send(List<ScoreInfo> players, string scorername, string scorerid, string attestname, string attestid, string courseName, string courseSlope, string courseRating, Dictionary<string, string> nameemails)
    {
        Envelope _envelope = new Envelope();

        List<Document> documents = new List<Document>();
        try { documents.Add(DSDoc(players, courseName, courseSlope, courseRating)); }
        catch { return false; }
        _envelope.Documents = documents.ToArray();
        List<Recipient> recipients = new List<Recipient>();
        int recipid = 1;
        recipients.Add(DSRecip(recipid++, scorerid, scorername, "", RecipientTypeCode.Signer));
        bool addattest = attestid != "" && attestname != "" && (attestid != scorerid || attestname != scorername);
        if (addattest)
        {
            recipients.Add(DSRecip(recipid++, attestid, attestname, "", RecipientTypeCode.Signer));
        }
        //recipients.Add(DSRecip(recipid, "", "", "", RecipientTypeCode.CarbonCopy));
        foreach (string key in nameemails.Keys) recipients.Add(DSRecip(recipid, "", nameemails[key], key, RecipientTypeCode.CarbonCopy));
        _envelope.Recipients = recipients.ToArray();
        _envelope.Subject = "Monster Scoring";

        List<Tab> tabs = new List<Tab>();
        tabs.Add(DSTab("Scorer Signature", "1", TabTypeCode.SignHere, 110, 40));
        //tabs.Add(DSTab("Scorer Full Name", "1", TabTypeCode.FullName, 115, 235));
        tabs.Add(DSTab("Scorer Date Signed", "1", TabTypeCode.DateSigned, 240, 75));
        if (addattest)
        {
            tabs.Add(DSTab("Attest Signature", "2", TabTypeCode.SignHere, 340, 40));
            //tabs.Add(DSTab("Attest Full Name", "2", TabTypeCode.FullName, 115, 305));
            tabs.Add(DSTab("Attest Date Signed", "2", TabTypeCode.DateSigned, 470, 75));
        }
        _envelope.Tabs = tabs.ToArray();

        bool success = false;
        try
        {
            _envelope.AccountId = ConfigurationManager.AppSettings["ds_accountid"];
            _envstatus = DSApi().CreateAndSendEnvelope(_envelope);
            _envId = _envstatus.EnvelopeID;
            success = true;
        }
        catch
        {
            success = false;
        }
        return success;
    }
    public string SignURL(int recipId, string tourneyid, string roundnum, string userlookup)
    {
        RecipientStatus recip = null;
        if (_envId != "" && _envstatus != null)
        {
            RecipientStatus[] recips = _envstatus.RecipientStatuses;
            foreach (RecipientStatus r in recips)
            {
                if (r.RoutingOrder == (ushort)recipId)
                {
                    recip = r;
                    break;
                }
            }
            if (recip != null)
            {
                RequestRecipientTokenAuthenticationAssertion rrta = new RequestRecipientTokenAuthenticationAssertion();
                rrta.AssertionID = "1";
                rrta.AuthenticationInstant = DateTime.Now;
                rrta.AuthenticationMethod = RequestRecipientTokenAuthenticationAssertionAuthenticationMethod.Email;
                rrta.SecurityDomain = "monstergolf";
                RequestRecipientTokenClientURLs rrtcu = new RequestRecipientTokenClientURLs();
                string backurl = string.Format(ConfigurationManager.AppSettings["ds_backurl"] + "dscatch.aspx?c={{0}}&e={0}&s={1}&t={2}&r={3}&u={4}", _envId, recip.ClientUserId, tourneyid, roundnum, userlookup);
                rrtcu.OnAccessCodeFailed = string.Format(backurl, "acfail");
                rrtcu.OnCancel = string.Format(backurl, "cancel");
                rrtcu.OnDecline = string.Format(backurl, "decline");
                rrtcu.OnException = string.Format(backurl, "exception");
                rrtcu.OnFaxPending = string.Format(backurl, "faxpend");
                rrtcu.OnIdCheckFailed = string.Format(backurl, "idcheckfail");
                rrtcu.OnSessionTimeout = string.Format(backurl, "sessiontimeout");
                rrtcu.OnSigningComplete = string.Format(backurl, "complete");
                rrtcu.OnTTLExpired = string.Format(backurl, "ttlexpired");
                rrtcu.OnViewingComplete = string.Format(backurl, "viewcomplete");
                return DSApi().RequestRecipientToken(_envId, recip.ClientUserId, recip.UserName, recip.Email, rrta, rrtcu);
            }
        }
        return "";
    }
    public Document DSDoc(List<ScoreInfo> players, string courseName, string courseSlope, string courseRating)
    {
        StringBuilder output = new StringBuilder("<html><head><title>Monster Scores</title>");
        output.Append("<style type='text/css'>");
        output.Append("body{font-family:sans-serif;font-size:10pt;}");
        output.Append("td{font-family:sans-serif;font-size:9pt;}");
        output.Append("</style></head><body>");
        output.Append("<table cellpadding='5'>");
        output.Append("<tr><td>Scorer</td><td>______________________________________</td><td>Attest</td><td>______________________________________</td></tr>");
        output.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
        output.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
        output.Append("</table>");
        output.AppendFormat("{0} &nbsp; &nbsp; {1}/{2} &nbsp; &nbsp; Date: {3}", courseName, courseSlope, courseRating, DateTime.Now.ToShortDateString());
        output.Append("<table cellpadding='2' cellspacing='0' border='1'>");

        players.Insert(0, new ScoreInfo("label", "", "", ScoreInfo.empty18List(true), "out", "in", "total", "hcp", "net"));

        foreach (ScoreInfo player in players)
        {
            output.Append("<tr>");
            output.AppendFormat("<td width='140' nowrap='nowrap' style='background-color:#efefef;'>{0}</td>", ((player.Name == "") ? "&nbsp;" : player.Name));
            for (int x = 1; x <= 18; x++)
            {
                output.AppendFormat("<td align='center' style='width:30px'>{0}</td>", player.Scores[x.ToString()]);
                if (x == 9) output.AppendFormat("<td width='40' align='center' style='background-color:#efefef;'>{0}</td>", player.Out);
            }
            output.AppendFormat("<td align='center' style='background-color:#efefef;'>{0}</td>", player.In);
            output.AppendFormat("<td align='center' style='background-color:#efefef;'>{0}</td>", player.Total);
            output.AppendFormat("<td align='center' style='background-color:#efefef;'>{0}</td>", player.HCP);
            output.AppendFormat("<td align='center' style='background-color:#efefef;'>{0}</td>", player.Net);
            output.Append("</tr>");
        }

        output.Append("</table>");
        output.Append("</body></html>");
        Document document = new Document();
        document.ID = "1";
        document.Name = "MonsterScores";
        UTF8Encoding encode = new UTF8Encoding();
        document.PDFBytes = encode.GetBytes(output.ToString());
        document.TransformPdfFields = false;
        document.FileExtension = "html";
        return document;
    }
    public Tab DSTab(string label, string recipid, TabTypeCode tabtype, int x, int y)
    {
        Tab tab = new Tab();
        tab.Type = tabtype;
        tab.Name = label;
        tab.TabLabel = label;
        tab.DocumentID = "1";
        tab.PageNumber = "1";
        tab.XPosition = x.ToString();
        tab.YPosition = y.ToString();
        tab.RecipientID = recipid;
        return tab;
    }
    private Recipient DSRecip(int recipid, string userid, string name, string email, RecipientTypeCode type)
    {
        Recipient recip = new Recipient();
        recip.ID = recipid.ToString();
        if (type == RecipientTypeCode.CarbonCopy) recip.ID = recipid.ToString() + name.Replace(" ", "");
        recip.RoutingOrder = (ushort)recipid;
        recip.RoutingOrderSpecified = true;
        recip.Type = type;
        if (name == "")
        {
            recip.Email = "aaronwald@hotmail.com";
            recip.UserName = "Aaron Wald";
        }
        else
        {
            recip.Email = (email == "") ? "monster@monstergolf.org" : email;
            recip.UserName = name;
        }
        if (userid != "")
        {
            RecipientCaptiveInfo rci = new RecipientCaptiveInfo();
            rci.ClientUserId = userid;
            recip.CaptiveInfo = rci;
        }
        return recip;
    }
}

public class APIClient : APIService
{
    protected override System.Net.WebRequest GetWebRequest(Uri uri)
    {
        string credsheader = string.Format(
           "<DocuSignCredentials><Username>{0}</Username><Password>{1}</Password><IntegratorKey>{2}</IntegratorKey></DocuSignCredentials>",
           ConfigurationManager.AppSettings["ds_email"], ConfigurationManager.AppSettings["ds_pass"], ConfigurationManager.AppSettings["ds_key"]); ;
        System.Net.HttpWebRequest r = base.GetWebRequest(uri) as System.Net.HttpWebRequest;
        if (credsheader != null) r.Headers.Add("X-DocuSign-Authentication", credsheader);
        return r;
    }
}

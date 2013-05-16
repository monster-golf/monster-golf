using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;

public partial class ScoresXml : System.Web.UI.Page
{
    private string UpdateStr(Dictionary<int, int> holescore, int tourneyid, int round, int userid, string userlookup, string groupid, string name, int hcp, string coursename, int? courseslope, float? courserating, bool update)
    {
        string cmd = "";
        if (update)
        {
            cmd = "update mg_tourneyscores set ";
            cmd += "Name = '" + DB.stringSql(name) + "'";
            cmd += ",HCP = " + hcp;
            if (coursename != null) cmd += string.Format(",CourseName = '{0}'", DB.stringSql(coursename));
            if (courseslope.HasValue) cmd += ",CourseSlope = " + courseslope.Value;
            if (courserating.HasValue) cmd += ",CourseRating = " + courserating.Value;
            foreach (int hole in holescore.Keys) cmd += string.Format(",Hole{0}={1}", hole, holescore[hole]);
            cmd += " where ";
            if (tourneyid > 0 && round > 0) cmd += string.Format(" UserId={0} and TourneyId={1} and RoundNum={2};", userid, tourneyid, round);
            else cmd += string.Format(" UserLookup='{0}' and GroupId='{1}';", DB.stringSql(userlookup), DB.stringSql(groupid));
        }
        else
        {
            cmd = "insert into mg_tourneyscores(UserId,TourneyId,RoundNum,Name,HCP,DateOfRound";
            if (coursename != null) cmd+=",CourseName";
            if (courseslope.HasValue) cmd+=",CourseSlope";
            if (courserating.HasValue) cmd+=",CourseRating";
            if (userlookup != "") cmd += ",UserLookup";
            if (groupid != "") cmd += ",GroupId";
            foreach (int hole in holescore.Keys) cmd += string.Format(",Hole{0}", hole);
            cmd += string.Format(") values ({0},{1},{2},'{3}',{4},getDate()", userid, tourneyid, round, name, hcp);
            if (coursename != null) cmd += string.Format(",'{0}'", DB.stringSql(coursename));
            if (courseslope.HasValue) cmd += string.Format(",{0}", courseslope.Value);
            if (courserating.HasValue) cmd += string.Format(",{0}", courserating.Value);
            if (userlookup != "") cmd += string.Format(",'{0}'", DB.stringSql(userlookup));
            if (groupid != "") cmd += string.Format(",'{0}'", DB.stringSql(groupid));
            foreach (int hole in holescore.Keys) cmd += string.Format(",{0}", holescore[hole]);
            cmd += ");";
        }
        return cmd;
    }
    protected bool SaveScore()
    {
        if (Request.QueryString["savescore"] != null)
        {
            int userid = 0;
            string userlookup = "";
            Dictionary<int, int> holescore = new Dictionary<int, int>();
            int hole = 0;
            int tourneyid = 0;
            int round = 0;
            string groupid = null;
            int score = 0;
            string name = "";
            int hcp = 0;
            int? slope = null;
            float? rating = null;
            string course = null;

            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("score_"))
                {
                    string[] splitit = key.Split('_');
                    int.TryParse(splitit[1], out hole);
                    int.TryParse(Request.Form[key], out score);
                    holescore.Add(hole, score);
                }
                else if (key.StartsWith("userid")) int.TryParse(Request.Form[key], out userid);
                else if (key.StartsWith("userlookup")) userlookup = Request.Form[key];
                else if (key.StartsWith("hcp")) int.TryParse(Request.Form[key], out hcp);
                else if (key.StartsWith("name")) name = Request.Form[key];
                else if (key.StartsWith("coursename")) course = Request.Form[key];
                else if (key.StartsWith("tourney")) int.TryParse(Request.Form[key], out tourneyid);
                else if (key.StartsWith("round"))
                {
                    if (!int.TryParse(Request.Form[key], out round)) groupid = Request.Form[key];
                }
                else if (key.StartsWith("courseslope"))
                {
                    int tryslope;
                    if (int.TryParse(Request.Form[key], out tryslope)) slope = tryslope;
                }
                else if (key.StartsWith("courserating"))
                {
                    float tryrating;
                    if (float.TryParse(Request.Form[key], out tryrating)) rating = tryrating;
                }
            }
            if (holescore.Count > 0 && ((round > 0 && tourneyid > 0) || (userlookup != "" && groupid != "")))
            {
                string sel = "select * from mg_tourneyscores where ";
                if (tourneyid > 0 && round > 0) sel += string.Format("UserId={0} and TourneyId={1} and RoundNum={2};", userid, tourneyid, round);
                else sel += string.Format("UserLookup='{0}' and GroupId='{1}';", DB.stringSql(userlookup), DB.stringSql(groupid));
                SqlDataReader sdr = db.Get(sel);
                bool hasrows = sdr.HasRows;
                db.Close(sdr, false);
                db.Exec(UpdateStr(holescore, tourneyid, round, userid, userlookup, groupid, name, hcp, course, slope, rating, hasrows));
                db.Close(sdr);
            }
        }
        return false;
    }
    protected void Search()
    {
        SqlDataReader sdr = null;
        if (Request["player"] != null)
        {
            string[] playernames = Request["player"].Split(' ');
            string select = "";
            select += "FirstName + ' ' + LastName like '%" + DB.stringSql(Request["player"]) + "%'";
            StringBuilder div = new StringBuilder("<div>");
            if (select != "")
            {
                sdr = db.Get("select distinct UserId,FirstName,LastName,Email,Handicap from mg_users where " + select + " order by FirstName");
                while (sdr.Read())
                {
                    string first = (sdr.IsDBNull(1)) ? "" : sdr[1].ToString();
                    string last = (sdr.IsDBNull(2)) ? "" : sdr[2].ToString();
                    string email = (sdr.IsDBNull(3)) ? "" : sdr[3].ToString();
                    div.AppendFormat("<div><a href=\"javascript:PlayerSelect('{0}','{1}','{2}','{3}','{4}')\">{5} {6} {4}</a></div>", sdr[0], DB.stringSql(first), DB.stringSql(last), DB.stringSql(email), sdr[4], first, last);
                }
                if (!sdr.HasRows) div.Append("Player not found<br/>in Monster Handicap<br/>");
            }
            else div.Append("Please enter search info. ");
            div.Append("<a href='javascript:void(0);' onclick='PlayerSearchClose(this.parentNode.parentNode);'>Close</a>");
            div.Append("</div>");
            db.Close(sdr);
            WEB.WriteEndResponse(Response, div);
        }
        if (Request["course"] != null)
        {
            string coursesch = DB.stringSql(Request["course"]);
            StringBuilder div = new StringBuilder("<div>");
            if (coursesch != "")
            {
                sdr = db.Get("select distinct CourseName,Slope,Rating from mg_scores where CourseName like '%" + coursesch +"%'");
                while (sdr.Read())
                {
                    string coursename = (sdr.IsDBNull(0)) ? "" : sdr[0].ToString();
                    div.AppendFormat("<div><a href=\"javascript:CourseSelect('{0}','{2}','{3}')\">{1} Slope:{2} Rating:{3}</a></div>", coursename.Replace("'", "\\'"), coursename, sdr[1], sdr[2]);
                }
                if (!sdr.HasRows) div.Append("No course found ");
                div.Append("<a href='javascript:CourseEnter()'>--Enter Course Information--</a>");
            }
            else div.Append("Please enter a course name");
            div.Append("</div>");
            db.Close(sdr);
            WEB.WriteEndResponse(Response, div);
        }
        if (Request["externalcourse"] != null)
        {
            string url = "http://www.mobilegolfstats.com/courses/course_view_step2.php";
            string formdata = "course_name=West Seattle";
            formdata += "&country=United States";
            //formdata += "&state_select=Washington";
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            byte[] resp = wc.UploadData(url, "POST", System.Text.UTF8Encoding.UTF8.GetBytes(formdata));
            string respstr = System.Text.Encoding.ASCII.GetString(resp);
            List<string> courses = new List<string>();
            int idxofView = respstr.IndexOf("course_view.php");
            while (idxofView > -1)
            {
                int idxOfA = respstr.LastIndexOf("<a ", idxofView);
                int idxOfAEnd = respstr.IndexOf("</a>", idxofView) + "</a>".Length;
                string course = respstr.Substring(idxOfA, idxOfAEnd - idxOfA);
                course = course.Insert(course.IndexOf("course_view.php"), "http://www.mobilegolfstats.com/courses/");
                courses.Add(course);
                idxofView = respstr.IndexOf("course_view.php", idxofView + "course_view.php".Length);
            }
            StringBuilder div = new StringBuilder("<div>");
            foreach (string course in courses) div.Append(course);
            div.Append("</div>");
            WEB.WriteEndResponse(Response, div);
        }
    }
    protected void CurrentRoundCheck()
    {
        if (Request["checkforround"] != null)
        {
            int userid;
            if (int.TryParse(Request["checkforround"], out userid))
            {
                string tourney = "";
                string roundnum = "";
                string lookup = "";
                string group = "";
                SqlDataReader sdr = db.Get(string.Format("select tourneyid, roundnum, userlookup, groupid from mg_tourneyscores where userid = {0} and dateofround between '{1}' and '{2}' order by dateofround desc", userid, DateTime.Now.ToShortDateString(), DateTime.Now.AddDays(1.0).ToShortDateString()));
                if (sdr.Read())
                {
                    tourney = (sdr.IsDBNull(0)) ? "" : sdr[0].ToString();
                    if (tourney == "0") tourney = "";
                    roundnum = (sdr.IsDBNull(1)) ? "" : sdr[1].ToString();
                    if (roundnum == "0") roundnum = "";
                    lookup = (sdr.IsDBNull(2)) ? "" : sdr[2].ToString();
                    group = (sdr.IsDBNull(3)) ? "" : sdr[3].ToString();
                }
                db.Close(sdr);
                StringBuilder div = new StringBuilder("<div>");
                div.AppendFormat("<div tourney='{0}' roundnum='{1}' lookup='{2}' groupid='{3}'></div>", tourney, roundnum, lookup, group);
                WEB.WriteEndResponse(Response, div);
            }
        }
    }
    protected void GroupDelete()
    {
        if (Request["deletegroup"] != null)
        {
            db.Exec(string.Format("delete from mg_tourneyscores where GroupId='{0}'", DB.stringSql(Request["deletegroup"])));
        }
    }
    DB db;
    protected void Page_Load(object sender, EventArgs e)
    {
        db = new DB();
        SaveScore();
        Search();
        CurrentRoundCheck();
        GroupDelete();
        Response.Write("<complete>true</complete>");
    }
    protected override void OnUnload(EventArgs e)
    {
        base.OnUnload(e);
        db.Close();
    }
}

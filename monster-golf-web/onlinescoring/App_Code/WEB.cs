using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

/// <summary>
/// Summary description for WEB
/// </summary>
public class WEB
{
    public static Literal TB(string id, string val, string css, int max, string onkeyup, string onchange, string onfocus, string onblur, bool enabled, bool hide, bool isnum, string score, int tabindex)
    {
        Literal lit = new Literal();
        string type = ((isnum) ? "number\" inputscope=\"Number" : "text");
        lit.Text += string.Format("<input name=\"{0}\" class=\"{1}\" id=\"{0}\" size=\"1\" autocomplete=\"off\" onkeypress=\"{2}\" onkeyup=\"{2}\" onchange=\"{3}\" type=\"{4}\" maxLength=\"{5}\" score=\"{6}\" value=\"{7}\" onfocus=\"{8}\" onblur=\"{9}\"",
            id, css, onkeyup, onchange, type, max, score, val, onfocus, onblur);
        if (!enabled) lit.Text += " disabled=\"disabled\"";
        if (hide) lit.Text += " style=\"display:none\"";
        if (tabindex > 0) lit.Text += " tabindex=\"" + tabindex.ToString() + "\"";
        lit.Text += " />";
        return lit;
    }
    public static TextBox TB(string id, string val, string css, int max, string onkeyup, string onchange, bool enabled, bool hide, int size)
    {
        TextBox tb = new TextBox();
        tb.ID = id;
        tb.Text = val;
        tb.CssClass = css;
        tb.MaxLength = max;
        if (onkeyup == "") onkeyup = "return KeyEntKill(event);";
        tb.Attributes.Add("onkeypress", "return KeyEntKill(event);");
        tb.Attributes.Add("onkeyup", onkeyup);
        tb.Attributes.Add("onchange", onchange);
        tb.Enabled = enabled;
        tb.Attributes["type"] = "number";
        if (hide) tb.Style.Add(HtmlTextWriterStyle.Display, "none");
        if (size > 0) tb.Attributes.Add("size", size.ToString());
        return tb;
    }
    public static Panel PNL(string id, string val, string css, bool hide)
    {
        Panel pnl = new Panel();
        pnl.ID = id;
        pnl.CssClass = css;
        Literal lb = new Literal();
        lb.Text = val;
        pnl.Controls.Add(lb);
        if (hide) pnl.Style.Add(HtmlTextWriterStyle.Display, "none");
        return pnl;
    }
    public static Image IMG(string id, string url, string css, string onmousedown, bool hide)
    {
        Image img = new Image();
        img.ID = id;
        img.ImageUrl = url;
        img.CssClass = css;
        img.Attributes.Add("onmousedown", onmousedown);
        if (hide) img.Style.Add(HtmlTextWriterStyle.Display, "none");
        return img;
    }
    public static CheckBox CB(string id, string text, bool check, string css, string onclick, bool hide)
    {
        CheckBox cb = new CheckBox();
        cb.ID = id;
        cb.Text = text;
        cb.CssClass = css;
        cb.Attributes.Add("onclick", onclick);
        cb.Checked = check;
        if (hide) cb.Style.Add(HtmlTextWriterStyle.Display, "none");
        return cb;
    }

	public WEB()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void TourneyRow(DB db, StringBuilder sb, ScoreInfo s, int roundnum, int tourneyid, bool email, bool enterscores, string groupclass, bool addstartinghole, int golfersingroup, bool addpagebreak)
    {
        string name = "";
        string group = "";
        if (s.LookupID != "" && s.GroupID == "")
        {
            name = string.Format("<input type='checkbox' class='cb' name='playerchk' id='playerchk{0}' onclick='ChkPlayer(this, {0}, {1});' />", s.ID, roundnum);
        }
        else if (s.LookupID != "" && s.GroupID != "" && roundnum > 0 && tourneyid > 0)
        {
            string scoringlink = string.Format("http://monstergolf.org/monsterscoring/?t={0}&r={1}&u={2}&overwrite=1", tourneyid, roundnum, s.LookupID);
            if (email) group += string.Format("<a href=\"javascript:BreakGroup('{0}',{1});\">Change</a> ", s.GroupID, roundnum);
            if (enterscores) group += string.Format("<a href='{0}' target='scoreenter'>Scores</a> ", scoringlink);
        }
        name += s.Name;
        sb.AppendFormat("<tr{0}>", (addpagebreak) ? " class='PageBreak'" : "");
        if (addstartinghole) sb.AppendFormat("<td class='Detail" + groupclass + " DetailStart'{0}>", (golfersingroup > 1) ? " rowspan='" + golfersingroup + "'" : "");
        if (addstartinghole && golfersingroup > 1)
        {
            sb.Append("<div>Hole</div>");
            if (email)
            {
                sb.AppendFormat("<input type='number' pattern='[0-9]*' class='starthole' name='startinghole{0}' maxlength='2' onchange=\"StartingHole(this, '{0}');\" value='{1}' />", s.GroupID, s.StartingHole);
                if (s.DateOfRound != DateTime.MinValue) sb.AppendFormat("<div><input class='starthole' name='starttime{0}' maxlength='8' onchange=\"StartingTime(this, '{0}', {1});\" value='{2}' /></div>", s.GroupID, roundnum, s.DateOfRound.ToShortTimeString());
            }
            else
            {
                sb.AppendFormat("<div class='starthole'>{0}</div>", s.StartingHole);
                if (s.DateOfRound != DateTime.MinValue) sb.AppendFormat("<div>{0}</div>", s.DateOfRound.ToShortTimeString());
            }
            if (group != "") sb.AppendFormat("<div>{0}</div>", group);
        }
        if (addstartinghole) sb.Append("</td>");

        //TODO: add flight, need to add to ScoreInfo
        //sb.AppendFormat("<td class='Detail1" + groupclass + "'>{0}</td>", "");
        sb.AppendFormat("<td class='Detail1" + groupclass + "'>{0}</td>", name);

        if (!email)
        {
            for (int x = 1; x <= 18; x++)
            {
                if (x == 10) sb.AppendFormat("<td class='Detail" + groupclass + "'>{0}</td>", ((s.Out == "") ? " " : s.Out));
                string score = s.Scores[x.ToString()];
                if (score == "") score += " ";
                sb.AppendFormat("<td class='Detail" + groupclass + "'>{0}</td>", score);
            }
        }
        if (!email) sb.AppendFormat("<td class='Detail" + groupclass + "'>{0}</td>", ((s.In == "") ? " " : s.In));
        if (!email) sb.AppendFormat("<td class='Detail" + groupclass + "'>{0}</td>", ((s.Total == "") ? " " : s.Total));
        sb.AppendFormat("<td class='Detail" + groupclass + "'>{0}</td>", ((s.HCP == "") ? " " : s.HCP));
        if (!email) sb.AppendFormat("<td class='Detail" + groupclass + "'>{0}</td>", ((s.Net == "") ? " " : s.Net));
        sb.Append("</tr>");    
        //sb.Append("<div style='clear:both'> </div>");
    }
    public StringBuilder TourneyScores(DB db, int tourneyid, int roundnum, bool email, bool enterscores, string order)
    {
        StringBuilder sb = new StringBuilder("<div style='position:relative;'><table cellpadding='0' cellspacing='0'>");
        sb.AppendFormat("<input type='button' id='setgroup{0}' onclick='SetGroup({0})' value='Set Group' style='position:absolute;display:none;top:0;left:30;z-index:10;' />", roundnum);
        List<ScoreInfo> si = ScoreInfo.LoadTourneyRound(tourneyid.ToString(), roundnum.ToString(), order);
        ScoreInfo headerCol = new ScoreInfo("label", "", "Player", ScoreInfo.empty18List(true), "out", "in", "total", "hcp", "net");
        if (si.Count == 0) sb.Append("No Scores Available");
        else TourneyRow(db, sb, headerCol, 0, 0, email, false, "_Head", true, 0, false);
        string currGroupId = "";
        string groupclass = "";
        int count = 1;
        foreach (ScoreInfo s in si)
        {
            bool addstartinghole = false;
            int golfersingroup = 0;
            if (order.ToLower().Contains("groupid"))
            {
                if (currGroupId == "")
                {
                    currGroupId = s.GroupID;
                    addstartinghole = true;
                    golfersingroup = s.PlayersInGroup;
                }
                if (currGroupId != "" && currGroupId != s.GroupID)
                {
                    addstartinghole = true;
                    currGroupId = s.GroupID;
                    if (groupclass == "") groupclass = "_2";
                    else groupclass = "";
                    golfersingroup = s.PlayersInGroup;
                }
            }
            if (count >= 48 && addstartinghole)
            {
                TourneyRow(db, sb, headerCol, 0, 0, email, false, "_Head", true, 0, true);
                count = 1;
            }
            count++;
            TourneyRow(db, sb, s, roundnum, tourneyid, email && !s.CardSigned, enterscores, groupclass, addstartinghole, golfersingroup, false);
        }
        sb.Append("</table></div>");
        return sb;
    }
    public static void WriteEndResponse(HttpResponse resp, StringBuilder output)
    {
        output.Replace("&", "&amp;");
        output.Replace("&amp;amp;", "&amp;");
        resp.Write(output);
        resp.End();
    }
    public static MailAddress CreateAddress(string emailnameitem, bool overwriteemail) {
        MailAddress m = null;
        string[] emname = emailnameitem.Split(':');
        string email = null, name = null;
        if (emname.Length > 0) email = emname[0];
        if (emname.Length > 1) name = emname[1];
        if (!string.IsNullOrEmpty(email))
        {
            if (overwriteemail) email = "aaronwald@hotmail.com";
            if (string.IsNullOrEmpty(name)) m = new MailAddress(email);
            else m = new MailAddress(email, name);
        }
        return m;
    }
    public static bool SendMessage(string email, string subject, string body, bool isHtml, List<Attachment> attachments, HttpServerUtility webserver)
    {
        bool success = true;
        try
        {
            if (attachments == null) attachments = new List<Attachment>(1);
            string mailsvr = ConfigurationManager.AppSettings.Get("mailserver");
            SmtpClient client = new SmtpClient(mailsvr);
            client.UseDefaultCredentials = false;
            string mailfrom = ConfigurationManager.AppSettings.Get("mailfrom");
            MailAddress from = CreateAddress(mailfrom, false);
            if (from == null) return false;
            client.Credentials = new System.Net.NetworkCredential(from.Address, ConfigurationManager.AppSettings.Get("mailpassword"));
            MailMessage msg = new MailMessage();
            msg.From = from;
            string mailreplyto = ConfigurationManager.AppSettings.Get("mailreplyto");
            MailAddress reply = CreateAddress(mailreplyto, false);
            if (reply != null) msg.ReplyTo = reply;
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            string[] emailnamelist = email.Split(';');
            for (int x = 0; x < emailnamelist.Length; x++)
            {
                MailAddress to = CreateAddress(emailnamelist[x], false);
                if (to != null) msg.To.Add(to);
            }
            if (attachments != null && attachments.Count > 0)
            {
                foreach (Attachment attach in attachments)
                {
                    msg.Attachments.Add(attach);
                }
            }
            client.Send(msg);
        }
        catch (Exception ex)
        {
            success = false;
            if (webserver != null)
            {
                string msg = ex.Message;
                msg += string.Format("\n\nTo: {0}\nSubject: {1}\nAttachments: {2}\nBody: {3}", email, subject, ((attachments == null) ? 0 : attachments.Count), body);
                string writefile = string.Format("{0}SendError_{1}.txt", webserver.MapPath(".\\logs\\"), DateTime.Now.ToString("yyMMddhhmmss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                if (ex.InnerException != null)
                {
                    System.IO.File.WriteAllText(writefile, ex.Message + "\n\n" + ex.InnerException.Message);
                }
                else
                {
                    System.IO.File.WriteAllText(writefile, ex.Message);
                }
            }
        }
        return success;
    }

}

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

    public void TourneyRow(DB db, StringBuilder sb, ScoreInfo s)
    {
        TourneyRow(db, sb, s, 0, 0, false, false, "", false);
    }
    public void TourneyRow(DB db, StringBuilder sb, ScoreInfo s, int roundnum, int tourneyid, bool email, bool enterscores, string groupclass, bool addstartinghole)
    {
        string name = "";
        string hole = "";
        if (s.LookupID != "" && s.GroupID == "")
        {
            name = string.Format("<input type='checkbox' class='cb' name='playerchk' id='playerchk{0}' onclick='ChkPlayer(this, {0}, {1});' />", s.ID, roundnum);
        }
        else if (s.LookupID != "" && s.GroupID != "" && roundnum > 0 && tourneyid > 0)
        {
            string scoringlink = string.Format("http://monstergolf.org/monsterscoring/?t={0}&r={1}&u={2}&overwrite=1", tourneyid, roundnum, s.LookupID);
            if (email) name += string.Format("<a href=\"javascript:BreakGroup('{0}',{1});\">Change</a> ", s.GroupID, roundnum);
            if (enterscores) name += string.Format("<a href='{0}' target='scoreenter'>Enter Scores</a> ", scoringlink);
            if (addstartinghole) hole = string.Format("<input type='number' class='tb' name='startinghole{0}' maxlength='2' onchange=\"StartingHole(this, '{0}');\" value='{1}' />", s.GroupID, s.StartingHole);
        }
        name += s.Name + hole;
        sb.AppendFormat("<div class='Detail1" + groupclass + "'>{0}</div>", name);
        for (int x = 1; x <= 18; x++)
        {
            if (x == 10) sb.AppendFormat("<div class='Detail" + groupclass + "'>{0}</div>", ((s.Out == "") ? " " : s.Out));
            string score = s.Scores[x.ToString()];
            if (score == "") score += " ";
            sb.AppendFormat("<div class='Detail" + groupclass + "'>{0}</div>", score);
        }
        sb.AppendFormat("<div class='Detail" + groupclass + "'>{0}</div>", ((s.In == "") ? " " : s.In));
        sb.AppendFormat("<div class='Detail" + groupclass + "'>{0}</div>", ((s.Total == "") ? " " : s.Total));
        sb.AppendFormat("<div class='Detail" + groupclass + "'>{0}</div>", ((s.HCP == "") ? " " : s.HCP));
        sb.AppendFormat("<div class='Detail" + groupclass + "'>{0}</div>", ((s.Net == "") ? " " : s.Net));
        sb.Append("<div style='clear:both'> </div>");
    }
    public StringBuilder TourneyScores(DB db, int tourneyid, int roundnum, bool email, bool enterscores, string order)
    {
        StringBuilder sb = new StringBuilder("<div style='position:relative;'>");
        sb.AppendFormat("<input type='button' id='setgroup{0}' onclick='SetGroup({0})' value='Set Group' style='position:absolute;display:none;top:0;left:30;z-index:10;' />", roundnum);
        List<ScoreInfo> si = ScoreInfo.LoadTourneyRound(tourneyid.ToString(), roundnum.ToString(), order);
        ScoreInfo headerCol = new ScoreInfo("label", "", "Player", ScoreInfo.empty18List(true), "out", "in", "total", "hcp", "net");
        if (si.Count == 0) sb.Append("No Scores Available");
        else TourneyRow(db, sb, headerCol, 0, 0, false, false, "_Head", false);
        string currGroupId = "";
        string groupclass = "";
        int count = 1;
        foreach (ScoreInfo s in si)
        {
            bool addstartinghole = false;
            if (order.ToLower().Contains("groupid"))
            {
                if (currGroupId == "")
                {
                    currGroupId = s.GroupID;
                    addstartinghole = email;
                }
                if (currGroupId != "" && currGroupId != s.GroupID)
                {
                    addstartinghole = email;
                    currGroupId = s.GroupID;
                    if (groupclass == "") groupclass = "_2";
                    else groupclass = "";
                }
            }
            if (count == 25)
            {
                sb.Append("<div style='page-break-before:always'></div>");
                //TourneyRow(db, sb, s, 0, 0, false, false, "");
                TourneyRow(db, sb, headerCol, 0, 0, false, false, "_Head", false);
                count = 1;
            }
            count++;
            TourneyRow(db, sb, s, roundnum, tourneyid, email && !s.CardSigned, enterscores, groupclass, addstartinghole);
        }
        sb.Append("</div>");
        return sb;
    }
    public static void WriteEndResponse(HttpResponse resp, StringBuilder output)
    {
        output.Replace("&", "&amp;");
        output.Replace("&amp;amp;", "&amp;");
        resp.Write(output);
        resp.End();
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
            MailAddress from;
            string mailfrom = ConfigurationManager.AppSettings.Get("mailfrom");
            string[] emname = mailfrom.Split(':');
            if (emname.Length == 2) from = new MailAddress(emname[0], emname[1]);
            else from = new MailAddress(emname[0]);
            client.Credentials = new System.Net.NetworkCredential(from.Address, ConfigurationManager.AppSettings.Get("mailpassword"));
            string[] emailnamelist = email.Split(';');
            emname = emailnamelist[0].Split(':');
            MailAddress to;
            if (emname.Length == 2) to = new MailAddress(emname[0], emname[1]);
            else to = new MailAddress(emname[0]);
            MailMessage msg = new MailMessage(from, to);
            string mailreplyto = ConfigurationManager.AppSettings.Get("mailreplyto");
            string[] replyto = mailreplyto.Split(':');
            if (replyto.Length > 1) msg.ReplyTo = new MailAddress(replyto[0], replyto[1]);
            else msg.ReplyTo = new MailAddress(replyto[0]);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            for (int x = 1; x < emailnamelist.Length; x++)
            {
                if (emailnamelist[x].Trim() != "")
                {
                    emname = emailnamelist[x].Split(':');
                    if (emname.Length == 2) msg.To.Add(new MailAddress(emname[0], emname[1]));
                    else msg.To.Add(new MailAddress(emname[0]));
                }
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
                System.IO.File.WriteAllText(writefile, ex.Message);
            }
        }
        return success;
    }

}

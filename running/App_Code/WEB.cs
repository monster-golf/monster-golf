using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

namespace Running
{
    public class EMAIL
    {
        public const string c_3RiverWireless = "@sms.3rivers.net";
        public const string c_ACSWireless = "@paging.acswireless.com";
        public const string c_ATT = "@txt.att.net";
        public const string c_Alltel = "@message.alltel.com";
        public const string c_BPLmobile = "@bplmobile.com";
        public const string c_BeeLineGSM = "@sms.beemail.ru";
        public const string c_BellAtlantic = "@message.bam.com";
        public const string c_BellCanada = "@txt.bellmobility.ca";
        public const string c_BellMobility = "@txt.bellmobility.ca";
        public const string c_BellSouthBlackberry = "@bellsouthtips.com";
        public const string c_BellSouthMobility = "@blsdcs.net";
        public const string c_BellSouth = "@wireless.bellsouth.com";
        public const string c_BoostMobile = "@myboostmobile.com";
        public const string c_CellularOne = "@mobile.celloneusa.com";
        public const string c_CenturyTel = "@messaging.centurytel.net";
        public const string c_CingularWireless = "@mobile.mycingular.com";
        public const string c_Clearnet = "@msg.clearnet.com";
        public const string c_Comcast = "@comcastpcs.textmsg.com";
        public const string c_Cricket = "@sms.mycricket.com";
        public const string c_DobsonCellularSystems = "@mobile.dobson.net";
        public const string c_EdgeWireless = "@sms.edgewireless.com";
        public const string c_GTE = "@messagealert.com";
        public const string c_Metrocall = "pagernumber@page.metrocall.com";
        public const string c_Microcell = "@fido.ca";
        public const string c_MidwestWireless = "@clearlydigital.com";
        public const string c_Mobilecomm = "@mobilecomm.net";
        public const string c_MobileOne = "@m1.com.sg";
        public const string c_Mobilfone = "@page.mobilfone.com";
        public const string c_Nextel = "@messaging.nextel.com";
        public const string c_PCSOne = "@pcsone.net";
        public const string c_PacificBell = "@pacbellpcs.net";
        public const string c_Qwest = "@qwestmp.com";
        public const string c_SkytelPagers = "@email.skytel.com";
        public const string c_SprintPCS = "@messaging.sprintpcs.com";
        public const string c_TMobile = "@tmomail.net";
        public const string c_Telus = "@msg.telus.com";
        public const string c_USCellular = "@email.uscc.net";
        public const string c_USWest = "@uswestdatamail.com";
        public const string c_VirginMobile = "@vmobl.com";
        public const string c_Vodafone = "@vodafone.net";
        public const string c_VoiceStream = "@voicestream.net";
        public const string c_Verizon = "@vtext.com";
        #region other cell txt
        //public const string c_BluegrassCellular 	@sms.bluecell.com
        //public const string c_Cellular One East Coast 	@phone.cellone.net
        //public const string c_Cellular One PCS 	@paging.cellone-sf.com
        //public const string c_Cellular One South West 	@swmsg.com
        //Cellular One 	@cell1.textmsg.com
        //Cellular One 	@cellularone.textmsg.com
        //Cellular One 	@cellularone.txtmsg.com
        //Cellular One 	@message.cellone-sf.com
        //Cellular One 	@sbcemail.com
        //Cellular South 	@csouth1.com
        //Central Vermont Communications 	pagernumber@cvcpaging.com
        //Chennai RPG Cellular 	@rpgmail.net
        //Chennai Skycell / Airtel 	@airtelchennai.com
        //Cincinnati Bell 	@gocbw.com
        //Communication Specialist Companies 	pin@pager.comspeco.com
        //Communication Specialists 	7digitpin@pageme.comspeco.net
        //Comviq 	@sms.comviq.se
        //Cook Paging 	pagernumber@cookmail.com
        //Corr Wireless Communications 	@corrwireless.net
        //Delhi Aritel 	@airtelmail.com
        //Delhi Hutch 	@delhi.hutch.co.in
        //Digi-Page / Page Kansas 	pagernumber@page.hit.net
        //Dobson-Alex Wireless / Dobson-Cellular One 	@mobile.cellularone.com
        //EMT 	@sms.emt.ee
        //Escotel 	@escotelmobile.com
        //Fido 	@fido.ca
        //GCS Paging 	pagernumber@webpager.us
        //GTE 	@airmessage.net
        //GTE 	@gte.pagegate.net
        //Goa BPLMobil 	@bplmobile.com
        //Golden Telecom 	@sms.goldentele.com
        //GrayLink / Porta-Phone 	pagernumber@epage.porta-phone.com
        //Gujarat Celforce 	@celforce.com
        //Helio 	@myhelio.com
        //Houston Cellular 	@text.houstoncellular.net
        //Idea Cellular 	@ideacellular.net
        //Infopage Systems 	pinnumber@page.infopagesystems.com
        //Inland Cellular Telephone 	@inlandlink.com
        //JSM Tele-Page 	pinnumber@jsmtel.com
        //Kerala Escotel 	@escotelmobile.com
        //Kolkata Airtel 	@airtelkol.com
        //Kyivstar 	@smsmail.lmt.lv
        //LMT 	@smsmail.lmt.lv
        //Lauttamus Communication 	pagernumber@e-page.net
        //MCI Phone 	@mci.com
        //MCI 	@pagemci.com
        //Maharashtra BPL Mobile 	@bplmobile.com
        //Maharashtra Idea Cellular 	@ideacellular.net
        //Manitoba Telecom Systems 	@text.mtsmobility.com
        //Meteor 	@sms.mymeteor.ie
        //Metro PCS 	@mymetropcs.com
        //Metrocall 2-way 	pagernumber@my2way.com
        //MiWorld 	@m1.com.sg
        //Mobilecom PA 	pagernumber@page.mobilcom.net
        //Mobility Bermuda 	@ml.bm
        //Mobistar Belgium 	@mobistar.be
        //Mobitel Tanzania 	@sms.co.tz
        //Mobtel Srbija 	@mobtel.co.yu
        //Morris Wireless 	pagernumber@beepone.net
        //Motient 	@isp.com
        //Movistar 	@correo.movistar.net
        //Mumbai BPL Mobile 	@bplmobile.com
        //Mumbai Orange 	@orangemail.co.in
        //NBTel 	@wirefree.informe.ca
        //NPI Wireless 	@npiwireless.com
        //Netcom 	@sms.netcom.no
        //Nextel(Brazil) 	@nextel.com.br
        //Ntelos 	@pcs.ntelos.com
        //O2 	@o2.co.uk
        //Omnipoint 	@omnipointpcs.com
        //Omnipoint 	@omnipoint.com
        //One Connect Austria 	@onemail.at
        //OnlineBeep 	@onlinebeep.net
        //Optus Mobile 	@optusmobile.com.au
        //Orange – NL / Dutchtone 	@sms.orange.nl
        //Orange Mumbai 	@orangemail.co.in
        //Orange 	@orange.net
        //Oskar 	@mujoskar.cz
        //P&T Luxembourg 	@sms.luxgsm.lu
        //PageMart Advanced /2way 	pagernumber@airmessage.net
        //PageMart Canada 	pagernumber@pmcl.net
        //PageMart 	7digitpinnumber@pagemart.net
        //PageNet Canada 	@pagegate.pagenet.ca
        //PageOne NorthWest 	10digitnumber@page1nw.com
        //Pioneer / Enid Cellular 	@msg.pioneerenidcellular.com
        //PlusGSM 	@text.plusgsm.pl
        //Pocket Wireless 	@sms.pocket.com
        //Pondicherry BPL Mobile 	@bplmobile.com
        //Powertel 	@ptel.net
        //Price Communications 	@mobilecell1se.com
        //Primco 	@primeco.textmsg.com
        //Primtel 	@sms.primtel.ru
        //ProPage 	7digitpagernumber@page.propage.net
        //Public Service Cellular 	@sms.pscel.com
        //RAM Page 	@ram-page.com
        //Rogers AT&T Wireless 	@pcs.rogers.com
        //Rogers Canada 	@pcs.rogers.com
        //SBC Ameritech Paging 	pagernumber@paging.acswireless.com
        //SCS-900 	@scs-900.ru
        //SFR France 	@sfr.fr
        //ST Paging 	pin@page.stpaging.com
        //Safaricom 	@safaricomsms.com
        //Satelindo GSM 	@satelindogsm.com
        //Satellink 	pagernumber.pageme@satellink.net
        //Simple Freedom 	@text.simplefreedom.net
        //Skytel Pagers 	7digitpinnumber@skytel.com
        //Smart Telecom 	@mysmart.mymobile.ph
        //Southern LINC 	@page.southernlinc.com
        //Southwestern Bell 	@email.swbw.com
        //SunCom 	@tms.suncom.com
        //Sunrise Mobile 	@freesurf.ch
        //Sunrise Mobile 	@mysunrise.ch
        //Surewest Communicaitons 	@mobile.surewest.com
        //Swisscom 	@bluewin.ch
        //T-Mobile(UK) 	@t-mobile.uk.net
        //T-Mobile(Austria) 	Austria @sms.t-mobile.at
        //T-Mobile(Germany) 	Germany @t-d1-sms.de
        //TIM 	@timnet.com
        //TSR Wireless 	pagernumber@alphame.com
        //TSR Wireless 	pagernumber@beep.com
        //Tamil Nadu BPL Mobile 	@bplmobile.com
        //Tele2 Latvia 	@sms.tele2.lv
        //Telefonica Movistar 	@movistar.net
        //Telenor 	@mobilpost.no
        //Teletouch 	pagernumber@pageme.teletouch.com
        //Telia Denmark 	@gsm1800.telia.dk
        //The Indiana Paging Co 	last4digits@pager.tdspager.com
        //Triton 	@tms.suncom.com
        //Unicel 	@utext.com
        //Uraltel 	@sms.uraltel.ru
        //Uttar Pradesh Escotel 	@escotelmobile.com
        //Vessotel 	@pager.irkutsk.ru
        //Virgin Mobile Canada 	@vmobile.ca
        //Vodafone Italy 	@sms.vodafone.it
        //Vodafone Japan 	@c.vodafone.ne.jp
        //Vodafone Japan 	@h.vodafone.ne.jp
        //Vodafone Japan 	@t.vodafone.ne.jp
        //Vodafone Spain 	@vodafone.es
        //Vodafone UK 	@vodafone.net
        //WebLink Wiereless 	pagernumber@airmessage.net
        //WebLink Wiereless 	pagernumber@pagemart.net
        //West Central Wireless 	@sms.wcc.net
        //Western Wireless 	@cellularonewest.com
        //Wyndtell 	@wyndtell.com
        #endregion

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
                client.Credentials = new System.Net.NetworkCredential(mailfrom, ConfigurationManager.AppSettings.Get("mailpassword"));
                string[] emailnamelist = email.Split(';');
                MailAddress from = new MailAddress(mailfrom);
                string[] emname = emailnamelist[0].Split(':');
                MailAddress to;
                if (emname.Length == 2) to = new MailAddress(emname[0], emname[1]);
                else to = new MailAddress(emname[0]);
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(from, to);
                string mailreplyto = ConfigurationManager.AppSettings.Get("mailreplyto");
                string[] replyto = mailreplyto.Split(':');
                if (replyto.Length > 1) msg.ReplyTo = new MailAddress(replyto[0], replyto[1]);
                else msg.ReplyTo = new MailAddress(replyto[0]);
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;
                for (int x = 1; x < emailnamelist.Length; x++)
                {
                    emname = emailnamelist[x].Split(':');
                    if (emname.Length == 2) msg.To.Add(new MailAddress(emname[0], emname[1]));
                    else msg.To.Add(new MailAddress(emname[0]));
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
        public int GetInt(SqlDataReader sdr, string col)
        {
            int outint = 0;
            int colord = sdr.GetOrdinal(col);
            if (!sdr.IsDBNull(colord)) int.TryParse(sdr[colord].ToString(), out outint);
            return outint;
        }
        public string GetStr(SqlDataReader sdr, string col)
        {
            string outstr = "";
            int colord = sdr.GetOrdinal(col);
            if (!sdr.IsDBNull(colord)) outstr = sdr[colord].ToString();
            return outstr;
        }
        public float GetFloat(SqlDataReader sdr, string col)
        {
            float outflt = 0;
            int colord = sdr.GetOrdinal(col);
            if (!sdr.IsDBNull(colord)) float.TryParse(sdr[colord].ToString(), out outflt);
            return outflt;
        }
        public bool GetBool(SqlDataReader sdr, string col)
        {
            bool outbool = false;
            int colord = sdr.GetOrdinal(col);
            if (!sdr.IsDBNull(colord)) bool.TryParse(sdr[colord].ToString(), out outbool);
            return outbool;
        }
        public DateTime GetDate(SqlDataReader sdr, string col)
        {
            DateTime outdate = DateTime.MinValue;
            int colord = sdr.GetOrdinal(col);
            if (!sdr.IsDBNull(colord)) DateTime.TryParse(sdr[colord].ToString(), out outdate);
            return outdate;
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
            if (val == null) val = "";
            val = stripWord(val, "delete");
            val = stripWord(val, "insert");
            val = stripWord(val, "update");
            val = stripWord(val, "alter");
            val = val.Replace("'", "''");
            return "'" + val + "'";
        }
    }

    /// <summary>
    /// Summary description for WEB
    /// </summary>
    /// 
    public class WEB
    {
        public static string WriteControl(Control webControl, string wrapTag)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(sw);
            webControl.RenderControl(writer);
            StringBuilder output = new StringBuilder();
            if (wrapTag != null && wrapTag != "") output.Append("<" + wrapTag + ">");
            output.Append(sw.ToString());
            if (wrapTag != null && wrapTag != "") output.Append("</" + wrapTag + ">");
            output.Replace("&", "&amp;");
            output.Replace("&amp;amp;", "&amp;");
            return output.ToString();
        }

        public static string WorkoutTotals(DB db, Panel pnlHolder, string label, int runnerId, float workoutsforyear, float mileageforyear, int progressworkoutsforyear, float progressmileageforyear, int progressworkoutminutes, int progressworkoutseconds, int addat, bool runnerAnimateInvludeAvg)
        {
            string allstring = "";
            string allprogress = label + " Workouts: " + progressworkoutsforyear;
            DateTime day1 = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime today = DateTime.Now;
            float weeks = (float)Math.Ceiling(today.Subtract(day1).Days / 7.0);
            if (workoutsforyear > 0)
            {
                allprogress += " of " + workoutsforyear;
                if (runnerAnimateInvludeAvg)
                {
                    float perweek = (float)Math.Round(progressworkoutsforyear / weeks, 0);
                    allprogress += " - " + perweek.ToString("#") + " per week";
                }
            }
            allstring += allprogress;
            pnlHolder.Controls.AddAt(addat++, WEB.PNL(label.ToLower() + "workouts", allprogress, "Row", false));
            allprogress = label + " Mileage: " + progressmileageforyear;
            if (mileageforyear > 0)
            {
                allprogress += " of " + mileageforyear;
                if (runnerAnimateInvludeAvg)
                {
                    float perweek = (float)Math.Round(progressmileageforyear / weeks, 2);
                    allprogress += " - " + perweek.ToString("#.00") + " per week";
                    allprogress += "<div id='runnerAnim" + runnerId + "' class='RunnerAnim'><div id='runnerStatus" + runnerId + "' class='RunnerStatus'></div><img id='runnerIcon" + runnerId + "' src='running.png' class='RunnerIcon' /></div>";
                }
            }
            allstring += "\n" + allprogress;
            pnlHolder.Controls.AddAt(addat++, WEB.PNL(label.ToLower() + "mileage", allprogress, "Row", false));
            progressworkoutminutes += progressworkoutseconds / 60;
            progressworkoutseconds = progressworkoutseconds % 60;
            int progressworkouthours = progressworkoutminutes / 60;
            progressworkoutminutes = progressworkoutminutes % 60;
            if (progressworkouthours > 0 || progressworkoutminutes > 0)
            {
                allprogress = string.Format("{2} Workout Time: {0} hours {1} minutes", progressworkouthours, progressworkoutminutes, label);
                if (progressworkoutseconds > 0) allprogress += string.Format(" {0} seconds", progressworkoutseconds);
                allstring += "\n" + allprogress;
                pnlHolder.Controls.AddAt(addat, WEB.PNL(label.ToLower() + "time", allprogress, "Row", false));
            }
            return allstring;
        }

        public static TextBoxEmail TBEM(string id, string val, string css, int max, string onkeyup, string onchange, bool enabled, bool hide, int width)
        {
            TextBoxEmail tb = new TextBoxEmail();
            tb.ID = id;
            tb.Text = val;
            tb.CssClass = css;
            tb.MaxLength = max;
            if (onkeyup == "") onkeyup = "return KeyEntKill(event);";
            tb.Attributes.Add("onkeypress", "return KeyEntKill(event);");
            tb.Attributes.Add("onkeyup", onkeyup);
            tb.Attributes.Add("onchange", onchange);
            tb.Enabled = enabled;
            tb.TabIndex = 1;
            if (hide) tb.Style.Add(HtmlTextWriterStyle.Display, "none");
            if (width > 0) tb.Style.Add(HtmlTextWriterStyle.Width, Unit.Pixel(width).ToString());
            return tb;
        }
        public static TextBoxNumber TBNUM(string id, float val, string css, int max, string onkeyup, string onchange, bool enabled, bool hide, int width)
        {
            TextBoxNumber tb = new TextBoxNumber();
            tb.ID = id;
            if (val != 0) tb.Text = val.ToString();
            tb.CssClass = css;
            tb.MaxLength = max;
            if (onkeyup == "") onkeyup = "return KeyEntKill(event);";
            tb.Attributes.Add("onkeypress", "return KeyEntKill(event);");
            tb.Attributes.Add("onkeyup", onkeyup);
            tb.Attributes.Add("onchange", onchange);
            tb.Enabled = enabled;
            tb.TabIndex = 1;
            if (hide) tb.Style.Add(HtmlTextWriterStyle.Display, "none");
            if (width > 0) tb.Style.Add(HtmlTextWriterStyle.Width, Unit.Pixel(width).ToString());
            return tb;
        }
        public static TextBoxDate TBDT(string id, DateTime val, string css, string onkeyup, string onchange, bool enabled, bool hide, int width)
        {
            TextBoxDate tb = new TextBoxDate();
            tb.ID = id;
            if (val != DateTime.MinValue) tb.Text = val.ToShortDateString();
            tb.CssClass = css;
            tb.MaxLength = 10;
            if (onkeyup == "") onkeyup = "return KeyEntKill(event);";
            tb.Attributes.Add("onkeypress", "return KeyEntKill(event);");
            tb.Attributes.Add("onkeyup", onkeyup);
            tb.Attributes.Add("onchange", onchange);
            tb.Enabled = enabled;
            tb.TabIndex = 1;
            if (hide) tb.Style.Add(HtmlTextWriterStyle.Display, "none");
            if (width > 0) tb.Style.Add(HtmlTextWriterStyle.Width, Unit.Pixel(width).ToString());
            return tb;
        }
        public static TextBox TB(string id, string val, string css, int max, string onkeyup, string onchange, bool enabled, bool hide, int width)
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
            tb.TabIndex = 1;
            if (hide) tb.Style.Add(HtmlTextWriterStyle.Display, "none");
            if (width > 0) tb.Style.Add(HtmlTextWriterStyle.Width, Unit.Pixel(width).ToString());
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
            img.TabIndex = 1;
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
            cb.TabIndex = 1;
            if (hide) cb.Style.Add(HtmlTextWriterStyle.Display, "none");
            return cb;
        }
        public static Literal BR(string text)
        {
            Literal lit = new Literal();
            lit.Text = (text == "") ? "<br/>" : text;
            return lit;
        }
        public WEB()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //public void TourneyRow(DB db, StringBuilder sb, ScoreInfo s)
        //{
        //    TourneyRow(db, sb, s, 0, 0, false);
        //}
        //public void TourneyRow(DB db, StringBuilder sb, ScoreInfo s, int roundnum, int tourneyid, bool email)
        //{
        //    string name = "";
        //    if (s.LookupID != "" && s.GroupID == "")
        //    {
        //        name = string.Format("<input type='checkbox' name='playerchk' id='playerchk{0}' onclick='ChkPlayer(this, {0}, {1});' />", s.ID, roundnum);
        //    }
        //    else if (s.LookupID != "" && s.GroupID != "" && roundnum > 0 && tourneyid > 0)
        //    {
        //        string scoringlink = string.Format("http://monstergolf.org/monsterscoring/?t={0}&r={1}&u={2}", tourneyid, roundnum, s.LookupID);
        //        if (email)
        //        {
        //            string te = "Welcome to Monster XXI Day 2.\\n\\nTo keep score for your group, click on the link\\n\\n" + scoringlink + "\\n\\nWhen your card is fully filled out validate all scores, scroll down to below the score card and select a golfer from the other team to attest the scores. Then, you sign the card, and when you are done click Confirm Signing.  Hand your mobile device to the attester from the other team and they should validate the scores, then sign the card.  Once the scores have been attested you can no longer make changes.\\n\\nSee Aaron Wald or Brian Giesinger at the end of the round for any issues.";
        //            SqlDataReader sdr = db.Get("select MobileEmail from mg_users where userid = " + s.ID);
        //            if (sdr.Read()) name += string.Format("<a href='mailto:{0}?subject=Monster Scoring&body={1}'>Send Email</a> ", sdr[0], HttpContext.Current.Server.UrlEncode(te).Replace("+", " "));
        //            db.Close(sdr, false);
        //        }
        //        else
        //        {
        //            name += string.Format("<a href='{0}' target='scoreenter'>Enter Scores</a> ", scoringlink);
        //        }
        //    }
        //    name += s.Name;
        //    sb.AppendFormat("<div class='Detail1'>{0}</div>", name);
        //    for (int x = 1; x <= 18; x++)
        //    {
        //        if (x == 10) sb.AppendFormat("<div class='Detail'>{0}</div>", ((s.Out == "") ? " " : s.Out));
        //        string score = s.Scores[x.ToString()];
        //        if (score == "") score += " ";
        //        sb.AppendFormat("<div class='Detail'>{0}</div>", score);
        //    }
        //    sb.AppendFormat("<div class='Detail'>{0}</div>", ((s.In == "") ? " " : s.In));
        //    sb.AppendFormat("<div class='Detail'>{0}</div>", ((s.Total == "") ? " " : s.Total));
        //    sb.AppendFormat("<div class='Detail'>{0}</div>", ((s.HCP == "") ? " " : s.HCP));
        //    sb.AppendFormat("<div class='Detail'>{0}</div>", ((s.Net == "") ? " " : s.Net));
        //    sb.Append("<div style='clear:both'> </div>");
        //}
        //public string TourneyScores(DB db, int tourneyid, int roundnum, bool email)
        //{
        //    StringBuilder sb = new StringBuilder("<div style='position:relative;'>");
        //    sb.AppendFormat("<input type='button' id='setgroup{0}' onclick='SetGroup({0})' value='Set Group' style='position:absolute;display:none;top:0;left:30;z-index:10;' />", roundnum);
        //    List<ScoreInfo> si = ScoreInfo.LoadTourneyRound(tourneyid.ToString(), roundnum.ToString());
        //    if (si.Count == 0) sb.Append("No Scores Available");
        //    else TourneyRow(db, sb, new ScoreInfo("label", "", "Player", ScoreInfo.empty18List(true), "out", "in", "total", "hcp", "net"));
        //    foreach (ScoreInfo s in si) TourneyRow(db, sb, s, roundnum, tourneyid, email);
        //    sb.Append("</div>");
        //    return sb.ToString();
        //}
        public static void WriteEndResponse(HttpResponse resp, StringBuilder output) { WriteEndResponse(resp, output.ToString()); }
        public static void WriteEndResponse(HttpResponse resp, string output)
        {
            output.Replace("&", "&amp;");
            output.Replace("&amp;amp;", "&amp;");
            resp.Write(output);
            resp.End();
        }
    }

    public class TextBoxDate : System.Web.UI.WebControls.TextBox
    {
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute("type", "date");
            base.AddAttributesToRender(writer);
        }
    }

    public class TextBoxNumber : System.Web.UI.WebControls.TextBox
    {
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute("type", "number");
            base.AddAttributesToRender(writer);
        }
    }

    public class TextBoxEmail : System.Web.UI.WebControls.TextBox
    {
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute("type", "email");
            base.AddAttributesToRender(writer);
        }
    }
}
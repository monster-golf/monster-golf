using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using docusign;
using System.Data.SqlClient;

public partial class dscatch : System.Web.UI.Page
{
    protected string GetGroupId(DB db, string user)
    {
        string groupid = "";
        SqlDataReader sdr = db.Get(string.Format("select GroupId from mg_tourneyscores where UserLookup = '{0}'", DB.stringSql(user)));
        if (sdr.Read())
        {
            groupid = (sdr.IsDBNull(0)) ? "" : sdr[0].ToString();
        }
        db.Close(sdr, false);
        return groupid;
    }
    protected void UpdateTourney(DB db, string setstr, string tourneyid, string roundnum, string user)
    {
        string groupid = GetGroupId(db, user);
        if (groupid != "")
            db.Exec(string.Format("update mg_tourneyscores set {0} where TourneyId = {1} and RoundNum = {2} and GroupId = '{3}'", setstr, tourneyid, roundnum, DB.stringSql(groupid)));
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string envid = Request.QueryString["e"];
        string code = Request.QueryString["c"];
        string tourneyid = Request.QueryString["t"];
        string roundnum = Request.QueryString["r"];
        string user = Request.QueryString["u"];
        DS ds = new DS(envid);
        switch (code)
        {
            case "acfail":
            case "cancel":
            case "exception":
            case "faxpend":
            case "idcheckfail":
            case "sessiontimeout":
                litPass.Text = string.Format("Failure during signing. Please show or email the tournament coordinators this page.<br/><br/>e:{0} t:{1} r:{2} c:{3} ", envid, tourneyid, roundnum, code);
                break;
            case "ttlexpired":
                if (ds.EnvStatus.Status != docusign.EnvelopeStatusCode.Completed) Response.Redirect(ds.SignURL(2, tourneyid, roundnum, user));
                break;
            case "decline":
                break;
            case "viewcomplete":
            case "complete":
                DB db = new DB();
                if (ds.EnvStatus.Status != docusign.EnvelopeStatusCode.Completed)
                {
                    if (tourneyid != "") UpdateTourney(db, "CardSigned=1", tourneyid, roundnum, user);
                    else db.Exec(string.Format(string.Format("update mg_tourneyscores set CardSigned=1 where GroupId = '{0}'", DB.stringSql(roundnum))));
                    db.Close();
                    string attestname = "Attester";
                    foreach (RecipientStatus rs in ds.EnvStatus.RecipientStatuses)
                    {
                        if (rs.RoutingOrder == 2)
                        {
                            attestname = rs.UserName;
                        }
                    }
                    string url = ds.SignURL(2, tourneyid, roundnum, user);
                    litPass.Text = "Please pass your scoring device to " + attestname + "<br/><br/><a href='" + url + "'>click to attest</a>";
                }
                else
                {
                    if (tourneyid != "") UpdateTourney(db, "CardSigned=1, CardAttested=1", tourneyid, roundnum, user);
                    else db.Exec(string.Format(string.Format("update mg_tourneyscores set CardSigned=1, CardAttested=1 where GroupId = '{0}'", DB.stringSql(roundnum))));
                    db.Close();
                    Response.Redirect(string.Format("Default.aspx?signed=1&t={0}&r={1}&u={2}", tourneyid, roundnum, user));
                }
                break;
        }
    }
}

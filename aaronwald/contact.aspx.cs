using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

[AjaxPro.AjaxNamespace("AjaxContact")]
public class AjaxContact : System.Web.UI.Page
{
   private string SaveAddress(string line1, AW.AWBiz.Enums.AddressType addressType, AW.AWBiz.Employer objemployer, out string error)
   {
      AW.AWBiz.Address objaddress = null;
      error = "";
      foreach (AW.AWBiz.Address address in objemployer.GetAddresses())
      {
         if (address.AddressType == addressType)
         {
            objaddress = address;
            break;
         }
      }
      if (objaddress != null)
      {
         objaddress.Line1 = line1;
         if (!objaddress.Save(out error)) return "";
      }
      else if (line1 != "")
      {
         objaddress = new AW.AWBiz.Address();
         objaddress.AddressType = addressType;
         objaddress.Line1 = line1;
         if (!objemployer.AddAddress(objaddress, out error)) return "";
      }
      if (line1 != "") return string.Format("<br />{0}: {1}", addressType, objaddress.Line1);
      else return "";
   }
   [AjaxPro.AjaxMethod]
   public string SaveContact(string company, string firstName, string lastName, string phone, string email, string referredBy, string timeToCall, string description)
   {
      string contactInfo = "Here is the information I have collected from you:";
      string error = "";
      try
      {
         AW.AWBiz.Company objcompany = new AW.AWBiz.Company(company.Trim());
         if (objcompany.ID == 0)
         {
            if (!objcompany.Save(out error)) return "There has been a problem saving your information.<br />" + error + "<br />Please contact via email aaron@aaronwald.net or call 425 591 6584.";
         }
         contactInfo += "<br /><br />Company: " + objcompany.Text;
         string firstLetter;
         if (firstName.Trim() != "")
         {
            firstLetter = firstName[0].ToString();
            firstLetter = firstLetter.ToUpper();
            firstName = firstLetter + firstName.Remove(0, 1);
         }
         else firstName = firstName.Trim();
         if (lastName.Trim() != "")
         {
            firstLetter = lastName[0].ToString();
            firstLetter = firstLetter.ToUpper();
            lastName = firstLetter + lastName.Remove(0, 1);
         }
         else lastName = lastName.Trim();
         AW.AWBiz.Employer objemployer = new AW.AWBiz.Employer(objcompany, firstName, lastName);
         if (objemployer.FirstName != "") contactInfo += "<br />First Name: " + objemployer.FirstName;
         if (objemployer.LastName != "") contactInfo += "<br />Last Name: " + objemployer.LastName;
         objemployer.ReferredBy = referredBy.Trim();
         objemployer.ContactTimes = timeToCall.Trim();
         objemployer.ProjectDescription = description.Trim();
         if (!objemployer.Save(out error)) return "There has been a problem saving your information.<br />" + error + "<br />Please contact via email aaron@aaronwald.net or call 425 591 6584.";
         contactInfo += SaveAddress(phone.Trim(), AW.AWBiz.Enums.AddressType.Phone, objemployer, out error);
         if (error != "") return "There has been a problem saving your information.<br />" + error + "<br />Please contact via email aaron@aaronwald.net or call 425 591 6584.";
         contactInfo += SaveAddress(email.Trim(), AW.AWBiz.Enums.AddressType.Email, objemployer, out error);
         if (error != "") return "There has been a problem saving your information.<br />" + error + "<br />Please contact via email aaron@aaronwald.net or call 425 591 6584.";
         if (objemployer.ReferredBy != "") contactInfo += "<br />Referred By: " + objemployer.ReferredBy;
         if (objemployer.ContactTimes != "") contactInfo += "<br />Best Time to Contact You: " + objemployer.ContactTimes;
         if (objemployer.ProjectDescription != "") contactInfo += "<br />Brief Project Description: " + objemployer.ProjectDescription;
         if (objemployer.ContactTimes == "") contactInfo += "<br /><br />I will contact you within 24 hours.";
         contactInfo += "<br /><br />Thank you very much, Aaron Wald.";
      }
      catch (Exception err)
      {
         contactInfo = "There has been a problem saving your information.<br />" + err.Message + "<br />Please contact via email aaron@aaronwald.net or call 425 591 6584.";
      }
      contactInfo += "<br /><br /><a href=\"javascript:CloseSaveMessage()\" class=\"promise\">-Click to close this message and update your information-</a><br /><br />";
      contactInfo += "<a href=\"index.htm\" class=\"promise\">-Click to go Back-</a>";
      return contactInfo;
   }
}

public partial class Contact : System.Web.UI.Page
{
	protected void Page_Load(object sender, System.EventArgs e)
	{
      AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxContact));
   }

	#region Web Form Designer generated code
	override protected void OnInit(EventArgs e)
	{
		//
		// CODEGEN: This call is required by the ASP.NET Web Form Designer.
		//
		InitializeComponent();
		base.OnInit(e);
	}
	
	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{    
	}
	#endregion
}

<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="admin, App_Web_5ov0mykl" validaterequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script>
    function checkEnter(id, e) {
        if (e.keyCode == 13) {
            document.getElementById(id).focus();
        }
    }
    function showParticipants() {
        document.getElementById("hdnAction").value = "showparticipantschanged";
        document.forms[0].submit();
    }
    function enableRegistration() {
        document.getElementById("hdnAction").value = "enableregistration";
        document.forms[0].submit();
    }
    function deleteTraining(id) {
        if (confirm("Delete this training session?")) {
            document.getElementById("hdnDeleteTraining").value = id;
            document.forms[0].submit();
        }
    }
    function editTraining(id) {
        document.getElementById("hdnEditTraining").value = id;
        document.getElementById("hdnStartEdit").value = "1";
        document.forms[0].submit();
    }
    function openTraining(id) {
        if (confirm("Open this training session?")) {
            document.getElementById("hdnOpenTraining").value = id;
            document.forms[0].submit();
        }
    }
    function addParticipant(trainingid) {
        document.getElementById("hdnAddParticipantTo").value = trainingid;
        document.getElementById("divManageTrainings").style.display = "none";
        document.getElementById("divAddParticipant").style.display = "block";
        window.scrollTo(0, 0);
    }
    function cancelAddParticipant() {
        document.getElementById("hdnAddParticipantTo").value = "";
        document.getElementById("divAddParticipant").style.display = "none";
        document.getElementById("divManageTrainings").style.display = "block";
    }
    function removeParticipant(name, id) {
        if (confirm("Are you sure you want to remove " + name + " from this class?")) {
            document.getElementById("hdnAction").value = "delparticipant";
            document.getElementById("hdnDeleteParticipant").value = id;
            document.forms[0].submit();
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="pnlAuth" runat="server" Visible="true" style="padding: 50px 0px 20px 50px;background-color:#ffffff;color:#000000;font-size:16px;">
        Admin Key:
        <asp:TextBox ID="txtAdmin" TextMode="SingleLine" runat="server" Font-Size="16" onkeyup="checkEnter('ctl00_ContentPlaceHolder1_btnAdmin', event);" />
        <br />
        <br />
        <asp:Button ID="btnAdmin" runat="server" OnClick="btnAdmin_Click" Text="Login" UseSubmitBehavior="false" Font-Size="16" />
        <script>document.getElementById("ctl00_ContentPlaceHolder1_txtAdmin").focus();</script>
    </asp:Panel>
    <asp:Panel ID="pnlUpload" runat="server" Visible="false" style="padding: 50px 0px 0px 50px;background-color:#ffffff;color:#000000;">
        Upload training flyer
        <asp:FileUpload ID="fupFlyer" runat="server" />
        <br />
        <br />
        Upload schedule 7 to 12
        <asp:FileUpload ID="fupSched" runat="server" />
        <br />
        <br />
        Upload schedule elementary
        <asp:FileUpload ID="fupSchedElem" runat="server" />
        <br />
        <br />
        <asp:Label ID="txtError" runat="server" />
        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload file(s)" UseSubmitBehavior="false" />
        <br />
        <div style="margin-top: 20px; padding-top: 20px; border-top: solid 1px #dedede;">
            <asp:Button ID="btnManageTrainings" runat="server" OnClick="btnManageTrainings_Click" Text="Manage Trainings" UseSubmitBehavior="false" />
            &nbsp;&nbsp;&nbsp;&nbsp; <a href="Default.aspx">Back to Southend Basketball Academy</a>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlManageTrainings" runat="server" Visible="false" Style="padding: 50px 0px 20px 50px;background-color:#ffffff;color:#000000;">
        <div id="divAddParticipant" style="display:none;font-size:14px;font-weight:normal;">
            <div style="font-size:16px;font-weight:bold;margin-bottom:5px;">Add Participant</div>
            <table>
            <tr><td>Name:</td><td><asp:TextBox runat="server" ID="txtParticipantName" Width="300" /></td></tr>
            <tr><td>Email:</td><td><asp:TextBox runat="server" ID="txtParticipantEmail" Width="300" /></td></tr>
            <tr><td>Phone:</td><td><asp:TextBox runat="server" ID="txtParticipantPhone" Width="300" /></td></tr>
            <tr><td>Grade:</td><td><asp:TextBox runat="server" ID="txtParticipantGrade" Width="300" /></td></tr>
            </table>
            <asp:Button ID="btnSaveParticipant" runat="server" UseSubmitBehavior="false" OnClick="btnSaveParticipant_Click" Text="Save" />
            <input type="button" onclick="cancelAddParticipant();" value="Cancel" />
        </div>
        <div id="divManageTrainings">
            <div style="font-size:16px;font-weight:bold;margin-bottom:5px;"><asp:Label ID="lblAddTraining" runat="server" Text="Add Training Class" /></div>
            <table cellpadding="5" cellspacing="0">
            <tr><td style="font-weight:bold;white-space:nowrap;" colspan="10">Day of the week: 
            <asp:RadioButtonList RepeatDirection="Horizontal" runat="server" ID="rbDay">
            <asp:ListItem>Sunday's</asp:ListItem>
            <asp:ListItem>Monday's</asp:ListItem>
            <asp:ListItem>Tuesday's</asp:ListItem>
            <asp:ListItem>Wednesday's</asp:ListItem>
            <asp:ListItem>Thursday's</asp:ListItem>
            <asp:ListItem>Fridays's</asp:ListItem>
            <asp:ListItem>Saturday's</asp:ListItem>
            <asp:ListItem>Use Title Below</asp:ListItem>
            </asp:RadioButtonList> 
            Title: <asp:TextBox ID="txtDay" runat="server" Width="400"></asp:TextBox>
            </td>
            </tr>
            <tr>
            <td style="font-weight:bold;white-space:nowrap;">Time</td>
            <td style="font-weight:bold;white-space:nowrap;">Class</td>
            <td style="font-weight:bold;white-space:nowrap;">Course Description</td>
            <td style="font-weight:bold;white-space:nowrap;">Trainer</td>
            <td style="font-weight:bold;white-space:nowrap;">Grade Levels</td>
            </tr>
            <tr>
            <td style="font-weight:normal;vertical-align:top;"><asp:TextBox runat="server" ID="txtTime" Height="50" Width="150" TextMode="MultiLine" /></td>
            <td style="font-weight:normal;vertical-align:top;"><asp:TextBox runat="server" ID="txtClass" Height="50" Width="150" TextMode="MultiLine" /></td>
            <td style="font-weight:normal;vertical-align:top;"><asp:TextBox runat="server" ID="txtDescription" Height="50" Width="150" TextMode="MultiLine" /></td>
            <td style="font-weight:normal;vertical-align:top;"><asp:TextBox runat="server" ID="txtTrainer" Height="50" Width="150" TextMode="MultiLine" /></td>
            <td style="font-weight:normal;vertical-align:top;"><asp:TextBox runat="server" ID="txtGrades" Height="50" Width="150" TextMode="MultiLine" /></td>
            </tr>
            <tr>
            <td style="font-weight:bold;white-space:nowrap;">Location</td>
            <td style="font-weight:bold;white-space:nowrap;">Session Dates</td>
            <td style="font-weight:bold;white-space:nowrap;">Cost</td>
            <td style="font-weight:bold;white-space:nowrap;">Type</td>
            <td style="font-weight:bold;white-space:nowrap;">Max Participants</td>
            </tr>
            <tr>
            <td style="font-weight:normal;vertical-align:top;"><asp:TextBox runat="server" ID="txtLocation" Height="50" Width="150" TextMode="MultiLine" /></td>
            <td style="font-weight:normal;vertical-align:top;"><asp:TextBox runat="server" ID="txtDates" Height="50" Width="150" TextMode="MultiLine" /></td>
            <td style="font-weight:normal;vertical-align:top;"><asp:TextBox runat="server" ID="txtCost" Height="50" Width="150" TextMode="MultiLine" /></td>
            <td style="font-weight:normal;vertical-align:top;"><asp:DropDownList ID="ddType" runat="server"></asp:DropDownList></td>
            <td style="font-weight:normal;vertical-align:top;"><asp:TextBox runat="server" id="txtMaxParticipants" Height="50" Width="150" TextMode="MultiLine" /></td>
            <td style="font-weight:normal;vertical-align:top;"><asp:Checkbox runat="server" id="chkExcludeFromDiscount" Text="Exclude From Discount" /></td>
            </tr>
            </table>
            <div style="margin-bottom:10px;font-size:14px;">
                <asp:Button ID="btnSaveTraining" runat="server" Text="Save" OnClick="btnSaveTraining_Click" UseSubmitBehavior="false" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancelEdit" runat="server" Text="Cancel Edit" OnClick="btnCancelEdit_Click" UseSubmitBehavior="false" Visible="false" />
            </div>
        </div>
        <asp:Label ID="lblManageTrainings" runat="server" Text="Current Training List" Font-Bold="true" Font-Size="16px"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkShowSignups" runat="server" Text="Show Participants" onclick="showParticipants();" />
        &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkEnableRegistration" runat="server" Text="Enable Registration Button" onclick="enableRegistration();" />
        &nbsp;&nbsp;&nbsp;&nbsp;Discount Pct: <asp:TextBox Width="40" ID="txtDiscountPct" runat="server" />
        &nbsp;&nbsp;&nbsp;&nbsp;Min cost to get discount: <asp:TextBox Width="40" ID="txtDiscountMin" runat="server" />
        <asp:Button ID="btnSaveDiscount" runat="server" Text="Save Discount" OnClick="btnSaveDiscount_Click" UseSubmitBehavior="false" />
        <asp:Table ID="dgManageTrainings" runat="server" CellPadding="5" CellSpacing="0">
        </asp:Table>
        <div style="margin-top: 20px; padding-top: 20px; border-top: solid 1px #dedede;">
            <asp:Button ID="btnManageFiles" runat="server" OnClick="btnManageFiles_Click" Text="Manage Schedules" UseSubmitBehavior="false" />
            &nbsp;&nbsp;&nbsp;&nbsp; <a href="Default.aspx">Back to Southend Basketball Academy</a>
        </div>
        <input type="hidden" id="hdnDeleteTraining" value="" name="hdnDeleteTraining" />
        <input type="hidden" id="hdnOpenTraining" value="" name="hdnOpenTraining" />
        <input type="hidden" id="hdnEditTraining" value="<%=_editId %>" name="hdnEditTraining" />
        <input type="hidden" id="hdnStartEdit" value="" name="hdnStartEdit" />
        <input type="hidden" id="hdnAction" value="" name="hdnAction" />
        <input type="hidden" id="hdnAddParticipantTo" value="" name="hdnAddParticipantTo" />
        <input type="hidden" id="hdnDeleteParticipant" value="" name="hdnDeleteParticipant" />
    </asp:Panel>
</asp:Content>

<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Schedule, App_Web_5ov0mykl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    function Limit(which) {
        document.getElementById("hdnChoice").value = which;
        document.forms[0].submit();
    }
</script>
<style>
.Limiter 
{
	float:left; padding:20px;cursor:pointer;text-decoration:underline;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div style="color:#000000;background-color:#ffffff;min-height:600px;width:1320px;text-align:center;">
<div style="font-size:36px;color:#ff0000;text-align:center;font-weight:bold;padding-top:10px;">
    Summer training schedule should be posted in mid-May.<br />Check back at that time. Don't wait, classes will fill quickly.
    <!--Southend Skills Academy Spring Training Schedule--></div>

    <!--
<div style="font-size:16px;font-weight:bold;margin-top:0px;padding-left:150px;" id="divMenuLinks" runat="server">
<asp:Literal ID="litLimiters" runat="server"></asp:Literal>
</div>

<div style="clear:both;"></div>
<asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Size="16" />
<asp:Table ID="tblTrainings" runat="server" CellPadding="4" GridLines="Both" BorderColor="#dedede" Width="100%" style="margin-top:0px"></asp:Table>
<div style="width:1310px;text-align:right;padding-right:10px;font-weight:bold;font-size:16px;" id="divCheckout" runat="server">
Participant's Name: <asp:TextBox ID="txtPlayersName" runat="server" Width="250px" /><br />
Participant's Grade: <asp:TextBox ID="txtGrade" runat="server" Width="250px" /><br />
Your Name: <asp:TextBox ID="txtSignersName" runat="server" Width="250px" /><br />
Primary Email: <asp:TextBox ID="txtEmail" runat="server" Width="250px" /><br />
Primary Phone: <asp:TextBox ID="txtPhone" runat="server" Width="250px" /><br />
Relationship to Participant: <asp:RadioButton ID="radMother" GroupName="radRelationship" runat="server" Text="Mother" /> <asp:RadioButton ID="radFather" GroupName="radRelationship" runat="server" Text="Father" /> <br />
<asp:Button UseSubmitBehavior="false" ID="btnCheckout" runat="server" Text="Checkout" Font-Size="20" OnClick="btnCheckout_Click" />

</div>
    -->

<input type="hidden" id="hdnChoice" name="hdnChoice" value="<%=_limit %>" />
</div>
</asp:Content>
<asp:Content ID="ContentPayPal" ContentPlaceHolderID="ContentPlaceHolderOutsideForm" runat="server">
<asp:Literal ID="litPP" runat="server" Visible="false"></asp:Literal>
</asp:Content>


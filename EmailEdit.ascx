<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EmailEdit" CodeBehind="EmailEdit.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%-- Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" --%>

<%--<ajaxToolkit:CalendarExtender ID="defaultCalendarExtender" runat="server" TargetControlID="txtDate"  PopupButtonID="imgCalendarButton" />
--%>
<h2 class="Head">
    Event:
    <asp:Label ID="EventNameLabel" runat="server"></asp:Label></h2>
<div class="border" style="left: 16px; width: 800px; top: 8px; height: 48px" title="Define Segment">
    <br />
    <asp:Label ID="SegmentLabel" runat="server" CssClass="NormalBold" Text="Who would you like to send the e-mail to?"></asp:Label>
    <asp:DropDownList ID="RolesDropDown" runat="server" />
</div>
<div class="border" style="left: 16px; width: 800px; top: 64px; height: 112px" title="Define E-Mail">
    <asp:Label ID="lblEmailType" runat="server" CssClass="NormalBold" Text="Please select e-mail you would like to send:"></asp:Label>
    <asp:RadioButtonList ID="EmailTypeRadioButtons" runat="server" CssClass="Normal"
        AutoPostBack="true" RepeatDirection="Horizontal" Width="440px">
        <asp:ListItem Selected="True" Value="Invitation">Invitation</asp:ListItem>
        <asp:ListItem Value="Reminder">Reminder</asp:ListItem>
        <asp:ListItem Value="Recap">Recap</asp:ListItem>
    </asp:RadioButtonList>
    <br />
    <asp:Label ID="lblApprovalRequired" runat="server" CssClass="NormalBold" Text="Are approvals required?"></asp:Label><br />
    <asp:RadioButtonList ID="ApprovalRequiredRadioButtons" runat="server" CssClass="Normal"
        RepeatDirection="Horizontal">
        <asp:ListItem Selected="True">Yes</asp:ListItem>
        <asp:ListItem>No</asp:ListItem>
    </asp:RadioButtonList>
</div>
<div class="border" style="left: 16px; width: 800px; top: 208px; height: 220px" title="Create e-mail">
    <br />
    <asp:Label ID="SubjectLabel" runat="server" CssClass="NormalBold" Text="Subject:"></asp:Label>
    <asp:TextBox ID="SubjectTextBox" runat="server" Width="280px"></asp:TextBox>
    <asp:ImageButton ID="ImageButtonSubject" runat="server" ImageUrl="~/Images/up.gif" />
    <asp:Label ID="ApproversLabel" runat="server" CssClass="NormalBold" Text="Approvers:"></asp:Label>
    <asp:TextBox ID="ApproversTextBox" runat="server" Width="280px"></asp:TextBox>
    <asp:ImageButton ID="ImageButtonApprovers" runat="server" ImageUrl="~/Images/up.gif" />
    <br />
    <asp:Label ID="FromLabel" runat="server" CssClass="NormalBold" Text="From Name:"></asp:Label>
    <asp:TextBox ID="FromTextBox" runat="server" Width="280px"></asp:TextBox>
    <asp:ImageButton ID="ImageButtonFrom" runat="server" ImageUrl="~/Images/up.gif" />
    <asp:Label ID="FromEmailLabel" runat="server" CssClass="NormalBold" Text="From Email:"></asp:Label>
    <asp:TextBox ID="FromEmailTextBox" runat="server" Width="280px"></asp:TextBox>
    <asp:ImageButton ID="ImageButtonFromEmail" runat="server" ImageUrl="~/Images/up.gif" />
    <br />
    <asp:Label ID="EmailLocationLabel1" runat="server" CssClass="NormalBold" Text="Locate e-mail body:"></asp:Label>
    <asp:TextBox ID="EmailLocationTextBox1" runat="server" Width="280px"></asp:TextBox>
    <asp:ImageButton ID="btnBrowse" runat="server" ImageUrl="~/Images/view.gif" />
    <br />
    <div class="Normal" id="dvReminderOnly" visible="false" runat="server">
        <asp:Label ID="EmailLocationLabel2" runat="server" CssClass="NormalBold" Text="Locate e-mail body:"></asp:Label>
        <asp:TextBox ID="EmailLocationTextBox2" runat="server" Width="280px"></asp:TextBox>
        <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Images/view.gif" />
        <br />
        <asp:Label ID="EmailLocationLabel3" runat="server" CssClass="NormalBold" Text="Locate e-mail body:"></asp:Label>
        <asp:TextBox ID="EmailLocationTextBox3" runat="server" Width="280px"></asp:TextBox>
        <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/Images/view.gif" />
        <br />
    </div>
    <asp:CheckBox ID="ShowRichEditorCheckBox" runat="server" Text="Show Rich Text Editor"
        CssClass="NormalBold" />
    <br />
    <div class="EmailSendDate">
        <asp:Label ID="EmailSendLabel" runat="server" ResourceKey="EmailSendDateLabel" 
            CssClass="NormalBold" />
        <telerik:RadDateTimePicker runat="server" ID="EmailSendTimePicker" Skin="WebBlue">
            <TimeView Skin="WebBlue" />
            <Calendar Skin="WebBlue" />
            <DateInput InvalidStyleDuration="100" />
        </telerik:RadDateTimePicker>
        <asp:RequiredFieldValidator ID="EmailSendTimePickerRequired" runat="server" ControlToValidate="EmailSendTimePicker"
            ResourceKey="EmailSendTimePicker" Display="None" EnableClientScript="false" />
    </div>
</div>
<div class="EventButtons">
    <asp:ImageButton ID="SaveAndCreateNewEmailButton" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/save_create_new.gif"/>
    &nbsp;<asp:ImageButton ID="SaveEmailButton" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/save.gif" />
    &nbsp;<asp:HyperLink ID="CancelEmailLink" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/cancel_go_home.gif" />
</div>

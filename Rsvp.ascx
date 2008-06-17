<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Rsvp" Codebehind="Rsvp.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Controls" Assembly="Engage.Utilityv3.0" %>
<asp:MultiView ID="RsvpMultiView" ActiveViewIndex="0" runat="server">
    <asp:View runat="server">
        <div class="Normal" id="Description" align="left">
            <p><asp:Label ResourceKey="titleLabel" CssClass="SubHead" runat="server"/></p>
            <asp:Label id="EventNameLabel" CssClass="SubSubHead" runat="server"/><br />
            <asp:Label ResourceKey="emailAddressLabel" runat="server"/><asp:TextBox ID="EmailAddressTextbox" runat="server"/>
        </div>
        <div class="Normal" id="rbOptions" align="left">
            <asp:RadioButtonList ID="RsvpStatusRadioButtons" runat="server" CssClass="Normal"/>
            <asp:Button ID="SubmitButton" runat="server" Text="Submit" CssClass="CommandButton" />
            <asp:Button ID="AddToCalendarButton" runat="server" ResourceKey="addToCalendarButton" Enabled="false" CssClass="CommandButton" ValidationGroup="addToCalendar"/>
            <asp:RequiredFieldValidator runat="server" CssClass="NormalRed" ControlToValidate="EmailAddressTextbox" ValidationGroup="addToCalendar" ResourceKey="emailAddressRequired" Display="None" EnableClientScript="false" />
            <asp:CustomValidator ID="EmailAddressValidator" runat="server" CssClass="NormalRed" ControlToValidate="EmailAddressTextbox" ValidationGroup="addToCalendar" ResourceKey="invalidEmailAddress" Display="None" EnableClientScript="false" />
            <engage:ValidationSummary runat="server" ValidationGroup="addToCalendar" EnableClientScript="false" />
        </div>
    </asp:View>
    <asp:View runat="server">
        <engage:ModuleMessage ID="ThankYouMessage" runat="server" TextResourceKey="ThankYou" MessageType="Success" />
    </asp:View>
</asp:MultiView>
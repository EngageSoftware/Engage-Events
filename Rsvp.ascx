<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Rsvp" Codebehind="Rsvp.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Controls" Assembly="Engage.Utilityv3.0" %>

<asp:MultiView ID="RsvpMultiView" ActiveViewIndex="0" runat="server">
    <asp:View ID="View1" runat="server">
        <div id="Description">
            <h2 class="Head"><asp:Label ID="Label1" ResourceKey="titleLabel" runat="server"/></p>
            <h3 class="SubHead"><asp:Label id="EventNameLabel" runat="server"/></p>
        </div>
        <div class="Normal" id="rbOptions">
            <asp:RadioButtonList ID="RsvpStatusRadioButtons" runat="server" />
            <asp:Button ID="SubmitButton" runat="server" CssClass="registerSubmitBt" Text="Submit" />
        </div>
    </asp:View>
    <asp:View ID="View2" runat="server">
        <engage:ModuleMessage ID="ThankYouMessage" runat="server" MessageType="Success" TextResourceKey="ThankYou" />
        <div class="registerMessage Normal">
            <asp:Button ID="AddToCalendarButton" runat="server" ResourceKey="addToCalendarButton" Enabled="false" />
            <asp:Button ID="BackToEventsButton" runat="server" OnClick="BackToEventsButton_Click" ResourceKey="backToEventsButton" />
        </div>
    </asp:View>
</asp:MultiView>
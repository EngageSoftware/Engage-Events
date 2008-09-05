<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Respond" Codebehind="Respond.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Controls" Assembly="Engage.Dnn.Framework" %>

<asp:MultiView ID="ResponseMultiview" ActiveViewIndex="0" runat="server">
    <asp:View ID="View1" runat="server">
        <div id="Description">
            <h2 class="Head"><asp:Label ID="Label1" ResourceKey="titleLabel" runat="server"/></p>
            <h3 class="SubHead"><asp:Label id="EventNameLabel" runat="server"/></p>
        </div>
        <div id="rbOptions">
            <asp:RadioButtonList ID="ResponseStatusRadioButtons" CssClass="Normal" runat="server" />
            <asp:Button ID="SubmitButton" runat="server" CssClass="registerSubmitBt Normal" Text="Submit" />
        </div>
    </asp:View>
    <asp:View ID="View2" runat="server">
        <engage:ModuleMessage ID="ThankYouMessage" runat="server" MessageType="Success" TextResourceKey="ThankYou" />
        <div class="registerMessage Normal">
            <asp:Button ID="AddToCalendarButton" runat="server" ResourceKey="addToCalendarButton" Enabled="false" />
            <asp:Button ID="BackToEventsButton" runat="server" ResourceKey="backToEventsButton" />
        </div>
    </asp:View>
</asp:MultiView>
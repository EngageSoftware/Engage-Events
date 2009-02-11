<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Respond" Codebehind="Respond.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Controls" Assembly="Engage.Dnn.Framework" %>
<div class="respond-wrap">
    <asp:MultiView ID="ResponseMultiview" ActiveViewIndex="0" runat="server">
        <asp:View ID="ResponseView" runat="server">
            <div id="Description">
                <h3 class="SubHead"><asp:Label ResourceKey="titleLabel" runat="server"/></h3>
                <h2 class="Head"><asp:Label id="EventNameLabel" runat="server"/></h2>
            </div>
            <div id="rbOptions">
                <asp:RadioButtonList ID="ResponseStatusRadioButtons" CssClass="Normal" runat="server" />
                <asp:Button ID="SubmitButton" runat="server" CssClass="registerSubmitBt Normal" Text="Submit" />
            </div>
        </asp:View>
        <asp:View ID="ThankYouView" runat="server">
            <engage:ModuleMessage ID="ThankYouMessage" runat="server" MessageType="Success" TextResourceKey="ThankYou" />
            <div class="registerMessage Normal">
                <asp:Button ID="AddToCalendarButton" runat="server" ResourceKey="addToCalendarButton" Enabled="false" />
                <asp:Button ID="BackToEventsButton" runat="server" OnClientClick="parent.jQuery.fn.fancybox.close();return false;" ResourceKey="backToEventsButton" />
            </div>
        </asp:View>
        <asp:View ID="EventFullView" runat="server">
            <engage:ModuleMessage ID="EventFullMessage" runat="server" MessageType="Information" />
            <div class="registerMessage Normal">
                <asp:Button ID="BackToEventsButton2" runat="server" OnClientClick="parent.jQuery.fn.fancybox.close();return false;" ResourceKey="backToEventsButton" />
            </div>      
        </asp:View>
    </asp:MultiView>
</div>
    
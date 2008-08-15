<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Navigation.EventAdminActions" CodeBehind="EventAdminActions.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="ButtonAction" Src="../Actions/ButtonAction.ascx" %>
<div class="EventButtons Normal">
    <asp:Button ID="EditEventButton" runat="server" ResourceKey="EditEventButton"/>
    <asp:Button ID="ResponsesButton" runat="server" ResourceKey="ResponsesButton"/>
    <asp:Button ID="RegisterButton" runat="server" ResourceKey="RegisterButton"/>
    <asp:Button ID="AddToCalendarButton" runat="server" resourceKey="AddToCalendarButton"/>
    <asp:Button ID="DeleteEventButton" runat="server" ResourceKey="DeleteEventButton"/>
    <asp:Button ID="CancelButton" runat="server" />
    <asp:Button ID="ViewInviteButton" runat="server" ResourceKey="ViewInviteButton" Visible="false"/>
    <asp:Button ID="EditEmailButton" runat="server" ResourceKey="EditEmailButton" Visible="false"/>
</div>
<div class="EventButtons Normal">
    <engage:ButtonAction ID="EventEditButtonAction" runat="server" />
</div>
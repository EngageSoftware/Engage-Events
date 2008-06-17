<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventAdminActions" CodeBehind="EventAdminActions.ascx.cs" %>
<div class="EventButtons">
    <asp:Button ID="EditEventButton" runat="server" CssClass="CommandButton" ResourceKey="EditEventButton"/>
    <asp:Button ID="ResponsesButton" runat="server" CssClass="CommandButton" ResourceKey="ResponsesButton"/>
    <asp:Button ID="RegisterButton" runat="server" CssClass="CommandButton" ResourceKey="RegisterButton"/>
    <asp:Button ID="AddToCalendarButton" runat="server" CssClass="CommandButton" resourceKey="AddToCalendarButton"/>
    <asp:Button ID="DeleteEventButton" runat="server" CssClass="CommandButton" ResourceKey="DeleteEventButton"/>
    <asp:Button ID="CancelButton" runat="server" CssClass="CommandButton"/>
    <asp:Button ID="ViewInviteButton" runat="server" CssClass="CommandButton" ResourceKey="ViewInviteButton" Visible="false"/>
    <asp:Button ID="EditEmailButton" runat="server" CssClass="CommandButton" ResourceKey="EditEmailButton" Visible="false"/>
</div>

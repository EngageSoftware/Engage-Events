<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventAdminActions" CodeBehind="EventAdminActions.ascx.cs" %>
<div class="EventButtons">
    <asp:Button ID="EditEventButton" runat="server" CssClass="Normal" ResourceKey="EditEventButton"/>
    <asp:Button ID="ResponsesButton" runat="server" CssClass="Normal" ResourceKey="ResponsesButton"/>
    <asp:Button ID="RegisterButton" runat="server" CssClass="Normal" ResourceKey="RegisterButton"/>
    <asp:Button ID="AddToCalendarButton" runat="server" CssClass="Normal" resourceKey="AddToCalendarButton"/>
    <asp:Button ID="DeleteEventButton" runat="server" CssClass="Normal" ResourceKey="DeleteEventButton"/>
    <asp:Button ID="CancelButton" runat="server" CssClass="Normal"/>
    <asp:Button ID="ViewInviteButton" runat="server" CssClass="Normal" ResourceKey="ViewInviteButton" Visible="false"/>
    <asp:Button ID="EditEmailButton" runat="server" CssClass="Normal" ResourceKey="EditEmailButton"/>
</div>
<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Navigation.EventAdminActions" CodeBehind="EventAdminActions.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="ButtonAction" Src="../Actions/ButtonAction.ascx" %>
<%@ Register TagPrefix="engage" TagName="RegisterAction" Src="../Actions/RegisterAction.ascx" %>
<div class="EventButtons Normal">
    <asp:Button ID="EditEventButton" runat="server" CssClass="Normal" ResourceKey="EditEventButton"/>
    <asp:Button ID="ResponsesButton" runat="server" CssClass="Normal" ResourceKey="ResponsesButton"/>
    <engage:RegisterAction ID="RegisterButton" runat="server" ResourceKey="RegisterButton"/>
    <asp:Button ID="AddToCalendarButton" runat="server" CssClass="Normal" resourceKey="AddToCalendarButton"/>
    <asp:Button ID="DeleteEventButton" runat="server" CssClass="Normal" ResourceKey="DeleteEventButton"/>
    <asp:Button ID="CancelButton" runat="server" CssClass="Normal" />
    <asp:Button ID="ViewInviteButton" runat="server" CssClass="Normal" ResourceKey="ViewInviteButton" Visible="false"/>
    <asp:Button ID="EditEmailButton" runat="server" CssClass="Normal" ResourceKey="EditEmailButton" Visible="false"/>
</div>
<%--<div class="EventButtons Normal">
    <engage:ButtonAction ID="EventEditButtonAction" runat="server" />
</div>--%>
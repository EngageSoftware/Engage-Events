<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventAdminActions" CodeBehind="EventAdminActions.ascx.cs" %>
<div class="EventButtons">
    <asp:Button ID="lbEditEvent" runat="server" CssClass="CommandButton" ResourceKey="lbEditEvent" OnClick="lbEditEvent_OnClick"></asp:Button>
    <asp:Button ID="lbResponses" runat="server" CssClass="CommandButton" ResourceKey="lbResponses" OnClick="lbResponses_OnClick"></asp:Button>
    <asp:Button ID="lbRegister" runat="server" CssClass="CommandButton" ResourceKey="lbRegister" OnClick="lbRegister_OnClick"></asp:Button>
    <asp:Button ID="lbAddToCalendar" runat="server" CssClass="CommandButton" resourceKey="lbAddToCalendar" OnClick="lbAddToCalendar_OnClick" Text="Add To Calendar"></asp:Button>
    <asp:Button ID="lbDelete" runat="server" CssClass="CommandButton" ResourceKey="lbDelete" OnClick="lbDeleteEvent_OnClick"></asp:Button>
    <asp:Button ID="lbCancel" runat="server" CssClass="CommandButton" OnClick="lbCancel_OnClick" Text="Cancel"></asp:Button>
    <asp:Button ID="lbViewInvite" runat="server" CssClass="CommandButton" ResourceKey="lbViewInvite" OnClick="lbViewInvite_OnClick" Visible="false" T></asp:Button>
    <asp:Button ID="lbEditEmail" runat="server" CssClass="CommandButton" ResourceKey="lbEditEmail" Visible="false" OnClick="lbEditEmail_OnClick"></asp:Button>
</div>

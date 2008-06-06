<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventAdminActions" CodeBehind="EventAdminActions.ascx.cs" %>
<div class="EventButtons">
    <asp:LinkButton ID="lbEditEvent" runat="server" CssClass="CommandButton" ResourceKey="lbEditEvent" OnClick="lbEditEvent_OnClick">Edit</asp:LinkButton>
    <asp:LinkButton ID="lbResponses" runat="server" CssClass="CommandButton" ResourceKey="lbResponses" OnClick="lbResponses_OnClick">View RSVP</asp:LinkButton>
    <asp:LinkButton ID="lbRegister" runat="server" CssClass="CommandButton" ResourceKey="lbRegister" OnClick="lbRegister_OnClick">Register</asp:LinkButton>
    <asp:LinkButton ID="lbAddToCalendar" runat="server" CssClass="CommandButton" resourceKey="lbAddToCalendar" OnClick="lbAddToCalendar_OnClick" Text="Add To Calendar"></asp:LinkButton>
    <asp:LinkButton ID="lbDelete" runat="server" CssClass="CommandButton" ResourceKey="lbDelete" OnClick="lbDeleteEvent_OnClick">Delete</asp:LinkButton>
    <asp:LinkButton ID="lbCancel" runat="server" CssClass="CommandButton" OnClick="lbCancel_OnClick" Text="Cancel"></asp:LinkButton>
    <asp:HyperLink ID="lbViewInvite" runat="server" CssClass="CommandButton" ResourceKey="lbViewInvite" Visible="false" Target="_new">View Invite</asp:HyperLink>
    <asp:LinkButton ID="lbEditEmail" runat="server" CssClass="CommandButton" ResourceKey="lbEditEmail" Visible="false" OnClick="lbEditEmail_OnClick">Create/Edit Email</asp:LinkButton>
</div>

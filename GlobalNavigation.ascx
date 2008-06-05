<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.GlobalNavigation" Codebehind="GlobalNavigation.ascx.cs" %>

<div id="divAdminButtons" class="AdminButtons">
    <asp:ImageButton ID="lbSettings" runat="server" CssClass="CommandButton" CausesValidation="false" OnClick="lbSettings_OnClick" ImageUrl="~/desktopmodules/EngageEvents/Images/Settings.gif"></asp:ImageButton>
    <asp:ImageButton ID="lbManageEvents" runat="server" CssClass="CommandButton" CausesValidation="false" OnClick="lbManageEvents_OnClick" ImageUrl="~/desktopmodules/EngageEvents/Images/manage_events.gif"></asp:ImageButton>
    <asp:ImageButton ID="lbAddAnEvent" runat="server" CssClass="CommandButton" CausesValidation="false" OnClick="lbAddAnEvent_OnClick" ImageUrl="~/desktopmodules/EngageEvents/Images/add_event.gif"></asp:ImageButton>
    <asp:ImageButton ID="lbResponses" runat="server" CssClass="CommandButton" CausesValidation="false" OnClick="lbResponses_OnClick" ImageUrl="~/desktopmodules/EngageEvents/Images/responses.gif"></asp:ImageButton>
</div>

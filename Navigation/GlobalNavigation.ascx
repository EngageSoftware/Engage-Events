<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.GlobalNavigation" Codebehind="GlobalNavigation.ascx.cs" %>

<div id="divAdminButtons" class="AdminButtons">
    <br />
    <asp:HyperLink ID="SettingsLink" runat="server" CssClass="CommandButton" CausesValidation="false" OnClick="lbSettings_OnClick" ImageUrl="~/desktopmodules/EngageEvents/Images/Settings.gif"/>
    <asp:HyperLink ID="ManageEventsLink" runat="server" CssClass="CommandButton" CausesValidation="false" OnClick="lbManageEvents_OnClick" ImageUrl="~/desktopmodules/EngageEvents/Images/manage_events.gif"/>
    <asp:HyperLink ID="AddAnEventLink" runat="server" CssClass="CommandButton" CausesValidation="false" OnClick="lbAddAnEvent_OnClick" ImageUrl="~/desktopmodules/EngageEvents/Images/add_event.gif"/>
    <asp:HyperLink ID="ResponsesLink" runat="server" CssClass="CommandButton" CausesValidation="false" OnClick="lbResponses_OnClick" ImageUrl="~/desktopmodules/EngageEvents/Images/responses.gif"/>
</div>

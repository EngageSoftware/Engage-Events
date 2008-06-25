<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.GlobalNavigation" Codebehind="GlobalNavigation.ascx.cs" %>

<div id="divAdminButtons" class="AdminButtons">
    <asp:HyperLink ID="HomeLink" runat="server" CssClass="CommandButton" CausesValidation="false" ImageUrl="~/DesktopModules/EngageEvents/Images/home.gif"/>
    <asp:HyperLink ID="SettingsLink" runat="server" CssClass="CommandButton" CausesValidation="false" ImageUrl="~/DesktopModules/EngageEvents/Images/settings.gif"/>
    <asp:HyperLink ID="ManageEventsLink" runat="server" CssClass="CommandButton" CausesValidation="false" ImageUrl="~/DesktopModules/EngageEvents/Images/manage_events.gif"/>
    <asp:HyperLink ID="AddAnEventLink" runat="server" CssClass="CommandButton" CausesValidation="false" ImageUrl="~/DesktopModules/EngageEvents/Images/add_event.gif"/>
    <asp:HyperLink ID="ResponsesLink" runat="server" CssClass="CommandButton" CausesValidation="false" ImageUrl="~/DesktopModules/EngageEvents/Images/responses.gif"/>
</div>
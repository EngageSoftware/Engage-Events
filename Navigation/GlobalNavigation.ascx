<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Navigation.GlobalNavigation" Codebehind="GlobalNavigation.ascx.cs" %>

<div id="divAdminButtons" class="AdminButtons">
    <asp:HyperLink ID="HomeLink" runat="server" CausesValidation="false" ImageUrl="~/DesktopModules/EngageEvents/Images/home.gif" ResourceKey="Home"/>
    <asp:HyperLink ID="SettingsLink" runat="server" CausesValidation="false" ImageUrl="~/DesktopModules/EngageEvents/Images/settings.gif" ResourceKey="Settings"/>
    <asp:HyperLink ID="ChooseDisplayLink" runat="server" CausesValidation="false" ImageUrl="~/DesktopModules/EngageEvents/Images/choose_display.gif" ResourceKey="Choose Display"/>
    <asp:HyperLink ID="ManageEventsLink" runat="server" CausesValidation="false" ImageUrl="~/DesktopModules/EngageEvents/Images/manage_events.gif" ResourceKey="Manage Events"/>
    <asp:HyperLink ID="AddAnEventLink" runat="server" CausesValidation="false" ImageUrl="~/DesktopModules/EngageEvents/Images/add_event.gif" ResourceKey="Add Event"/>
    <asp:HyperLink ID="ResponsesLink" runat="server" CausesValidation="false" ImageUrl="~/DesktopModules/EngageEvents/Images/responses.gif" ResourceKey="Responses"/>
</div>
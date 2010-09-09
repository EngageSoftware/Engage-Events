<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Navigation.GlobalNavigation" Codebehind="GlobalNavigation.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="wrapper">
<telerik:RadMenu ID="NavigationMenu" runat="server" EnableEmbeddedSkins="false" Skin="Engage" EnableSelection="true" style="z-index:1000">
  <Items>
     <telerik:RadMenuItem ID="HomeItem" runat="server" CssClass="RAD-home" Value="H" />
     <telerik:RadMenuItem ID="AddEventItem" runat="server" CssClass="RAD-add" Value="A" />
     <telerik:RadMenuITem ID="ManageItem" runat="server" CssClass="RAD-manage" Value="M">
       <Items>
         <telerik:RadMenuItem ID="ManageEventsItem" runat="server"/>
         <telerik:RadMenuItem ID="ManageResponsesItem" runat="server"/>
         <telerik:RadMenuItem ID="ManageCategoriesItem" runat="server"/>
       </Items>
     </telerik:RadMenuITem>
     <telerik:RadMenuItem ID="SettingsItem" runat="server" CssClass="RAD-settings" Text="Settings" Value="S">
       <Items>
         <telerik:RadMenuItem ID="ModuleSettingsItem" runat="server"/>
         <telerik:RadMenuItem ID="ChooseDisplayItem" runat="server"/>
       </Items>
     </telerik:RadMenuItem>
  </Items>
</telerik:RadMenu>
</div>
<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Navigation.GlobalNavigation" Codebehind="GlobalNavigation.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="wrapper">
<telerik:RadMenu ID="NavigationMenu" runat="server" EnableEmbeddedSkins="false" Skin="Engage" EnableSelection="true" style="z-index:999">
  <Items>
     <telerik:RadMenuItem ID="HomeItem" runat="server" OuterCssClass="RAD-home" AccessKey="H" />
     <telerik:RadMenuItem ID="AddEventItem" runat="server" OuterCssClass="RAD-add" AccessKey="A" />
     <telerik:RadMenuITem ID="ManageItem" runat="server" OuterCssClass="RAD-manage" AccessKey="M">
       <Items>
         <telerik:RadMenuItem ID="ManageEventsItem" runat="server" AccessKey="E" />
         <telerik:RadMenuItem ID="ManageResponsesItem" runat="server" AccessKey="R" />
         <telerik:RadMenuItem ID="ManageCategoriesItem" runat="server" AccessKey="C" />
       </Items>
     </telerik:RadMenuITem>
     <telerik:RadMenuItem ID="SettingsItem" runat="server" OuterCssClass="RAD-settings" AccessKey="S">
       <Items>
         <telerik:RadMenuItem ID="ModuleSettingsItem" runat="server" AccessKey="M" />
         <telerik:RadMenuItem ID="ChooseDisplayItem" runat="server" AccessKey="C" />
       </Items>
     </telerik:RadMenuItem>
  </Items>
</telerik:RadMenu>
</div>
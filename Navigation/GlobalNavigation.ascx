<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Navigation.GlobalNavigation" Codebehind="GlobalNavigation.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="wrapper">
<telerik:RadMenu ID="NavigationMenu" runat="server" EnableEmbeddedSkins="false" Skin="Engage" EnableSelection="true" style="z-index:999">
  <Items>
     <telerik:RadMenuItem ID="HomeItem" runat="server" OuterCssClass="eng-nav-home" AccessKey="H" />
     <telerik:RadMenuItem ID="AddEventItem" runat="server" OuterCssClass="eng-nav-add" AccessKey="A" />
     <telerik:RadMenuITem ID="ManageItem" runat="server" OuterCssClass="eng-nav-manage" AccessKey="M">
       <Items>
         <telerik:RadMenuItem ID="ManageEventsItem" runat="server" OuterCssClass="eng-nav-manage-events" AccessKey="E" />
         <telerik:RadMenuItem ID="ManageResponsesItem" runat="server" OuterCssClass="eng-nav-manage-responses" AccessKey="R" />
         <telerik:RadMenuItem ID="ManageCategoriesItem" runat="server" OuterCssClass="eng-nav-manage-categories" AccessKey="C" />
       </Items>
     </telerik:RadMenuITem>
     <telerik:RadMenuItem ID="SettingsItem" runat="server" OuterCssClass="eng-nav-settings" AccessKey="S">
       <Items>
         <telerik:RadMenuItem ID="ModuleSettingsItem" runat="server" OuterCssClass="eng-nav-manage-settings" AccessKey="M" />
         <telerik:RadMenuItem ID="ChooseDisplayItem" runat="server" OuterCssClass="eng-nav-choose-display" AccessKey="C" />
       </Items>
     </telerik:RadMenuItem>
  </Items>
</telerik:RadMenu>
</div>
<div style="clear:both;">&nbsp;</div>
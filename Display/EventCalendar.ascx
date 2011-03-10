<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.EventCalendar" CodeBehind="EventCalendar.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="CategoryFilterAction" Src="..\Actions\CategoryFilterAction.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script type="text/javascript">
/*global Telerik, Sys */
(function (window, radToolTip, pageRequestManager) {
    "use strict";
    window.hideActiveToolTip = function () {            
        var tooltip = radToolTip.getCurrent();
        if (tooltip) {
            tooltip.hide(); 
        }
    };
    
    pageRequestManager.getInstance().add_beginRequest(function (sender, args) {
        if (args.get_postBackElement().id.indexOf('EventsCalendarDisplay') !== -1) { 
            window.hideActiveToolTip(); 
        } 
    });
}(this, Telerik.Web.UI.RadToolTip, Sys.WebForms.PageRequestManager));
</script>

<div class="EventHeader">
    <h2 class="NormalBold">
        <asp:Label runat="server" ResourceKey="EventsTitle" />
    </h2>
    <div class="EventFilter">
        <engage:CategoryFilterAction ID="CategoryFilterAction" runat="server" />
    </div>
</div>
<div class="EventCalendar">
    <telerik:RadScheduler ID="EventsCalendarDisplay" runat="server" SelectedView="MonthView"
        EnableEmbeddedSkins="True" AllowDelete="False" AllowEdit="False" AllowInsert="False"
        OverflowBehavior="Expand" ReadOnly="true" ShowFullTime="true" TimelineView-UserSelectable="False" />
    <telerik:RadToolTipManager runat="server" ID="EventsCalendarToolTipManager" Width="300" Height="150"
        Animation="None" Position="BottomRight" Sticky="true" Text="Loading..." AutoTooltipify="false" />
</div>

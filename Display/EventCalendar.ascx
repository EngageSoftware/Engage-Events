<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.EventCalendar" CodeBehind="EventCalendar.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="MultipleCategoriesFilterAction" Src="..\Actions\MultipleCategoriesFilterAction.ascx" %>
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
    <asp:Panel CssClass="EventFilter" runat="server" ID="EventFilterPanel">
        <engage:MultipleCategoriesFilterAction ID="CategoryFilterAction" runat="server" DialogPosition="left top" ButtonPosition="left bottom" />
    </asp:Panel>
</div>
<div class="EventCalendar">
    <telerik:RadScheduler ID="EventsCalendarDisplay" runat="server" 
                          SelectedView="MonthView"
                          EnableEmbeddedSkins="True" 
                          AllowDelete="False" 
                          AllowEdit="False" 
                          AllowInsert="False"
                          ReadOnly="true" 
                          OverflowBehavior="Expand" 
                          ShowFullTime="true" 
                          TimelineView-UserSelectable="False" />
    <telerik:RadToolTipManager ID="EventsCalendarToolTipManager" runat="server" 
                               Width="300" 
                               Height="150"
                               Animation="None" 
                               Position="BottomRight" 
                               Sticky="true" 
                               Text="Loading..." 
                               AutoTooltipify="false" />
</div>
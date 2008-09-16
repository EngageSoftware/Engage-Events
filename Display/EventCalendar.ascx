<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.EventCalendar" CodeBehind="EventCalendar.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<script type="text/javascript">
//<![CDATA[
    function hideActiveToolTip()
    {            
        var controller = Telerik.Web.UI.RadToolTipController.getInstance();
        var tooltip = controller.get_activeToolTip();
        if (tooltip)
        {
            tooltip.hide(); 
        }
    }
    
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequestHandler);
    function beginRequestHandler(sender, args)
    {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (args.get_postBackElement().id.indexOf('EventsCalendarDisplay') != -1) 
        { 
            hideActiveToolTip(); 
        } 
    } 
//]]>
</script>

<div class="EventHeader">
    <h2 class="NormalBold">
        <asp:Label runat="server" ResourceKey="EventsTitle" />
    </h2>
</div>
<div class="EventCalendar">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:radscheduler id="EventsCalendarDisplay" runat="server" selectedview="MonthView"
                enableembeddedskins="True" allowdelete="False" allowedit="False" allowinsert="False"
                overflowbehavior="Expand" ReadOnly="true" ShowFullTime="true">
                <timelineview userselectable="False" />
            </telerik:radscheduler>
            <telerik:radtooltipmanager runat="server" id="EventsCalendarToolTipManager" width="300" height="150"
                animation="None" position="BottomRight" sticky="true" text="Loading..." />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

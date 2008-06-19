<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventCalendar" CodeBehind="EventCalendar.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register Src="../Navigation/GlobalNavigation.ascx" TagName="GlobalNavigation" TagPrefix="engage" %>
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

<span class="GlobalNavigation">
    <engage:GlobalNavigation ID="GlobalNavigation" runat="server" />
</span>
<div class="EventHeader">
    <h2 class="NormalBold">
        <asp:Label runat="server" ResourceKey="EventsTitle" />
    </h2>
</div>
<div class="EventCalendar">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:radscheduler id="EventsCalendarDisplay" runat="server" selectedview="MonthView"
                width="750px" enableembeddedskins="True" daystarttime="08:00:00" dayendtime="18:00:00"
                timezoneoffset="03:00:00" datakeyfield="Id" datasubjectfield="Title" datastartfield="EventStart"
                dataendfield="EventEnd" allowdelete="False" allowedit="False" allowinsert="False"
                onappointmentdelete="EventsCalendarDisplay_AppointmentDelete" onappointmentinsert="EventsCalendarDisplay_AppointmentInsert"
                onappointmentupdate="EventsCalendarDisplay_AppointmentUpdate" onappointmentcreated="EventsCalendarDisplay_AppointmentCreated"
                overflowbehavior="Expand" onappointmentdatabound="EventsCalendarDisplay_AppointmentDataBound"
                customattributenames="Overview">
                <timelineview userselectable="False" />
            </telerik:radscheduler>
            <telerik:radtooltipmanager runat="server" id="EventsCalendarToolTipManager" width="300" height="150"
                skin="WebBlue" animation="None" position="BottomRight" sticky="true" text="Loading..."
                onajaxupdate="EventsCalendarToolTipManager_AjaxUpdate" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

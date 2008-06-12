<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventCalendar"
    CodeBehind="EventCalendar.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register Src="../Navigation/GlobalNavigation.ascx" TagName="GlobalNavigation"
    TagPrefix="uc1" %>
<%@ Register Src="../Navigation/EventAdminActions.ascx" TagName="actions" TagPrefix="uc2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<uc1:GlobalNavigation ID="GlobalNavigation1" runat="server" />
<br />
<br />
<div class="EventHeader">
    <h4 class="NormalBold">
        Events</h4>
</div>
<div class="EventCalendar">
    <telerik:RadScheduler ID="RadScheduler1" runat="server" SelectedView="MonthView"
        Width="750px" EnableEmbeddedSkins="True" DayStartTime="08:00:00"
        DayEndTime="18:00:00" TimeZoneOffset="03:00:00" DataKeyField="Id" DataSubjectField="Title"
        DataStartField="EventStart" DataEndField="EventEnd" AllowDelete="False" AllowEdit="False"
        AllowInsert="False" onappointmentdelete="RadScheduler1_AppointmentDelete" onappointmentinsert="RadScheduler1_AppointmentInsert"
        onappointmentupdate="RadScheduler1_AppointmentUpdate" 
        onappointmentcreated="RadScheduler1_AppointmentCreated" 
        OverflowBehavior="Expand">
        <timelineview userselectable="False" />
    </telerik:RadScheduler>
</div>

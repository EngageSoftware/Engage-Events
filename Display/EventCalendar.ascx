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
        if (args.get_postBackElement().id.indexOf('RadScheduler1') != -1) 
        { 
            hideActiveToolTip(); 
        } 
    } 
//]]>
</script>

<span class="GlobalNavigation"><engage:GlobalNavigation ID="GlobalNavigation" runat="server" /></span>

<div class="EventHeader">
    <h2 class="NormalBold"><asp:Label runat="server" ResourceKey="EventsTitle"></asp:Label></h4>
</div>

<div class="EventCalendar">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
        
            <telerik:RadScheduler ID="RadScheduler1"
                runat="server"
                SelectedView="MonthView"
                Width="750px"
                EnableEmbeddedSkins="True"
                DayStartTime="08:00:00"
                DayEndTime="18:00:00"
                TimeZoneOffset="03:00:00"
                DataKeyField="Id"
                DataSubjectField="Title"
                DataStartField="EventStart"
                DataEndField="EventEnd"
                AllowDelete="False"
                AllowEdit="False"
                AllowInsert="False"
                onappointmentdelete="RadScheduler1_AppointmentDelete"
                onappointmentinsert="RadScheduler1_AppointmentInsert"
                onappointmentupdate="RadScheduler1_AppointmentUpdate" 
                onappointmentcreated="RadScheduler1_AppointmentCreated" 
                OverflowBehavior="Expand" 
                onappointmentdatabound="RadScheduler1_AppointmentDataBound" 
                CustomAttributeNames="Overview">
                <timelineview userselectable="False" />
            </telerik:RadScheduler>
            
            <telerik:RadToolTipManager runat="server" ID="RadToolTipManager1" Width="300" Height="150"
            Skin="WebBlue" Animation="None" Position="BottomRight" Sticky="true" Text="Loading..."
            OnAjaxUpdate="RadToolTipManager1_AjaxUpdate"  />
            
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

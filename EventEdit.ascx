<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventEdit" Codebehind="EventEdit.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Controls" Assembly="Engage.Utilityv3.0" %>
<%@ Import Namespace="DotNetNuke.Services.Localization"%>

<%@ Register src="Recurrence/RecurrenceEditor.ascx" tagname="RecurrenceEditor" tagprefix="uc1" %>

<engage:ModuleMessage runat="server" ID="SuccessModuleMessage" MessageType="Success" TextResourceKey="AddEventSuccess" CssClass="AddEventSuccessMessage"/>

<div id="AddNewEvent" runat="server" class="AddNewEvent">

    <h2 class="SubHead">
        <asp:Label ID="AddEditEventLabel" runat="server"/>
    </h2>
    
    <div class="AEEventTitle">
        <asp:Label runat="server" ResourceKey="EventTitleLabel" CssClass="NormalBold"/>
        <asp:TextBox ID="EventTitleTextBox" runat="server" CssClass="NormalTextBox" MaxLength="250"/>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EventTitleTextBox" ResourceKey="EventTitleTextBoxRequired" Display="None" EnableClientScript="false"/>
    </div>
    
    <div class="AEEventStartDate">
        <asp:Label runat="server" ResourceKey="EventStartDateLabel" CssClass="NormalBold"/>
        <telerik:raddatetimepicker runat="server" id="StartDateTimePicker" skin="WebBlue">
            <timeview skin="WebBlue"/>
            <calendar skin="WebBlue"/>
            <DateInput InvalidStyleDuration="100"/>
        </telerik:raddatetimepicker>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="StartDateTimePicker" ResourceKey="StartDateTimePickerRequired" Display="None" EnableClientScript="false"/>
    </div>
    
    <div class="AEEventEndDate">
        <asp:Label runat="server" ResourceKey="EventEndDateLabel" CssClass="NormalBold"/>
        <telerik:raddatetimepicker runat="server" id="EndDateTimePicker" skin="WebBlue">
            <timeview skin="WebBlue"/>
            <calendar skin="WebBlue"/>
            <DateInput InvalidStyleDuration="100"/>
        </telerik:raddatetimepicker>
        
        <asp:CompareValidator 
            runat="server" Display="None" EnableClientScript="false"
            ControlToCompare="StartDateTimePicker"
            ControlToValidate="EndDateTimePicker" 
            ResourceKey="EndDateCompareValidator"
            Operator="GreaterThan"/>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EndDateTimePicker" ResourceKey="EndDateTimePickerRequired" Display="None" EnableClientScript="false"/>
    </div>
    
    <div class="AEEventLocationAdd">
        <asp:Label runat="server" ResourceKey="EventLocationLabel" CssClass="NormalBold"/>
        <asp:TextBox ID="EventLocationTextBox" runat="server" CssClass="NormalTextBox"/>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EventLocationTextBox" ResourceKey="EventLocationTextBoxRequired" Display="None" EnableClientScript="false"/>
    </div>

    <div class="AEEventEditor">
        <asp:Label runat="server" ResourceKey="EventOverviewLabel" CssClass="NormalBold"/>
        <dnn:TextEditor ID="EventOverviewTextEditor" runat="server" Width="400" TextRenderMode="Raw" HtmlEncode="False" DefaultMode="Rich" Height="350" ChooseMode="True" ChooseRender="False" />
    </div>
    
    <div class="AEEventEditor">
        <asp:Label runat="server" ResourceKey="EventDescriptionLabel" CssClass="NormalBold"/>
        <dnn:TextEditor ID="EventDescriptionTextEditor" runat="server" Width="400" TextRenderMode="Raw" HtmlEncode="False" DefaultMode="Rich" Height="350" ChooseMode="True" ChooseRender="False" />
        <asp:CustomValidator ID="EventDescriptionTextEditorValidator" runat="server" ResourceKey="EventDescriptionTextEditorRequired" Display="None"/>
    </div>
    
    <div class="AEEventEditor">
        <asp:Label ID="FeaturedEventLabel" runat="server" ResourceKey="FeaturedEventLabel" CssClass="NormalBold"/>
        <asp:CheckBox ID="FeaturedCheckbox" runat="server" />        
    </div>
    <div>
        <asp:CheckBox ID="RecurringCheckbox" runat="server" CssClass="NormalBold" Text="Recurring Event" AutoPostBack="true" />
        <uc1:RecurrenceEditor ID="RecurrenceEditor1" runat="server" Visible="false" />
    </div>
    
</div>

<engage:ValidationSummary runat="server" />

<div class="AddEventFooterButtons AdminButtons FooterButtons" runat="server" id="AddEventFooterButtons">
    <asp:ImageButton ID="SaveEventButton" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/save.gif" />
    <asp:HyperLink ID="CancelEventLink" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/cancel_go_home.gif" />
    <asp:ImageButton ID="SaveAndCreateNewEventButton" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/save_create_new.gif"/>
</div>

<div class="FinalButtons AdminButtons FooterButtons" runat="server" id="FinalButtons">
    <asp:ImageButton ID="CreateAnotherEventButton" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/create_another_event.gif" />
    <%--<asp:LinkButton ID="CreateEventEmailButton" runat="server">Create E-Mail For This Event</asp:LinkButton>--%>
    <asp:HyperLink ID="CancelGoHomeLink" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/home.gif" />
</div>

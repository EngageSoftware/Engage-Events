<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventEdit" Codebehind="EventEdit.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register tagprefix="engage" tagname="GlobalNavigation" src="Navigation/GlobalNavigation.ascx" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Controls" Assembly="Engage.Utilityv3.0" %>

<span class="GlobalNavigation">
    <engage:GlobalNavigation ID="GlobalNavigation" runat="server" />
</span>

<engage:ModuleMessage runat="server" ID="SuccessModuleMessage" MessageType="Success" TextResourceKey="AddEventSuccess" CssClass="AddEventSuccessMessage"/>

<div id="AddNewEvent" runat="server" class="AddNewEvent">

    <h2 class="Head">
        <asp:Label ID="AddEditEventLabel" runat="server"/>
    </h2>
    
    <div class="EventTitle">
        <asp:Label runat="server" ResourceKey="EventTitleLabel" CssClass="NormalBold"/>
        <asp:TextBox ID="EventTitleTextBox" runat="server" CssClass="NormalTextBox" MaxLength="250"/>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EventTitleTextBox" ResourceKey="EventTitleTextBoxRequired" Display="None" EnableClientScript="false"/>
    </div>
    
    <div class="EventStartDate">
        <asp:Label runat="server" ResourceKey="EventStartDateLabel" CssClass="NormalBold"/>
        <telerik:raddatetimepicker runat="server" id="StartDateTimePicker" skin="WebBlue">
            <timeview skin="WebBlue"/>
            <calendar skin="WebBlue"/>
            <DateInput InvalidStyleDuration="100"/>
        </telerik:raddatetimepicker>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="StartDateTimePicker" ResourceKey="StartDateTimePickerRequired" Display="None" EnableClientScript="false"/>
    </div>
    
    <div class="EventEndDate">
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
        
    </div>
    
    <div class="EventLocationAdd">
        <asp:Label runat="server" ResourceKey="EventLocationLabel" CssClass="NormalBold"/>
        <asp:TextBox ID="EventLocationTextBox" runat="server" CssClass="NormalTextBox"/>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EventLocationTextBox" ResourceKey="EventLocationTextBoxRequired" Display="None" EnableClientScript="false"/>
    </div>
    
    <div class="EventEditor">
        <asp:Label runat="server" ResourceKey="EventDescriptionLabel" CssClass="NormalBold"/>
        <dnn:TextEditor ID="EventDescriptionTextEditor" runat="server" Width="550" TextRenderMode="Raw" HtmlEncode="False" defaultmode="Rich" height="350" choosemode="True" chooserender="False" />
        <asp:CustomValidator runat="server" OnServerValidate="EventDescriptionTextEditorValidator_ServerValidate" ResourceKey="EventDescriptionTextEditorRequired" Display="None"/>
    </div>
    
</div>

<engage:ValidationSummary runat="server" />

<div class="AddEventFooterButtons" runat="server" id="AddEventFooterButtons">
    <asp:ImageButton ID="SaveEventButton" runat="server" OnClick="SaveEventButton_OnClick" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/save.gif" />
    <asp:ImageButton ID="CancelEventButton" runat="server" OnClick="CancelEventButton_OnClick" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/cancel.gif" CausesValidation="false"/>
    <asp:ImageButton ID="SaveAndCreateNewEventButton" runat="server" OnClick="SaveAndCreateNewEventButton_OnClick" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/save_create_new.gif" />
</div>

<div class="FinalButtons" runat="server" id="FinalButtons">
    <asp:LinkButton ID="CreateAnotherEventButton" runat="server" onclick="CreateAnotherEventButton_Click">Create Another Event</asp:LinkButton>
    <%--<asp:LinkButton ID="CreateEventEmailButton" runat="server">Create E-Mail For This Event</asp:LinkButton>--%>
    <asp:LinkButton ID="ExitButton" runat="server" onclick="ExitButton_Click">Exit</asp:LinkButton>
</div>

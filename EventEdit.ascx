<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventEdit" Codebehind="EventEdit.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="sectionheadcontrol" Src="~/controls/sectionheadcontrol.ascx" %>
<%@ Register src="Navigation/GlobalNavigation.ascx" tagname="GlobalNavigation" tagprefix="engage" %>
<%@ Register Src="Controls/ModuleMessage.ascx" TagName="ModuleMessage" TagPrefix="engage" %>

<span class="GlobalNavigation"><engage:GlobalNavigation ID="GlobalNavigation" runat="server" /></span>

<div class="AddEventSuccessMessage">
    <engage:ModuleMessage runat="server" ID="SuccessModuleMessage" MessageType="Success" TextResourceKey="AddEventSuccess"></engage:ModuleMessage>
</div>

<div id="AddNewEvent" runat="server" class="AddNewEvent">

    <h2 class="Head">
        <asp:Label ID="AddEditEventLabel" runat="server"></asp:Label>
    </h2>
    
    <div class="EventTitle">
        <asp:Label runat="server" ResourceKey="EventTitleLabel" CssClass="NormalBold"></asp:Label>
        <asp:TextBox ID="EventTitleTextBox" runat="server" CssClass="NormalTextBox" MaxLength="250"></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EventTitleTextBox" ResourceKey="EventTitleTextBoxRequired" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
    
    <div class="EventStartDate">
        <asp:Label runat="server" ResourceKey="EventStartDateLabel" CssClass="NormalBold"></asp:Label>
        <telerik:raddatetimepicker runat="server" id="StartDateTimePicker" skin="WebBlue">
            <timeview skin="WebBlue">
            </timeview>
                <DateInput InvalidStyleDuration="100"></DateInput>
            <calendar skin="WebBlue">
            </calendar>
        </telerik:raddatetimepicker>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="StartDateTimePicker" ResourceKey="StartDateTimePickerRequired" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
    
    <div class="EventEndDate">
        <asp:Label runat="server" ResourceKey="EventEndDateLabel" CssClass="NormalBold"></asp:Label>
        <telerik:raddatetimepicker runat="server" id="EndDateTimePicker" skin="WebBlue">
            <timeview skin="WebBlue">
            </timeview>
                <DateInput InvalidStyleDuration="100"></DateInput>
            <calendar skin="WebBlue">
            </calendar>
        </telerik:raddatetimepicker>
        
        <asp:CompareValidator 
            runat="server" Display="Dynamic"
            ControlToCompare="StartDateTimePicker"
            ControlToValidate="EndDateTimePicker" 
            ResourceKey="EndDateCompareValidator"
            Operator="GreaterThan">
        </asp:CompareValidator>
        
    </div>
    
    <div class="EventLocationAdd">
        <asp:Label runat="server" ResourceKey="EventLocationLabel" CssClass="NormalBold"></asp:Label>
        <asp:TextBox ID="EventLocationTextBox" runat="server" CssClass="NormalTextBox"></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EventLocationTextBox" ResourceKey="EventLocationTextBoxRequired" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
    
    <div class="EventEditor">
        <asp:Label runat="server" ResourceKey="EventDescriptionLabel" CssClass="NormalBold"></asp:Label>
        <dnn:TextEditor ID="EventDescriptionTextEditor" runat="server" Width="550" TextRenderMode="Raw" HtmlEncode="False" defaultmode="Rich" height="350" choosemode="True" chooserender="False" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EventDescriptionTextEditor" ResourceKey="EventDescriptionTextEditorRequired" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
    
</div>

<div class="AddEventFooterButtons" runat="server" id="AddEventFooterButtons">
    <asp:ImageButton ID="SaveEventButton" runat="server" OnClick="SaveEventButton_OnClick" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/save.gif" />
    <asp:ImageButton ID="CancelEventButton" runat="server" OnClick="CancelEventButton_OnClick" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/cancel.gif" CausesValidation="false"/>
    <asp:ImageButton ID="SaveAndCreateNewEventButton" runat="server" OnClick="SaveAndCreateNewEventButton_OnClick" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/save_create_new.gif" />
</div>

<div class="FinalButtons" runat="server" id="FinalButtons">
    <asp:LinkButton ID="CreateAnotherEventButton" runat="server" 
        onclick="CreateAnotherEventButton_Click">Create Another Event</asp:LinkButton>
    <%--<asp:LinkButton ID="CreateEventEmailButton" runat="server">Create E-Mail For This Event</asp:LinkButton>--%>
    <asp:LinkButton ID="ExitButton" runat="server" onclick="ExitButton_Click">Exit</asp:LinkButton>
</div>

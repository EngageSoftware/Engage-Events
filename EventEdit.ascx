<%@ Import Namespace="DotNetNuke.Services.Localization"%>
<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventEdit" Codebehind="EventEdit.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" TagName="RecurrenceEditor" Src="Controls/RecurrenceEditor.ascx" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Controls" Assembly="Engage.Framework" %>

<engage:ModuleMessage runat="server" ID="SuccessModuleMessage" MessageType="Success" TextResourceKey="AddEventSuccess" CssClass="AddEventSuccessMessage"/>

<div id="AddNewEvent" runat="server" class="AddNewEvent">

    <h2 class="SubHead">
        <asp:Label ID="AddEditEventLabel" runat="server"/>
    </h2>
    
    <div class="AEEventTitle">
        <asp:Label runat="server" ResourceKey="EventTitleLabel" CssClass="NormalBold" AssociatedControlID="EventTitleTextBox"/>
        <asp:TextBox ID="EventTitleTextBox" runat="server" CssClass="NormalTextBox" MaxLength="250"/>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EventTitleTextBox" ResourceKey="EventTitleTextBoxRequired" Display="None" EnableClientScript="false"/>
    </div>
    
    <div class="AEEventStartDate">
        <asp:Label runat="server" ResourceKey="EventStartDateLabel" CssClass="NormalBold" AssociatedControlID="StartDateTimePicker"/>
        <telerik:raddatetimepicker runat="server" id="StartDateTimePicker" skin="WebBlue">
            <timeview skin="WebBlue"/>
            <calendar skin="WebBlue" ShowRowHeaders="false"/>
            <DateInput InvalidStyleDuration="100"/>
            <ClientEvents OnDateSelected="StartDateTimePicker_DateSelected" />
        </telerik:raddatetimepicker>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="StartDateTimePicker" ResourceKey="StartDateTimePickerRequired" Display="None" EnableClientScript="false"/>
    </div>
    
    <div class="AEEventEndDate">
        <asp:Label runat="server" ResourceKey="EventEndDateLabel" CssClass="NormalBold" AssociatedControlID="EndDateTimePicker"/>
        <telerik:raddatetimepicker runat="server" id="EndDateTimePicker" skin="WebBlue">
            <timeview skin="WebBlue"/>
            <calendar skin="WebBlue" ShowRowHeaders="false"/>
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
    
    <div class="AEEventTimeZone">
        <asp:Label runat="server" ResourceKey="EventTimeZoneLabel" CssClass="NormalBold" AssociatedControlID="TimeZoneDropDownList" />
        <div class="timezone_display">
        	<asp:DropDownList runat="server" ID="TimeZoneDropDownList" />
	        <div class="Normal"><asp:CheckBox runat="server" ID="InDaylightTimeCheckBox" ResourceKey="EventTimeZoneCheckBox" /></div>
        </div>
    </div>
    
    <div class="AEEventLocationAdd">
        <asp:Label runat="server" ResourceKey="EventLocationLabel" CssClass="NormalBold" AssociatedControlID="EventLocationTextBox"/>
        <asp:TextBox ID="EventLocationTextBox" runat="server" CssClass="NormalTextBox"/>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EventLocationTextBox" ResourceKey="EventLocationTextBoxRequired" Display="None" EnableClientScript="false"/>
    </div>

    <div class="AEEventEditor">
        <asp:Label runat="server" ResourceKey="EventOverviewLabel" CssClass="NormalBold" AssociatedControlID="EventOverviewTextEditor"/>
        <asp:TextBox  ID="EventOverviewTextEditor" runat="server" Columns="30" Rows="5" Width="400" TextMode="MultiLine" Height="350" />
    </div>
    
    <div class="AEEventEditor">
        <asp:Label runat="server" ResourceKey="EventDescriptionLabel" CssClass="NormalBold" AssociatedControlID="EventDescriptionTextEditor"/>
        <dnn:TextEditor ID="EventDescriptionTextEditor" runat="server" Width="400" TextRenderMode="Raw" HtmlEncode="False" DefaultMode="Rich" Height="350" ChooseMode="True" ChooseRender="False" />
        <asp:CustomValidator ID="EventDescriptionTextEditorValidator" runat="server" ResourceKey="EventDescriptionTextEditorRequired" Display="None"/>
    </div>
    
    <div class="AEEventFeature">
        <asp:Label runat="server" ResourceKey="FeaturedEventLabel" CssClass="NormalBold" AssociatedControlID="FeaturedCheckBox"/>
        <asp:CheckBox ID="FeaturedCheckBox" runat="server" />        
    </div>
    <div class="AEEventRegister">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="AllowRegistrationsCheckBox" />
                <asp:AsyncPostBackTrigger ControlID="LimitRegistrationsCheckBox" />
                <asp:AsyncPostBackTrigger ControlID="CapacityMetMessageRadioButtonList" />
            </Triggers>
            <ContentTemplate>
                <asp:Label runat="server" ResourceKey="AllowRegistrationsLabel" CssClass="NormalBold MainLabel" AssociatedControlID="AllowRegistrationsCheckBox"/>
                <asp:CheckBox ID="AllowRegistrationsCheckBox" runat="server" Checked="true" AutoPostBack="true" />
                <fieldset class="registration_fs">
                    <legend class="registrationoptions SubHead"><%=Localize("Registration Options:") %></legend>
                    <asp:Panel ID="LimitRegistrationsPanel" runat="server">
                        <asp:Label ID="LimitRegistationsLabel" runat="server" ResourceKey="LimitRegistrationsLabel" CssClass="NormalBold RegCap" AssociatedControlID="LimitRegistrationsCheckBox" />
                        <asp:CheckBox ID="LimitRegistrationsCheckBox" runat="server" CssClass="NormalBold" AutoPostBack="true" />
                        <asp:Panel ID="RegistrationLimitPanel" runat="server" Visible="false">
                            <asp:Label runat="server" ResourceKey="RegistrationLimitLabel" CssClass="NormalBold RegCap" AssociatedControlID="LimitRegistrationsCheckBox" />
                            <telerik:RadNumericTextBox ID="RegistrationLimitTextBox" runat="server" Value="25" MinValue="1" ShowSpinButtons="True"> 
                                <NumberFormat AllowRounding="True" DecimalDigits="0"/>
                            </telerik:RadNumericTextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="RegistrationLimitTextBox" ResourceKey="RegistrationLimitRequiredValidator" CssClass="NormalRed" Display="None" />
                            <div>
                                <asp:Label runat="server" ResourceKey="CapacityMetMessageLabel" CssClass="NormalBold RegCap" AssociatedControlID="CapacityMetMessageRadioButtonList" />
                                <asp:RadioButtonList ID="CapacityMetMessageRadioButtonList" runat="server" ResourceKey="CapacityMetMessageRadioButtonList" CssClass="NormalBold" AutoPostBack="true">
                                    <asp:ListItem ResourceKey="DefaultMessage" Value="False" Selected="True"/>
                                    <asp:ListItem ResourceKey="CustomMessage" Value="True"/>
                                </asp:RadioButtonList>
                            </div>
                            <asp:Panel ID="CustomCapacityMetMessagePanel" runat="server" Visible="false">
                                <asp:Label runat="server" ResourceKey="CustomCapacityMetMessageLabel" CssClass="NormalBold RegCap" AssociatedControlID="CustomCapacityMetMessageTextEditor" />
                                <dnn:TextEditor ID="CustomCapacityMetMessageTextEditor" runat="server" Width="400" TextRenderMode="Raw" HtmlEncode="False" DefaultMode="Rich" Height="350" ChooseMode="True" ChooseRender="False" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>                
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="AEEventRecur">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="RecurringCheckbox" />
            </Triggers>
            <ContentTemplate>
                <asp:Label ID="RecurringEventLabel" runat="server" ResourceKey="RecurringEventLabel" CssClass="NormalBold" AssociatedControlID="RecurringCheckBox"/>
                <asp:CheckBox ID="RecurringCheckBox" runat="server" AutoPostBack="true" />
                <engage:RecurrenceEditor ID="RecurrenceEditor" runat="server" Visible="false" DatePickerSkin="WebBlue" />
                <asp:CustomValidator ID="RecurrenceEditorValidator" runat="server" ResourceKey="InvalidRecurrence" Display="None" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

<engage:ValidationSummary runat="server" />

<asp:MultiView ID="FooterMultiview" runat="server" ActiveViewIndex="0">
    <asp:View ID="AddEventFooterView" runat="server">
        <div class="AddEventFooterButtons AdminButtons FooterButtons">
            <asp:ImageButton ID="SaveEventButton" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/save.gif" />
            <asp:HyperLink ID="CancelEventLink" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/cancel_go_home.gif" ResourceKey="Cancel.Alt" />
            <asp:ImageButton ID="SaveAndCreateNewEventButton" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/save_create_new.gif"/>
        </div>
    </asp:View>
    <asp:View ID="FinalFooterView" runat="server">
        <div class="FinalButtons AdminButtons FooterButtons">
            <asp:HyperLink ID="CreateAnotherEventLink" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/create_another_event.gif" ResourceKey="CreateAnother.Alt" />
            <%--<asp:LinkButton ID="CreateEventEmailButton" runat="server">Create E-Mail For This Event</asp:LinkButton>--%>
            <asp:HyperLink ID="CancelGoHomeLink" runat="server" CssClass="Normal" ImageUrl="~/DesktopModules/EngageEvents/Images/home.gif" ResourceKey=CancelGoHome.Alt" />
        </div>
    </asp:View>
</asp:MultiView>

<script type="text/ecmascript">
    function StartDateTimePicker_DateSelected(sender, eventArgs) {
        var EndDateTimePicker = $find("<%= EndDateTimePicker.ClientID %>");
        // don't update end date if there's already an end date but not an old start date
        if (EndDateTimePicker.isEmpty() || eventArgs.get_oldDate() || EndDateTimePicker.get_selectedDate() <= eventArgs.get_newDate()) {
            var selectedDateSpan = 1800000; // 30 minutes
            if (!EndDateTimePicker.isEmpty() && EndDateTimePicker.get_selectedDate() > eventArgs.get_newDate()) {
                selectedDateSpan = EndDateTimePicker.get_selectedDate() - eventArgs.get_oldDate();
            }

            EndDateTimePicker.set_selectedDate(new Date(eventArgs.get_newDate().getTime() + selectedDateSpan));
        }
    }
</script>
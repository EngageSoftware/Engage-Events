<%@ Import Namespace="DotNetNuke.Services.Localization"%>
<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventEdit" Codebehind="EventEdit.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" TagName="RecurrenceEditor" Src="Controls/RecurrenceEditor.ascx" %>
<%@ Register TagPrefix="engage" TagName="DeleteAction" Src="Actions/DeleteAction.ascx" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Controls" Assembly="Engage.Framework" %>

<engage:ModuleMessage runat="server" ID="SuccessModuleMessage" MessageType="Success" CssClass="AddEventSuccessMessage"/>

<div id="AddNewEvent" runat="server" class="AddNewEvent">

    <h2 class="SubHead">
        <asp:Label ID="AddEditEventLabel" runat="server"/>
    </h2>
    
    <div class="AEEventTitle eng-section">
        <asp:Label runat="server" ResourceKey="EventTitleLabel" CssClass="NormalBold eng-label" AssociatedControlID="EventTitleTextBox"/>
        <telerik:RadTextBox ID="EventTitleTextBox" runat="server" CssClass="NormalTextBox eng-input eng-input-large" MaxLength="250" skin="WebBlue" ShouldResetWidthInPixels="false"/>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EventTitleTextBox" ResourceKey="EventTitleTextBoxRequired" Display="None" EnableClientScript="false"/>
    </div>
    
    <asp:Panel ID="CategoryPanel" runat="server" CssClass="AEEventCategory eng-section">
        <asp:Label runat="server" ResourceKey="EventCategoryLabel" CssClass="NormalBold eng-label" AssociatedControlID="CategoryComboBox"/>
        <telerik:RadComboBox ID="CategoryComboBox" runat="server" CssClass="eng-input eng-input-large" skin="WebBlue" MarkFirstMatch="true" ShowDropDownOnTextboxClick="true" 
            OnClientSelectedIndexChanged="CategoryComboBox_SelectedIndexChanged" OnClientTextChange="CategoryComboBox_TextChange" ShouldResetWidthInPixels="false" />
        <asp:Label ID="CategoryCreationPendingLabel" runat="server" ResourceKey="CategoryCreationPending" CssClass="InformationBody" style="display:none;" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="CategoryComboBox" ResourceKey="EventCategoryRequired" Display="None" EnableClientScript="false"/>
        <asp:CustomValidator ID="UniqueCategoryNameValidator" runat="server" ControlToValidate="CategoryComboBox" ResourceKey="EventCategoryUnique" Display="None" EnableClientScript="false"/>
    </asp:Panel>
    
    <div class="AEEventStartDate eng-section">
        <asp:Label runat="server" ResourceKey="EventStartDateLabel" CssClass="NormalBold eng-label" AssociatedControlID="StartDateTimePicker"/>
        <telerik:raddatetimepicker runat="server" id="StartDateTimePicker" skin="WebBlue" ShouldResetWidthInPixels="false">
            <timeview skin="WebBlue"/>
            <calendar skin="WebBlue" ShowRowHeaders="false"/>
            <DateInput InvalidStyleDuration="100" CssClass="eng-input eng-input-medium"/>
            <ClientEvents OnDateSelected="StartDateTimePicker_DateSelected" />
        </telerik:raddatetimepicker>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="StartDateTimePicker" ResourceKey="StartDateTimePickerRequired" Display="None" EnableClientScript="false"/>
    </div>
    
    <div class="AEEventEndDate eng-section">
        <asp:Label runat="server" ResourceKey="EventEndDateLabel" CssClass="NormalBold eng-label" AssociatedControlID="EndDateTimePicker"/>
        <telerik:raddatetimepicker runat="server" id="EndDateTimePicker" skin="WebBlue" ShouldResetWidthInPixels="false">
            <timeview skin="WebBlue"/>
            <calendar skin="WebBlue" ShowRowHeaders="false"/>
            <DateInput InvalidStyleDuration="100" CssClass="eng-input eng-input-medium"/>
        </telerik:raddatetimepicker>
        
        <asp:CompareValidator 
            runat="server" Display="None" EnableClientScript="false"
            ControlToCompare="StartDateTimePicker"
            ControlToValidate="EndDateTimePicker" 
            ResourceKey="EndDateCompareValidator"
            Operator="GreaterThan"/>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="EndDateTimePicker" ResourceKey="EndDateTimePickerRequired" Display="None" EnableClientScript="false"/>
    </div>
    
    <div class="AEEventTimeZone eng-section">
        <asp:Label runat="server" ResourceKey="EventTimeZoneLabel" CssClass="NormalBold eng-label" AssociatedControlID="TimeZoneDropDownList" />
        <div class="timezone_display">
        	<asp:DropDownList runat="server" ID="TimeZoneDropDownList" CssClass="eng-input eng-input-x-large" />
        </div>
    </div>
    
    <div class="AEEventLocationAdd eng-section">
        <asp:Label runat="server" ResourceKey="EventLocationLabel" CssClass="NormalBold eng-label" AssociatedControlID="EventLocationTextBox"/>
        <telerik:RadTextBox ID="EventLocationTextBox" runat="server" CssClass="NormalTextBox eng-input eng-input-large" Skin="WebBlue" ShouldResetWidthInPixels="false" />
    </div>

    <div class="AEEventEditor eng-section">
        <asp:Label runat="server" ResourceKey="EventOverviewLabel" CssClass="NormalBold eng-label" AssociatedControlID="EventOverviewTextEditor"/>
        <telerik:RadTextBox ID="EventOverviewTextEditor" runat="server" CssClass="eng-input eng-input-x-large" TextMode="MultiLine" Skin="WebBlue" ShouldResetWidthInPixels="false" />
    </div>
    
    <div class="AEEventEditor eng-section">
        <asp:Label runat="server" ResourceKey="EventDescriptionLabel" CssClass="NormalBold eng-label" AssociatedControlID="EventDescriptionTextEditor"/>
        <dnn:TextEditor ID="EventDescriptionTextEditor" runat="server" Height="400" Width="400" CssClass="eng-input eng-input-x-large" TextRenderMode="Raw" HtmlEncode="False" DefaultMode="Rich" ChooseMode="True" ChooseRender="False" />
    </div>
    
    <div class="AEEventFeature eng-section">
        <asp:Label runat="server" ResourceKey="FeaturedEventLabel" CssClass="NormalBold eng-label" AssociatedControlID="FeaturedCheckBox"/>
        <asp:CheckBox ID="FeaturedCheckBox" runat="server" />        
    </div>
    <div class="AEEventRegister eng-section">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="AllowRegistrationsCheckBox" />
                <asp:AsyncPostBackTrigger ControlID="LimitRegistrationsCheckBox" />
                <asp:AsyncPostBackTrigger ControlID="CapacityMetMessageRadioButtonList" />
            </Triggers>
            <ContentTemplate>
                <asp:Label runat="server" ResourceKey="AllowRegistrationsLabel" CssClass="NormalBold eng-label MainLabel" AssociatedControlID="AllowRegistrationsCheckBox"/>
                <asp:CheckBox ID="AllowRegistrationsCheckBox" runat="server" Checked="true" AutoPostBack="true" />
                <asp:Panel ID="LimitRegistrationsPanel" runat="server">
                    <fieldset class="registration_fs">
                        <legend class="registrationoptions SubHead"><%=Localize("Registration Options:") %></legend>
                        <asp:Label ID="LimitRegistrationsLabel" runat="server" ResourceKey="LimitRegistrationsLabel" CssClass="NormalBold RegCap" AssociatedControlID="LimitRegistrationsCheckBox" />
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
                    </fieldset>
                </asp:Panel>                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="AEEventRecur eng-section">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="RecurringCheckbox" />
            </Triggers>
            <ContentTemplate>
                <asp:Label ID="RecurringEventLabel" runat="server" ResourceKey="RecurringEventLabel" CssClass="NormalBold eng-label" AssociatedControlID="RecurringCheckBox"/>
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
            <asp:Button ID="SaveEventButton" runat="server" CssClass="Normal save-btn primary-btn" ResourceKey="Save.Alt" />
            <asp:Button ID="SaveAndCreateNewEventButton" runat="server" CssClass="Normal save-new-btn primary-btn" ResourceKey="SaveAndCreateNew.Alt" />
            <asp:HyperLink ID="CancelEventLink" runat="server" CssClass="Normal cancel-btn secondary-btn" ResourceKey="Cancel.Alt" />
            <engage:DeleteAction ID="DeleteAction" runat="server" CssClass="Normal delete-btn tertiary-btn" />
        </div>
    </asp:View>
    <asp:View ID="FinalFooterView" runat="server">
        <div class="FinalButtons AdminButtons FooterButtons">
            <asp:Button ID="CreateAnotherEventLink" runat="server" CssClass="Normal" ResourceKey="CreateAnother.Alt" />
            <asp:HyperLink ID="CancelGoHomeLink" runat="server" CssClass="Normal" ResourceKey="Home" />
        </div>
    </asp:View>
</asp:MultiView>
<div style="clear:both">&nbsp;</div>
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
    function CategoryComboBox_SelectedIndexChanged(sender, eventArgs) {
        $get("<%=CategoryCreationPendingLabel.ClientID %>").style.display = 'none';
    }
    function CategoryComboBox_TextChange(sender, eventArgs) {
        $get("<%=CategoryCreationPendingLabel.ClientID %>").style.display = 'inline';
    }
</script>
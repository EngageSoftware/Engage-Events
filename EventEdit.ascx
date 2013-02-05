<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventEdit" Codebehind="EventEdit.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" TagName="RecurrenceEditor" Src="Controls/RecurrenceEditor.ascx" %>
<%@ Register TagPrefix="engage" TagName="DeleteAction" Src="Actions/DeleteAction.ascx" %>

<engage:ModuleMessage runat="server" ID="SuccessModuleMessage" MessageType="Success" CssClass="AddEventSuccessMessage"/>

<asp:Panel ID="EventEditWrap" runat="server" CssClass="eng-form eng-events-add-edit AddNewEvent" DefaultButton="SaveEventButton">
    <asp:ValidationSummary runat="server" ID="ValidationSummary"
        ForeColor="" 
        CssClass="eng-form-message eng-form-validation-summary" />

    <asp:Placeholder ID="EventEditForm" runat="server">
        <fieldset>
            <legend><%=this.Localize(this.EventId.HasValue ? "EditEvent.Text" : "AddNewEvent.Text")%></legend>
            <ul class="eng-form-items">
                <li class="eng-events-title eng-medium eng-form-item">
                    <asp:Label runat="server" ResourceKey="EventTitleLabel" AssociatedControlID="EventTitleTextBox"/>
                    <telerik:RadTextBox ID="EventTitleTextBox" runat="server" MaxLength="250" skin="WebBlue" ShouldResetWidthInPixels="false"/>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="EventTitleTextBox" ResourceKey="EventTitleTextBoxRequired" Display="None" EnableClientScript="false" ValidationGroup="EditEvent" />
                </li>
    
                <asp:Placeholder ID="CategoryPanel" runat="server">
                    <li class="eng-events-category eng-medium eng-form-item">
                        <asp:Label runat="server" ResourceKey="EventCategoryLabel" AssociatedControlID="CategoryComboBox"/>
                        <telerik:RadComboBox ID="CategoryComboBox" runat="server" skin="WebBlue" MarkFirstMatch="true" ShowDropDownOnTextboxClick="true" 
                            OnClientSelectedIndexChanged="CategoryComboBox_SelectedIndexChanged" OnClientTextChange="CategoryComboBox_TextChange" ShouldResetWidthInPixels="false" />
                        <asp:Label ID="CategoryCreationPendingLabel" runat="server" ResourceKey="CategoryCreationPending" CssClass="InformationBody" style="display:none;" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="CategoryComboBox" ResourceKey="EventCategoryRequired" Display="None" EnableClientScript="false" ValidationGroup="EditEvent" />
                        <asp:CustomValidator ID="UniqueCategoryNameValidator" runat="server" ControlToValidate="CategoryComboBox" ResourceKey="EventCategoryUnique" Display="None" EnableClientScript="false" ValidationGroup="EditEvent" />
                    </li>
                </asp:Placeholder>
    
                <li class="eng-events-start-date eng-medium eng-form-item">
                    <asp:Label runat="server" ResourceKey="EventStartDateLabel" AssociatedControlID="StartDateTimePicker"/>
                    <telerik:raddatetimepicker runat="server" id="StartDateTimePicker" skin="WebBlue" ShouldResetWidthInPixels="false">
                        <timeview skin="WebBlue"/>
                        <calendar skin="WebBlue" ShowRowHeaders="false"/>
                        <DateInput InvalidStyleDuration="100" />
                        <ClientEvents OnDateSelected="StartDateTimePicker_DateSelected" />
                    </telerik:raddatetimepicker>
        
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="StartDateTimePicker" ResourceKey="StartDateTimePickerRequired" Display="None" EnableClientScript="false" ValidationGroup="EditEvent" />
                </li>
    
                <li class="eng-events-end-date eng-medium eng-form-item">
                    <asp:Label runat="server" ResourceKey="EventEndDateLabel" AssociatedControlID="EndDateTimePicker"/>
                    <telerik:raddatetimepicker runat="server" id="EndDateTimePicker" skin="WebBlue" ShouldResetWidthInPixels="false">
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
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="EndDateTimePicker" ResourceKey="EndDateTimePickerRequired" Display="None" EnableClientScript="false" ValidationGroup="EditEvent" />
                </li>
    
                <li class="eng-events-time-zone eng-medium eng-form-item">
                    <asp:Label runat="server" ResourceKey="EventTimeZoneLabel" AssociatedControlID="TimeZoneDropDownList" />
        	        <asp:DropDownList runat="server" ID="TimeZoneDropDownList" />
                </li>
    
                <li class="eng-events-location eng-medium eng-form-item">
                    <asp:Label runat="server" ResourceKey="EventLocationLabel" AssociatedControlID="EventLocationTextBox"/>
                    <telerik:RadTextBox ID="EventLocationTextBox" runat="server" Skin="WebBlue" ShouldResetWidthInPixels="false" />
                </li>

                <li class="eng-events-overview eng-x-large eng-form-item">
                    <asp:Label runat="server" ResourceKey="EventOverviewLabel" AssociatedControlID="EventOverviewTextEditor"/>
                    <telerik:RadTextBox ID="EventOverviewTextEditor" runat="server" TextMode="MultiLine" Skin="WebBlue" ShouldResetWidthInPixels="false" />
                </li>
    
                <li class="eng-events-description eng-x-large eng-form-item">
                    <asp:Label runat="server" ResourceKey="EventDescriptionLabel" AssociatedControlID="EventDescriptionTextEditor"/>
                    <dnn:TextEditor ID="EventDescriptionTextEditor" runat="server" Height="400" TextRenderMode="Raw" HtmlEncode="False" DefaultMode="Rich" ChooseMode="True" ChooseRender="False" />
                </li>
                <li class="eng-events-featured eng-tiny eng-form-item">
                    <asp:Label runat="server" ResourceKey="FeaturedEventLabel" AssociatedControlID="FeaturedCheckBox"/>
                    <asp:CheckBox ID="FeaturedCheckBox" runat="server" />        
                </li>
            </ul>
        </fieldset>
        <fieldset>
            <legend><%=this.Localize("Registration.Header")%></legend>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="AllowRegistrationsCheckBox" />
                    <asp:AsyncPostBackTrigger ControlID="LimitRegistrationsCheckBox" />
                    <asp:AsyncPostBackTrigger ControlID="CapacityMetMessageRadioButtonList" />
                </Triggers>
                <ContentTemplate>
                    <ul class="eng-form-items">
                        <li class="eng-form-item eng-tiny eng-events-allow-registration">
                            <asp:Label runat="server" ResourceKey="AllowRegistrationsLabel" AssociatedControlID="AllowRegistrationsCheckBox"/>
                            <asp:CheckBox ID="AllowRegistrationsCheckBox" runat="server" Checked="true" AutoPostBack="true" />
                        </li>
                        <asp:PlaceHolder ID="LimitRegistrationsPanel" runat="server" Visible="false">
                            <li class="eng-form-item eng-tiny eng-events-registration-cap">
                                <asp:Label ID="LimitRegistrationsLabel" runat="server" ResourceKey="LimitRegistrationsLabel" CssClass="RegCap" AssociatedControlID="LimitRegistrationsCheckBox" />
                                <asp:CheckBox ID="LimitRegistrationsCheckBox" runat="server" AutoPostBack="true" />
                                <asp:Label ID="RegistrationCountLabel" runat="server" CssClass="eng-form-message eng-form-info" Visible="false"/>
                            </li>
                            <asp:PlaceHolder ID="RegistrationLimitPanel" runat="server" Visible="false">
                                <li class="eng-form-item eng-tiny eng-events-registration-limit">
                                    <asp:Label runat="server" ResourceKey="RegistrationLimitLabel" CssClass="RegCap" AssociatedControlID="RegistrationLimitTextBox" />
                                    <telerik:RadNumericTextBox ID="RegistrationLimitTextBox" runat="server" Value="25" MinValue="1" ShowSpinButtons="True" NumberFormat-AllowRounding="True" NumberFormat-DecimalDigits="0"/>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="RegistrationLimitTextBox" ResourceKey="RegistrationLimitRequiredValidator" Display="None" ValidationGroup="EditEvent" />
                                    <asp:CompareValidator ID="RegistrationLimitValidator" runat="server" ControlToValidate="RegistrationLimitTextBox" ResourceKey="RegistrationLimitValidator" Display="None" Operator="GreaterThanEqual" Type="Integer" ValueToCompare="0" ValidationGroup="EditEvent" />
                                </li>
                                <li class="eng-form-item eng-tiny eng-events-capacity-met-message-type">
                                    <asp:Label runat="server" ResourceKey="CapacityMetMessageLabel" CssClass="RegCap" AssociatedControlID="CapacityMetMessageRadioButtonList" />
                                    <asp:RadioButtonList ID="CapacityMetMessageRadioButtonList" runat="server" ResourceKey="CapacityMetMessageRadioButtonList" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="true">
                                        <asp:ListItem ResourceKey="DefaultMessage" Value="False" Selected="True"/>
                                        <asp:ListItem ResourceKey="CustomMessage" Value="True"/>
                                    </asp:RadioButtonList>
                                </li>
                                <asp:Placeholder ID="CustomCapacityMetMessagePanel" runat="server" Visible="false">
                                    <li class="eng-form-item eng-x-large eng-events-capacity-met-message">
                                        <asp:Label runat="server" ResourceKey="CustomCapacityMetMessageLabel" CssClass="RegCap" AssociatedControlID="CustomCapacityMetMessageTextEditor" />
                                        <dnn:TextEditor ID="CustomCapacityMetMessageTextEditor" runat="server" TextRenderMode="Raw" HtmlEncode="False" DefaultMode="Rich" Height="350" ChooseMode="True" ChooseRender="False" />
                                    </li>
                                </asp:Placeholder>
                            </asp:PlaceHolder>
                        </asp:PlaceHolder>
                    </ul>              
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <fieldset>
            <legend><%=this.Localize("Recurrence.Header")%></legend>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="RecurringCheckbox" />
                </Triggers>
                <ContentTemplate>
                    <ul class="eng-form-items">
                        <li class="eng-events-recuring eng-tiny eng-form-item">
                            <asp:Label ID="RecurringEventLabel" runat="server" ResourceKey="RecurringEventLabel" AssociatedControlID="RecurringCheckBox"/>
                            <asp:CheckBox ID="RecurringCheckBox" runat="server" AutoPostBack="true" />
                        </li>
                        <engage:RecurrenceEditor ID="RecurrenceEditor" runat="server" Visible="false" DatePickerSkin="WebBlue" />
                    </ul>
                    <asp:CustomValidator ID="RecurrenceEditorValidator" runat="server" ResourceKey="InvalidRecurrence" Display="None" ValidationGroup="EditEvent" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </asp:Placeholder>
    
    <asp:MultiView ID="FooterMultiview" runat="server" ActiveViewIndex="0">
        <asp:View ID="AddEventFooterView" runat="server">
            <ul class="eng-actions">
                <li><asp:Button ID="SaveEventButton" runat="server" CssClass="save-btn eng-primary-action" ResourceKey="Save.Alt" ValidationGroup="EditEvent" /></li>
                <li><asp:Button ID="SaveAndCreateNewEventButton" runat="server" CssClass="save-new-btn eng-primary-action" ResourceKey="SaveAndCreateNew.Alt" ValidationGroup="EditEvent" /></li>
                <li><asp:Button ID="CancelButton" runat="server" CssClass="cancel-btn eng-secondary-action" ResourceKey="Cancel.Alt" CausesValidation="False" /></li>
                <li><engage:DeleteAction ID="DeleteAction" runat="server" CssClass="delete-btn eng-tertiary-action" /></li>
            </ul>
        </asp:View>
        <asp:View ID="FinalFooterView" runat="server">
            <ul class="eng-actions">
                <li><asp:Button ID="CreateAnotherEventLink" runat="server" CssClass="eng-primary-action" ResourceKey="CreateAnother.Alt" CausesValidation="False" /></li>
                <li><asp:Button ID="CancelGoHomeButton" runat="server" CssClass="eng-secondary-action" ResourceKey="Home" CausesValidation="False" /></li>
            </ul>
        </asp:View>
    </asp:MultiView>
    <div class="eng-end-form"></div>
    <asp:ScriptManagerProxy runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="EngageEvents" Name="Engage.Dnn.Events.JavaScript.EngageEvents.EventEdit.combined.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <script type="text/ecmascript">
    engageEvents.setupEventEdit({ 
        endDateTimePickerId: "<%= EndDateTimePicker.ClientID %>", 
        categoryCreationPendingLabelId: "<%=CategoryCreationPendingLabel.ClientID %>", 
        startDateTimePickerDateSelectedFunctionName: "<%= StartDateTimePicker.ClientEvents.OnDateSelected %>",
        categoryComboBoxSelectedIndexChangedFunctionName: "<%= CategoryComboBox.OnClientSelectedIndexChanged %>",
        categoryComboBoxTextChangeFunctionName: "<%= CategoryComboBox.OnClientTextChange %>"
    });
    </script>
</asp:Panel>
<div class="eng-end-form"></div>
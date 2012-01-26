<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.TemplateDisplayOptions" CodeBehind="TemplateDisplayOptions.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<fieldset id="eng-date-range-wrap">
    <legend><%= Localize("DisplayModeLabel")%></legend>
    <ul class="eng-form-items">
        <li class="eng-form-item eng-range-start-setting">
            <dnn:label runat="server" ControlName="RangeStartDropDownList" ResourceKey="RangeStartLabel" CssClass="SubHead" />
            <asp:DropDownList ID="RangeStartDropDownList" runat="server" CssClass="eng-range" data-bind="value:start.value">
                <asp:ListItem Value="">The Beginning of Time</asp:ListItem>
                <asp:ListItem Value="0|Year">This Year</asp:ListItem>
                <asp:ListItem Value="0|Month">This Month</asp:ListItem>
                <asp:ListItem Value="-1|Day">Yesterday</asp:ListItem>
                <asp:ListItem Value="0|Day">Today</asp:ListItem>
                <asp:ListItem Value="1|Day">Tomorrow</asp:ListItem>
                <asp:ListItem Value="1|Month">Next Month</asp:ListItem>
                <asp:ListItem Value="specific-date" class="eng-specific-date-opt">A Specific Date</asp:ListItem>
                <asp:ListItem Value="window" class="eng-window-opt">A Window of Days Before End</asp:ListItem>
            </asp:DropDownList>
            <div class="eng-specific-date-wrap" data-bind="visible:start.showSpecificDateSection">
                <telerik:RadDatePicker ID="StartSpecificDatePicker" runat="server" Calendar-ShowRowHeaders="false" data-bind="date:start.specificDate" data-client-id="<%# StartSpecificDatePicker.ClientID %>" />
            </div>
            <div class="eng-window-wrap" data-bind="visible: start.showWindowSection">
                <telerik:RadNumericTextBox ID="StartWindowAmountTextBox" runat="server" Width="3em" MinValue="0" ShowSpinButtons="True" data-bind="integer:start.windowAmount" data-client-id="<%# this.StartWindowAmountTextBox.ClientID %>">
                    <NumberFormat AllowRounding="True" DecimalDigits="0"/>
                </telerik:RadNumericTextBox>
                <asp:DropDownList ID="StartWindowIntervalDropDownList" runat="server" data-bind="value:start.windowInterval">
                    <asp:ListItem Value="Day">Days</asp:ListItem>
                    <asp:ListItem Value="Month">Months</asp:ListItem>
                    <asp:ListItem Value="Year">Years</asp:ListItem>
                </asp:DropDownList>
                <asp:Label runat="server" AssociatedControlID="StartWindowIntervalDropDownList" ResourceKey="before end" />
            </div>
        </li>
        <li class="eng-form-item eng-range-end-setting">
            <dnn:label runat="server" ControlName="RangeEndDropDownList" ResourceKey="RangeEndLabel" CssClass="SubHead" />
            <asp:DropDownList ID="RangeEndDropDownList" runat="server" CssClass="eng-range" data-bind="value:end.value">
                <asp:ListItem Value="-1|Day">Yesterday</asp:ListItem>
                <asp:ListItem Value="0|Day">Today</asp:ListItem>
                <asp:ListItem Value="1|Day">Tomorrow</asp:ListItem>
                <asp:ListItem Value="0|Month">This Month</asp:ListItem>
                <asp:ListItem Value="1|Month">Next Month</asp:ListItem>
                <asp:ListItem Value="0|Year">This Year</asp:ListItem>
                <asp:ListItem Value="">The End of Time</asp:ListItem>
                <asp:ListItem Value="specific-date" class="eng-specific-date-opt">A Specific Date</asp:ListItem>
                <asp:ListItem Value="window" class="eng-window-opt">A Window of Days After Start</asp:ListItem>
            </asp:DropDownList>
            <div class="eng-specific-date-wrap" data-bind="visible:end.showSpecificDateSection">
                <telerik:RadDatePicker ID="EndSpecificDatePicker" runat="server" Calendar-ShowRowHeaders="false" data-bind="date:end.specificDate" data-client-id="<%# EndSpecificDatePicker.ClientID %>" />
            </div>
            <div class="eng-window-wrap" data-bind="visible:end.showWindowSection">
                <telerik:RadNumericTextBox ID="EndWindowAmountTextBox" runat="server" Width="3em" MinValue="0" ShowSpinButtons="True" data-bind="integer:end.windowAmount" data-client-id="<%# this.EndWindowAmountTextBox.ClientID %>">
                    <NumberFormat AllowRounding="True" DecimalDigits="0"/>
                </telerik:RadNumericTextBox>
                <asp:DropDownList ID="EndWindowIntervalDropDownList" runat="server" data-bind="value:end.windowInterval">
                    <asp:ListItem Value="Day">Days</asp:ListItem>
                    <asp:ListItem Value="Month">Months</asp:ListItem>
                    <asp:ListItem Value="Year">Years</asp:ListItem>
                </asp:DropDownList>
                <asp:Label runat="server" AssociatedControlID="EndWindowIntervalDropDownList" ResourceKey="after start" />
            </div>
        </li>
    </ul>
    <p id="eng-date-range-example" data-bind="html:exampleDateRangeHtml,css:{NormalRed:dateRangeIsValid}"></p>
    <asp:CustomValidator ID="DateRangeValidator" runat="server" CssClass="NormalRed" Display="None" />
</fieldset>

<ul class="eng-form-items">
    <li class="eng-form-item eng-medium eng-list-records-per-page-setting">
        <dnn:label runat="server" ControlName="RecordsPerPageTextBox" ResourceKey="PagingLabel" CssClass="SubHead" />
        <span class="NumericTextBoxWrapper">
            <telerik:RadNumericTextBox ID="RecordsPerPageTextBox" runat="server" 
                                       MaxLength="3" 
                                       MaxValue="100" 
                                       MinValue="1" 
                                       ShowSpinButtons="True"
                                       NumberFormat-AllowRounding="True" 
                                       NumberFormat-DecimalDigits="0" />
        </span>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="RecordsPerPageTextBox" ResourceKey="RecordsPerPageRequiredValidator" CssClass="NormalRed" Display="None" />
    </li>
</ul>

<script type="text/javascript">
var engageEventsDateRangeData = {
    serviceUrl: '<%= this.ResolveUrl("../Services/SettingsService.asmx") %>',
    start: <%= StartDateRangeBoundJson %>,
    end: <%= EndDateRangeBoundJson %>
};
</script>
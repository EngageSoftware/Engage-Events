<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.CalendarDisplayOptions" Codebehind="CustomDisplayOptions.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<ul class="eng-form-items">
    <li class="eng-form-item eng-medium eng-calendar-skin-setting">
        <dnn:label id="SkinLabel" runat="server" controlname="SkinDropDownList" ResourceKey="SkinLabel" CssClass="SubHead"/>
        <asp:dropdownlist id="SkinDropDownList" Runat="server"/>
    </li>
    <li class="eng-form-item eng-medium eng-calendar-events-per-day-setting">
        <dnn:label runat="server" controlname="EventsPerDayTextBox" ResourceKey="EventsPerDayLabel" CssClass="SubHead"/>
        <span class="NumericTextBoxWrapper">
            <telerik:RadNumericTextBox ID="EventsPerDayTextBox" runat="server" 
                                       MaxLength="3" 
                                       MaxValue="100" 
                                       MinValue="1" 
                                       ShowSpinButtons="True"
                                       NumberFormat-AllowRounding="True"
                                       NumberFormat-DecimalDigits="0" />
        </span>
    </li>
</ul>
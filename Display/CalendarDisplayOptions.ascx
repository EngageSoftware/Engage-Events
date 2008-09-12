<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.CalendarDisplayOptions" Codebehind="CustomDisplayOptions.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<div class="EventsSetting">
    <dnn:label id="SkinLabel" runat="server" controlname="SkinDropDownList" ResourceKey="SkinLabel" CssClass="SubHead"/>
    <asp:dropdownlist id="SkinDropDownList" Runat="server"/>
</div>
<div class="EventsSetting">
    <dnn:label runat="server" controlname="EventsPerDayTextBox" ResourceKey="EventsPerDayLabel" CssClass="SubHead"/>
    <span class="NumericTextBoxWrapper">
        <telerik:radnumerictextbox id="EventsPerDayTextBox" runat="server" value="3" maxlength="3" maxvalue="100" minvalue="1" showspinbuttons="True"> 
            <NumberFormat AllowRounding="True" DecimalDigits="0"/>
        </telerik:radnumerictextbox>
    </span>
</div>
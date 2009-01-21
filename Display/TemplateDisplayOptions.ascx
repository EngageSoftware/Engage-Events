<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.TemplateDisplayOptions" CodeBehind="TemplateDisplayOptions.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="sectionhead" Src="~/controls/sectionheadcontrol.ascx" %>

<div class="EventsSetting">
    <dnn:label ID="DisplayModeLabel" runat="server" ControlName="DisplayModeDropDown" Text="Select an Display Mode:" ResourceKey="DisplayModeLabel" CssClass="SubHead" />
    <asp:DropDownList ID="DisplayModeDropDown" runat="server" />
</div>

<dnn:sectionhead ID="TemplateSettingsSectionHead" CssClass="Head SectionHead" runat="server" Text="Templates" Section="TemplatesSection" ResourceKey="TemplateSettingsSectionHead" IsExpanded="True" />
<div id="TemplatesSection" runat="server" class="TemplatesSection">
    <div class="EventsSetting">
        <dnn:label ID="HeaderLabel" runat="server" ControlName="HeaderDropdownlist" Text="Select an Header Template:" ResourceKey="HeaderLabel" CssClass="SubHead" />
        <asp:DropDownList ID="HeaderDropDownList" runat="server" />
    </div>
    <div class="EventsSetting">
        <dnn:label ID="ItemLabel" runat="server" ControlName="ItemDropdownlist" Text="Select an Item Template:" ResourceKey="ItemLabel" CssClass="SubHead" />
        <asp:DropDownList ID="ItemDropDownList" runat="server" />
    </div>
    <div class="EventsSetting">
        <dnn:label ID="FooterLabel" runat="server" ControlName="FooterDropdownlist" Text="Select an Footer Template:" ResourceKey="FooterLabel" CssClass="SubHead" />
        <asp:DropDownList ID="FooterDropDownList" runat="server" />
    </div>
    <div class="EventsSetting">
        <dnn:label ID="DetailLabel" runat="server" ControlName="DetailDropdownlist" Text="Select an Detail Template:" ResourceKey="DetailLabel" CssClass="SubHead" />
        <asp:DropDownList ID="DetailDropDownList" runat="server" />
    </div>
</div>

<div class="EventsSetting">
    <dnn:label ID="PagingLabel" runat="server" ControlName="RecordsPerPageTextBox" Text="Enter number of records to display per page:" ResourceKey="PagingLabel" CssClass="SubHead" />
    <span class="NumericTextBoxWrapper">
        <telerik:radnumerictextbox id="RecordsPerPageTextBox" runat="server" maxlength="3" maxvalue="100" minvalue="1" showspinbuttons="True"> 
            <NumberFormat AllowRounding="True" DecimalDigits="0"/>
        </telerik:radnumerictextbox>
    </span>
    <asp:RequiredFieldValidator runat="server" ControlToValidate="RecordsPerPageTextBox" ResourceKey="RecordsPerPageRequiredValidator" CssClass="NormalRed" Display="None" />
</div>

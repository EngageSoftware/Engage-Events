<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.TemplateDisplayOptions" Codebehind="TemplateDisplayOptions.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="sectionhead" Src="~/controls/sectionheadcontrol.ascx" %>
<style type="text/css">
    @import url(<%=Engage.Dnn.Events.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Events.ModuleBase.DesktopModuleFolderName %>Module.css);
</style>

<div id="TemplateSettings" class="normal">
    <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
        <tr>
            <td class="SubHead"><dnn:label id="DisplayModeLabel" runat="server" controlname="DisplayModeDropDown" text="Select an Display Mode:" ResourceKey="DisplayModeLabel"/></td>
		    <td class="NormalTextBox" style="width: 252px" colspan="3"><asp:dropdownlist id="DisplayModeDropDown" Runat="server"/></td>
	    </tr>
    </table>
</div>

<div id="TemplateSettings" class="normal">
    <dnn:sectionhead ID="TemplateSettingsSectionHead" CssClass="Head" runat="server" Text="Templates" Section="TemplateTable" ResourceKey="TemplateSettingsSectionHead" IsExpanded="True" />
    <div id="TemplateTable" runat="server" class="TemplateTable">
        <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
            <tr>
                <td class="SubHead">
                    <dnn:label ID="HeaderLabel" runat="server" ControlName="HeaderDropdownlist" Text="Select an Header Template:"
                        ResourceKey="HeaderLabel" />
                </td>
                <td class="NormalTextBox" style="width: 252px" colspan="3">
                    <asp:DropDownList ID="HeaderDropdownlist" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <dnn:label ID="ItemLabel" runat="server" ControlName="ItemDropdownlist" Text="Select an Item Template:"
                        ResourceKey="ItemLabel" />
                </td>
                <td class="NormalTextBox" style="width: 252px" colspan="3">
                    <asp:DropDownList ID="ItemDropdownlist" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <dnn:label ID="FooterLabel" runat="server" ControlName="FooterDropdownlist" Text="Select an Footer Template:"
                        ResourceKey="FooterLabel" />
                </td>
                <td class="NormalTextBox" style="width: 252px" colspan="3">
                    <asp:DropDownList ID="FooterDropdownlist" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <dnn:label ID="DetailLabel" runat="server" ControlName="DetailDropdownlist" Text="Select an Detail Template:"
                        ResourceKey="DetailLabel" />
                </td>
                <td class="NormalTextBox" style="width: 252px" colspan="3">
                    <asp:DropDownList ID="DetailDropdownlist" runat="server" />
                </td>
            </tr>
             <tr><td>&nbsp;</td></tr>
             <tr>
                <td class="SubHead">
                    <dnn:label ID="PagingLabel" runat="server" ControlName="RadNumericRecordsPerPage" Text="Enter number of records to display per page:"
                        ResourceKey="PagingLabel" />
                </td>
                <td class="NormalTextBox" style="width: 252px" colspan="3">
                    <telerik:RadNumericTextBox ID="RadNumericRecordsPerPage" runat="server" 
                        MaxLength="3" MaxValue="100" MinValue="0" 
                        ShowSpinButtons="True" Value="0" Width="50px">
<NumberFormat AllowRounding="True" KeepNotRoundedValue="False" DecimalDigits="0"></NumberFormat>
                    </telerik:RadNumericTextBox>
                </td>
            </tr>
        </table>
    </div>
</div>

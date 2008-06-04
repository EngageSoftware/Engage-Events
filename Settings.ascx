<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Settings" Codebehind="Settings.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<style type="text/css">
    @import url(<%=Engage.Dnn.Events.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Events.ModuleBase.DesktopModuleFolderName %>Module.css);
    .dvUpdateBtns { DISPLAY: none }
</style>
<br />
<div style="text-align:left">
    <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
        <tr>
            <td class="SubHead"><dnn:label id="lblChooseDisplayType" resourcekey="lblChooseDisplayType" runat="server" /></td>
            <td class="NormalTextBox"><asp:dropdownlist id="ddListingDisplay" Runat="server"></asp:dropdownlist></td>
        </tr>
        <tr>
            <td class="SubHead"><dnn:label id="lblUnsubscribeUrl" resourcekey="lblUnsubscribeUrl" runat="server" /></td>
            <td class="NormalTextBox"><asp:TextBox id="txtUnsubscribeUrl" Runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="SubHead"><dnn:label id="lblPrivacyPolicyUrl" resourcekey="lblPrivacyPolicyUrl" runat="server" /></td>
            <td class="NormalTextBox"><asp:TextBox id="txtPrivacyPolicyUrl" Runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="SubHead"><dnn:label id="lblOpenLinkUrl" resourcekey="lblOpenLinkUrl" runat="server" /></td>
            <td class="NormalTextBox"><asp:TextBox id="txtOpenLinkUrl" Runat="server"></asp:TextBox></td>
        </tr>
    </table>
</div>
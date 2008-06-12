<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Settings"
    CodeBehind="Settings.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<style type="text/css">
    @import url(<%=Engage.Dnn.Events.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Events.ModuleBase.DesktopModuleFolderName %>Module.css);
    .dvUpdateBtns
    {
        display: none;
    }
</style>
<br />
<asp:UpdatePanel ID="upnlSettings" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="text-align: left" class="SettingsTable">
            <dnn:label ID="lblChooseDisplayType" ResourceKey="lblChooseDisplayType" runat="server"
                CssClass="Normal" />
            <asp:DropDownList ID="ddlChooseDisplayType" CssClass="NormalTextBox" runat="server"
                AutoPostBack="True" OnSelectedIndexChanged="ddlChooseDisplayType_SelectedIndexChanged">
            </asp:DropDownList>
            <div id="dvDisplay" runat="server">
                <br />
                <asp:PlaceHolder ID="phControls" runat="server"></asp:PlaceHolder>
            </div>
            <br />
          <%--  <div visible="false">
                <table id="tblEmailSettings" cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
                    <tr>
                        <td class="Normal">
                            <dnn:label ID="lblUnsubscribeUrl" ResourceKey="lblUnsubscribeUrl" runat="server" />
                        </td>
                        <td class="NormalTextBox">
                            <asp:TextBox ID="txtUnsubscribeUrl" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal">
                            <dnn:label ID="lblPrivacyPolicyUrl" ResourceKey="lblPrivacyPolicyUrl" runat="server" />
                        </td>
                        <td class="NormalTextBox">
                            <asp:TextBox ID="txtPrivacyPolicyUrl" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal">
                            <dnn:label ID="lblOpenLinkUrl" ResourceKey="lblOpenLinkUrl" runat="server" />
                        </td>
                        <td class="NormalTextBox">
                            <asp:TextBox ID="txtOpenLinkUrl" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>--%>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

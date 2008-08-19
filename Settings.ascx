<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Settings" CodeBehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<style type="text/css">
    @import url(<%=Engage.Dnn.Framework.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Framework.ModuleBase.DesktopModuleFolderName %>Module.css);
</style>
<br />
<asp:UpdatePanel ID="SettingsUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="text-align: left" class="SettingsTable">
            <dnn:label ID="LabelChooseDisplayType" ResourceKey="LabelChooseDisplayType" runat="server" CssClass="Normal" />
            <asp:DropDownList ID="DropDownChooseDisplay" CssClass="NormalTextBox" runat="server" AutoPostBack="True" />
            <div id="DisplayDiv" runat="server">
                <br />
                <asp:PlaceHolder ID="phControls" runat="server"></asp:PlaceHolder>
            </div>
            <br />
            <table id="tblEmailSettings" cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
                <tr>
                    <td class="Normal">
                        <dnn:label ID="FeaturedEventLabel" ResourceKey="FeaturedEventLabel" runat="server" CssClass="SubHead" />
                    </td>
                    <td class="Normal">
                        <asp:CheckBox ID="FeaturedCheckBox" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
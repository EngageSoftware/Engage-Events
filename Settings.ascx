<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Settings" CodeBehind="Settings.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<style type="text/css">
    @import url(<%=Engage.Dnn.Framework.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Framework.ModuleBase.DesktopModuleFolderName %>Module.css);
</style>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="EventsSetting">
            <dnn:label ID="LabelChooseDisplayType" ResourceKey="LabelChooseDisplayType" runat="server" CssClass="SubHead" ControlName="DropDownChooseDisplay" />
            <asp:DropDownList ID="DropDownChooseDisplay" CssClass="NormalTextBox" runat="server" AutoPostBack="True" />
        </div>
        
        <asp:PlaceHolder ID="ControlsPlaceholder" runat="server"/>
        
        <div class="EventsSetting">
            <dnn:label ID="FeaturedEventLabel" ResourceKey="FeaturedEventLabel" runat="server" CssClass="SubHead" ControlName="FeaturedCheckBox" />
            <asp:CheckBox ID="FeaturedCheckBox" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<%@ Import Namespace="System.Globalization"%>
<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Settings" CodeBehind="Settings.ascx.cs" %>
<%@ Import Namespace="DotNetNuke.Entities.Tabs"%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<style type="text/css">
    @import url(<%=Engage.Dnn.Framework.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Framework.Utility.GetDesktopModuleFolderName(Engage.Dnn.Events.Utility.DesktopModuleName) %>Module.css);
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
        <div class="EventsSetting">
            <dnn:label ResourceKey="DetailsDisplayModuleLabel" runat="server" CssClass="SubHead" />
            <asp:GridView ID="DetailsDisplayModuleGrid" runat="server" GridLines="None" AutoGenerateColumns="false" CssClass="Normal" UseAccessibleHeader="true">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <asp:RadioButton ID="DetailsDisplayModuleRadioButton" runat="server" CssClass="Normal" AutoPostBack="true" OnCheckedChanged="DetailsDisplayModuleRadioButton_CheckedChanged"/>
                            <asp:HiddenField ID="TabModuleIdHiddenField" runat="server" Value='<%#((int)Eval("TabModuleID")).ToString(CultureInfo.InvariantCulture) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Page Name">
                        <ItemTemplate>
                            <%#new TabController().GetTab((int)this.Eval("TabID"), this.PortalId, false).TabName %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Tab ID" DataField="TabID"/>
                    <asp:BoundField HeaderText="Module Title" DataField="ModuleName"/>
                    <asp:BoundField HeaderText="Module ID" DataField="ModuleID"/>
                </Columns>
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#eeeeee" />
                <RowStyle BackColor="#f8f8f8" ForeColor="Black" />
            </asp:GridView>
            <asp:CustomValidator ID="DetailsDisplayModuleValidator" runat="server" CssClass="Normal" ResourceKey="DetailsDisplayModuleValidator" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
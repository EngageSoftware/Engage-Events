<%@ Import Namespace="System.Globalization"%>
<%@ Import Namespace="DotNetNuke.Entities.Tabs"%>
<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Settings" CodeBehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<style type="text/css">
    @import url(<%=Engage.Dnn.Framework.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Framework.Utility.GetDesktopModuleFolderName(Engage.Dnn.Events.Utility.DesktopModuleName) %>Module.css);
</style>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>        
        <asp:PlaceHolder ID="ControlsPlaceholder" runat="server"/>
        
        <div class="EventsSetting">
            <dnn:label ID="FeaturedEventLabel" ResourceKey="FeaturedEventLabel" runat="server" CssClass="SubHead" ControlName="FeaturedCheckBox" />
            <asp:CheckBox ID="FeaturedCheckBox" runat="server" />
        </div>
        <div class="EventsSetting">
            <dnn:label ResourceKey="CategoriesLabel" runat="server" CssClass="SubHead" />
            <asp:CheckBox ID="AllCategoriesCheckBox" runat="server" ResourceKey="All Categories" AutoPostBack="true" />
            <asp:CheckBoxList ID="CategoriesCheckBoxList" runat="server" />
            <asp:CustomValidator ID="CategoriesListValidator" runat="server" CssClass="NormalRed" ResourceKey="CategoriesListValidator" Display="None" ForeColor="" />
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
            <asp:CustomValidator ID="DetailsDisplayModuleValidator" runat="server" CssClass="NormalRed" ResourceKey="DetailsDisplayModuleValidator" Display="None" ForeColor="" />
        </div>
        <asp:ValidationSummary runat="server" ShowMessageBox="false" ShowSummary="true" CssClass="NormalRed" />
    </ContentTemplate>
</asp:UpdatePanel>
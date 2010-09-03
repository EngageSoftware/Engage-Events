<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.ManageCategories" Codebehind="ManageCategories.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Controls" Assembly="Engage.Framework" %>

<engage:ModuleMessage runat="server" ID="SuccessModuleMessage" MessageType="Success" CssClass="CategorySaveSuccessMessage"/>

<telerik:RadGrid ID="CategoriesGrid" runat="server" AutoGenerateColumns="false" AllowMultiRowEdit="true" ValidationSettings-ValidationGroup="ManageCategories">
    <MasterTableView DataKeyNames="Id" EditMode="InPlace" CommandItemDisplay="Top">
        <Columns>
            <telerik:GridEditCommandColumn UniqueName="EditButtons" ItemStyle-CssClass="buttons-col" />
            <telerik:GridTemplateColumn UniqueName="Name" ItemStyle-CssClass="name-col">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# string.IsNullOrEmpty((string)Eval("Name")) ? this.GetDefaultCategoryName() : Eval("Name") %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>' MaxLength="250" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="NameTextBox" ValidationGroup="ManageCategories" 
                        ResourceKey="NameRequired" ForeColor="" CssClass="NormalRed" Display="None" />
                    <asp:CustomValidator runat="server" ControlToValidate="NameTextBox" ValidationGroup="ManageCategories"
                        ResourceKey="NameUnique" ForeColor="" CssClass="NormalRed" Display="None" OnServerValidate="UniqueNameValidator_ServerValidate" />
                </EditItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridButtonColumn UniqueName="Delete" ItemStyle-CssClass="delete-col" CommandName="Delete" />
        </Columns>
    </MasterTableView>
</telerik:RadGrid>

<engage:ValidationSummary runat="server" ValidationGroup="ManageCategories" />
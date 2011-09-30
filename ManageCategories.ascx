<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.ManageCategories" Codebehind="ManageCategories.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Controls" Assembly="Engage.Framework" %>

<engage:ModuleMessage runat="server" ID="SuccessModuleMessage" MessageType="Success" CssClass="CategorySaveSuccessMessage"/>

<telerik:RadGrid ID="CategoriesGrid" runat="server" AutoGenerateColumns="false" AllowMultiRowEdit="true" ValidationSettings-ValidationGroup="ManageCategories" 
    CssClass="ManageCategoriesGrid">
    <MasterTableView HierarchyDefaultExpanded="true" DataKeyNames="Id, ParentId" EditMode="InPlace" CommandItemDisplay="Top">
        <Columns>
            <telerik:GridEditCommandColumn UniqueName="EditButtons" ItemStyle-CssClass="buttons-col" EditText="Edit" CancelText="Cancel" UpdateText="Update" />
            <telerik:GridTemplateColumn UniqueName="Name" ItemStyle-CssClass="name-col">
                <ItemTemplate>
                    <asp:PlaceHolder runat="server" ID="ExpandCollapseButtonPlaceHolder"/>
                    <asp:Label ID="NameLabel" runat="server" Text='<%# string.IsNullOrEmpty((string)Eval("Name")) ? this.GetDefaultCategoryName() : Eval("Name") %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>' MaxLength="250" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="NameTextBox" ValidationGroup="ManageCategories" 
                        ResourceKey="NameRequired" ForeColor="" CssClass="NormalRed" Display="None" />
                    <asp:CustomValidator runat="server" ControlToValidate="NameTextBox" ValidationGroup="ManageCategories"
                        ResourceKey="NameUnique" ForeColor="" CssClass="NormalRed" Display="None" OnServerValidate="UniqueNameValidator_ServerValidate" />
                </EditItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="ParentId" ItemStyle-CssClass="parent-col">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# this.FindCategoryName(Eval("ParentId") as int?) %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <telerik:RadComboBox ID="ParentCategoriesComboBox" runat="server" DataTextField="Name" DataValueField="Id">
                        <%--<ItemTemplate>
                            <telerik:RadTreeView runat="server" ID="ParentCategoriesTreeView" DataTextField="Name" DataValueField="Id" DataFieldParentID="ParentId" DataFieldID="Id"/>
                        </ItemTemplate>--%>
                    </telerik:RadComboBox>
                </EditItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="Color" ItemStyle-CssClass="color-col">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# this.GetColorName((string)Eval("Color")) %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <telerik:RadComboBox ID="ColorComboBox" runat="server" SelectedValue='<%#Bind("Color") %>' ShowDropDownOnTextboxClick="true" MarkFirstMatch="true">
                        <Items>
                            <telerik:RadComboBoxItem Text='<%#Localize("NoColor") %>' Value="" />
                            <telerik:RadComboBoxItem Text='<%#Localize("DarkBlue") %>' Value="DarkBlue" />
                            <telerik:RadComboBoxItem Text='<%#Localize("Blue") %>' Value="Blue" />
                            <telerik:RadComboBoxItem Text='<%#Localize("DarkGreen") %>' Value="DarkGreen" />
                            <telerik:RadComboBoxItem Text='<%#Localize("Green") %>' Value="Green" />
                            <telerik:RadComboBoxItem Text='<%#Localize("DarkRed") %>' Value="DarkRed" />
                            <telerik:RadComboBoxItem Text='<%#Localize("Red") %>' Value="Red" />
                            <telerik:RadComboBoxItem Text='<%#Localize("Orange") %>' Value="Orange" />
                            <telerik:RadComboBoxItem Text='<%#Localize("Pink") %>' Value="Pink" />
                            <telerik:RadComboBoxItem Text='<%#Localize("Violet") %>' Value="Violet" />
                            <telerik:RadComboBoxItem Text='<%#Localize("Yellow") %>' Value="Yellow" />
                        </Items>
                    </telerik:RadComboBox>
                </EditItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridButtonColumn UniqueName="Delete" ItemStyle-CssClass="delete-col" CommandName="Delete" Text="Delete" ConfirmText="Are you sure that you want to permanently delete this category?"/>
        </Columns>
        <SelfHierarchySettings ParentKeyName="ParentId" KeyName="Id"/>
    </MasterTableView>
    <ClientSettings AllowExpandCollapse="true"/>
</telerik:RadGrid>
<engage:ValidationSummary runat="server" ValidationGroup="ManageCategories" />
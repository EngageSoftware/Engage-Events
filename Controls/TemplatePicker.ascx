<%@ Control Language="c#" Codebehind="TemplatePicker.ascx.cs" Inherits="Engage.Dnn.Events.TemplatePicker" AutoEventWireup="false" %>
<div class="Normal">
    <asp:Label ResourceKey="Template" runat="server" EnableViewState="false" />
    <asp:DropDownList ID="TemplatesDropDownList" runat="server" AutoPostBack="true" />
    <fieldset id="TemplateDescriptionPanel" runat="server">
        <legend><asp:Label runat="server" resourcekey="Description" /></legend>
        <asp:Label ID="TemplateDescriptionLabel" runat="server" />
    </fieldset>
    <asp:Image ID="TemplatePreviewImage" runat="server" />

    <div>
        <asp:Label ID="SettingsExplanationLabel" runat="server" CssClass="SubSubHead" ResourceKey="SettingsExplanation" />
        <asp:GridView ID="SettingsGrid" runat="server" AutoGenerateColumns="false" CssClass="Normal DataGrid_Container" GridLines="None">
            <AlternatingRowStyle CssClass="DataGrid_AlternatingItem" />
            <HeaderStyle CssClass="DataGrid_Header" />
            <RowStyle CssClass="DataGrid_Item" />
            <Columns>
                <asp:BoundField HeaderText="Key" DataField="Key" />
                <asp:BoundField HeaderText="Value" />
                <asp:BoundField HeaderText="OriginalValue" />
            </Columns>
        </asp:GridView>
    </div>

    <asp:Label ID="ManifestValidationErrorsLabel" ResourceKey="ManifestValidation" runat="server" CssClass="NormalRed" Visible="false" EnableViewState="false"/>
</div>
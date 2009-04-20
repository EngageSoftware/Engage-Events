<%@ Control Language="c#" Codebehind="TemplatePicker.ascx.cs" Inherits="Engage.Dnn.Events.TemplatePicker" AutoEventWireup="false" %>
<div class="events-choose-display-wrap Normal">
	<div class="cd-type">
		<asp:Label ResourceKey="TemplateLabel.Text" runat="server" EnableViewState="false" />
   		<asp:DropDownList ID="TemplatesDropDownList" runat="server" AutoPostBack="true" />
	</div>	 
	<div class="cd-list cd-single-item">
		<div class="cd-single-item-info cd-list-info">
            <div class="cd-list-description cd-single-item-description">
                <fieldset id="TemplateDescriptionPanel" runat="server">
                    <legend><asp:Label runat="server" resourcekey="Description" /></legend>
                    <asp:Label ID="TemplateTitleLabel" runat="server" CssClass="cd-template-title" />
                    <asp:Label ID="TemplateDescriptionLabel" runat="server" />
                 </fieldset>
            </div>
        	<div class="cd-list-preview cd-single-item-preview">
				<asp:Panel ID="TemplatePreviewImagePanel" runat="server" CssClass="ts-preview">
                    <asp:Label runat="server" resourcekey="Preview" />
                    <asp:Image ID="TemplatePreviewImage" runat="server" />
				</asp:Panel>
			</div>
        </div>   
		<div class="settings-table">
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
		<div class="ts-buttons">
    		<asp:Label ID="ManifestValidationErrorsLabel" runat="server" CssClass="NormalRed"/>
		</div>
	</div>
</div>
<div style="clear:both"></div>
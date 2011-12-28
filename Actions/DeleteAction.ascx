<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.DeleteAction" CodeBehind="DeleteAction.ascx.cs" %>

<asp:Button ID="DeleteEventButton"
            runat="server"
            UseSubmitBehavior="false" 
            Visible="<%# this.IsEditable || this.PermissionsService.CanManageEvents %>"
            CssClass="<%# this.CssClass %>"
            ResourceKey="DeleteEventButton"
            data-eng-events-confirm='<%# HttpUtility.HtmlAttributeEncode(Localize("ConfirmDelete.Text")) %>' />
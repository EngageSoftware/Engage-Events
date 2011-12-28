<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.CancelAction" CodeBehind="CancelAction.ascx.cs" %>

<asp:Button ID="CancelButton" 
            runat="server" 
            UseSubmitBehavior="false" 
            Visible="<%# this.IsEditable || this.PermissionsService.CanManageEvents %>"
            CssClass="<%# this.CssClass %>"
            ResourceKey='<%# this.Canceled ? "UnCancel" : "Cancel" %>'
            data-eng-events-confirm='<%# HttpUtility.HtmlAttributeEncode(Localize(this.Canceled ? "ConfirmUnCancel" : "ConfirmCancel")) %>' />
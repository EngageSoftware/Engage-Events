<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.CancelAction" CodeBehind="CancelAction.ascx.cs" %>
<asp:Button ID="CancelButton" UseSubmitBehavior="True" CssClass="Normal" runat="server" />
<script type="text/javascript">
    jQuery(function ($) {
        var <%= CancelButton.ClientID%>_onclick = $("#<%= CancelButton.ClientID%>").attr('onclick');
        $("#<%= CancelButton.ClientID%>").removeAttr('onclick').click(function (e) {
            if (confirm('<%= Localization.GetString(this.Canceled ? "ConfirmUnCancel" : "ConfirmCancel", this.LocalResourceFile) %>')) {
                if (<%= CancelButton.ClientID%>_onclick != undefined)
                {
                    eval( <%= CancelButton.ClientID%>_onclick );
                }
                
                return true;
            }

            return false;
        });
    });
</script>
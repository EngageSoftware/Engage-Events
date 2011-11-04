<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.DeleteAction" CodeBehind="DeleteAction.ascx.cs" %>
<asp:Button ID="DeleteEventButton" UseSubmitBehavior="True" CssClass="Normal" runat="server" ResourceKey="DeleteEventButton" />
<script type="text/javascript">
    jQuery(function ($) {
        var <%= DeleteEventButton.ClientID%>_onclick = $("#<%= DeleteEventButton.ClientID%>").attr('onclick');
        $("#<%= DeleteEventButton.ClientID%>").removeAttr('onclick').click(function () {
            if (confirm('<%= Localization.GetString("ConfirmDelete.Text", this.LocalResourceFile) %>')) {
                if (<%= DeleteEventButton.ClientID%>_onclick != undefined)
                {
                    eval( <%= DeleteEventButton.ClientID%>_onclick );
                }
                
                return true;
            }

            return false;
        });
    });
</script>
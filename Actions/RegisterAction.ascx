<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.RegisterAction" CodeBehind="RegisterAction.ascx.cs" %>

<asp:Button ID="RegisterButton" runat="server"
            CssClass='<%# this.CssClass + " RegisterButton" %>'
            UseSubmitBehavior="False"
            ResourceKey='<%# IsLoggedIn ? "RegisterButton.Text" : "LoginToRegisterButton.Text" %>' />
<asp:HyperLink ID="PopupTriggerLink" runat="server" 
               CssClass="PopupTriggerLink" 
               Target="_blank" />
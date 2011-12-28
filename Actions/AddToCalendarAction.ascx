<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.AddToCalendarAction" CodeBehind="AddToCalendarAction.ascx.cs" %>
<asp:Button ID="AddToCalendarButton" runat="server" 
            ResourceKey="AddToCalendarButton"
            Visible="<%# IsLoggedIn %>"
            CssClass="<%# this.CssClass %>" />
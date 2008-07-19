<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.RsvpDisplay" CodeBehind="RsvpDisplay.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<div class="rsvpDisplay ResponseRow">
    <div class="rsvpEventDisplay">
    	<h3><asp:Label ID="TitleLabel" runat="server" /></h3>
	    <p><asp:Label ID="DateLabel" CssClass="Normal" runat="server" /></p>
    </div>
    <div class="rsvpEventStats">
        <p class="rsvpAtt"><asp:HyperLink ID="AttendingLink" runat="server" /></p>
        <p class="rsvpNotAtt"><asp:HyperLink ID="NotAttendingLink" runat="server" /></p>
        <%--<asp:HyperLink runat="server" NavigateUrl='<%# GetDetailUrl(Eval( "EventId"), "NoResponse", Eval("NoResponse")) %>' Text='<%# Eval("NoResponse") %>' />--%>
    </div>
</div>
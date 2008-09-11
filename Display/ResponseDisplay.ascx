<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.ResponseDisplay" CodeBehind="ResponseDisplay.ascx.cs" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<div class="ResponseDisplay ResponseRow">
    <div class="ResponseEventDisplay">
    	<h3><asp:Label ID="TitleLabel" runat="server" /></h3>
	    <p><asp:Label ID="DateLabel" CssClass="Normal" runat="server" /></p>
    </div>
    <div class="ResponseEventStats">
        <p class="ResponseAtt"><asp:HyperLink ID="AttendingLink" runat="server" /></p>
        <p class="ResponseNotAtt"><asp:HyperLink ID="NotAttendingLink" runat="server" /></p>
        <%--<asp:HyperLink runat="server" NavigateUrl='<%# GetDetailUrl(Eval( "EventId"), "NoResponse", Eval("NoResponse")) %>' Text='<%# Eval("NoResponse") %>' />--%>
    </div>
</div>
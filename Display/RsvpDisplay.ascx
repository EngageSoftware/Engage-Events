<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.RsvpDisplay" CodeBehind="RsvpDisplay.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>

<div class="ResponseRow">
    <h3 class="SubHead"><asp:Label ID="TitleLabel" runat="server" /></h3>
    <div class="EventStats">
        <asp:HyperLink ID="AttendingLink" runat="server" />
        <asp:HyperLink ID="NotAttendingLink" runat="server" />
        <%--<asp:HyperLink runat="server" NavigateUrl='<%# GetDetailUrl(Eval( "EventId"), "NoResponse", Eval("NoResponse")) %>' Text='<%# Eval("NoResponse") %>' />--%>
    </div>
    <div class="EventDate">
        <asp:Label ID="DateLabel" runat="server" />
    </div>
</div>

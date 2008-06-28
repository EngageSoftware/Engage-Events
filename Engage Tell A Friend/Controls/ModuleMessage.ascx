<%@ Import Namespace="DotNetNuke.Services.Localization"%>
<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ModuleMessage.ascx.cs" Inherits="Engage.Dnn.TellAFriend.ModuleMessage" EnableViewState="false" %>
<div class="<%=MessageStyle %>Message ModuleMessage">
        <div class="<%=MessageStyle %>Top mmTop"></div>
        <div class="<%=MessageStyle %>Body mmBody">
            <span class="<%=MessageStyle %>Icon mmIcon"><%=GetLocalizedText(MessageStyle, this)%></span>
            <div class="mmText">
                <h3 class="SubHead"><asp:Label ID="titleLabel" runat="server" /></h3>
                <p class="Normal"><asp:Label ID="messageLabel" runat="server" /></p>
            </div>
        </div>
        <div class="<%=MessageStyle %>Bt mmBt"></div>
</div>
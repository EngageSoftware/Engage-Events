<%@ Control Language="c#" Codebehind="Register.ascx.cs" Inherits="Engage.Dnn.Events.Register" AutoEventWireup="false" %>

<div class="Normal LoginToRegister">
    <h3><asp:Label runat="server" resourcekey="Title" CssClass="Head" /></h3>
    <p><asp:Label runat="server" resourcekey="Instructions"/></p>
    
    <div class="LoginButtons">
        <p><asp:HyperLink ID="LogOnLink" runat="server" CssClass="CommandButton" ResourceKey="LoginLink" /></p>
        <p><asp:HyperLink ID="RegisterLink" runat="server" CssClass="CommandButton" ResourceKey="RegisterLink" /></p>
    </div>
</div>

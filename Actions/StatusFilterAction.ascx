<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.StatusFilterAction" CodeBehind="StatusFilterAction.ascx.cs" %>

<asp:RadioButtonList ID="StatusRadioButtonList" runat="server" AutoPostBack="True" CssClass="<%# this.CssClass %>" RepeatDirection="Horizontal">
    <asp:ListItem Selected="True" Value="Active" ResourceKey="ActiveListItem"/>
    <asp:ListItem Value="All" ResourceKey="AllListItem"/>
</asp:RadioButtonList>
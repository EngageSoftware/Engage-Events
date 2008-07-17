<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.SortStatusAction" CodeBehind="SortStatusAction.ascx.cs" %>
<asp:RadioButtonList ID="RadioButtonListStatusSort" runat="server" AutoPostBack="True" CssClass="Normal" RepeatDirection="Horizontal">
    <asp:ListItem Selected="True" Value="Active" ResourceKey="ActiveListItem"/>
    <asp:ListItem Value="All" ResourceKey="AllListItem"/>
</asp:RadioButtonList>
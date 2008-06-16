<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RecurrenceSelector.ascx.cs" Inherits="Engage.Dnn.Events.Controls.RecurrenceSelector" %>
<asp:RadioButtonList ID="RecurrenceRadio" runat="server" AutoPostBack="True" CssClass="Normal">
    <asp:ListItem Value="0" Selected="true">Daily</asp:ListItem>
    <asp:ListItem Value="1">Weekly</asp:ListItem>
    <asp:ListItem Value="2">Monthly</asp:ListItem>
    <asp:ListItem Value="3">Yearly</asp:ListItem>
</asp:RadioButtonList>

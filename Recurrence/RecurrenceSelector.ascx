<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RecurrenceSelector.ascx.cs" Inherits="Engage.Dnn.Events.Recurrence.RecurrenceSelector" %>

<asp:RadioButtonList ID="RecurrenceOptionsList" runat="server" AutoPostBack="True" CssClass="Normal">
    <asp:ListItem Value="0" Selected="true" ResourceKey="Daily" />
    <asp:ListItem Value="1" ResourceKey="Weekly" />
    <asp:ListItem Value="2" ResourceKey="Monthly" />
    <asp:ListItem Value="3" ResourceKey="Yearly" />
</asp:RadioButtonList>
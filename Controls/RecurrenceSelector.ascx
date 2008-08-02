<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RecurrenceSelector.ascx.cs" Inherits="Engage.Dnn.Events.Controls.RecurrenceSelector" %>

<asp:RadioButtonList ID="RecurrenceOptionsList" runat="server" AutoPostBack="True">
    <asp:ListItem Value="0" Selected="true" ResourceKey="Daily" />
    <asp:ListItem Value="1" ResourceKey="Weekly" />
    <asp:ListItem Value="2" ResourceKey="Monthly" />
    <asp:ListItem Value="3" ResourceKey="Yearly" />
</asp:RadioButtonList>
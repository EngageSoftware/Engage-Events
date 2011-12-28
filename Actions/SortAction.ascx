<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.SortAction" CodeBehind="SortAction.ascx.cs" %>

<asp:RadioButtonList ID="SortRadioButtonList" runat="server" AutoPostBack="True" CssClass="<%# this.CssClass %>" RepeatDirection="Horizontal">
    <asp:ListItem Selected="True" Value="EventStart" ResourceKey="DateListItem" />
    <asp:ListItem Value="Title" ResourceKey="TitleListItem" />
</asp:RadioButtonList>
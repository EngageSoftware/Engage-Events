<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="DailyRecurrence.ascx.cs" Inherits="Engage.Dnn.Events.Controls.DailyRecurrence" %>
&nbsp;
<asp:Panel ID="dailyPanel" runat="server" Height="129px"
    Style="left: 8px;" Width="424px">
    <asp:RadioButtonList ID="dailyRadio" runat="server" AutoPostBack="True" Font-Names="Verdana"
        Font-Size="X-Small" Height="64px" Style="left: 0px; top: 40px" Width="144px">
        <asp:ListItem Value="0">Every</asp:ListItem>
        <asp:ListItem Value="1">Every weekday</asp:ListItem>
    </asp:RadioButtonList>
    <asp:TextBox ID="dailyEveryNthDayEdit" runat="server"></asp:TextBox>&nbsp;
    <asp:Label ID="Label3" runat="server">day(s)</asp:Label></asp:Panel>

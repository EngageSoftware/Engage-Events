<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="WeeklyRecurrence.ascx.cs" Inherits="Engage.Dnn.Events.Controls.WeeklyRecurrence" %>

<asp:Panel ID="weeklyPanel" runat="server" Height="129px" Style="left: 8px; top: 8px" Width="424px">
    <asp:TextBox ID="weeklyEveryNthWeekEdit" runat="server" Font-Names="Verdana" Font-Size="X-Small"
        Style="left: 72px; top: 8px" Width="56px"></asp:TextBox>
    <asp:Label ID="WeeklyWeeksLabel" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="
        left: 136px; top: 8px">week(s)</asp:Label>
    <asp:Label ID="WeeklyEveryLabel" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="
        left: 16px; top: 8px">Every</asp:Label>
    <asp:CheckBoxList ID="weekDaysBtns" runat="server" Font-Names="Verdana" Font-Size="X-Small"
        RepeatColumns="4" Style="left: 8px; top: 64px">
        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
        <asp:ListItem Value="Monday">Monday</asp:ListItem>
        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
        <asp:ListItem Value="Friday">Friday</asp:ListItem>
        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
    </asp:CheckBoxList>
</asp:Panel>

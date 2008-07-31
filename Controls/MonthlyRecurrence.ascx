<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="MonthlyRecurrence.ascx.cs" Inherits="Engage.Dnn.Events.Controls.MonthlyRecurrence" %>
<asp:Panel ID="monthlyPanel" runat="server" Height="152px" Style="left: 8px; top: 8px" Width="424px">
    <asp:Label ID="MonthsLabel" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 136px; top: 8px">month(s)</asp:Label>
    <asp:RadioButtonList ID="monthlyRadio" runat="server" AutoPostBack="True" Font-Names="Verdana" Font-Size="X-Small" Height="96px" Style="left: 8px; top: 48px" Width="56px">
        <asp:ListItem Value="0">Day</asp:ListItem>
        <asp:ListItem Value="1">The</asp:ListItem>
        <asp:ListItem Value="2">The</asp:ListItem>
    </asp:RadioButtonList>
    <asp:TextBox ID="monthlyEveryNthMonthEdit" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 72px; top: 8px" Width="56px"></asp:TextBox>
    <asp:DropDownList ID="monthlyNthOccurrenceCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 72px; top: 86px" Width="96px">
    </asp:DropDownList>
    <asp:DropDownList ID="monthlyDayOfWeekCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 176px; top: 86px" Width="96px">
    </asp:DropDownList>
    <asp:DropDownList ID="monthlyDayOccurrenceCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 72px; top: 117px" Width="96px">
    </asp:DropDownList>
    <asp:DropDownList ID="monthlyWeekDayTypeCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 176px; top: 117px" Width="96px">
    </asp:DropDownList>
    <asp:DropDownList ID="monthlyDayCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 72px; top: 56px">
    </asp:DropDownList>
    <asp:Label ID="MonthlyEveryLabel" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 16px; top: 8px">Every</asp:Label>
</asp:Panel>

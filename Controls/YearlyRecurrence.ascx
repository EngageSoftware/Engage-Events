<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="YearlyRecurrence.ascx.cs" Inherits="Engage.Dnn.Events.Controls.YearlyRecurrence" %>
<asp:Panel ID="yearlyPanel" runat="server" Style="left: 8px; top: 2px" Width="425px">
    <asp:Label ID="YearsLabel" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 144px; top: 8px">year(s)</asp:Label>
    <asp:Label ID="EveryLabel" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 24px; top: 8px">Every</asp:Label>
    <asp:RadioButtonList ID="yearlyRadio" runat="server" AutoPostBack="True" Font-Names="Verdana" Font-Size="X-Small" Height="96px" Style="left: 16px; top: 48px" Width="56px">
        <asp:ListItem Value="0">Day</asp:ListItem>
        <asp:ListItem Value="1">The</asp:ListItem>
        <asp:ListItem Value="2">The</asp:ListItem>
    </asp:RadioButtonList>
    <asp:TextBox ID="yearlyEveryNthYearEdit" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 80px; top: 8px" Width="56px"></asp:TextBox>
    <asp:DropDownList ID="yearlyNthOccurrenceCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 80px; top: 86px" Width="96px">
    </asp:DropDownList>
    <asp:DropDownList ID="yearlyDayOfWeekCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 184px; top: 86px" Width="96px">
    </asp:DropDownList>
    <asp:DropDownList ID="yearlyDayOccurrenceCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 80px; top: 117px" Width="96px">
    </asp:DropDownList>
    <asp:DropDownList ID="yearlyWeekDayTypeCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 184px; top: 117px" Width="96px">
    </asp:DropDownList>
    <asp:DropDownList ID="yearlyDayMonthCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 80px; top: 55px" Width="96px">
    </asp:DropDownList>
    <asp:DropDownList ID="yearlyDayOfWeekMonthCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 312px; top: 86px" Width="96px">
    </asp:DropDownList>
    <asp:DropDownList ID="yearlyWeekDayMonthCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 312px; top: 117px" Width="96px">
    </asp:DropDownList>
    <asp:Label ID="OfLabel1" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 288px; top: 88px">of</asp:Label>
    <asp:Label ID="OfLabel2" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 288px; top: 120px">of</asp:Label>
    <asp:DropDownList ID="yearlyDayCombo" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 184px; top: 56px" Width="96px">
    </asp:DropDownList>
</asp:Panel>

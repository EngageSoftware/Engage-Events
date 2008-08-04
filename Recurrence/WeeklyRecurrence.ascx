<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="WeeklyRecurrence.ascx.cs" Inherits="Engage.Dnn.Events.Recurrence.WeeklyRecurrence" %>

<asp:Panel ID="WeeklyRecurrencePanel" runat="server">

    <!-- Step 1: Every N week(s) -->
    <div>
        <asp:Label runat="server" ResourceKey="Every" />
        <asp:TextBox ID="WeeklyRecurrenceTextBox" runat="server" />
        <asp:Label runat="server" ResourceKey="Weeks" />
    </div>
    
    <!-- Step 2: What day of the week? -->
    <div>
        <asp:CheckBoxList ID="WeekDaysList" runat="server">
            <asp:ListItem Value="Sunday" ResourceKey="Sunday" />
            <asp:ListItem Value="Monday" ResourceKey="Monday" />
            <asp:ListItem Value="Tuesday" ResourceKey="Tuesday" />
            <asp:ListItem Value="Wednesday" ResourceKey="Wednesday" />
            <asp:ListItem Value="Thursday" ResourceKey="Thursday" />
            <asp:ListItem Value="Friday" ResourceKey="Friday" />
            <asp:ListItem Value="Saturday" ResourceKey="Saturday" />
        </asp:CheckBoxList>
    </div>
    
</asp:Panel>
<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="DailyRecurrence.ascx.cs" Inherits="Engage.Dnn.Events.Recurrence.DailyRecurrence" %>

<asp:Panel ID="DailyRecurrencePanel" runat="server" style="width: 100%;">

    <div style="float: left; width: 20%;">
        <asp:RadioButtonList ID="DailyRecurrenceList" runat="server" AutoPostBack="True" CssClass="Normal">
            <asp:ListItem Value="0" ResourceKey="Every" />
            <asp:ListItem Value="1" ResourceKey="EveryWeekday" />
        </asp:RadioButtonList>
    </div>
    
    <div style="float: right; width: 80%;">
        <asp:TextBox ID="DailyRecurrenceTextBox" runat="server" CssClass="Normal" />
        <asp:Label runat="server" ResourceKey="Days" CssClass="Normal" />    
    </div>
    
</asp:Panel>

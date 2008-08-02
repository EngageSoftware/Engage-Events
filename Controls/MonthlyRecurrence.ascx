<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="MonthlyRecurrence.ascx.cs" Inherits="Engage.Dnn.Events.Controls.MonthlyRecurrence" %>

<asp:Panel ID="MonthlyRecurrencePanel" runat="server">

    <div>
        <asp:Label runat="server" ResourceKey="Every" />
        <asp:TextBox ID="MonthlyRecurrenceTextBox" runat="server" />
        <asp:Label runat="server" ResourceKey="Months" />
    </div>
    
    <div style="width: 100%;">
        <div style="width: 20%; float: left;">
            <asp:RadioButtonList ID="MonthlyRecurrenceList" runat="server" AutoPostBack="True">
                <asp:ListItem Value="0" ResourceKey="Day" />
                <asp:ListItem Value="1" ResourceKey="The" />
            </asp:RadioButtonList>
        </div>
        <div style="width: 80%; float: right;">
            <div>
                <asp:DropDownList ID="DayOfMonthList" runat="server" />
            </div>
            <div>
                <asp:DropDownList ID="DaySequenceList" runat="server" />
                <asp:DropDownList ID="DayOfWeekList" runat="server" />
            </div>
        </div>
    </div>
    
</asp:Panel>

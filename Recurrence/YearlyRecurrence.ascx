<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="YearlyRecurrence.ascx.cs" Inherits="Engage.Dnn.Events.Recurrence.YearlyRecurrence" %>

<asp:Panel ID="YearlyRecurrencePanel" class="YearlyRecurrencePanel" runat="server">

    <!-- Step 1 -->

    <!-- Every N year(s) -->
    <div class="YearlyRecurrenceOption">
        <asp:Label runat="server" ResourceKey="Every" />
        <asp:TextBox ID="YearTextBox" runat="server" />
        <asp:Label runat="server" ResourceKey="Years" />
    </div>
    
    <!-- Step 2 -->
    <div class="DefineMe" style="width: 100%;">
        <div class="DefineMe" style="float: left; width: 20%;">
            <!-- Option A: Specific Date (Every <Month> <Day #>) -->
            <!-- Option B: Relative Date (The <sequence option> <Day of Week> of <Month>) -->
            <asp:RadioButtonList ID="YearlyRecurrenceList" runat="server" AutoPostBack="True">
                <asp:ListItem Value="SpecificDate" ResourceKey="Every" />
                <asp:ListItem Value="RelativeDate" ResourceKey="The" />
            </asp:RadioButtonList>
        </div>
        <div class="DefineMe" style="float: right; width: 80%;">
            <div>
                <!-- Step 2, Option A-->
                <asp:DropDownList id="SpecificMonthList" runat="server" />
                <asp:TextBox ID="DayOfMonthTextBox" runat="server" />
            </div>
            <div>
                <!-- Step 2, Option B -->
                <asp:DropDownList id="DaySequenceList" runat="server" />
                <asp:DropDownList id="DayOfWeekList" runat="server" />
                <asp:Label runat="server" ResourceKey="Of" />
                <asp:DropDownList id="RelativeMonthList" runat="server" />
            </div>
        </div>
    </div>
    
</asp:Panel>
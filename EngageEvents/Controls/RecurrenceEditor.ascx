<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Controls.RecurrenceEditor" Codebehind="RecurrenceEditor.ascx.cs" %>
<%@ Register Src="../Controls/RecurrenceSelector.ascx" TagName="RecurrenceSelector" TagPrefix="uc1" %>

<link rel="stylesheet" type="text/css" href="../Module.css"/>

<div>
    <div id="recurrenceleftNav" class="recurrenceleftNav" style="float:left;height:120px;width:150px;text-align:left;border-right: thin groove; border-top: thin groove; border-left: thin groove; border-bottom: thin groove;" >
        <uc1:RecurrenceSelector id="RecurrenceSelector1" runat="server"></uc1:RecurrenceSelector>
    </div>
    <div id="recurrenceCenter" class="recurrenceCenter" style="float:right;width:800px;text-align:left;border-right: thin groove; border-top: thin groove; border-left: thin groove; border-bottom: thin groove;">
        <asp:PlaceHolder id="phRecurrencePattern" runat="Server" />
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:Label ID="Label4" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 16px; top: 152px">Range of recurrence:</asp:Label>
    <div id="rangeGroup" align="left" style="border-right: thin groove;
        border-top: thin groove; border-left: thin groove;
        width: 544px; border-bottom: thin groove; top: 176px; height: 114px;">
        <asp:Label ID="Label5" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="left: 16px; top: 16px">Start:</asp:Label>
        <asp:TextBox ID="startDate" runat="server" Font-Names="Verdana" Font-Size="X-Small"
            Style="left: 64px; top: 8px" Width="88px"></asp:TextBox>
        <asp:TextBox ID="startTime" runat="server" Font-Names="Verdana" Font-Size="X-Small"
            Style="left: 160px; top: 8px" Width="88px"></asp:TextBox>
        <asp:RadioButtonList ID="endRadio" runat="server" AutoPostBack="True" Font-Names="Verdana"
            Font-Size="X-Small" Height="96px" Style="left: 266px; 
            top: 5px" Width="112px">
            <asp:ListItem Value="0">No end</asp:ListItem>
            <asp:ListItem Value="2">End after:</asp:ListItem>
            <asp:ListItem Value="1">End by:</asp:ListItem>
        </asp:RadioButtonList>
        <asp:TextBox ID="endMaxOccurrencesEdit" runat="server" Font-Names="Verdana" Font-Size="X-Small"
            Style="left: 384px; top: 45px" Width="56px"></asp:TextBox>
        <asp:TextBox ID="endDate" runat="server" Font-Names="Verdana" Font-Size="X-Small"
            Style="left: 384px; top: 77px" Width="88px"></asp:TextBox>
        <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="
            left: 452px; top: 48px">occurrences</asp:Label>
    </div>
    &nbsp;
    <div id="Div2" style="border-right: thin groove; border-top: thin groove;
        left: 16px; border-left: thin groove; width: 544px; border-bottom: thin groove;
        top: 312px; height: 80px">
        <asp:Button ID="refreshBtn" runat="server" CausesValidation="False" Font-Names="Verdana"
            Font-Size="X-Small" Style="left: 440px; top: 8px"
            Text="Refresh" Width="89px" />
        <asp:TextBox ID="iCalendarText" runat="server" Height="60px" ReadOnly="True" Style="
            left: 8px; top: 8px" TextMode="MultiLine" Width="424px"></asp:TextBox>
    </div>
</div>
    <asp:Button ID="cancelBtn" runat="server" Font-Names="Verdana" Font-Size="X-Small"
        Style="left: 472px; top: 400px" Text="Cancel"
        Width="88px" /><asp:Button ID="okBtn" runat="server" Font-Names="Verdana" Font-Size="X-Small" Style="
        left: 376px; top: 400px" Text="OK" Width="89px" />

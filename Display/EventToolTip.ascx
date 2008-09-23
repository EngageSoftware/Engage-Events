<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventToolTip.ascx.cs" Inherits="Engage.Dnn.Events.Display.EventToolTip" %>

<%--<div style="margin:5px 5px 0px 5px;">
    <div style="border-bottom:solid 1px #ccc;margin-bottom:9px;font-size:11px;">Starting on: <asp:Label runat="server" ID="StartingOn"></asp:Label></div>
    <asp:TextBox runat="server" ID="FullText" TextMode="MultiLine" Width="100%" Rows="7" style="border:0;font-size:12px;background:transparent;"></asp:TextBox>
</div>--%>

<div class="EventToolTip">
    <div class="EventTitleToolTip"><asp:Label runat="server" ID="EventTitle"/></div>
    <div class="EventStartToolTip"><asp:Label runat="server" ID="EventDate"/></div>
    <div><asp:Literal runat="server" ID="EventOverview" /></div>
    <asp:Button ID="EditButton" runat="server" CssClass="CommandButton" ResourceKey="EditButton"/>
    <asp:Button ID="RegisterButton" runat="server" CssClass="CommandButton" ResourceKey="RegisterButton"/>
    <asp:Button ID="AddToCalendarButton" runat="server" CssClass="CommandButton" resourceKey="AddToCalendarButton"/>
</div>
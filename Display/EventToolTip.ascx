<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventToolTip.ascx.cs" Inherits="Engage.Dnn.Events.Display.EventToolTip" %>
<%@ Register TagPrefix="engage" TagName="RegisterAction" Src="../Actions/RegisterAction.ascx" %>

<div class="EventToolTip">
    <h2 class="EventTitleToolTip Head"><asp:Label runat="server" ID="EventTitle"/></h2>
    <p class="EventStartToolTip NormalBold"><asp:Label runat="server" ID="EventDate"/></p>
    <div class="event_description_tooltip Normal"><asp:Literal runat="server" id="EventOverview" /></div>
    <div class="tooltip_buttons">
    	<asp:Button ID="EditButton" runat="server" CssClass="Normal" ResourceKey="EditButton"/>
        <engage:RegisterAction ID="RegisterButton" runat="server"/>
        <asp:Button ID="AddToCalendarButton" runat="server" CssClass="Normal" resourceKey="AddToCalendarButton"/>
	</div>        
</div>
<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Rsvp" Codebehind="Rsvp.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Controls" Assembly="Engage.Utilityv3.0" %>

<asp:MultiView ID="RsvpMultiView" ActiveViewIndex="0" runat="server">

    <asp:View runat="server">
    
        <div class="Normal" id="Description" align="left">
            <p><asp:Label ResourceKey="titleLabel" CssClass="SubHead" runat="server"/></p>
            <p><asp:Label id="EventNameLabel" CssClass="SubSubHead" runat="server"/></p>
        </div>
        
        <div class="Normal" id="rbOptions" align="left">
            <asp:RadioButtonList ID="RsvpStatusRadioButtons" runat="server" CssClass="Normal"/>
            <asp:Button ID="SubmitButton" runat="server" Text="Submit" CssClass="CommandButton" />
        </div>
        
    </asp:View>
    
    <asp:View runat="server">
    
        <engage:ModuleMessage ID="ThankYouMessage" runat="server" MessageType="Success" TextResourceKey="ThankYou" />
        <asp:Button ID="AddToCalendarButton" runat="server" ResourceKey="addToCalendarButton" Enabled="false" CssClass="CommandButton" ValidationGroup="addToCalendar"/>
        
    </asp:View>
        
</asp:MultiView>
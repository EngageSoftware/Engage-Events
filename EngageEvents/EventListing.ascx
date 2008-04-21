<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventListing" Codebehind="EventListing.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
                
<div class="EventHeader">
        <h4 class="NormalBold">Events</h4>
</div>

<asp:Repeater runat="server" id="rpEventListing">
	<ItemTemplate>
        <asp:Label id = "lblId" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Id")  %>'></asp:Label>
        <div class="EventTitle">
		    <h2 class="Head"><%# DataBinder.Eval(Container.DataItem, "Title")%></h2>
        </div>		
		<div class="EventDate">
		    <div class="SubHead">When</div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "EventStartFormatted")%></div>
		</div>
		
		<div class="EventLocation">
		    <div class="SubHead">Where</div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "Location")  %></div>
		</div>
		
		<div class="EventDescription">
		    <div class="SubHead">Description</div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "Overview")  %></div>
		</div>

		<div class="EventButtons">
            <asp:LinkButton ID="lbCRsvp" runat="server" CssClass="CommandButton" OnClick="lbCRsvp_OnClick">RSVP</asp:LinkButton>
            <asp:LinkButton ID="lbCICal" runat="server" CssClass="CommandButton" OnClick="lbCICal_OnClick">iCal</asp:LinkButton>
            <asp:LinkButton ID="lbCViewInvite" runat="server" CssClass="CommandButton" OnClick="lbCViewInvite_OnClick">View Invite</asp:LinkButton>
            <asp:LinkButton ID="lbCeMailAFriend" runat="server" CssClass="CommandButton" OnClick="lbCeMailAFriend_OnClick">E-mail A Friend</asp:LinkButton>
            <asp:LinkButton ID="lbCPrint" runat="server" CssClass="CommandButton" OnClick="lbCPrint_OnClick">Print</asp:LinkButton>
		</div>
	</ItemTemplate>

</asp:Repeater>

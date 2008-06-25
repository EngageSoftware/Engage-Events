<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventListing" Codebehind="EventListing.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register src="../Navigation/EventAdminActions.ascx" tagname="actions" tagprefix="engage" %>

<div class="EventHeader">
    <h4 class="NormalBold"><asp:Label runat="server" ResourceKey="EventHeader"></asp:Label></h4>
</div>

<asp:Repeater runat="server" id="EventListingRepeater" OnItemDataBound="EventListingRepeater_ItemDataBound">

	<ItemTemplate>
        <asp:Label Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Id") %>'></asp:Label>
        <div class="EventTitle">
		    <h2 class="Head"><%# DataBinder.Eval(Container.DataItem, "Title") %></h2>
        </div>		
		<div class="EventDate">
		    <div class="SubHead">When</div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "EventStartFormatted") %></div>
		</div>
		
		<div class="EventLocation">
		    <div class="SubHead">Where</div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "Location") %></div>
		</div>
		
		<div class="EventDescription">
		    <div class="SubHead">Description</div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "Overview") %></div>
		</div>
        <engage:actions ID="EventActions" runat="server" />
	</ItemTemplate>

</asp:Repeater>

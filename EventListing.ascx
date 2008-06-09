<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventListing" Codebehind="EventListing.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register src="GlobalNavigation.ascx" tagname="GlobalNavigation" tagprefix="uc1" %>
<%@ Register src="EventAdminActions.ascx" tagname="actions" tagprefix="uc2" %>
<uc1:GlobalNavigation ID="GlobalNavigation1" runat="server" />
<br />                
<br />
<div class="EventHeader">
        <h4 class="NormalBold">Events</h4>
</div>

<asp:Repeater runat="server" id="rpEventListing" OnItemDataBound="rpEventListing_ItemDataBound">
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
        <uc2:actions ID="ccEventActions" runat="server" UseCache="true" />
	</ItemTemplate>

</asp:Repeater>

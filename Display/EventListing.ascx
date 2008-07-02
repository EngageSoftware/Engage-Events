<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.EventListing" Codebehind="EventListing.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register src="../Navigation/EventAdminActions.ascx" tagname="actions" tagprefix="uc2" %>

<div id="EventListingCurrent">
        <div class="EventHeader">
                <h4 class="Normal">This Month</h4>
                <h4 class="NormalBold">Events</h4>
        </div>
    <asp:Repeater runat="server" id="rpCurrentEventListing" OnItemDataBound="Listing_ItemDataBound">
        <ItemTemplate>
            <asp:Label id = "lblId" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Id")  %>'></asp:Label>
                    <div id="EventItem">    
                            <div class="EventTitle">
                                <h2 class="Head"><%# DataBinder.Eval(Container.DataItem, "Title")  %></h2>
                            </div>
                    
                            <div class="EventDate">
                                <p class="NormalBold">When</p>
                                <p class="Normal"><%# DataBinder.Eval(Container.DataItem, "EventStartFormatted")%></p>
                            </div>
                            
                            <div class="EventLocation">
                                <p class="NormalBold">Where</p>
                                <p class="Normal"><%# DataBinder.Eval(Container.DataItem, "Location")  %></p>
                            </div>
                            
                            <div class="EventDescription">
                                <p class="NormalBold">Description</p>
                                <p class="Normal"><%# DataBinder.Eval(Container.DataItem, "Overview")  %></p>
                            </div>
                            <uc2:actions ID="ccEventActions" runat="server" />
					</div>
        </ItemTemplate>
    </asp:Repeater>
</div>    
    

<div id="EventListingUpcoming">
    <div class="EventHeader">
            <h4 class="Normal">Upcoming</h4>
            <h4 class="NormalBold">Events</h4>
    </div>	        
    
    <asp:Repeater runat="server" id="rpUpcomingEventListing" OnItemDataBound="Listing_ItemDataBound">
        <ItemTemplate>
            <asp:Label id = "lblId" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Id")  %>'></asp:Label>
				<div id="EventItem">
                        <div class="EventTitle">
                                <h2 class="Head"><%# DataBinder.Eval(Container.DataItem, "Title")  %></h2>
                        </div>		    

                        <div class="EventDate">
                            <p class="NormalBold">When</p>
                            <p class="Normal"><%# DataBinder.Eval(Container.DataItem, "EventStartFormatted")%></p>
                        </div>

                        <div class="EventLocation">
                            <p class="NormalBold">Where</p>
                            <p class="Normal"><%# DataBinder.Eval(Container.DataItem, "Location")  %></p>
                        </div>

                        <div class="EventDescription">
                            <p class="NormalBold">Description</p>
                            <p class="Normal"><%# DataBinder.Eval(Container.DataItem, "Overview")  %></p>
                        </div>
                        <uc2:actions ID="ccEventActions2" runat="server" />
				</div>                        
        </ItemTemplate>
    </asp:Repeater>
</div>
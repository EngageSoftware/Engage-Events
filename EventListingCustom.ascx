<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventListingCustom" Codebehind="EventListingCustom.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<div id="EventListingCustomCurrent">
        <div class="EventHeader">
                <h4 class="Normal">This Month</h4>
                <h4 class="NormalBold">Events</h4>
        </div>
    <asp:Repeater runat="server" id="rpCurrentEventListing">
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
                    
                            <div class="EventButtons">
                                <asp:LinkButton ID="lbCRsvp" runat="server" ResourceKey="lbRsvp" CssClass="CommandButton" OnClick="lbCRsvp_OnClick">RSVP</asp:LinkButton>
                                <asp:LinkButton ID="lbCICal" runat="server" ResourceKey="lbIcal" CssClass="CommandButton" OnClick="lbCICal_OnClick">iCal</asp:LinkButton>
                                <asp:HyperLink ID="lbCViewInvite" runat="server" ResourceKey="lbViewInvite" CssClass="CommandButton" Target="_new" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"InvitationUrl") %>' Visible=<%# HasInviteUrl(DataBinder.Eval(Container.DataItem, "InvitationUrl"))  %>>View Invite</asp:HyperLink>
                                <asp:LinkButton ID="lbCeMailAFriend" runat="server" ResourceKey="lbEmailAFriend" CssClass="CommandButton" OnClick="lbCeMailAFriend_OnClick">E-mail A Friend</asp:LinkButton>
                                <asp:LinkButton ID="lbCPrint" runat="server" ResourceKey="lbPrint" CssClass="CommandButton" OnClick="lbCPrint_OnClick">Print</asp:LinkButton>
                            </div>
					</div>
        </ItemTemplate>
    </asp:Repeater>
</div>    
    

<div id="EventListingCustomUpcoming">
    <div class="EventHeader">
            <h4 class="Normal">Upcoming</h4>
            <h4 class="NormalBold">Events</h4>
    </div>	        
    
    <asp:Repeater runat="server" id="rpUpcomingEventListing" OnItemDataBound="rpUpcomingEventListing_ItemDataBound">
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

                        <div class="EventButtons">
                            <asp:LinkButton ID="lbURsvp" runat="server" ResourceKey="lbRsvp" CssClass="CommandButton" OnClick="lbCRsvp_OnClick">RSVP</asp:LinkButton>
                            <asp:LinkButton ID="lbUIcal" runat="server" ResourceKey="lbIcal" CssClass="CommandButton" OnClick="lbCICal_OnClick">iCal</asp:LinkButton>
                            <asp:HyperLink ID="lbUViewInvite" runat="server" ResourceKey="lbViewInvite" CssClass="CommandButton" Target="_new" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"InvitationUrl") %>' Visible=<%# HasInviteUrl(DataBinder.Eval(Container.DataItem, "InvitationUrl"))  %>>View Invite</asp:HyperLink>
                            <asp:LinkButton ID="lbUEmailAFriend" runat="server" ResourceKey="lbEmailAFriend" CssClass="CommandButton" OnClick="lbCeMailAFriend_OnClick">E-mail A Friend</asp:LinkButton>
                            <asp:LinkButton ID="lbUPrint" runat="server" ResourceKey="lbPrint" CssClass="CommandButton" OnClick="lbCPrint_OnClick">Print</asp:LinkButton>
                        </div>
				</div>                        
        </ItemTemplate>
    </asp:Repeater>
</div>
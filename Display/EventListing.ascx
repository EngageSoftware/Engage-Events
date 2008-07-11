<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.EventListing" CodeBehind="EventListing.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register Src="../Navigation/EventAdminActions.ascx" TagName="actions" TagPrefix="engage" %>
<div id="EventListingCurrent">
    <div class="EventHeader">
        <h4 class="Normal">
            <asp:Label runat="server" ResourceKey="ThisMonth" />
        </h4>
        <h4 class="NormalBold">
            <asp:Label runat="server" ResourceKey="EventsHeader" />
        </h4>
    </div>
    <asp:Repeater runat="server" ID="CurrentEventListing">
        <ItemTemplate>
            <div id="EventItem">
                <div class="EventTitle">
                    <h2 class="Head">
                        <%# DataBinder.Eval(Container.DataItem, "Title")  %>
                    </h2>
                </div>
                <div class="EventDate">
                    <p class="NormalBold">
                        <asp:Label runat="server" ResourceKey="WhenHeader" />
                    </p>
                    <p class="Normal">
                        <%# DataBinder.Eval(Container.DataItem, "EventStartFormatted")%>
                    </p>
                </div>
                <div class="EventLocation">
                    <p class="NormalBold">
                        <asp:Label runat="server" ResourceKey="WhereHeader" />
                    </p>
                    <p class="Normal">
                        <%# DataBinder.Eval(Container.DataItem, "Location")  %>
                    </p>
                </div>
                <div class="EventDescription">
                    <p class="NormalBold">
                        <asp:Label runat="server" ResourceKey="DescriptionHeader" />
                    </p>
                    <div class="Normal">
                        <%# DataBinder.Eval(Container.DataItem, "Overview")  %>
                    </div>
                </div>
                <engage:actions ID="EventActions" runat="server" OnCancel="EventActions_Cancel" OnDelete="EventActions_Delete" />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<div id="EventListingUpcoming">
    <div class="EventHeader">
        <h4 class="Normal">
            <asp:Label runat="server" ResourceKey="Upcoming" />
        </h4>
        <h4 class="NormalBold">
            <asp:Label runat="server" ResourceKey="EventsHeader" />
        </h4>
    </div>
    <asp:Repeater runat="server" ID="UpcomingEventListing">
        <ItemTemplate>
            <div id="EventItem">
                <div class="EventTitle">
                    <h2 class="Head">
                        <%# DataBinder.Eval(Container.DataItem, "Title")  %>
                    </h2>
                </div>
                <div class="EventDate">
                    <p class="NormalBold">
                        <asp:Label runat="server" ResourceKey="WhenHeader" />
                    </p>
                    <p class="Normal">
                        <%# DataBinder.Eval(Container.DataItem, "EventStartFormatted")%>
                    </p>
                </div>
                <div class="EventLocation">
                    <p class="NormalBold">
                        <asp:Label runat="server" ResourceKey="WhereHeader" />
                    </p>
                    <p class="Normal">
                        <%# DataBinder.Eval(Container.DataItem, "Location")  %>
                    </p>
                </div>
                <div class="EventDescription">
                    <p class="NormalBold">
                        <asp:Label runat="server" ResourceKey="DescriptionHeader" />
                    </p>
                    <div class="Normal">
                        <%# DataBinder.Eval(Container.DataItem, "Overview")  %>
                    </div>
                </div>
                <engage:actions ID="EventActions" runat="server" OnCancel="EventActions_Cancel" OnDelete="EventActions_Delete" />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventListingAdmin" Codebehind="EventListingAdmin.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register src="../Navigation/GlobalNavigation.ascx" tagname="GlobalNavigation" tagprefix="uc1" %>
<%@ Register src="../Navigation/EventAdminActions.ascx" tagname="actions" tagprefix="uc2" %>

<uc1:GlobalNavigation ID="GlobalNavigation1" runat="server" />
<br />                
<br />
<div class="EventHeader">
    <h4 class="NormalBold">My Events</h4>
    <asp:Label ID="lblSortBy" runat="server" CssClass="NormalBold" Text="Sort By"></asp:Label>
    <asp:RadioButtonList ID="rbSort" runat="server" AutoPostBack="True" CssClass="Normal"
         RepeatDirection="Horizontal">
        <asp:ListItem Selected="True" Value="EventStart">Date</asp:ListItem>
        <asp:ListItem Value="Title">Title</asp:ListItem>
    </asp:RadioButtonList><br />
    <div align="left">
        <asp:Label ID="lblStatus" runat="server" CssClass="NormalBold" Text="Status"></asp:Label>
        <asp:RadioButtonList ID="rbStatus" runat="server" AutoPostBack="True" CssClass="Normal"
             RepeatDirection="Horizontal" >
            <asp:ListItem Selected="True" Value="Active">Active</asp:ListItem>
            <asp:ListItem Value="All">All</asp:ListItem>
        </asp:RadioButtonList>
    </div>
</div>	                        

<asp:Repeater runat="server" id="rpEventListing" OnItemDataBound="RpEventListing_ItemDataBound">
	<ItemTemplate>
        <asp:Label id = "lblId" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Id")  %>'></asp:Label>
		<div class="EventTitle">
		    <h2 class="Head"><%# DataBinder.Eval(Container.DataItem, "Title")  %></h2>
        </div>
		<div class="EventDate">
		    <div class="NormalBold">When</div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "EventStartFormatted")%></div>
		</div>
		
		<div class="EventLocation">
		    <div class="NormalBold">Where</div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "Location")  %></div>
		</div>
		
		<div class="EventDescription">
		    <div class="NormalBold">Description</div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "Overview")  %></div>
		</div>
        <uc2:actions ID="ccEventActions" runat="server" />
<%--		<div class="EventButtons">
		    <asp:LinkButton ID="lbEditEvent" runat="server" CssClass="CommandButton" ResourceKey="lbEditevent" OnClick="lbEditEvent_OnClick">Edit</asp:LinkButton>
            <asp:LinkButton ID="lbViewRsvp" runat="server" CssClass="CommandButton" ResourceKey="lbViewRsvp" OnClick="lbViewRsvp_OnClick">View RSVP</asp:LinkButton>
            <asp:HyperLink ID="lbViewInvite" runat="server" CssClass="CommandButton" ResourceKey="lbViewInvite" Visible = "false" Target="_new" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"InvitationUrl") %>'>View Invite</asp:HyperLink>
            <asp:LinkButton ID="lbEditeMail" runat="server" CssClass="CommandButton" ResourceKey="lbEditEmail" Visible = "false" OnClick="lbEditEmail_OnClick">Create/Edit Email</asp:LinkButton>
            <asp:LinkButton ID="lbDelete" runat="server" CssClass="CommandButton" ResourceKey="lbDelete" OnClick="lbDelete_OnClick">Delete</asp:LinkButton>
            <asp:LinkButton ID="lbCancel" runat="server" CssClass="CommandButton" OnClick="lbCancel_OnClick" Text='<%# GetActionText(DataBinder.Eval(Container.DataItem,"Cancelled")) %>'></asp:LinkButton>
            
		</div>
--%>	</ItemTemplate>

</asp:Repeater>

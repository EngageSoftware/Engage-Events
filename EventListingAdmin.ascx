<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventListingAdmin" Codebehind="EventListingAdmin.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<div class="AdminButtons">
    <asp:LinkButton ID="lbAdminSettings" runat="server" onclick="lbSettings_OnClick">Settings</asp:LinkButton>
    <asp:LinkButton ID="lbAddAnEvent" runat="server" OnClick="lbAddAnEvent_OnClick">Add An Event</asp:LinkButton>
    <asp:LinkButton ID="lbAdminEmail" runat="server" Visible="False" OnClick="lbManageEmail_OnClick">Manage E-Mail</asp:LinkButton>
    <asp:LinkButton ID="lbManageRsvp" runat="server" onclick="lbManageRsvp_OnClick">Rsvp</asp:LinkButton>
</div>
<br />                
<br />
<div class="EventHeader">
    <h4 class="NormalBold">My Events</h4>
    <asp:Label ID="lblSortBy" runat="server" CssClass="NormalBold" Text="Sort By"></asp:Label>
    <asp:RadioButtonList ID="rbSort" runat="server" AutoPostBack="True" CssClass="Normal"
         RepeatDirection="Horizontal" OnSelectedIndexChanged="rbSort_SelectedIndexChanged">
        <asp:ListItem Selected="True" Value="EventStart">Date</asp:ListItem>
        <asp:ListItem Value="Title">Title</asp:ListItem>
    </asp:RadioButtonList><br />
</div>	                        

<asp:Repeater runat="server" id="rpEventListing" OnItemDataBound="rpEventListing_ItemDataBound">
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

		<div class="EventButtons">
            <asp:LinkButton ID="lbViewRsvp" runat="server" CssClass="CommandButton" ResourceKey="lbViewRsvp" OnClick="lbViewRsvp_OnClick">View RSVP</asp:LinkButton>
            <asp:LinkButton ID="lbEditEvent" runat="server" CssClass="CommandButton" ResourceKey="lbEditevent" OnClick="lbEditEvent_OnClick">Edit Event</asp:LinkButton>
            <%--Visible=<%# HasInviteUrl(DataBinder.Eval(Container.DataItem, "InvitationUrl"))  %>--%>
            <asp:HyperLink ID="lbViewInvite" runat="server" CssClass="CommandButton" ResourceKey="lbViewInvite" Visible = "false" Target="_new" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"InvitationUrl") %>'>View Invite</asp:HyperLink>
            <asp:LinkButton ID="lbEditeMail" runat="server" CssClass="CommandButton" ResourceKey="lbEditEmail" Visible = "false" OnClick="lbEditEmail_OnClick">Create/Edit Email</asp:LinkButton>
            <asp:LinkButton ID="lbDelete" runat="server" CssClass="CommandButton" ResourceKey="lbDelete" OnClick="lbDelete_OnClick">Delete</asp:LinkButton>
            <asp:LinkButton ID="lbCancel" runat="server" CssClass="CommandButton" ResourceKey="lbCancel" OnClick="lbCancel_OnClick">Cancel</asp:LinkButton>
            
		</div>
	</ItemTemplate>

</asp:Repeater>

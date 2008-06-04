<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.RsvpSummary" Codebehind="RsvpSummary.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<div class="EventHeader">
    <asp:Label ID="lblSortBy" runat="server" CssClass="NormalBold" Text="Sort By"></asp:Label>
    <asp:RadioButtonList ID="rbSort" runat="server" AutoPostBack="True" CssClass="Normal"
         RepeatDirection="Horizontal" OnSelectedIndexChanged="rbSort_SelectedIndexChanged">
        <asp:ListItem Selected="True" Value="EventStart">Date</asp:ListItem>
        <asp:ListItem Value="Name">Title</asp:ListItem>
    </asp:RadioButtonList><br />
    <h4 class="NormalBold">Events<br />
        Status</h4>
	<div class="EventButtons">
        <asp:LinkButton ID="lbSettings" runat="server" CssClass="CommandButton" OnClick="lbSettings_OnClick">Settings</asp:LinkButton>
        <asp:LinkButton ID="lbMyEvents" runat="server" CssClass="CommandButton" OnClick="lbMyEvents_OnClick">My Events</asp:LinkButton>
        <asp:LinkButton ID="lbAddEvent" runat="server" CssClass="CommandButton" OnClick="lbAddEvents_OnClick">Add Event</asp:LinkButton>
        <asp:LinkButton ID="lbManageEmail" runat="server" CssClass="CommandButton" OnClick="lbManageEmail_OnClick">Mangage email</asp:LinkButton>
	</div>
    
</div>	        

<asp:Repeater runat="server" id="rpSummary">
	<ItemTemplate>
        <asp:Label id = "lblId" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EventId")  %>'></asp:Label>
		
		<h2 class="Head"><%# DataBinder.Eval(Container.DataItem, "Name")%></h2>

		<div class="EventDate">
		    <div class="SubHead">When</div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "EventStartFormatted")%></div>
		</div>
	    <div class="EventStats">
	        <asp:HyperLink id="lnkNotAttending" runat="server" NavigateUrl='<%# GetDetailUrl(DataBinder.Eval(Container.DataItem, "EventId"), "NotAttending", DataBinder.Eval(Container.DataItem,"NotAttending")) %>' Text='<%# DataBinder.Eval(Container.DataItem,"NotAttending") %>'></asp:HyperLink>        
            <asp:HyperLink id="lnkAttending" runat="server" NavigateUrl ='<%# GetDetailUrl(DataBinder.Eval(Container.DataItem, "EventId"), "Attending", DataBinder.Eval(Container.DataItem,"Attending")) %>' Text='<%# DataBinder.Eval(Container.DataItem,"Attending") %>'></asp:HyperLink>
            <asp:HyperLink id="lnkNoResponse" runat="server" NavigateUrl ='<%# GetDetailUrl(DataBinder.Eval(Container.DataItem, "EventId"), "NoResponse", DataBinder.Eval(Container.DataItem,"NoResponse")) %>' Text='<%# DataBinder.Eval(Container.DataItem,"NoResponse") %>'></asp:HyperLink>
        </div>
	</ItemTemplate>
</asp:Repeater>
<dnn:PagingControl ID="pager" runat="server"></dnn:PagingControl>


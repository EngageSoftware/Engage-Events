<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventListingAdmin" Codebehind="EventListingAdmin.ascx.cs" EnableViewState="true" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register src="../Navigation/GlobalNavigation.ascx" tagname="GlobalNavigation" tagprefix="engage" %>
<%@ Register src="../Navigation/EventAdminActions.ascx" tagname="actions" tagprefix="engage" %>

<span class="GlobalNavigation"><engage:GlobalNavigation ID="GlobalNavigation" runat="server" /></span>

<div class="EventHeader">
    <h4 class="NormalBold"><asp:Label runat="server" ResourceKey="MyEventsLabel"></asp:Label></h4>
    
    <asp:Label ID="SortByLabel" runat="server" CssClass="NormalBold" ResourceKey="SortByLabel"></asp:Label>
    
    <asp:RadioButtonList ID="SortRadioButtonList" runat="server" 
        AutoPostBack="True" CssClass="Normal" RepeatDirection="Horizontal" 
        onselectedindexchanged="SortRadioButtonList_SelectedIndexChanged">
        <asp:ListItem Selected="True" Value="EventStart" ResourceKey="DateListItem"></asp:ListItem>
        <asp:ListItem Value="Title" ResourceKey="TitleListItem"></asp:ListItem>
    </asp:RadioButtonList>
    
    <div>
        <asp:Label ID="StatusLabel" runat="server" CssClass="NormalBold" ResourceKey="StatusLabel"></asp:Label>
        <asp:RadioButtonList ID="StatusRadioButtonList" runat="server" 
            AutoPostBack="True" CssClass="Normal" RepeatDirection="Horizontal" 
            onselectedindexchanged="StatusRadioButtonList_SelectedIndexChanged" >
            <asp:ListItem Selected="True" Value="Active" ResourceKey="ActiveListItem"></asp:ListItem>
            <asp:ListItem Value="All" ResourceKey="AllListItem"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
</div>	                        

<asp:Repeater runat="server" id="EventListingRepeater" OnItemDataBound="EventListingRepeater_ItemDataBound">
	<ItemTemplate>
	
        <asp:Label id="IdLabel" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Id")  %>'></asp:Label>
        
		<div class="EventTitle">
		    <h2 class="Head"><%# DataBinder.Eval(Container.DataItem, "Title")  %></h2>
        </div>
        
		<div class="EventDate">
		    <div class="NormalBold"><asp:Label runat="server" ResourceKey="When"></asp:Label></div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "EventStartFormatted")%></div>
		</div>
		
		<div class="EventLocation">
		    <div class="NormalBold"><asp:Label runat="server" ResourceKey="Where"></asp:Label></div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "Location")  %></div>
		</div>
		
		<div class="EventDescription">
		    <div class="NormalBold"><asp:Label runat="server" ResourceKey="Description"></asp:Label></div>
		    <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "Overview")  %></div>
		</div>
		
        <engage:actions ID="EventActions" runat="server" />
        
    </ItemTemplate>
</asp:Repeater>

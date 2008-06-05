<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.RsvpSummary" Codebehind="RsvpSummary.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register src="GlobalNavigation.ascx" tagname="GlobalNavigation" tagprefix="uc1" %>
<uc1:GlobalNavigation ID="GlobalNavigation1" runat="server" />
<br />                
<br />
<div class="EventHeader">
    <asp:Label ID="lblSortBy" runat="server" CssClass="NormalBold" Text="Sort By"></asp:Label>
    <asp:RadioButtonList ID="rbSort" runat="server" AutoPostBack="True" CssClass="Normal"
         RepeatDirection="Horizontal" OnSelectedIndexChanged="rbSort_SelectedIndexChanged">
        <asp:ListItem Selected="True" Value="EventStart">Date</asp:ListItem>
        <asp:ListItem Value="Title">Title</asp:ListItem>
    </asp:RadioButtonList><br />
    <h4 class="NormalBold">Events<br />
        Status</h4>
</div>	        

<asp:Repeater runat="server" id="rpSummary">
	<ItemTemplate>
        <asp:Label id = "lblId" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EventId")  %>'></asp:Label>
		
		<h2 class="Head"><%# DataBinder.Eval(Container.DataItem, "Title")%></h2>

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


<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.RsvpSummary" CodeBehind="RsvpSummary.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="engage" TagName="RsvpDisplay" Src="Display/RsvpDisplay.ascx" %>
<%@ Import Namespace="DotNetNuke.Services.Localization"%>

<div class="Normal ResponsesSummary">
    <div class="ResponseHeader">
        <div class="SortByHeader">
            <h4 class="NormalBold">
                <asp:Label runat="server" CssClass="NormalBold" Resourcekey="SortByLabel" />
            </h4>
            <asp:RadioButtonList ID="SortRadioButtonList" runat="server" AutoPostBack="True" CssClass="Normal" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Selected="True" Value="EventStart" resourcekey="EventStart" />
                <asp:ListItem Value="Title" resourcekey="Title" />
            </asp:RadioButtonList>
        </div>
        <div class="StatusHeader">
            <h4 class="NormalBold">
                <asp:Label runat="server" CssClass="NormalBold" resourcekey="Status" />
            </h4>
        </div>
    </div>
    
    <div class="ResponseHeader">
        <h3><asp:Label runat="server" resourcekey="Events" /></h3>
        <div class="StatusTypeHeaders">
            <asp:Label runat="server" resourcekey="Attending" />
            <asp:Label runat="server" resourcekey="NotAttending" />
            <%--<asp:Label runat="server" resourcekey="NoResponse" />--%>
        </div>
    </div>
    <asp:Label ID="NoRsvpsMessageLabel" runat="server" resourcekey="NoRsvpsMessageLabel" />
    <asp:Repeater runat="server" ID="SummaryRepeater">
        <HeaderTemplate><ol></HeaderTemplate>
        <ItemTemplate>
            <li>
                <engage:RsvpDisplay ID="RsvpDisplay" runat="server" />
            </li>
        </ItemTemplate>
        <FooterTemplate></ol></FooterTemplate>
    </asp:Repeater>
    <dnn:PagingControl ID="SummaryPager" runat="server" />
    <asp:HyperLink ID="CancelGoHomeLink" runat="server" ImageUrl="~/DesktopModules/EngageEvents/Images/cancel_go_home.gif"/>
</div>
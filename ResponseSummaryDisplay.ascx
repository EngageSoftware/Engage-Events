<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.ResponseSummaryDisplay" CodeBehind="ResponseSummaryDisplay.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="engage" TagName="ResponseDisplay" Src="Display/ResponseDisplay.ascx" %>
<%@ Import Namespace="DotNetNuke.Services.Localization"%>

<div class="Normal ResponsesSummary">
    <div class="ResponseHeader">
        <div class="SortByHeader">
            <p class="Normal">
                <asp:Label ID="Label1" runat="server" Resourcekey="SortByLabel" />
            </p>
            <asp:RadioButtonList ID="SortRadioButtonList" runat="server" AutoPostBack="True" CssClass="Normal" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Selected="True" Value="EventStart" resourcekey="EventStart" />
                <asp:ListItem Value="Title" resourcekey="Title" />
            </asp:RadioButtonList>
        </div>
        <div class="StatusHeader">
            <p class="Normal">
                <asp:Label ID="Label2" runat="server" CssClass="NormalBold" resourcekey="Status" />
            </p>
        </div>
    </div>
    
    <div class="ResponseHeader">
        <p><asp:Label ID="Label3" runat="server" resourcekey="Events" /></p>
        <div class="StatusTypeHeaders Normal">
            <p class="ResponseAtt"><asp:Label ID="Label4" runat="server" resourcekey="Attending" /></p>
            <p class="ResponseNotAtt"><asp:Label ID="Label5" runat="server" resourcekey="NotAttending" /></p>
            <%--<asp:Label runat="server" resourcekey="NoResponse" />--%>
        </div>
    </div>
    <asp:Label ID="NoResponsesMessageLabel" runat="server" resourcekey="NoResponsesMessageLabel" />
    <asp:Repeater runat="server" ID="SummaryRepeater">
        <HeaderTemplate><ol></HeaderTemplate>
        <ItemTemplate>
            <li>
                <engage:ResponseDisplay ID="ResponseDisplay" runat="server" />
            </li>
        </ItemTemplate>
        <FooterTemplate></ol></FooterTemplate>
    </asp:Repeater>
    <dnn:PagingControl ID="SummaryPager" runat="server" PageSize="10"/>
    <asp:HyperLink ID="CancelGoHomeLink" runat="server" ImageUrl="~/DesktopModules/EngageEvents/Images/cancel_go_home.gif"/>
</div>
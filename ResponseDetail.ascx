<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.ResponseDetail" CodeBehind="ResponseDetail.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="engage" TagName="ResponseDisplay" Src="Display/ResponseDisplay.ascx" %>

<div class="ResponsesSummary">
    <div class="ResponseHeader">
        <div class="SortByHeader Normal">
            <p>
                <asp:Label ID="Label1" runat="server" ResourceKey="SortByLabel"/>
            </p>
            <asp:RadioButtonList ID="SortRadioButtonList" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Selected="True" Value="CreationDate" ResourceKey="ResponseDate"/>
                <asp:ListItem Value="FirstName" ResourceKey="FirstName"/>
                <asp:ListItem Value="LastName" ResourceKey="LastName"/>
            </asp:RadioButtonList>
        </div>
        <div class="StatusHeader Normal">
            <p>
                <asp:Label ID="Label2" runat="server" resourcekey="Status" />
            </p>
        </div>
    </div>
    <div class="ResponseHeader">
        <p class="Normal"><asp:Label ID="Label3" runat="server" resourcekey="Events" /></p>
        <div class="StatusTypeHeaders Normal">
            <p class="ResponseAtt"><asp:Label ID="Label4" runat="server" resourcekey="Attending" /></p>
            <p class="ResponseNotAtt"><asp:Label ID="Label5" runat="server" resourcekey="NotAttending" /></p>
            <%--<asp:Label runat="server" resourcekey="NoResponse" />--%>
        </div>
    </div>
    <engage:ResponseDisplay ID="responseDisplay" runat="server" />
    <asp:DataGrid ID="ResponseDetailGrid" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="False" PageSize="10" BorderWidth="0" GridLines="None" HeaderStyle-CssClass="ResponseDetailHeader" CssClass="Normal">
        <SelectedItemStyle CssClass="ResponseDetailItemSelect"></SelectedItemStyle>
        <PagerStyle HorizontalAlign="Center" Mode="NumericPages" PageButtonCount="15"></PagerStyle>
        <AlternatingItemStyle CssClass="ResponseDetailAltItem"></AlternatingItemStyle>
        <ItemStyle CssClass="ResponseDetailItem"></ItemStyle>
        <Columns>
            <asp:BoundColumn DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="ResponseMemberName">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="True"></ItemStyle>
                <FooterStyle Wrap="False"></FooterStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="CreationDate" HeaderText="Date" SortExpression="CreationDate">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="True"></ItemStyle>
                <FooterStyle Wrap="False"></FooterStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Attending">
                <ItemTemplate>
                    <asp:Image ID="lnkAttending" CssClass="Normal" runat="server" ImageUrl='<%# GetStatusIcon(Eval("Status")) %>'></asp:Image>
                </ItemTemplate>
            </asp:TemplateColumn>
            <%-- <asp:TemplateColumn HeaderText ="Not Attending">
	        <ItemTemplate>
		        <asp:HyperLink id="lnkAttending" CssClass="Normal"  runat="server" NavigateUrl ='<%# GetDetailUrl(Eval("EventId")) %>' Text ='<%# DataBinder.Eval(Container.DataItem,"NotAttending") %>'></asp:HyperLink>
	        </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText ="NoResponse">
	        <ItemTemplate>
		        <asp:HyperLink id="lnkAttending" CssClass="Normal"  runat="server" NavigateUrl ='<%# GetDetailUrl(Eval("EventId")) %>' Text ='<%# DataBinder.Eval(Container.DataItem,"NoResponse") %>'></asp:HyperLink>
	        </ItemTemplate>
        </asp:TemplateColumn>--%>
        </Columns>
        <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#E0E0E0"></HeaderStyle>
    </asp:DataGrid>
    <dnn:PagingControl ID="pager" runat="server"/>
    <asp:HyperLink ID="CancelGoHomeLink" runat="server" ImageUrl="~/DesktopModules/EngageEvents/Images/cancel_go_home.gif" />
</div>

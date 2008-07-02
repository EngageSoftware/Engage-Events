<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.RsvpDetail" CodeBehind="RsvpDetail.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="engage" TagName="RsvpDisplay" Src="Display/RsvpDisplay.ascx" %>

<div class="Normal ResponsesSummary">
    <div class="ResponseHeader">
        <div class="SortByHeader">
            <h4 class="NormalBold">
                <asp:Label runat="server" CssClass="NormalBold" ResourceKey="SortByLabel"/>
            </h4>
            <asp:RadioButtonList ID="SortRadioButtonList" runat="server" AutoPostBack="True" CssClass="Normal" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Selected="True" Value="CreationDate" ResourceKey="ResponseDate"/>
                <asp:ListItem Value="FirstName" ResourceKey="FirstName"/>
                <asp:ListItem Value="LastName" ResourceKey="LastName"/>
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
    <engage:RsvpDisplay ID="RsvpDisplay" runat="server" />

    <asp:DataGrid ID="grdRsvpDetail" runat="server" Font-Size="X-Small" Font-Names="Verdana" Width="100%" Height="72px" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Vertical" AllowSorting="False" PageSize="10">
        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages" PageButtonCount="15"></PagerStyle>
        <AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
        <ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
        <Columns>
            <asp:BoundColumn DataField="Name" HeaderText="Name" SortExpression="Name">
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
                    <asp:Image ID="lnkAttending" CssClass="Normal" runat="server" ImageUrl='<%# GetStatusIcon(DataBinder.Eval(Container.DataItem, "Status")) %>'></asp:Image>
                </ItemTemplate>
            </asp:TemplateColumn>
            <%-- <asp:TemplateColumn HeaderText ="Not Attending">
	        <ItemTemplate>
		        <asp:HyperLink id="lnkAttending" CssClass="Normal"  runat="server" NavigateUrl ='<%# GetDetailUrl(DataBinder.Eval(Container.DataItem, "EventId")) %>' Text ='<%# DataBinder.Eval(Container.DataItem,"NotAttending") %>'></asp:HyperLink>
	        </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText ="NoResponse">
	        <ItemTemplate>
		        <asp:HyperLink id="lnkAttending" CssClass="Normal"  runat="server" NavigateUrl ='<%# GetDetailUrl(DataBinder.Eval(Container.DataItem, "EventId")) %>' Text ='<%# DataBinder.Eval(Container.DataItem,"NoResponse") %>'></asp:HyperLink>
	        </ItemTemplate>
        </asp:TemplateColumn>--%>
        </Columns>
        <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#E0E0E0"></HeaderStyle>
    </asp:DataGrid>
    <dnn:PagingControl ID="pager" runat="server"/>
    <asp:HyperLink ID="CancelGoHomeLink" runat="server" ImageUrl="~/DesktopModules/EngageEvents/Images/cancel_go_home.gif" />
</div>

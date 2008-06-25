<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.RsvpDetail" Codebehind="RsvpDetail.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<div>
    <asp:Label ID="lblSortBy" runat="server" CssClass="NormalBold" Text="Sort By"></asp:Label>
    <asp:RadioButtonList ID="rbSort" runat="server" AutoPostBack="True" CssClass="Normal"
         RepeatDirection="Horizontal" OnSelectedIndexChanged="RbSort_SelectedIndexChanged">
        <asp:ListItem Selected="True" Value="CreationDate">Response Date</asp:ListItem>
        <asp:ListItem Value="FirstName">First Name</asp:ListItem>
        <asp:ListItem Value="LastName">Last Name</asp:ListItem>
    </asp:RadioButtonList><br />
</div>
<h2 class="Head"><asp:Label ID="lblName" runat="server"></asp:Label></h2>
<div class="EventDate">
    <div class="Normal"><asp:Label ID="lblDate" runat="server"></asp:Label></div>
</div>

<dnn:PagingControl ID="pager" runat="server"></dnn:PagingControl>
<asp:DataGrid ID="grdRsvpDetail" runat="server" Font-Size="X-Small" Font-Names="Verdana"
    Width="100%" Height="72px" AutoGenerateColumns="False" BorderColor="#999999"
    BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Vertical"
    AllowSorting="False" PageSize="10">
    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
    <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"
        PageButtonCount="15"></PagerStyle>
    <AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
    <ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
    <Columns>
        <asp:BoundColumn DataField="Name" HeaderText="Name" SortExpression="Name">
            <HeaderStyle Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="True"></ItemStyle>
            <FooterStyle Wrap="False"></FooterStyle>
        </asp:BoundColumn>
        <asp:BoundColumn DataField="Company" HeaderText="Company" SortExpression="Company">
            <HeaderStyle Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="True"></ItemStyle>
            <FooterStyle Wrap="False"></FooterStyle>
        </asp:BoundColumn>
        <asp:BoundColumn DataField="CreationDate" HeaderText="Date" SortExpression="CreationDate">
            <HeaderStyle Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="True"></ItemStyle>
            <FooterStyle Wrap="False"></FooterStyle>
        </asp:BoundColumn>
        <asp:TemplateColumn HeaderText ="Attending">
	        <ItemTemplate>
		        <asp:Image id="lnkAttending" CssClass="Normal"  runat="server" ImageUrl ='<%# GetStatusIcon(DataBinder.Eval(Container.DataItem, "Status")) %>'></asp:Image>
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
<asp:HyperLink ID="CancelGoHomeLink" runat="server" ImageUrl="~/DesktopModules/EngageEvents/Images/cancel_go_home.gif"/>

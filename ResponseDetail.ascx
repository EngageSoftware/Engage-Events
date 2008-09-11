<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.ResponseDetail" CodeBehind="ResponseDetail.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="engage" TagName="ResponseDisplay" Src="Display/ResponseDisplay.ascx" %>

<div class="ResponsesSummary">
    <div class="ResponseHeader">
        <div class="SortByHeader Normal">
            <p>
                <asp:Label runat="server" ResourceKey="SortByLabel"/>
            </p>
            <asp:RadioButtonList ID="SortRadioButtonList" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Selected="True" Value="CreationDate" ResourceKey="ResponseDate"/>
                <asp:ListItem Value="FirstName" ResourceKey="FirstName"/>
                <asp:ListItem Value="LastName" ResourceKey="LastName"/>
            </asp:RadioButtonList>
        </div>
        <div class="StatusHeader Normal">
            <p>
                <asp:Label runat="server" resourcekey="Status" />
            </p>
        </div>
    </div>
    <div class="ResponseHeader">
        <p class="Normal"><asp:Label runat="server" resourcekey="Events" /></p>
        <div class="StatusTypeHeaders Normal">
            <p class="ResponseAtt"><asp:Label runat="server" resourcekey="Attending" /></p>
            <p class="ResponseNotAtt"><asp:Label runat="server" resourcekey="NotAttending" /></p>
            <%--<asp:Label runat="server" resourcekey="NoResponse" />--%>
        </div>
    </div>
    <engage:ResponseDisplay ID="responseDisplay" runat="server" />
    <asp:GridView ID="ResponseDetailGrid" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="False" PageSize="10" GridLines="None" CssClass="Normal">
        <HeaderStyle CssClass="ResponseDetailHeader" />
        <SelectedRowStyle CssClass="ResponseDetailItemSelect" />
        <AlternatingRowStyle CssClass="ResponseDetailAltItem" />
        <RowStyle CssClass="ResponseDetailItem" />
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="ResponseMemberName">
                <HeaderStyle Wrap="False"/>
                <FooterStyle Wrap="False"/>
            </asp:BoundField>
            <asp:BoundField DataField="CreationDate" HeaderText="Date">
                <HeaderStyle Wrap="False"/>
                <FooterStyle Wrap="False"/>
            </asp:BoundField>
            <asp:TemplateField HeaderText="Attending">
                <ItemTemplate>
                    <asp:Image CssClass="Normal" runat="server" ImageUrl='<%# GetStatusIcon(Eval("Status")) %>'/>
                </ItemTemplate>
            </asp:TemplateField>
            <%-- <asp:TemplateField HeaderText ="Not Attending">
	        <ItemTemplate>
		        <asp:HyperLink CssClass="Normal"  runat="server" NavigateUrl ='<%# GetDetailUrl(Eval("EventId")) %>' Text ='<%# DataBinder.Eval(Container.DataItem,"NotAttending") %>'></asp:HyperLink>
	        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText ="NoResponse">
	        <ItemTemplate>
		        <asp:HyperLink CssClass="Normal"  runat="server" NavigateUrl ='<%# GetDetailUrl(Eval("EventId")) %>' Text ='<%# DataBinder.Eval(Container.DataItem,"NoResponse") %>'></asp:HyperLink>
	        </ItemTemplate>
        </asp:TemplateField>--%>
        </Columns>
    </asp:GridView>
    <dnn:PagingControl ID="pager" runat="server"/>
    <asp:HyperLink ID="CancelGoHomeLink" runat="server" ImageUrl="~/DesktopModules/EngageEvents/Images/cancel_go_home.gif" />
</div>

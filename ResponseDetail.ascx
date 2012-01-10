<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.ResponseDetail" CodeBehind="ResponseDetail.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="engage" TagName="ResponseDisplay" Src="Display/ResponseDisplay.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

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
    <telerik:RadGrid ID="ResponseDetailGrid" runat="server" AutoGenerateColumns="False" AllowSorting="False" PageSize="10" GridLines="None" CssClass="Normal ResponseDetailGrid" Skin="" EnableViewState="false">
        <ExportSettings ExportOnlyData="true"/>
        <HeaderStyle CssClass="ResponseDetailHeader" />
        <SelectedItemStyle CssClass="ResponseDetailItemSelect" />
        <AlternatingItemStyle CssClass="ResponseDetailAltItem" />
        <ItemStyle CssClass="ResponseDetailItem" />
        <MasterTableView><Columns>
            <telerik:GridBoundColumn HeaderText="Name" DataField="Name" ItemStyle-CssClass="ResponseMemberName">
                <HeaderStyle Wrap="False"/>
                <FooterStyle Wrap="False"/>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn HeaderText="Date" DataField="CreationDate">
                <HeaderStyle Wrap="False"/>
                <FooterStyle Wrap="False"/>
            </telerik:GridBoundColumn>
            <telerik:GridTemplateColumn HeaderText="Attending" UniqueName="Status">
                <ItemTemplate>
                    <asp:Image CssClass="Normal" runat="server" ImageUrl='<%# GetStatusIcon(Eval("Status")) %>' AlternateText='<%#Localize(Eval("Status").ToString()) %>'/>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridBoundColumn HeaderText="Attending" UniqueName="ExportStatus" DataField="Status" Visible="false" />
        </Columns></MasterTableView>
    </telerik:RadGrid>
    <dnn:PagingControl ID="ResponsePager" runat="server"/>
    <asp:HyperLink ID="CancelGoHomeLink" runat="server">
        <asp:Image ID="CancelGoHomeImage" runat="server" ImageUrl="Images/cancel_go_home.gif" />
    </asp:HyperLink>
    <asp:ImageButton ID="ExportToCsvButton" runat="server" ImageUrl="Images/export_to_csv.gif" />
    <asp:ImageButton ID="ExportToExcelButton" runat="server" ImageUrl="Images/export_to_excel.gif" />
</div>

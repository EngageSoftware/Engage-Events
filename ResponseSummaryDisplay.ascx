<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.ResponseSummaryDisplay" CodeBehind="ResponseSummaryDisplay.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="engage" TagName="ResponseDisplay" Src="Display/ResponseDisplay.ascx" %>

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
        <HeaderTemplate><ol start="<%=StartIndex %>"></HeaderTemplate>
        <ItemTemplate>
            <li>
                <engage:ResponseDisplay ID="ResponseDisplay" runat="server" />
            </li>
        </ItemTemplate>
        <FooterTemplate></ol></FooterTemplate>
    </asp:Repeater>
    <dnn:PagingControl ID="SummaryPager" runat="server" PageSize="10" />
    <asp:HyperLink ID="CancelGoHomeLink" runat="server">
        <asp:Image ID="CancelGoHomeImage" runat="server" ImageUrl="Images/cancel_go_home.gif" />
    </asp:HyperLink>
    <asp:ImageButton ID="ExportToExcelButton" runat="server" ImageUrl="Images/export_to_excel.gif" />
    <asp:ImageButton ID="ExportToCsvButton" runat="server" ImageUrl="Images/export_to_csv.gif" />
</div>

<telerik:RadGrid ID="ReportExportGrid" runat="server" Visible="false" AutoGenerateColumns="false">
    <ExportSettings ExportOnlyData="true" />
    <MasterTableView>
        <Columns>
            <telerik:GridBoundColumn HeaderText="CategoryName" DataField="CategoryName"/>
            <telerik:GridBoundColumn HeaderText="EventTitle" DataField="EventTitle" />
            <telerik:GridBoundColumn HeaderText="EventStartDate" DataField="EventStart" DataFormatString="{0:d}" />
            <telerik:GridBoundColumn HeaderText="EventStartTime" DataField="EventStart" DataFormatString="{0:t}" />
            <telerik:GridBoundColumn HeaderText="EventEndDate" DataField="EventEnd" DataFormatString="{0:d}" />
            <telerik:GridBoundColumn HeaderText="EventEndTime" DataField="EventEnd" DataFormatString="{0:t}" />
            <telerik:GridBoundColumn HeaderText="ResponderName" DataField="ResponderName"/>
            <telerik:GridBoundColumn HeaderText="ResponderEmail" DataField="Email" />
            <telerik:GridBoundColumn HeaderText="ResponseDate" DataField="ResponseDate" DataFormatString="{0:g}" />
            <telerik:GridBoundColumn HeaderText="ResponseStatus" DataField="Status" />
        </Columns>
    </MasterTableView>
</telerik:RadGrid>
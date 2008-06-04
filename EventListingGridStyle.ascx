<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventListingGridStyle"
    Codebehind="EventListingGridStyle.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<dnn:PagingControl ID="pager" runat="server"></dnn:PagingControl>
<asp:DataGrid ID="grdEvents" runat="server" Font-Size="X-Small" Font-Names="Verdana"
    Width="100%" Height="72px" AutoGenerateColumns="False" BorderColor="#999999"
    BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Vertical"
    AllowSorting="True" OnSortCommand="grdEvents_SortCommand" PageSize="10" OnDeleteCommand="grdEvents_DeleteCommand"
    OnEditCommand="grdEvents_EditCommand" DataKeyField="Id" OnSelectedIndexChanged="grdEvents_SelectedIndexChanged">
    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
    <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"
        PageButtonCount="15"></PagerStyle>
    <AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
    <ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
    <Columns>
        <asp:BoundColumn Visible="False" DataField="Id" ReadOnly="True" SortExpression="EventId">
        </asp:BoundColumn>
        <asp:BoundColumn DataField="Title" HeaderText="Title" SortExpression="Title">
            <HeaderStyle Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="True"></ItemStyle>
            <FooterStyle Wrap="False"></FooterStyle>
        </asp:BoundColumn>
        <asp:BoundColumn DataField="Location" ReadOnly="True" HeaderText="Location" SortExpression="Location">
            <HeaderStyle Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="False"></ItemStyle>
            <FooterStyle Wrap="False"></FooterStyle>
        </asp:BoundColumn>
        <asp:BoundColumn DataField="EventStart" ReadOnly="True" HeaderText="Date" SortExpression="EventStart">
            <HeaderStyle Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="False"></ItemStyle>
            <FooterStyle Wrap="False"></FooterStyle>
        </asp:BoundColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete"></asp:LinkButton>&nbsp;
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:LinkButton ID="lnkiCal" runat="server" CommandName="SaveICAL" Text="Add to Celendar"
                    OnClick="lnkAddToCalendar_Click"></asp:LinkButton>&nbsp;
            </ItemTemplate>
            <ItemStyle Wrap="False" />
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:LinkButton ID="lnkEmailEdit" runat="server" CommandName="SaveICAL" Text="Add Email"
                    OnClick="lnkEmailEdit_Click"></asp:LinkButton>&nbsp;
            </ItemTemplate>
            <ItemStyle Wrap="False" />
        </asp:TemplateColumn>
    </Columns>
    <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#E0E0E0"></HeaderStyle>
</asp:DataGrid>
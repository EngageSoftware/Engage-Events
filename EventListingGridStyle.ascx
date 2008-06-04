<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventListingGridStyle"
    Codebehind="EventListingGridStyle.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<div class="AdminButtons">
        <asp:LinkButton ID="lbSettings" runat="server" onclick="lbSettings_OnClick">Settings</asp:LinkButton>
        <asp:LinkButton ID="lbManageEvents" runat="server" onclick="lbManageEvents_OnClick">Manage Events</asp:LinkButton>
        <asp:LinkButton ID="lbAddAnEvent" runat="server" OnClick="lbAddAnEvent_OnClick">Add An Event</asp:LinkButton>
        <asp:LinkButton ID="lbManageEmail" runat="server" Visible="False" OnClick="lbManageEmail_OnClick">Manage E-Mail</asp:LinkButton>
        <asp:LinkButton ID="lbManageRsvp" runat="server" onclick="lbManageRsvp_OnClick">Rsvp</asp:LinkButton>
    </div>
    
<dnn:PagingControl ID="pager" runat="server"></dnn:PagingControl>
<asp:DataGrid ID="grdEvents" runat="server" Font-Size="X-Small" Font-Names="Verdana"
    Width="100%" Height="72px" AutoGenerateColumns="False" BorderColor="#999999"
    BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Vertical"
    AllowSorting="True" OnSortCommand="grdEvents_SortCommand" PageSize="10" OnDeleteCommand="grdEvents_DeleteCommand"
    OnEditCommand="grdEvents_EditCommand" DataKeyField="Id" 
    OnItemDataBound="grdEvents_ItemDataBound">
    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
    <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"
        PageButtonCount="15"></PagerStyle>
    <AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
    <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#E0E0E0"></HeaderStyle>
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
                <asp:LinkButton ID="lnkiCal" runat="server" CommandName="SaveICAL" Text="Add to Celendar"
                    OnClick="lnkAddToCalendar_OnClick"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" Visible="<%#IsAdmin %>"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete"  Visible="<%#IsAdmin %>"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
       <%-- <asp:TemplateColumn>
            <ItemTemplate>
                <asp:LinkButton ID="lnkEmailEdit" runat="server" CommandName="SaveICAL" Text="Add Email"
                    OnClick="lbEditEmail_OnClick"  Visible="<%#IsAdmin %>"></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle Wrap="False" />
        </asp:TemplateColumn>--%>
    </Columns>
    
</asp:DataGrid>
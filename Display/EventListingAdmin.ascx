<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.EventListingAdmin" CodeBehind="EventListingAdmin.ascx.cs" EnableViewState="true" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register Src="../Navigation/EventAdminActions.ascx" TagName="actions" TagPrefix="engage" %>

<div class="EventHeader">
    <h4 class="NormalBold">
        <asp:Label runat="server" ResourceKey="MyEventsLabel" /></h4>
    <asp:Label ID="SortByLabel" runat="server" CssClass="NormalBold" ResourceKey="SortByLabel" />
    <asp:RadioButtonList ID="SortRadioButtonList" runat="server" AutoPostBack="True" CssClass="Normal" RepeatDirection="Horizontal">
        <asp:ListItem Selected="True" Value="EventStart" ResourceKey="DateListItem" />
        <asp:ListItem Value="Title" ResourceKey="TitleListItem" />
    </asp:RadioButtonList>
    <div>
        <asp:Label ID="StatusLabel" runat="server" CssClass="NormalBold" ResourceKey="StatusLabel" />
        <asp:RadioButtonList ID="StatusRadioButtonList" runat="server" AutoPostBack="True" CssClass="Normal" RepeatDirection="Horizontal">
            <asp:ListItem Selected="True" Value="Active" ResourceKey="ActiveListItem"/>
            <asp:ListItem Value="All" ResourceKey="AllListItem"/>
        </asp:RadioButtonList>
    </div>
</div>
<asp:Repeater runat="server" ID="EventListingRepeater" >
    <ItemTemplate>
        <asp:Label ID="IdLabel" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Id")  %>'/>
        <div class="EventTitle">
            <h2 class="Head">
                <%# DataBinder.Eval(Container.DataItem, "Title")  %>
            </h2>
        </div>
        <div class="EventDate">
            <div class="NormalBold">
                <asp:Label runat="server" ResourceKey="When"/></div>
            <div class="Normal">
                <%# DataBinder.Eval(Container.DataItem, "EventStartFormatted")%>
            </div>
        </div>
        <div class="EventLocation">
            <div class="NormalBold">
                <asp:Label runat="server" ResourceKey="Where"/>
            </div>
            <div class="Normal">
                <%# DataBinder.Eval(Container.DataItem, "Location")  %>
            </div>
        </div>
        <div class="EventDescription">
            <div class="NormalBold">
                <asp:Label runat="server" ResourceKey="Description"/>
            </div>
            <div class="Normal">
                <%# DataBinder.Eval(Container.DataItem, "Overview")  %>
            </div>
        </div>
        <engage:actions ID="EventActions" runat="server" OnCancel="EventActions_Cancel" OnDelete="EventActions_Delete" />
    </ItemTemplate>
</asp:Repeater>

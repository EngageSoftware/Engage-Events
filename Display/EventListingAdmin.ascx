<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.EventListingAdmin" CodeBehind="EventListingAdmin.ascx.cs" EnableViewState="true" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register Src="../Navigation/EventAdminActions.ascx" TagName="actions" TagPrefix="engage" %>

<div class="MEEventHeader">
    <h2 class="MEvents SuhHead"><asp:Label ID="Label1" runat="server" ResourceKey="MyEventsLabel" /></h2>
	<div class="METop">
        <div class="MESorting">
            <asp:Label ID="SortByLabel" runat="server" CssClass="NormalBold" ResourceKey="SortByLabel" />
            <asp:RadioButtonList ID="SortRadioButtonList" runat="server" AutoPostBack="True" CssClass="Normal" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="EventStart" ResourceKey="DateListItem" />
                <asp:ListItem Value="Title" ResourceKey="TitleListItem" />
            </asp:RadioButtonList>
        </div>        
        <div class="MEStatus">
            <asp:Label ID="StatusLabel" runat="server" CssClass="NormalBold" ResourceKey="StatusLabel" />
            <asp:RadioButtonList ID="StatusRadioButtonList" runat="server" AutoPostBack="True" CssClass="Normal" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="Active" ResourceKey="ActiveListItem"/>
                <asp:ListItem Value="All" ResourceKey="AllListItem"/>
            </asp:RadioButtonList>
        </div>
	</div>
</div>
<asp:Repeater runat="server" ID="EventListingRepeater" >
    <ItemTemplate>
        <asp:Label ID="IdLabel" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Id")  %>'/>
		<div id="EventItem">
            <div class="EventTitle">
                <h2 class="Head"><%# DataBinder.Eval(Container.DataItem, "Title")  %></h2>
            </div>
            <div class="EventDate">
                <p class="NormalBold"><asp:Label runat="server" ResourceKey="When"/></p>
                <p class="Normal"><%# DataBinder.Eval(Container.DataItem, "EventStartFormatted")%><span class="RecurrenceSummary"><%#GetRecurrenceSummary(Container.DataItem)%></span></p>
            </div>
            <div class="EventLocation">
                <p class="NormalBold"><asp:Label runat="server" ResourceKey="Where"/></p>
                <p class="Normal"><%# DataBinder.Eval(Container.DataItem, "Location")  %></p>
            </div>
            <div class="EventDescription">
                <p class="NormalBold"><asp:Label runat="server" ResourceKey="Description"/></p>
                <div class="Normal"><%# DataBinder.Eval(Container.DataItem, "Overview")  %></div>
            </div>
            <div class="EventButtons"><engage:actions ID="EventActions" runat="server" OnCancel="EventActions_Cancel" OnDelete="EventActions_Delete" /></div><%-- Cancel and Delete must be wired-up here, rather than in the code behind.  Otherwise they get wired-up too late in the page lifecycle. --%>
		</div>
    </ItemTemplate>
</asp:Repeater>
<dnn:PagingControl ID="PagingControl" runat="server" PageSize="10"  />

<%--<telerik:RadWindowManager runat="server">
    <windows>
        <telerik:radwindow ID="EditTypeDialogWindow" runat="server" InitialBehaviors="close" NavigateUrl="../Controls/EditTypeDialog.aspx" ClientCallBackFunction="EngageEvents.EditTypeDialogWindow_Callback" />
    </windows>
</telerik:RadWindowManager>--%>
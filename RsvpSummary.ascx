<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.RsvpSummary" CodeBehind="RsvpSummary.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Import Namespace="DotNetNuke.Services.Localization"%>

<div class="EventHeader">
    <asp:Label ID="SortByLabel" runat="server" CssClass="NormalBold" Resourcekey="SortByLabel" />
    <asp:RadioButtonList ID="SortRadioButtonList" runat="server" AutoPostBack="True"
        CssClass="Normal" RepeatDirection="Horizontal" OnSelectedIndexChanged="SortRadioButtonList_SelectedIndexChanged">
        <asp:ListItem Selected="True" Value="EventStart" resourcekey="EventStart" />
        <asp:ListItem Value="Title" resourcekey="Title" />
    </asp:RadioButtonList>
    <br />
    <h4 class="NormalBold">
        <asp:Label runat="server" resourcekey="Events" /><br />
        <asp:Label runat="server" resourcekey="Status" />
    </h4>
</div>
<asp:Label ID="NoRsvpsMessageLabel" runat="server" resourcekey="NoRsvpsMessageLabel" />
<asp:Repeater runat="server" ID="SummaryRepeater">
    <ItemTemplate>
        <asp:Label ID="lblId" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EventId")  %>'/>
        <h2 class="Head">
            <%# DataBinder.Eval(Container.DataItem, "Title")%>
        </h2>
        <div class="EventDate">
            <div class="SubHead">
                <asp:Label runat="server" resourcekey="When" />
            </div>
            <div class="Normal">
                <%# DataBinder.Eval(Container.DataItem, "EventStartFormatted")%>
            </div>
        </div>
        <div class="EventStats">
            <asp:HyperLink runat="server" NavigateUrl='<%# GetDetailUrl(DataBinder.Eval(Container.DataItem, "EventId"), "NotAttending", DataBinder.Eval(Container.DataItem,"NotAttending")) %>' Text='<%# DataBinder.Eval(Container.DataItem,"NotAttending") %>'/>
            <asp:HyperLink runat="server" NavigateUrl='<%# GetDetailUrl(DataBinder.Eval(Container.DataItem, "EventId"), "Attending", DataBinder.Eval(Container.DataItem,"Attending")) %>' Text='<%# DataBinder.Eval(Container.DataItem,"Attending") %>' />
            <%--<asp:HyperLink runat="server" NavigateUrl='<%# GetDetailUrl(DataBinder.Eval(Container.DataItem, "EventId"), "NoResponse", DataBinder.Eval(Container.DataItem,"NoResponse")) %>' Text='<%# DataBinder.Eval(Container.DataItem,"NoResponse") %>' />--%>
        </div>
    </ItemTemplate>
</asp:Repeater>
<dnn:PagingControl ID="SummaryPager" runat="server"></dnn:PagingControl>
<asp:HyperLink ID="ExitLink" runat="server" ImageUrl="~/DesktopModules/EngageEvents/Images/exit.gif"/>
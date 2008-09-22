<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.EventListingItem"  CodeBehind="EventListingItem.ascx.cs" %>

<style type="text/css">
    @import url(<%=ApplicationUrl %><%=DesktopModuleFolderName %>Module.css);
</style>

<asp:UpdatePanel runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:PlaceHolder ID="HeaderPlaceholder" runat="server"/>
        <asp:PlaceHolder ID="ItemPlaceholder" runat="server" />
        <asp:PlaceHolder ID="FooterPlaceholder" runat="server"/>
    </ContentTemplate>
</asp:UpdatePanel>

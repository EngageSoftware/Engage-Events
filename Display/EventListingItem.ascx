<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.EventListingItem"  CodeBehind="EventListingItem.ascx.cs" %>

<style type="text/css">
    @import url(<%=ApplicationUrl %><%=DesktopModuleFolderName %>Module.css);
</style>

<asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
    <ContentTemplate>
        <asp:PlaceHolder ID="PlaceHolderHeader" runat="server"></asp:PlaceHolder>
        <asp:Repeater ID="RepeaterEvents" runat="server" OnItemDataBound="RepeaterEvents_ItemDataBound">
      </asp:Repeater>
        <asp:PlaceHolder ID="PlaceHolderFooter" runat="server"></asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>

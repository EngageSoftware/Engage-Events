<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Display.EventDetail"  CodeBehind="EventDetail.ascx.cs" %>
<%@ Register src="EventListingItem.ascx" tagname="EventListingItem" tagprefix="uc1" %>

<style type="text/css">
    @import url(<%=ApplicationUrl %><%=DesktopModuleFolderName %>Module.css);
</style>

<asp:PlaceHolder ID="DetailPlaceHolder" runat="server"></asp:PlaceHolder>
<asp:HyperLink ID="BackHyperLink" runat="server"></asp:HyperLink>

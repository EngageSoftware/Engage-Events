<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.MainContainer" Codebehind="MainContainer.ascx.cs" %>
<%@ Register src="Navigation/GlobalNavigation.ascx" tagname="GlobalNavigation" tagprefix="engage" %>

<div class="GlobalNavigation">
    <engage:GlobalNavigation runat="server" />
</div>

<asp:PlaceHolder id="ControlsPlaceholder" runat="Server" />
<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.CalendarDisplayOptions" Codebehind="CustomDisplayOptions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<div class="EventsSetting">
    <dnn:label id="SkinLabel" runat="server" controlname="SkinDropDownList" ResourceKey="SkinLabel" CssClass="SubHead"/>
    <asp:dropdownlist id="SkinDropDownList" Runat="server"/>
</div>
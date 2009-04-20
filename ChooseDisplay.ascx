<%@ Control Language="c#" Codebehind="ChooseDisplay.ascx.cs" Inherits="Engage.Dnn.Events.ChooseDisplay" AutoEventWireup="false" %>
<%@ Register TagPrefix="engage" TagName="TemplatePicker" Src="Controls/TemplatePicker.ascx" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelcontrol.ascx" %>
<div class="Normal">
    <div class="EventsSetting">
        <h4><asp:Label runat="server" resourcekey="ChooseDisplayType.Text" /></h4>
        <asp:DropDownList ID="ChooseDisplayDropDown" CssClass="NormalTextBox" runat="server" AutoPostBack="True" />
    </div>

    <asp:PlaceHolder ID="TemplatePickersSection" runat="server">
        <div>
        <h4><asp:Label runat="server" resourcekey="List Template.Text" /></h4>
        <engage:TemplatePicker ID="ListTemplatePicker" runat="server" TemplateType="List" />
        </div>
        <div>
        <h4><asp:Label runat="server" resourcekey="Single Item Template.Text" /></h4>
        <engage:TemplatePicker ID="SingleItemTemplatePicker" runat="server" TemplateType="SingleItem" />
        </div>
    </asp:PlaceHolder>
    
    <asp:Button ID="SubmitButton" runat="server" resourcekey="Submit" EnableViewState="false" />&nbsp;
    <asp:Button ID="CancelButton" runat="server" resourcekey="Cancel" CausesValidation="false" EnableViewState="false" />
</div>
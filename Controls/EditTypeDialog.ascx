<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Controls.EditTypeDialog" CodeBehind="EditTypeDialog.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:radwindow runat="server">
    <div class="eventEditTypeDialog">
        <h3><%=Localize("Editing a Recurring Event.Text") %></h3>
        <ul>
            <li>
                <asp:RadioButton ID="EditOccurrenceItem" runat="server" ResourceKey="Edit this Occurrence.Text" GroupName="EditType" Checked="True" />
            </li>
            <li>
                <asp:RadioButton ID="EditSeriesItem" runat="server" ResourceKey="Edit the Series.Text" GroupName="EditType" />
            </li>
        </ul>
        <div>
            <asp:Button ID="OKButton" runat="server" ResourceKey="OK" />
	        <asp:Button ID="CancelButton" runat="server" CausesValidation="false" ResourceKey="Cancel" />
	    </div>
    </div>    
</telerik:radwindow>
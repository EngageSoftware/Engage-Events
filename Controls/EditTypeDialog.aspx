<%@ Page Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Controls.EditTypeDialog" CodeBehind="EditTypeDialog.aspx.cs" %>
<%@ Import Namespace="DotNetNuke.Services.Localization"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="EngageEvents" Name="Engage.Dnn.Events.JavaScript.EngageEvents.EditTypeDialog.js" />
            </Scripts>
        </asp:ScriptManager>
    </form>
    <div class="eventEditTypeDialog">
        <h3><%=Localization.GetString("Editing a Recurring Event.Text", LocalResourceFile) %></h3>
        <ul>
            <li>
                <input type="radio" id='EditOccurrenceItem' name="EditType" checked="checked" /> <label for="EditOccurrenceItem"><%=Localization.GetString("Edit this Occurrence.Text", LocalResourceFile)%></label>
            </li>
            <li>
                <input type="radio" id='EditSeriesItem' name="EditType" checked="checked" /> <label for="EditSeriesItem"><%=Localization.GetString("Edit the Series.Text", LocalResourceFile)%></label>
            </li>
        </ul>
        <div>
            <button id="OKButton" onclick="EngageEvents.OK_Clicked('EditOccurrenceItem');return false;"><%=Localization.GetString("OK.Text", LocalResourceFile)%></button>
            <button id="CancelButton" onclick="EngageEvents.Cancel_Clicked();return false;"><%=Localization.GetString("Cancel.Text", LocalResourceFile)%></button>
        </div>
    </div>
</body>
</html>
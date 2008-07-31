<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Controls.RecurrenceEditor" CodeBehind="RecurrenceEditor.ascx.cs" %>
<%@ Register Src="../Controls/RecurrenceSelector.ascx" TagName="RecurrenceSelector" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<link rel="stylesheet" type="text/css" href="../Module.css" />
<div>
    <asp:CheckBox ID="RecurringCheckbox" runat="server" CssClass="NormalBold" Text="Recurring Event" AutoPostBack="true" />
</div>
&nbsp;
<div id="RecurrenceEditorDiv" runat="server" visible ="false">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="recurrenceleftNav" class="recurrenceleftNav" style="float: left; width: 100px; height: 250px; border-right: thin groove; border-top: thin groove; border-bottom: thin groove;">
                <uc1:RecurrenceSelector ID="RecurrenceSelector1" runat="server"></uc1:RecurrenceSelector>
            </div>
            <div id="recurrenceCenter" class="recurrenceCenter" style="position: relative; left: 105px; width: 500; height: 250px; border-right: thin groove; border-top: thin groove; border-bottom: thin groove;">
                <br />
                <asp:PlaceHolder ID="phRecurrencePattern" runat="Server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;
    <div>
        <asp:Label ID="RecurrenceRangeLabel" runat="server" CssClass="NormalBold">Range of recurrence:</asp:Label>
    </div>
    &nbsp;
    <div id="rangeGroup" style="border-right: thin groove; border-top: thin groove; border-left: thin groove; border-bottom: thin groove; width: 605px; height: 170;">
        <div class="RangeStartDate">
            <asp:Label ID="Label1" runat="server" ResourceKey="RangeStartDateLabel" CssClass="NormalBold" />
            <telerik:raddatetimepicker runat="server" id="StartDateTimePicker" skin="WebBlue">
            <timeview skin="WebBlue"/>
            <calendar skin="WebBlue"/>
            <DateInput InvalidStyleDuration="100"/>
        </telerik:raddatetimepicker>
        </div>
        <%--<asp:TextBox ID="startTime" runat="server" CssClass="NormalTextBox"></asp:TextBox>--%>
        <asp:RadioButtonList ID="endRadio" runat="server" AutoPostBack="True" CssClass="Normal">
            <asp:ListItem Value="0">No end</asp:ListItem>
            <asp:ListItem Value="2">End after:</asp:ListItem>
            <asp:ListItem Value="1">End by:</asp:ListItem>
        </asp:RadioButtonList>
        <br />
        <%--    <asp:Label ID="EndDateLabel" runat="server" Text="End Date:" CssClass="Normal"></asp:Label><asp:TextBox ID="endDate" runat="server" CssClass="NormalTextBox"></asp:TextBox>
--%>
        <div class="RangeStartDate">
            <asp:Label ID="EndDateLabel" runat="server" ResourceKey="RangeEndDateLabel" CssClass="NormalBold" />
            <telerik:raddatetimepicker runat="server" id="EndDateTimePicker" skin="WebBlue">
            <timeview skin="WebBlue"/>
            <calendar skin="WebBlue"/>
            <DateInput InvalidStyleDuration="100"/>
        </telerik:raddatetimepicker>
        </div>
        <asp:TextBox ID="endMaxOccurrencesEdit" runat="server" CssClass="NormalTextBox"></asp:TextBox>
        <asp:Label ID="Label6" runat="server" CssClass="Normal">occurrences</asp:Label>
    </div>
</div>

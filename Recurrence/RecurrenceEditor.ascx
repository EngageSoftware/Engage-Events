<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Recurrence.RecurrenceEditor" CodeBehind="RecurrenceEditor.ascx.cs" %>
<%@ Register Src="../Recurrence/RecurrenceSelector.ascx" TagName="RecurrenceSelector" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div id="RecurrenceEditorDiv" runat="server" style="width: 100%;">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="recurrenceleftNav" style="width: 20%; float: left;">
                <uc1:RecurrenceSelector ID="RecurrenceSelector" runat="server" />
            </div>
            <div class="recurrenceCenter" style="width: 80%; float: right;">
                <asp:PlaceHolder ID="RecurrencePatternPlaceHolder" runat="Server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:Label runat="server" CssClass="NormalBold" ResourceKey="RangeOfRecurrence" />
    
    <div style="clear: both; padding-top: 20px;">
        <div class="RangeStartDate">
            <asp:Label runat="server" ResourceKey="StartDate" CssClass="NormalBold" />
            <telerik:raddatetimepicker runat="server" id="StartDateTimePicker" skin="WebBlue">
                <timeview skin="WebBlue" />
                <calendar skin="WebBlue" />
                <DateInput InvalidStyleDuration="100" />
            </telerik:raddatetimepicker>
        </div>
        
        <asp:RadioButtonList ID="endRadio" runat="server" AutoPostBack="True" CssClass="Normal" RepeatDirection="Horizontal">
            <asp:ListItem Value="0" ResourceKey="NoEnd" />
            <asp:ListItem Value="1" ResourceKey="EndAfter" />
            <asp:ListItem Value="2" ResourceKey="EndBy" />
        </asp:RadioButtonList>
                
        <div class="RangeEndDate">
            <asp:Label runat="server" ResourceKey="RangeEndDate" CssClass="NormalBold" />
            <telerik:raddatetimepicker runat="server" id="EndDateTimePicker" skin="WebBlue">
                <timeview skin="WebBlue"/>
                <calendar skin="WebBlue"/>
                <DateInput InvalidStyleDuration="100"/>
            </telerik:raddatetimepicker>
        </div>
        
        <asp:TextBox ID="endMaxOccurrencesEdit" runat="server" CssClass="NormalTextBox" />
        <asp:Label runat="server" CssClass="Normal" ResourceKey="Occurences" />
    </div>
</div>

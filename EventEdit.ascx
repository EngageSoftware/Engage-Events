<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EventEdit" Codebehind="EventEdit.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="sectionheadcontrol" Src="~/controls/sectionheadcontrol.ascx" %>
<%@ Register src="Navigation/GlobalNavigation.ascx" tagname="GlobalNavigation" tagprefix="uc1" %>

<uc1:GlobalNavigation ID="GlobalNavigation1" runat="server" />
<br />                
<br />

<ajaxToolkit:CalendarExtender ID="defaultCalendarExtender" runat="server" TargetControlID="txtEventDate" PopupButtonID="imgCalendarButton" />
<ajaxToolkit:TextBoxWatermarkExtender ID="txtWatermarkEventTitle" runat="server" TargetControlID="txtEventTitle" WatermarkText="Please enter event title" WatermarkCssClass="watermarked" />
<ajaxToolkit:TextBoxWatermarkExtender ID="txtWatermarkEventDate" runat="server" TargetControlID="txtEventDate" WatermarkText="Pick a date" WatermarkCssClass="watermarked" />
<ajaxToolkit:TextBoxWatermarkExtender ID="txtWatermarkEventTime" runat="server" TargetControlID="txtEventTime" WatermarkText="Pick a time" WatermarkCssClass="watermarked" />
<ajaxToolkit:TextBoxWatermarkExtender ID="txtWatermarkEventLocation" runat="server" TargetControlID="txtEventLocation" WatermarkText="Please enter a detailed location" WatermarkCssClass="watermarked" />

<div id="AddNewEvent">

 <%--   <div class="AdminButtons">
        <asp:LinkButton ID="lbSettings" runat="server" onclick="lbSettings_OnClick">Settings</asp:LinkButton>
        <asp:LinkButton ID="lbManageEvents" runat="server" onclick="lbManageEvents_OnClick">Manage Events</asp:LinkButton>
        <asp:LinkButton ID="lbAddAnEvent" runat="server" OnClick="lbAddAnEvent_OnClick">Add An Event</asp:LinkButton>
        <asp:LinkButton ID="lbManageEmail" runat="server" Visible="False" OnClick="lbManageEmail_OnClick">Manage E-Mail</asp:LinkButton>
        <asp:LinkButton ID="lbManageRsvp" runat="server" onclick="lbManageRsvp_OnClick">Rsvp</asp:LinkButton>
    </div>--%>

    <h2 class="Head">
        <asp:Label ID="lblAddNewEvent" runat="server">Add A New Event</asp:Label>
    </h2>
    
    <div class="EventTitle">
        <asp:Label ID="lblEventTitle" runat="server">Event Title</asp:Label>
        <asp:TextBox ID="txtEventTitle" runat="server"></asp:TextBox>
    </div>
    
    <div class="EventDate">
    
        <div>
            <asp:Label ID="lblEventDate" runat="server">When</asp:Label>
            <asp:TextBox ID="txtEventDate" runat="server" ></asp:TextBox>
            <asp:ImageButton runat="server" ID="imgCalendarButton" ImageUrl="~/desktopmodules/engageevents/Images/vcalendar.png" Height="20" Width="20" AlternateText="Click to show calendar" />          
        </div>

        <div>
            <asp:Label ID="lblEventTime" runat="server">At</asp:Label>
            <asp:TextBox ID="txtEventTime" runat="server"></asp:TextBox>
        </div>

        <div>
            <asp:RadioButtonList ID="rblEventTime" runat="server">
                <asp:ListItem Selected="True" Value="AM" Text="AM"></asp:ListItem>
                <asp:ListItem Selected="False" Value="PM" Text="PM"></asp:ListItem>
            </asp:RadioButtonList>
        </div>  

    </div>
    
    <div class="EventLocation">
        <asp:Label ID="lblEventLocation" runat="server">Where</asp:Label>
        <asp:TextBox ID="txtEventLocation" runat="server"></asp:TextBox>
    </div>
    
    <div class="EventEditor">
        <asp:Label ID="lblDescription" runat="server">Description</asp:Label>
        <dnn:TextEditor ID="txtEventDescription" runat="server" HtmlEncode="false" ChooseRender="false" TextRenderMode="Text"></dnn:TextEditor>
    </div>
    
    <div class="AddEventFooterButtons">
        <asp:LinkButton ID="lbSave" runat="server" OnClick="lbSave_OnClick">Save</asp:LinkButton>
        <asp:LinkButton ID="lbCancel" runat="server" OnClick="lbCancel_OnClick">Cancel</asp:LinkButton>
        <asp:LinkButton ID="lbSaveAndCreateNew" runat="server" OnClick="lbSaveAndCreateNew_OnClick">Save & Create New</asp:LinkButton>
    </div>

</div>
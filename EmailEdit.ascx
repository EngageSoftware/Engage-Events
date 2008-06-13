<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.EmailEdit" Codebehind="EmailEdit.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register src="Navigation/GlobalNavigation.ascx" tagname="GlobalNavigation" tagprefix="uc1" %>
<uc1:GlobalNavigation ID="GlobalNavigation1" runat="server" />
<br />                
<br />

<ajaxToolkit:CalendarExtender ID="defaultCalendarExtender" runat="server" TargetControlID="txtDate" PopupButtonID="imgCalendarButton" />
<%--<ajaxToolkit:TextBoxWatermarkExtender ID="txtWatermarkEventTitle" runat="server" TargetControlID="txtEventTitle" WatermarkText="Please enter event title" WatermarkCssClass="watermarked" />
<ajaxToolkit:TextBoxWatermarkExtender ID="txtWatermarkEventDate" runat="server" TargetControlID="txtEventDate" WatermarkText="Pick a date" WatermarkCssClass="watermarked" />
<ajaxToolkit:TextBoxWatermarkExtender ID="txtWatermarkEventTime" runat="server" TargetControlID="txtEventTime" WatermarkText="Pick a time" WatermarkCssClass="watermarked" />
<ajaxToolkit:TextBoxWatermarkExtender ID="txtWatermarkEventLocation" runat="server" TargetControlID="txtEventLocation" WatermarkText="Please enter a detailed location" WatermarkCssClass="watermarked" />
--%>

<h2 class="Head">Event: <asp:Label ID="lblEvent" runat="server"></asp:Label></h2>
   
<div class="border" style="LEFT: 16px; WIDTH: 800px; TOP: 8px; HEIGHT: 48px" title="Define Segment">
    <br />
    <asp:Label ID="lblSegment" runat="server" CssClass="NormalBold" Text="Who would you like to send the e-mail to?"></asp:Label>
    <asp:DropDownList ID="ddlRoles" runat="server" /></div>
<div class="border" style="LEFT: 16px; WIDTH: 800px; TOP: 64px; HEIGHT: 112px" title="Define E-Mail">
    <asp:Label ID="lblEmailType" runat="server" CssClass="NormalBold" Text="Please select e-mail you would like to send:"></asp:Label>
    <asp:RadioButtonList ID="rblEmailType" runat="server" CssClass="Normal" AutoPostBack="true" RepeatDirection="Horizontal"
        Width="440px" OnSelectedIndexChanged="rblEmailType_SelectedIndexChanged">
        <asp:ListItem Selected="True" Value="Invitation">Invitation</asp:ListItem>
        <asp:ListItem Value="Reminder">Reminder</asp:ListItem>
        <asp:ListItem Value="Recap">Recap</asp:ListItem>
    </asp:RadioButtonList><br />
    <asp:Label ID="lblApprovalRequired" runat="server" CssClass="NormalBold" Text="Are approvals required?"></asp:Label><br />
    <asp:RadioButtonList ID="rblRequireApproval" runat="server" CssClass="Normal" RepeatDirection="Horizontal">
        <asp:ListItem Selected="True">Yes</asp:ListItem>
        <asp:ListItem>No</asp:ListItem>
    </asp:RadioButtonList>
</div>
<div class="border" style="LEFT: 16px; WIDTH: 800px; TOP: 208px; HEIGHT: 220px" title="Create e-mail">
    <br />
    <asp:Label ID="lblSubject" runat="server" CssClass="NormalBold" Text="Subject:"></asp:Label>
    <asp:TextBox ID="txtSubject" runat="server" Width="280px"></asp:TextBox>
    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/up.gif" />
    
    <asp:Label ID="lblApprovers" runat="server" CssClass="NormalBold" Text="Approvers:"></asp:Label>
    <asp:TextBox ID="txtApprovers" runat="server" Width="280px"></asp:TextBox>
    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/up.gif" />
    <br />
    <asp:Label ID="lblFrom" runat="server" CssClass="NormalBold" Text="From Name:"></asp:Label>
    <asp:TextBox ID="txtFrom" runat="server" Width="280px"></asp:TextBox>
    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/up.gif" />
    <asp:Label ID="lblFromEmail" runat="server" CssClass="NormalBold" Text="From Email:"></asp:Label>
    <asp:TextBox ID="txtFromEmail" runat="server" Width="280px"></asp:TextBox>
    <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/up.gif" />
    <br />
    <asp:Label ID="lblLocation" runat="server" CssClass="NormalBold" Text="Locate e-mail body:"></asp:Label>
    <asp:TextBox ID="txtLocation" runat="server" Width="280px"></asp:TextBox>
    <asp:ImageButton ID="btnBrowse" runat="server" ImageUrl="~/Images/view.gif" />
    <br />
    <div class="Normal" id="dvReminderOnly" visible="false" runat="server">
        <asp:Label ID="lblLocation2" runat="server" CssClass="NormalBold" Text="Locate e-mail body:"></asp:Label>
        <asp:TextBox ID="txtLocation2" runat="server" Width="280px"></asp:TextBox>
        <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Images/view.gif" />
        <br />
        <asp:Label ID="lblLocation3" runat="server" CssClass="NormalBold" Text="Locate e-mail body:"></asp:Label>
        <asp:TextBox ID="txtLocation3" runat="server" Width="280px"></asp:TextBox>
        <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/Images/view.gif" />
        <br />
    </div>

    <asp:CheckBox ID="chkShowRichEditor" runat="server" Text="Show Rich Text Editor" CssClass="NormalBold" />
    <br />
    
    <div class="NormalBold">
    
        <div>
            <asp:Label ID="lblate" runat="server">When</asp:Label>
            <asp:TextBox ID="txtDate" runat="server" ></asp:TextBox>
            <asp:ImageButton runat="server" ID="ImageButton4" ImageUrl="~/desktopmodules/engageevents/Images/vcalendar.png" Height="20" Width="20" AlternateText="Click to show calendar" />          
        </div>

        <div>
            <asp:Label ID="lblTime" runat="server">At</asp:Label>
            <asp:TextBox ID="txtTime" runat="server"></asp:TextBox>
        </div>

        <div>
            <asp:RadioButtonList ID="rblTime" runat="server">
                <asp:ListItem Selected="True" Value="AM" Text="AM"></asp:ListItem>
                <asp:ListItem Selected="False" Value="PM" Text="PM"></asp:ListItem>
            </asp:RadioButtonList>
        </div>  

    </div>     
   <br />
    
</div>
 

<div class="EventButtons">
    <asp:LinkButton ID="lbPreview" runat="server" CssClass="CommandButton" OnClick="lbPreview_OnClick">PREVIEW E-MAIIL</asp:LinkButton>
    <asp:LinkButton ID="lbSendNow" runat="server" CssClass="CommandButton" OnClick="lbSendNow_OnClick">SEND NOW</asp:LinkButton>
    <asp:LinkButton ID="lblSave" runat="server" CssClass="CommandButton" OnClick="lbSave_OnClick">SAVE</asp:LinkButton>
    <asp:LinkButton ID="lbCancel" runat="server" CssClass="CommandButton" OnClick="lbCancel_OnClick">CANCEL</asp:LinkButton>

</div>
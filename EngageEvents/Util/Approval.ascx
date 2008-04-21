<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Approval" Codebehind="Approval.ascx.cs" %>
<div class="border" style="LEFT: 16px; WIDTH: 800px; TOP: 8px; HEIGHT: 48px">
    <br />
    <asp:Label ID="lblSegment" runat="server" CssClass="NormalBold" Text="Who would you like to send the e-mail to?"></asp:Label>
    <asp:ImageButton ID="btnSegment" runat="server" ImageUrl="~/Images/rt.gif" /></div>
<div class="border" style="LEFT: 16px; WIDTH: 800px; TOP: 64px; HEIGHT: 112px">
    <asp:Label ID="lblEmailType" runat="server" CssClass="NormalBold" Text="Please select e-mail you would like to send:"></asp:Label>
    <asp:RadioButtonList ID="rblEmailType" runat="server" CssClass="Normal" RepeatDirection="Horizontal"
        Width="440px">
        <asp:ListItem Selected="True">Invitation</asp:ListItem>
        <asp:ListItem>Reminder</asp:ListItem>
        <asp:ListItem>Recap</asp:ListItem>
    </asp:RadioButtonList><br />
    <asp:Label ID="lblApprovalRequired" runat="server" CssClass="NormalBold" Text="Are approvals required?"></asp:Label><br />
    <asp:RadioButtonList ID="rblRequireApproval" runat="server" CssClass="Normal" RepeatDirection="Horizontal">
        <asp:ListItem Selected="True">Yes</asp:ListItem>
        <asp:ListItem>No</asp:ListItem>
    </asp:RadioButtonList>
</div>
<div class="border" style="LEFT: 16px; WIDTH: 800px; TOP: 208px; HEIGHT: 184px">
    <br />
    <asp:Label ID="lblSubject" runat="server" CssClass="NormalBold" Text="Subject:"></asp:Label>
    <asp:TextBox ID="txtSubject" runat="server" Width="280px"></asp:TextBox>
    &nbsp; &nbsp;
    <asp:Label ID="lblApprovers" runat="server" CssClass="NormalBold" Text="Approvers:"></asp:Label><asp:TextBox
        ID="txtApprovers" runat="server" Width="280px"></asp:TextBox><br />
    <asp:Label ID="lblFrom" runat="server" CssClass="NormalBold" Text="From:"></asp:Label><asp:TextBox
        ID="txtFrom" runat="server" Width="280px"></asp:TextBox><asp:Label ID="lblLocate"
            runat="server" CssClass="NormalBold" Text="Locate e-mail body:"></asp:Label><asp:TextBox
                ID="txtLocated" runat="server" Width="280px"></asp:TextBox>
    <asp:ImageButton ID="btnBrowse" runat="server" ImageUrl="~/Images/up.gif" /></div>

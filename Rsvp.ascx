<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.Rsvp" Codebehind="Rsvp.ascx.cs" %>
<div class="Normal" id="Description" align="left">
    <asp:Label id="lblTitle" Text="Event Rsvp" ResourceKey="lblTitle" CssClass = "NormalBold" runat="server"></asp:Label>
    <br />
    <br />
    <asp:Label id="lblEventName" Text="To RSVP for ... please enter your email address below." CssClass = "NormalBold" runat="server"></asp:Label>
    <br />
    <asp:Label ID="lblEmail" ResourceKey="lblEmail" Text="Email:" runat="server"></asp:Label><asp:TextBox ID="txtEmail" runat="server" OnTextChanged="txtEmail_TextChanged"></asp:TextBox>
</div>
<div class="Normal" id="rbOptions" align="left">
    <asp:RadioButtonList ID="rbRsvp" runat="server" CssClass="Normal">
        <asp:ListItem Selected = "true" Text="Yes, I will be able to Attend" Value="Attending"></asp:ListItem>
        <asp:ListItem Text="No, unfortunately I will not be able to attend." Value="NotAttending"></asp:ListItem>
    </asp:RadioButtonList>
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
    <asp:Button ID="lbAddToCalendar" runat="server" ResourceKey="lbAddToCalendar" Enabled="false" CssClass="CommandButton" OnClick="lbAddToCalendar_OnClick"></asp:Button>
    
</div>

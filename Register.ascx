<%@ Control Language="c#" Codebehind="Register.ascx.cs" Inherits="Engage.Dnn.Events.Register" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="Profile" Src="~/Admin/Users/Profile.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Password" Src="~/Admin/Users/Password.ascx" %>
<%@ Register TagPrefix="dnn" TagName="User" Src="~/Admin/Users/User.ascx" %>
<div class="register">

<div id="div_existingCustomers" class="existing_customer">
    <fieldset>
        <legend class="SubHead">Returning Users</legend>
        <div class="inst">
            <p><asp:Label ID="lblReturningHeading" runat="server" CssClass="NormalBold" resourceKey="lblReturningHeading" Text="Sign in using your existing account"></asp:Label></p>
            <p><asp:Label ID="lblReturningMessage" runat="server" CssClass="Normal" resourceKey="lblReturningMessage" Text="Use a single account to register for all your events."></asp:Label></p>
            <table cellspacing="0" cellpadding="3" border="0" summary="SignIn Design Table" class="altloginTable" width="100%">
                <tr>
                    <td width="150" class="SubHead" align="left">
                        <dnn:label id="plUsername" controlname="txtUsername" runat="server" resourcekey="Username" /></td>
                    <td width="200" align="left">
                        <asp:TextBox ID="txtUsername" Columns="9" Width="120" CssClass="NormalTextBox" runat="server" /></td>
                </tr>
                <tr id="trCaptcha" runat="server" visible="false">
                    <td width="150" class="SubHead" align="left">
                        <dnn:label id="plCaptcha" controlname="ctlCaptcha" runat="server" resourcekey="Captcha" /></td>
                    <td width="200" align="left">
                        <dnn:captchacontrol id="ctlCaptcha" captchawidth="120" captchaheight="40" cssclass="Normal"
                            runat="server" errorstyle-cssclass="NormalRed" /></td>
                </tr>
                <tr>
                    <td width="150" class="SubHead" align="left">
                        <dnn:label id="plPassword" controlname="txtPassword" runat="server" resourcekey="Password" /></td>
                    <td width="200" align="left" valign="middle">
                        <asp:TextBox ID="txtPassword" Columns="9" Width="80" TextMode="password" CssClass="NormalTextBox"
                            runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:ImageButton ID="cmdLogin" ImageUrl="~/DesktopModules/EngageEvents/images/bt_login.gif" alt="click here to login if you have an account" resourcekey="cmdLogin" OnClick="cmdLogin_Click" CssClass="CommandButton" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </fieldset>
</div>
<div id="div_newCustomers" class="new_customers">
    <fieldset>
        <legend class="SubHead">New Users</legend>
        <div class="inst">
            <p><asp:Label ID="lblNewCustomerHeading" runat="server" CssClass="NormalBold" resourceKey="lblNewCustomerHeading" Text="Create an Account"></asp:Label></p>
            <p><asp:Label ID="lblNewCustomerMessage" runat="server" CssClass="Normal" resourceKey="lblNewCustomerMessage" Text="Use a single account to register for all your events."></asp:Label></p>
            <p><asp:ImageButton ID="btnCreate" runat="server" ImageUrl="~/DesktopModules/EngageEvents/images/bt_create_account.gif" alt="click here to register for a new account" OnClick="btnCreate_Click" CssClass="CommandButton" /></p>
        </div>
    </fieldset>
</div>
</div>
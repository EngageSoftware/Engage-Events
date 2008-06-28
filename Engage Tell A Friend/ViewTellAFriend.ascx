<%@ Control language="C#" Inherits="Engage.Dnn.TellAFriend.ViewTellAFriend" AutoEventWireup="false" Codebehind="ViewTellAFriend.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="Controls/ModuleMessage.ascx" %>
<%@ Import Namespace="DotNetNuke.Services.Localization"%>

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.2.6/jquery.min.js"></script>

<script type="text/javascript">

    function ExpressValidate()
    {
        if (typeof(Page_ClientValidate) == 'function')
        {
            var validationResult = Page_ClientValidate('EngageTellAFriend');
            if (!validationResult)
            {
                jQuery(".ErrorModuleMessage").slideDown("slow");
                jQuery(".SuccessModuleMessage").slideUp("slow");
            }
            else
            {
                jQuery(".SuccessModuleMessage").slideDown("slow");
                jQuery(".ErrorModuleMessage").slideUp("slow");
            }
        }
    }

</script>

<%= Localization.GetString("Introduction", LocalResourceFile) %>

<div id="dnnGalleryTellAFriend">
<div class="tafName">

	<p class="tafFirst"><%= Localization.GetString("FirstName", LocalResourceFile) %><br />
	<asp:TextBox ID="FirstNameTextBox" runat="server" Width="80%"></asp:TextBox>
	<asp:RequiredFieldValidator runat="server" ID="FirstNameRequiredValidator" ControlToValidate="FirstNameTextBox" Display="Dynamic" Text="*" ValidationGroup="EngageTellAFriend"></asp:RequiredFieldValidator>
	</p>
	
	<p class="tafLast"><%= Localization.GetString("LastName", LocalResourceFile) %><br />
    <asp:TextBox ID="LastNameTextBox" runat="server" Width="80%"></asp:TextBox>
    <asp:RequiredFieldValidator runat="server" ID="LastNameRequiredValidator" ControlToValidate="LastNameTextBox" Display="Dynamic" Text="*" ValidationGroup="EngageTellAFriend"></asp:RequiredFieldValidator>
    </p>
    
</div>

<p><%= Localization.GetString("EmailAddress", LocalResourceFile) %><br />
	<asp:TextBox ID="SenderEmailTextBox" runat="server" Width="65%"></asp:TextBox>
    <asp:RequiredFieldValidator runat="server" ID="SenderEmailRequiredValidator" ControlToValidate="SenderEmailTextBox" Display="Dynamic" Text="*" ValidationGroup="EngageTellAFriend"></asp:RequiredFieldValidator>
</p>

<p><%= Localization.GetString("RecipientEmailAddress", LocalResourceFile) %><br />
    <asp:TextBox ID="FriendsEmailTextBox" runat="server" Width="65%"></asp:TextBox>
    <asp:RequiredFieldValidator runat="server" ID="FriendsEmailRequiredValidator" ControlToValidate="FriendsEmailTextBox" Display="Dynamic" Text="*" ValidationGroup="EngageTellAFriend"></asp:RequiredFieldValidator>
</p>

<p><%= Localization.GetString("Message", LocalResourceFile) %><br />
	<asp:TextBox ID="MessageTextBox" runat="server" TextMode="MultiLine" Width="65%"  Rows="6" ></asp:TextBox>
</p>

<div class="ErrorModuleMessage" runat="server" id="ErrorModuleMessageDiv" style="display: none;">
	<engage:ModuleMessage runat="server" ID="ErrorModuleMessage" MessageType="Error" TextResourceKey="EmailError" CssClass="EmailErrorMessage" />
</div>

<div class="SuccessModuleMessage" runat="server" id="SuccessModuleMessageDiv" style="display: none;">
    <engage:ModuleMessage runat="server" ID="SuccessModuleMessage" MessageType="Success" TextResourceKey="EmailSuccess" CssClass="EmailSuccessMessage" />
</div>

<div>
    <asp:Button runat="server" ID="SubmitButton" ResourceKey="SubmitButton" CssClass="SubmitButton" Width="20%" CausesValidation="true" OnClientClick="ExpressValidate();" onclick="SubmitButton_Click" ValidationGroup="EngageTellAFriend" /></div>
</div>
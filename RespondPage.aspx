<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RespondPage.aspx.cs" Inherits="Engage.Dnn.Events.RespondPage" %>
<%@ Register TagPrefix="engage" TagName="Respond" Src="Respond.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager runat="server" />
        <engage:Respond ID="RespondControl" runat="server" />
    </form>
</body>
</html>

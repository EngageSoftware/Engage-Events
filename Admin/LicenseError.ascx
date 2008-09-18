<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.LicenseError" Codebehind="LicenseError.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="ModuleMessage" Src="../Controls/ModuleMessage.ascx"%>

<engage:ModuleMessage runat="server" MessageType="Error" TextResourceKey="LicenseError" CssClass="LicenseErrorMessage"/>
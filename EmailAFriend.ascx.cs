// <copyright file="EmailAFriend.ascx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EmailAFriend : ModuleBase
    {
        ////protected override void OnInit(EventArgs e)
        ////{
        ////    this.Load += Page_Load;
        ////    base.OnInit(e);
        ////}

        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        ////protected void btnCancel_OnClick(object sender, EventArgs e)
        ////{
        ////    ClearCommentInput();
        ////    mpeEmailAFriend.Hide();
        ////}

        ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        ////protected void btnSend_OnClick(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        string message = Localization.GetString("EmailAFriend", LocalResourceFile);
        ////        message = message.Replace("[Engage:Recipient]", txtTo.Text.Trim());
        ////        message = message.Replace("[Engage:Url]", GetEventLinkUrl());
        ////        message = message.Replace("[Engage:From]", txtFrom.Text.Trim());
        ////        message = message.Replace("[Engage:Message]", txtMessage.Text.Trim());

        ////        string subject = Localization.GetString("EmailAFriendSubject", LocalResourceFile);
        ////        subject = subject.Replace("[Engage:Portal]", PortalSettings.PortalName);

        ////        Mail.SendMail(PortalSettings.Email.ToString(), txtTo.Text.Trim(), "", subject, message, "", "HTML", "", "", "", "");
        ////        ClearCommentInput();
        ////        mpeEmailAFriend.Hide();

        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        //the email or emails entered are invalid or mail services are not configured
        ////        Exceptions.LogException(ex);
        ////    }
        ////}

        ////private void Page_Load(object sender, EventArgs e)
        ////{
        ////    this.txtFrom.Text = this.UserInfo != null ? this.UserInfo.Email : string.Empty;
        ////}

        ////private void ClearCommentInput()
        ////{
        ////    txtFrom.Text = string.Empty;
        ////    txtMessage.Text = string.Empty;
        ////    txtTo.Text = string.Empty;
        ////}

        ////private string GetEventLinkUrl()
        ////{
        ////    return this.EditUrl("EventId", EventId.ToString(CultureInfo.InvariantCulture), "View");
        ////}
    }
}
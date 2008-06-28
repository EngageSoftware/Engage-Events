// <copyright file="ViewTellAFriend.ascx.cs" company="Engage Software">
// Engage: TellAFriend - http://www.engagesoftware.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.TellAFriend
{
    using System;
    using System.Web.UI;
    using DotNetNuke.Common;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Mail;

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The ViewTellAFriend class displays the content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class ViewTellAFriend : EngageModuleBase
    {
        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the SubmitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string message = Localization.GetString("EmailAFriend", LocalResourceFile);
                    message = message.Replace("[Engage:Recipient]", FriendsEmailTextBox.Text.Trim());
                    message = message.Replace("[Engage:Url]", "http://dnngallery.net");
                    message = message.Replace("[Engage:From]", FirstNameTextBox.Text.Trim() + " " + LastNameTextBox.Text.Trim());
                    message = message.Replace("[Engage:Message]", this.MessageTextBox.Text.Trim());

                    string subject = Localization.GetString("EmailAFriendSubject", LocalResourceFile);
                    subject = subject.Replace("[Engage:Portal]", PortalSettings.PortalName);

                    Mail.SendMail(PortalSettings.Email, FriendsEmailTextBox.Text.Trim(), "", subject, message, "", "HTML", "", "", "", "");

                    this.SuccessModuleMessageDiv.Style[HtmlTextWriterStyle.Display] = "";
                }
                else
                {
                    this.ErrorModuleMessageDiv.Style[HtmlTextWriterStyle.Display] = "";
                }
            }
            catch (Exception ex)
            {
                // the email or emails entered are invalid or mail services are not configured
                Exceptions.LogException(ex);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                SubmitButton.ToolTip = Localization.GetString("SubmitButtonToolTip", LocalResourceFile);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion
    }
}
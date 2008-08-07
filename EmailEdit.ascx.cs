// <copyright file="EmailEdit.ascx.cs" company="Engage Software">
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
    using System;
    using System.Collections;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Security.Roles;
    using DotNetNuke.Services.Exceptions;
    using Engage.Events;
    using Engage.Events.Util;
    using Engage.Util;
    using Communication.Email;
    using Services.Client;
    using Utility = Engage.Dnn.Utility;

    /// <summary>
    /// 
    /// </summary>
    public partial class EmailEdit : ModuleBase
    {
        /// <summary>
        /// Gets the unsubscribe URL.
        /// </summary>
        /// <value>The unsubscribe URL.</value>
        private string UnsubscribeUrl
        {
            get { return Utility.GetStringSetting(this.Settings, Setting.UnsubscribeUrl.PropertyName); }
        }

        /// <summary>
        /// Gets the privacy policy URL.
        /// </summary>
        /// <value>The privacy policy URL.</value>
        private string PrivacyPolicyUrl
        {
            get { return Utility.GetStringSetting(this.Settings, Setting.PrivacyPolicyUrl.PropertyName); }
        }

        /// <summary>
        /// Gets the open link URL.
        /// </summary>
        /// <value>The open link URL.</value>
        private string OpenLinkUrl
        {
            get { return Utility.GetStringSetting(this.Settings, Setting.OpenLinkUrl.PropertyName); }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += this.Page_Load;
            this.EmailTypeRadioButtons.SelectedIndexChanged += this.EmailTypeRadioButtons_SelectedIndexChanged;
            this.SaveEmailButton.Click += this.SaveEmailButton_Click;
            this.CancelEmailLink.NavigateUrl = Globals.NavigateURL();
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Page.IsPostBack == false)
                {
                    this.FillDropDowns();
                    if (EventId > 0)
                    {
                        this.BindData();
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Click event of the SaveEmailButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SaveEmailButton_Click(object sender, EventArgs e)
        {
            EmailEvent emailEvent;

            if (this.EmailTypeRadioButtons.SelectedValue == EmailEventType.Reminder.Description)
            {
                // reminder emails have three different messages depending on whether or not the person has RSVP'd.
                emailEvent = new EmailEvent(
                    EmailEventType.Reminder,
                    "Name Here",
                    "Purpose Here",
                    this.SubjectTextBox.Text,
                    this.FromTextBox.Text,
                    this.FromEmailTextBox.Text,
                    this.UserId);
                emailEvent.HtmlBodyLocation1 = this.EmailLocationTextBox1.Text;
                emailEvent.HtmlBodyLocation2 = this.EmailLocationTextBox2.Text;
                emailEvent.HtmlBodyLocation3 = this.EmailLocationTextBox3.Text;
            }
            else
            {
                EmailEventType type = (EmailEventType)EngageType.GetFromShortDescription(this.EmailTypeRadioButtons.SelectedValue, typeof(EmailEventType));

                emailEvent = new EmailEvent(
                    type, "Name Here", "Purpose Here", this.SubjectTextBox.Text, this.FromTextBox.Text, this.FromEmailTextBox.Text, this.UserId);
                emailEvent.HtmlBodyLocation1 = this.EmailLocationTextBox1.Text;
            }

            // approvals?
            this.FillApprovals(emailEvent);

            // add some recipients
            this.AddRecipients(emailEvent);

            CreateEmailCommand cmd = new CreateEmailCommand(emailEvent);

            // pass to server (locally or remotely based on config file of Engage.Services.
            cmd = DataServices.Execute(cmd);
            cmd.WriteLocalData(EventId, this.UserId);

            Event ee = Event.Load(EventId);

            if (this.EmailTypeRadioButtons.SelectedItem.Text == EmailEventType.Invitation.Description)
            {
                ee.InvitationUrl = emailEvent.HtmlBodyLocation1;
            }

            if (this.EmailTypeRadioButtons.SelectedItem.Text == EmailEventType.Recap.Description)
            {
                ee.RecapUrl = emailEvent.HtmlBodyLocation1;
            }

            ee.Save(this.UserId);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the EmailTypeRadioButtons control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EmailTypeRadioButtons_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dvReminderOnly.Visible = (this.EmailTypeRadioButtons.SelectedValue == EmailEventType.Reminder.Description);
        }

        /// <summary>
        /// Adds the recipients.
        /// </summary>
        /// <param name="emailEvent">The email event.</param>
        private void AddRecipients(EmailEvent emailEvent)
        {
            RoleController rc = new RoleController();
            ArrayList users = rc.GetUsersByRoleName(this.PortalId, this.RolesDropDown.SelectedItem.Text);

            // get setting information about URL's
            string unsubscribeUrl = this.UnsubscribeUrl;
            string privacyPolicyUrl = this.PrivacyPolicyUrl;
            string openLinkUrl = this.OpenLinkUrl;
            string replacementMessage = string.Empty;

            foreach (UserInfo user in users)
            {
                emailEvent.AddRecipient(
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    "Company Name Here",
                    this.EmailLocationTextBox1.Text,
                    DateTime.Now,
                    unsubscribeUrl,
                    privacyPolicyUrl,
                    openLinkUrl,
                    replacementMessage);
            }
        }

        /// <summary>
        /// Fills the approvals.
        /// </summary>
        /// <param name="emailEvent">The email event.</param>
        private void FillApprovals(EmailEvent emailEvent)
        {
            string[] recipients = this.ApproversTextBox.Text.Split(';');

            foreach (string recipient in recipients)
            {
                emailEvent.AddApprovalRecipient(recipient);
            }
        }

        /// <summary>
        /// Fills the drop downs.
        /// </summary>
        private void FillDropDowns()
        {
            RoleController rc = new RoleController();

            this.RolesDropDown.DataTextField = "RoleName";
            this.RolesDropDown.DataValueField = "RoleID";
            this.RolesDropDown.DataSource = rc.GetPortalRoles(this.PortalId);
            this.RolesDropDown.DataBind();
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            Event e = Event.Load(EventId);

            this.EventNameLabel.Text = e.Title;

            // now load the email details. using?
        }
    }
}
//Engage: Events - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


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
    using Communication.Email;
    using Engage.Util;
    using Services.Client;
    using Templating;
    using Utility=Engage.Dnn.Utility;

    /// <summary>
    /// 
    /// </summary>
    public partial class EmailEdit : ModuleBase
    {

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.EmailTypeRadioButtons.SelectedIndexChanged += this.EmailTypeRadioButtons_SelectedIndexChanged;
            this.SaveEmailButton.Click += this.SaveEmailButton_Click;
            this.CancelEmailLink.NavigateUrl = Globals.NavigateURL();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    FillDropDowns();
                    if (EventId > 0)
                    {
                        BindData();
                    }                    
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SaveEmailButton_Click(object sender, EventArgs e)
        {

            EmailEvent emailEvent;

            if (EmailTypeRadioButtons.SelectedValue == EmailEventType.Reminder.Description)
            {
                //reminder emails have three different messages depending on whether or not the person has RSVP'd.
                emailEvent = new EmailEvent(EmailEventType.Reminder, "Name Here", "Purpose Here", SubjectTextBox.Text, FromTextBox.Text, FromEmailTextBox.Text, UserId);
                emailEvent.HtmlBodyLocation1 = EmailLocationTextBox1.Text;
                emailEvent.HtmlBodyLocation2 = EmailLocationTextBox2.Text;
                emailEvent.HtmlBodyLocation3 = EmailLocationTextBox3.Text;
            }
            else
            {
                EmailEventType type = (EmailEventType) EngageType.GetFromShortDescription(this.EmailTypeRadioButtons.SelectedValue, typeof(EmailEventType));

                emailEvent = new EmailEvent(type, "Name Here", "Purpose Here", SubjectTextBox.Text, FromTextBox.Text, FromEmailTextBox.Text, UserId);
                emailEvent.HtmlBodyLocation1 = EmailLocationTextBox1.Text;

            }


            //approvals?
            FillApprovals(emailEvent);

            //add some recipients
            AddRecipients(emailEvent);

            CreateEmailCommand cmd = new CreateEmailCommand(emailEvent);
            //pass to server (locally or remotely based on config file of Engage.Services.
            cmd = DataServices.Execute(cmd);
            cmd.WriteLocalData(EventId, UserId);


            Event ee = Event.Load(EventId);

            if (EmailTypeRadioButtons.SelectedItem.Text == EmailEventType.Invitation.Description)
            {
                    ee.InvitationUrl = emailEvent.HtmlBodyLocation1;
            }

            if (EmailTypeRadioButtons.SelectedItem.Text == EmailEventType.Recap.Description)
            {
                    ee.RecapUrl = emailEvent.HtmlBodyLocation1;
            }

            ee.Save(UserId);

        }

        private void EmailTypeRadioButtons_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvReminderOnly.Visible = (EmailTypeRadioButtons.SelectedValue == EmailEventType.Reminder.Description);
        }

        #endregion

        private void FillApprovals(EmailEvent emailEvent)
        {
            string[] recipients = ApproversTextBox.Text.Split(';');

            foreach (string recipient in recipients)
            {
                emailEvent.AddApprovalRecipient(recipient);
            }
        }

        private void AddRecipients(EmailEvent emailEvent)
        {
            RoleController rc = new RoleController();
            ArrayList users = rc.GetUsersByRoleName(PortalId, RolesDropDown.SelectedItem.Text);

            //get setting information about URL's
            string unsubscribeUrl = UnsubscribeUrl;
            string privacyPolicyUrl = PrivacyPolicyUrl;
            string openLinkUrl = OpenLinkUrl;
            string replacementMessage = string.Empty;

            foreach (UserInfo user in users)
            {
                emailEvent.AddRecipient(user.FirstName, user.LastName, user.Email, "Company Name Here", EmailLocationTextBox1.Text, DateTime.Now, unsubscribeUrl, privacyPolicyUrl, openLinkUrl, replacementMessage);
            }
        }

        private string UnsubscribeUrl
        {
            get
            {
                return Utility.GetStringSetting(Settings, Setting.UnsubscribeUrl.PropertyName);
            }
        }

        private string PrivacyPolicyUrl
        {
            get
            {
                return Utility.GetStringSetting(Settings, Setting.PrivacyPolicyUrl.PropertyName);
            }
        }

        private string OpenLinkUrl
        {
            get
            {
                return Utility.GetStringSetting(Settings, Setting.OpenLinkUrl.PropertyName);
            }
        }

        private void FillDropDowns()
        {
            RoleController rc = new RoleController();

            RolesDropDown.DataTextField = "RoleName";
            RolesDropDown.DataValueField = "RoleID";
            RolesDropDown.DataSource = rc.GetPortalRoles(PortalId);
            RolesDropDown.DataBind();
        }

        private void BindData()
        {
            Event e = Event.Load(EventId);

            EventNameLabel.Text = e.Title;

            //now load the email details. using?

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        
      
    }
}


//Engage: Events - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using Engage.Dnn.Events.Data;
using Engage.Dnn.Events.Util;
using Engage.Events;
using Engage.Events.Util;
using Engage.Communication.Email;
using Engage.Services.Client;

namespace Engage.Dnn.Events
{
    public partial class EmailEdit : ModuleBase
    {

        #region Event Handlers

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

        protected void btnSegment_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void lbPreview_OnClick(object sender, EventArgs e)
        {

        }

        protected void lbSendNow_OnClick(object sender, EventArgs e)
        {

        }

        protected void lbSave_OnClick(object sender, EventArgs e)
        {

            EmailEvent emailEvent = null;

            if (rblEmailType.SelectedValue == EmailEventType.Reminder.Description)
            {
                //reminder emails have three different messages depending on whether or not the person has RSVP'd.
                emailEvent = new EmailEvent(EmailEventType.Reminder, "Name Here", "Purpose Here", txtSubject.Text, txtFrom.Text, txtFromEmail.Text, UserId);
                emailEvent.HtmlBodyLocation1 = txtLocation.Text;
                emailEvent.HtmlBodyLocation2 = txtLocation2.Text;
                emailEvent.HtmlBodyLocation3 = txtLocation3.Text;
            }
            else
            {
                EmailEventType type = (EmailEventType)Enum.Parse(typeof(EmailEventType), rblEmailType.SelectedValue);

                emailEvent = new EmailEvent(type, "Name Here", "Purpose Here", txtSubject.Text, txtFrom.Text, txtFromEmail.Text, UserId);
                emailEvent.HtmlBodyLocation1 = txtLocation.Text;

            }


            //approvals?
            FillApprovals(emailEvent);

            //add some recipients
            AddRecipients(emailEvent);

            CreateEmailCommand cmd = new CreateEmailCommand(emailEvent);
            //pass to server (locally or remotely based on config file of Engage.Services.
            cmd = DataServices.Execute<CreateEmailCommand>(cmd);
            cmd.WriteLocalData(EventId, UserId);


            Event ee = Event.Load(EventId);

            if (rblEmailType.SelectedItem.Text == EmailEventType.Invitation.Description)
            {
                    ee.InvitationUrl = emailEvent.HtmlBodyLocation1;
            }

            if (rblEmailType.SelectedItem.Text == EmailEventType.Recap.Description)
            {
                    ee.RecapUrl = emailEvent.HtmlBodyLocation1;
            }

            ee.Save(UserId);

        }

        protected void lbCancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(), true);
        }

        #endregion

        private void FillApprovals(EmailEvent emailEvent)
        {
            string[] recipients = txtApprovers.Text.Split(';');

            foreach (string recipient in recipients)
            {
                emailEvent.AddApprovalRecipient(recipient);
            }
        }

        private void AddRecipients(EmailEvent emailEvent)
        {
            RoleController rc = new RoleController();
            ArrayList users = rc.GetUsersByRoleName(PortalId, ddlRoles.SelectedItem.Text);

            //get setting information about URL's
            string unsubscribeUrl = UnsubscribeUrl;
            string privacyPolicyUrl = PrivacyPolicyUrl;
            string openLinkUrl = OpenLinkUrl;
            string replacementMessage = string.Empty;

            foreach (UserInfo user in users)
            {
                emailEvent.AddRecipient(user.FirstName, user.LastName, user.Email, "Company Name Here", txtLocation.Text, DateTime.Now, unsubscribeUrl, privacyPolicyUrl, openLinkUrl, replacementMessage);
            }
        }

        private string UnsubscribeUrl
        {
            get
            {
                object o = Settings[Setting.UnsubscribeUrl.Description];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    return o.ToString();
                }
                return string.Empty;
            }
        }

        private string PrivacyPolicyUrl
        {
            get
            {
                object o = Settings[Setting.PrivacyPolicyUrl.Description];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    return o.ToString();
                }
                return string.Empty;
            }
        }

        private string OpenLinkUrl
        {
            get
            {
                object o = Settings[Setting.OpenLinkUrl.Description];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    return o.ToString();
                }
                return string.Empty;
            }
        }
        private void FillDropDowns()
        {
            RoleController rc = new RoleController();

            ddlRoles.DataTextField = "RoleName";
            ddlRoles.DataValueField = "RoleID";
            ddlRoles.DataSource = rc.GetPortalRoles(PortalId);
            ddlRoles.DataBind();
        }

        private void BindData()
        {
            Event e = Event.Load(EventId);

            lblEvent.Text = e.Title;

            //now load the email details. using?

        }

        protected void rblEmailType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvReminderOnly.Visible = (rblEmailType.SelectedValue == EmailEventType.Reminder.Description);
        }
    }
}


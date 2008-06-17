// <copyright file="EventAdminActions.ascx.cs" company="Engage Software">
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
    using System.Globalization;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Utilities;
    using Engage.Events;

    /// <summary>
    /// Displays the actions that users can perform on an event instance.
    /// </summary>
    /// <remarks>
    /// This control's behavior changed from using LinkButtons to standard buttons. Something to do with a postback
    /// not occurring on the container form. Not sure why? Anyhow, it stores the EventID in viewstate and uses it if needed.hk
    /// </remarks>
    public partial class EventAdminActions : ModuleBase
    {
        /// <summary>
        /// Backing field for <see cref="CurrentEvent"/>
        /// </summary>
        private Engage.Events.Event currentEvent;

        /// <summary>
        /// Gets or sets the current event that this control is displaying actions for.
        /// </summary>
        /// <value>The current event that this control is displaying actions for.</value>
        internal Event CurrentEvent
        {
            get 
            {
                return this.currentEvent ?? Event.Load(this.CurrentEventId);
            }

            set 
            { 
                this.currentEvent = value;
                this.CurrentEventId = this.currentEvent.Id;
                this.BindData();
            }
        }

        /// <summary>
        /// Gets or sets the current event id.
        /// </summary>
        /// <value>The current event id.</value>
        private int CurrentEventId
        {
            get 
            {
                return Convert.ToInt32(this.ViewState["id"], CultureInfo.InvariantCulture);
            }

            set
            {
                this.ViewState["id"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // since the global navigation control is not loaded using DNN mechanisms we need to set it here so that calls to 
            // module related information will appear the same as the actual control this navigation is sitting on.hk
            this.ModuleConfiguration = ((PortalModuleBase)base.Parent.Parent.Parent).ModuleConfiguration;

            this.EditEventButton.Click += this.EditEventButton_Click;
            this.ResponsesButton.Click += this.ResponsesButton_Click;
            this.RegisterButton.Click += this.RegisterButton_Click;
            this.AddToCalendarButton.Click += this.AddToCalendarButton_Click;
            this.DeleteEventButton.Click += this.DeleteEventButton_Click;
            this.CancelButton.Click += this.CancelButton_Click;
            this.ViewInviteButton.Click += this.ViewInviteButton_Click;
            this.EditEmailButton.Click += this.EditEmailButton_Click;
        }

        /// <summary>
        /// Sets the visibility of each of the buttons.  Also, sets the text for the cancel/uncancel button, and the delete confirm.
        /// </summary>
        private void BindData()
        {
            this.AddToCalendarButton.Visible = this.IsLoggedIn;

            this.CancelButton.Visible = this.IsAdmin;
            this.DeleteEventButton.Visible = this.IsAdmin;
            this.EditEventButton.Visible = this.IsAdmin;
            this.ResponsesButton.Visible = this.IsAdmin;

            this.CancelButton.Text = this.CurrentEvent.Cancelled
                                ? Localization.GetString("UnCancel", this.LocalResourceFile)
                                : Localization.GetString("Cancel", this.LocalResourceFile);

            ////ViewInviteButton.Visible = Engage.Util.Utility.IsValidEmail(CurrentEvent.InvitationUrl);

            ClientAPI.AddButtonConfirm(this.DeleteEventButton, Localization.GetString("ConfirmDelete", this.LocalResourceFile));
            ClientAPI.AddButtonConfirm(this.CancelButton, Localization.GetString(this.CurrentEvent.Cancelled ? "ConfirmUnCancel" : "ConfirmCancel", LocalResourceFile));
        }

        /// <summary>
        /// Handles the OnClick event of the EditEventButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EditEventButton_Click(object sender, EventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EventEdit&eventId=" + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture));
            Response.Redirect(href, true);
        }

        /// <summary>
        /// Handles the OnClick event of the RegisterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=Register&eventid=" + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture));
            Response.Redirect(href, true);        
        }

        /// <summary>
        /// Handles the OnClick event of the ResponsesButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ResponsesButton_Click(object sender, EventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=RsvpDetail&eventid=" + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture));
            Response.Redirect(href, true);
        }

        /// <summary>
        /// Handles the OnClick event of the DeleteEventButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DeleteEventButton_Click(object sender, EventArgs e)
        {
            Event.Delete(this.CurrentEvent.Id);
            ////if (ActionCompleted != null)
            ////{
            ////    ActionCompleted(this, new ActionEventArg(Action.Success));
            ////}

            // Not the best way but I need to refresh the page this control is on. hk
            Response.Redirect(Request.Url.ToString());
        }

        /// <summary>
        /// Handles the OnClick event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Event thisEvent = this.CurrentEvent;
            thisEvent.Cancelled = !this.CurrentEvent.Cancelled;
            thisEvent.Save(UserId);

            ////if (ActionCompleted != null)
            ////{
            ////    ActionCompleted(this, new ActionEventArg(Action.Success));
            ////}

            // Not the best way but I need to refresh the page this control is on. hk
            Response.Redirect(Request.Url.ToString());
        }

        /// <summary>
        /// Handles the OnClick event of the EditEmailButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EditEmailButton_Click(object sender, EventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EmailEdit&eventid=" + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture));
            Response.Redirect(href, true);
        }

        /// <summary>
        /// Handles the OnClick event of the AddToCalendarButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AddToCalendarButton_Click(object sender, EventArgs e)
        {
            SendICalendarToClient(Response, this.CurrentEvent.ToICal(UserInfo.Email), this.CurrentEvent.Title);
        }

        /// <summary>
        /// Handles the OnClick event of the ViewInviteButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ViewInviteButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(this.CurrentEvent.InvitationUrl, true); 
        }
    }
}


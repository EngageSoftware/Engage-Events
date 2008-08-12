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

namespace Engage.Dnn.Events.Navigation
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web.UI;
    using Controls;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Framework;
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
        private Event currentEvent;

        /// <summary>
        /// Occurs when the Delete button is pressed.
        /// </summary>
        public event EventHandler Delete;

        /// <summary>
        /// Occurs when the Cancel (or UnCancel) button is pressed.
        /// </summary>
        public event EventHandler Cancel;

        /// <summary>
        /// Gets or sets the current event that this control is displaying actions for.
        /// </summary>
        /// <value>The current event that this control is displaying actions for.</value>
        internal Event CurrentEvent
        {
            get
            {
                if (this.currentEvent == null)
                {
                    this.currentEvent = Event.Load(this.CurrentEventId);
                }

                return this.currentEvent;
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
            get { return Convert.ToInt32(this.ViewState["id"], CultureInfo.InvariantCulture); }

            set { this.ViewState["id"] = value.ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Raises the <see cref="Cancel"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void OnCancel(EventArgs e)
        {
            this.InvokeCancel(e);
        }

        /// <summary>
        /// Raises the <see cref="Delete"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void OnDelete(EventArgs e)
        {
            this.InvokeDelete(e);
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
            this.LocalResourceFile = this.AppRelativeTemplateSourceDirectory + "App_LocalResources/" + Path.GetFileNameWithoutExtension(this.TemplateControl.AppRelativeVirtualPath);

            this.EditEventButton.Click += this.EditEventButton_Click;
            this.ResponsesButton.Click += this.ResponsesButton_Click;
            this.RegisterButton.Click += this.RegisterButton_Click;
            this.AddToCalendarButton.Click += this.AddToCalendarButton_Click;
            this.DeleteEventButton.Click += this.DeleteEventButton_Click;
            this.CancelButton.Click += this.CancelButton_Click;
            this.ViewInviteButton.Click += this.ViewInviteButton_Click;
            this.EditEmailButton.Click += this.EditEmailButton_Click;

            AJAX.RegisterPostBackControl(this.AddToCalendarButton);
        }

        /// <summary>
        /// Gets the script to show the event type dialog window.
        /// </summary>
        /// <param name="occurrenceUrl">The URL to navigate to when the user chooses to apply the action to that occurrence.</param>
        /// <param name="seriesUrl">The URL to navigate to when the user chooses to apply the action to the entire series.</param>
        /// <returns>The script to show the event type dialog window</returns>
        private static string GetShowEventTypeDialogScript(string occurrenceUrl, string seriesUrl)
        {
            return string.Format("EngageEvents.showEditTypeDialog('{0}', '{1}');return false;", ClientAPI.GetSafeJSString(occurrenceUrl), ClientAPI.GetSafeJSString(seriesUrl));
        }

        /// <summary>
        /// Invokes the cancel.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void InvokeCancel(EventArgs e)
        {
            EventHandler cancelHandler = this.Cancel;
            if (cancelHandler != null)
            {
                cancelHandler(this, e);
            }
        }

        /// <summary>
        /// Invokes the delete.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void InvokeDelete(EventArgs e)
        {
            EventHandler deleteHandler = this.Delete;
            if (deleteHandler != null)
            {
                deleteHandler(this, e);
            }
        }

        /// <summary>
        /// Sets the visibility of each of the buttons.  Also, sets the text for the cancel/uncancel button, and the delete confirm.
        /// </summary>
        private void BindData()
        {
            this.SetVisibility();
            this.LocalizeControls();
            this.SetupRecurringEditTypeDialog();
        }

        /// <summary>
        /// Sets up the edit buttons (i.e. <see cref="EditEventButton"/>, <see cref="DeleteEventButton"/>, and <see cref="CancelButton"/>)
        /// to display a dialog asking the user whether to perform the requested action on that occurrence or the whole series.
        /// </summary>
        private void SetupRecurringEditTypeDialog()
        {
            if (this.CurrentEvent.IsRecurring)
            {
                ScriptManager.RegisterClientScriptResource(this, typeof(EditTypeDialog), "Engage.Dnn.Events.JavaScript.EngageEvents.EditTypeDialog.js");
                this.EditEventButton.UseSubmitBehavior = this.DeleteEventButton.UseSubmitBehavior = this.CancelButton.UseSubmitBehavior = false;
                this.EditEventButton.OnClientClick = GetShowEventTypeDialogScript(
                    this.BuildLinkUrl(this.ModuleId, "EventEdit", "eventId=" + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture), "start=" + this.CurrentEvent.EventStart.Ticks.ToString(CultureInfo.InvariantCulture)),
                    this.BuildLinkUrl(this.ModuleId, "EventEdit", "eventId=" + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture)));
                this.DeleteEventButton.OnClientClick = GetShowEventTypeDialogScript(
                    this.Page.ClientScript.GetPostBackClientHyperlink(this.DeleteEventButton, this.CurrentEvent.EventStart.Ticks.ToString(CultureInfo.InvariantCulture)),
                    this.Page.ClientScript.GetPostBackClientHyperlink(this.DeleteEventButton, string.Empty));
                this.CancelButton.OnClientClick = GetShowEventTypeDialogScript(
                    this.Page.ClientScript.GetPostBackClientHyperlink(this.CancelButton, this.CurrentEvent.EventStart.Ticks.ToString(CultureInfo.InvariantCulture)),
                    this.Page.ClientScript.GetPostBackClientHyperlink(this.CancelButton, string.Empty));
            }
        }

        /// <summary>
        /// Sets the visibility of this control's child controls.
        /// </summary>
        private void SetVisibility()
        {
            this.AddToCalendarButton.Visible = IsLoggedIn;
            this.CancelButton.Visible = this.IsAdmin;
            this.DeleteEventButton.Visible = this.IsAdmin;
            this.EditEventButton.Visible = this.IsAdmin;
            this.ResponsesButton.Visible = this.IsAdmin;
            ////this.ViewInviteButton.Visible = Engage.Util.Utility.IsValidEmail(CurrentEvent.InvitationUrl);
        }

        /// <summary>
        /// Localizes this control's child controls.
        /// </summary>
        private void LocalizeControls()
        {
            this.CancelButton.Text = this.CurrentEvent.Cancelled
                                         ? Localization.GetString("UnCancel", this.LocalResourceFile)
                                         : Localization.GetString("Cancel", this.LocalResourceFile);

            ClientAPI.AddButtonConfirm(this.DeleteEventButton, Localization.GetString("ConfirmDelete", this.LocalResourceFile));
            ClientAPI.AddButtonConfirm(
                this.CancelButton, Localization.GetString(this.CurrentEvent.Cancelled ? "ConfirmUnCancel" : "ConfirmCancel", this.LocalResourceFile));
        }

        /// <summary>
        /// Handles the OnClick event of the EditEventButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EditEventButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(
                this.BuildLinkUrl(this.ModuleId, "EventEdit", "eventId=" + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture)), true);
        }

        /// <summary>
        /// Handles the OnClick event of the RegisterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(
                this.BuildLinkUrl(this.ModuleId, "Register", "eventid=" + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture)), true);
        }

        /// <summary>
        /// Handles the OnClick event of the ResponsesButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ResponsesButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(
                this.BuildLinkUrl(this.ModuleId, "RsvpDetail", "eventid=" + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture)), true);
        }

        /// <summary>
        /// Handles the OnClick event of the DeleteEventButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DeleteEventButton_Click(object sender, EventArgs e)
        {
            Event.Delete(this.CurrentEvent.Id);
            this.OnDelete(e);
        }

        /// <summary>
        /// Handles the OnClick event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            // since we are reloading the object each time (no caching yet) you can't do the following
            // this.CurrentEvent.Cancelled = !this.CurrentEvent.Cancelled;  hk
            Event ev = this.CurrentEvent;
            ev.Cancelled = !this.CurrentEvent.Cancelled;
            ev.Save(this.UserId);
            this.OnCancel(e);
        }

        /// <summary>
        /// Handles the OnClick event of the EditEmailButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EditEmailButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(
                this.BuildLinkUrl(this.ModuleId, "EmailEdit", "eventid=" + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture)), true);
        }

        /// <summary>
        /// Handles the OnClick event of the AddToCalendarButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AddToCalendarButton_Click(object sender, EventArgs e)
        {
            SendICalendarToClient(this.Response, this.CurrentEvent.ToICal(this.UserInfo.Email, Utility.GetUserTimeZoneOffset(this.UserInfo, this.PortalSettings)), this.CurrentEvent.Title);
        }

        /// <summary>
        /// Handles the OnClick event of the ViewInviteButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ViewInviteButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.CurrentEvent.InvitationUrl, true);
        }
    }
}
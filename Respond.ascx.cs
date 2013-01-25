// <copyright file="Respond.ascx.cs" company="Engage Sooftware">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
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
    using System.Web;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;

    using Engage.Events;

    /// <summary>
    /// Code-behind for a control <c>Respond.ascx</c> that allows users to register their attendance for an event.
    /// </summary>
    public abstract partial class Respond : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.SubmitButton.Click += this.SubmitButton_Click;
            this.AddToCalendarButton.Click += this.AddToCalendarButton_Click;
            this.BackToEventsButton.Click += this.BackToEventsButton_Click;

            AJAX.RegisterPostBackControl(this.AddToCalendarButton);
        }

        /// <summary>
        /// Determines whether the current user has already registered for the event with the given <paramref name="eventId"/>, and can therefore unregister from the event.
        /// </summary>
        /// <param name="eventId">The ID of the event being responded to.</param>
        /// <returns>
        /// <c>true</c> if the current user can unregistered for this event; otherwise, <c>false</c>.
        /// </returns>
        private bool CanUnregisterFrom(int? eventId)
        {
            var response = Engage.Events.Response.Load(eventId.Value, this.EventStart, this.UserInfo.Email);
            return response != null && response.Status == ResponseStatus.Attending;
        }

        /// <summary>
        /// Shows the <see cref="EventFullView"/>.
        /// </summary>
        /// <param name="eventBeingRespondedTo">The event being responded to.</param>
        private void ShowEventFullView(Event eventBeingRespondedTo)
        {
            this.ResponseMultiview.SetActiveView(this.EventFullView);
            if (string.IsNullOrEmpty(eventBeingRespondedTo.CapacityMetMessage))
            {
                this.EventFullMessage.TextResourceKey = "EventFullMessage.Text";
            }
            else
            {
                this.EventFullMessage.Text = eventBeingRespondedTo.CapacityMetMessage;
            }
        }

        /// <summary>
        /// Binds the data for this control.  Sets up values for the specific <see cref="Event"/> that this <see cref="Response"/> is for.
        /// </summary>
        private void BindData()
        {
            int? eventId = this.EventId;
            if (!eventId.HasValue)
            {
                return;
            }

            Event eventBeingRespondedTo = Event.Load(eventId.Value);
            if (!this.CanShowEvent(eventBeingRespondedTo))
            {
                return;
            }

            eventBeingRespondedTo = eventBeingRespondedTo.CreateOccurrence(this.EventStart);

            if (eventBeingRespondedTo.IsFull && !this.CanUnregisterFrom(eventBeingRespondedTo.Id))
            {
                this.ShowEventFullView(eventBeingRespondedTo);
            }
            else
            {
                this.EventNameLabel.Text = string.Format(CultureInfo.CurrentCulture, this.Localize("EventNameLabel.Text"), eventBeingRespondedTo.Title);
                this.AddToCalendarButton.Enabled = true;

                this.ResponseStatusRadioButtons.Items.Clear();
                this.ResponseStatusRadioButtons.Items.Add(new ListItem(
                    this.Localize(ResponseStatus.Attending.ToString()),
                    ResponseStatus.Attending.ToString()));
                this.ResponseStatusRadioButtons.Items.Add(new ListItem(
                    this.Localize(ResponseStatus.NotAttending.ToString()),
                    ResponseStatus.NotAttending.ToString()));
                this.ResponseStatusRadioButtons.Items[0].Selected = true;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (UserInfo.UserID < 0)
                {
                    Response.Redirect(this.RegisterUrl, true);
                }

                if (!this.IsPostBack)
                {
                    this.BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Click event of the SubmitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                int? eventId = this.EventId;
                if (!eventId.HasValue)
                {
                    return;
                }

                var eventBeingRespondedTo = Event.Load(eventId.Value).CreateOccurrence(this.EventStart);
                if (!this.CanShowEvent(eventBeingRespondedTo))
                {
                    return;
                }

                var responseStatus = (ResponseStatus)Enum.Parse(typeof(ResponseStatus), this.ResponseStatusRadioButtons.SelectedValue);
                if (responseStatus != ResponseStatus.Attending || !eventBeingRespondedTo.IsFull)
                {
                    var response = Engage.Events.Response.Load(eventId.Value, this.EventStart, this.UserInfo.Email) ??
                                   Engage.Events.Response.Create(
                                       eventId.Value, this.EventStart, this.UserInfo.FirstName, this.UserInfo.LastName, this.UserInfo.Email);

                    response.Status = responseStatus;
                    response.Save(this.UserId);

                    this.ResponseMultiview.SetActiveView(this.ThankYouView);
                }
                else
                {
                    this.ShowEventFullView(eventBeingRespondedTo);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Click event of the AddToCalendarButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AddToCalendarButton_Click(object sender, EventArgs e)
        {
            int? eventId = this.EventId;
            if (!eventId.HasValue)
            {
                return;
            }

            Event evnt = Event.Load(eventId.Value);
            if (!this.CanShowEvent(evnt))
            {
                return;
            }

            ModuleBase.SendICalendarToClient(HttpContext.Current.Response, evnt.ToICal(), evnt.Title);
        }

        /// <summary>
        /// Handles the Click event of the BackToEventsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BackToEventsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(), true);
        }
    }
}
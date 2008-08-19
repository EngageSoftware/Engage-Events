// <copyright file="EventToolTip.ascx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Display
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using DotNetNuke.Framework;
    using Engage.Events;
    using ModuleBase = Engage.Dnn.Events.ModuleBase;
    using Utility=Engage.Dnn.Utility;

    /// <summary>
    /// Used to display a "tool tip" for an appointment.
    /// </summary>
    public partial class EventToolTip : ModuleBase
    {
        /// <summary>
        /// The backing field for <see cref="SetEventId"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int currentEventId;

        /// <summary>
        /// The backing field for <see cref="EventStartDate"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string eventStartDate;

        /// <summary>
        /// The backing field for <see cref="EventEndDate"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string eventEndDate;

        /// <summary>
        /// The backing field for <see cref="Overview"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string overview;

        /// <summary>
        /// The backing field for <see cref="Title"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string title;

        /// <summary>
        /// Gets or sets the event start date to be displayed in the event tooltip.
        /// </summary>
        /// <value>The event start date to be displayed in the event tooltip.</value>
        public string EventStartDate
        {
            [DebuggerStepThrough]
            get { return this.eventStartDate; }
            [DebuggerStepThrough]
            set { this.eventStartDate = value; }
        }

        /// <summary>
        /// Gets or sets the event end date to be displayed in the event tooltip.
        /// </summary>
        /// <value>The event end date to be displayed in the event tooltip.</value>
        public string EventEndDate
        {
            [DebuggerStepThrough]
            get { return this.eventEndDate; }
            [DebuggerStepThrough]
            set { this.eventEndDate = value; }
        }

        /// <summary>
        /// Gets or sets the overview to be displayed in the event tooltip.
        /// </summary>
        /// <value>The overview to be displayed in the event tooltip.</value>
        public string Overview
        {
            [DebuggerStepThrough]
            get { return this.overview; }
            [DebuggerStepThrough]
            set { this.overview = value; }
        }

        /// <summary>
        /// Gets or sets the title to be displayed in the event tooltip.
        /// </summary>
        /// <value>The title to be displayed in the event tooltip.</value>
        public string Title
        {
            [DebuggerStepThrough]
            get { return this.title; }
            [DebuggerStepThrough]
            set { this.title = value; }
        }

        /// <summary>
        /// Sets the ID of the event to be displayed in the event tooltip.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        public void SetEventId(int eventId)
        {
            this.currentEventId = eventId;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += this.Page_PreRender;
            this.RegisterButton.Click += this.RegisterButton_Click;
            this.AddToCalendarButton.Click += this.AddToCalendarButton_Click;
            this.EditButton.Click += this.EditButton_Click;

            AJAX.RegisterPostBackControl(this.AddToCalendarButton);
            this.LocalResourceFile = this.AppRelativeTemplateSourceDirectory + "App_LocalResources/" + Path.GetFileNameWithoutExtension(this.TemplateControl.AppRelativeVirtualPath);
        }

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_PreRender(object sender, EventArgs e)
        {            
            string date = this.EventStartDate;
            if (this.EventEndDate != null)
            {
                date += " - " + this.EventEndDate;
            }

            this.EventDate.Text = date;
            this.EventOverview.Text = this.Overview;
            this.EventTitle.Text = this.Title;

            this.AddToCalendarButton.Visible = Engage.Utility.IsLoggedIn;
            this.EditButton.Visible = IsAdmin;
        }

        /// <summary>
        /// Handles the Click event of the RegisterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.BuildLinkUrl(this.ModuleId, "Register", "eventid=" + this.currentEventId.ToString(CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Handles the Click event of the AddToCalendarButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AddToCalendarButton_Click(object sender, EventArgs e)
        {
            Event currentEvent = Event.Load(this.currentEventId);
            SendICalendarToClient(this.Response, currentEvent.ToICal(this.UserInfo.Email, Utility.GetUserTimeZoneOffset(this.UserInfo, this.PortalSettings)), currentEvent.Title);
        }

        /// <summary>
        /// Handles the Click event of the EditButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EditButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.BuildLinkUrl(this.ModuleId, "EventEdit", "eventId=" + this.currentEventId.ToString(CultureInfo.InvariantCulture)), true);
        }
    }
}
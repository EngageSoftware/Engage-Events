// <copyright file="EventCalendar.ascx.cs" company="Engage Software">
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
    using System.Web.UI;
    using DotNetNuke.Services.Exceptions;
    using Engage.Events;
    using Telerik.Web.UI;
    using Setting = Events.Setting;
    using Utility = Dnn.Utility;

    /// <summary>
    /// Control to display the events calendar view
    /// </summary>
    public partial class EventCalendar : ModuleBase
    {
        /// <summary>
        /// Backing field for <see cref="IsFeatured"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isFeatured;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is displaying only featured events.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is displaying only featured events; otherwise, <c>false</c>.
        /// </value>
        public bool IsFeatured
        {
            [DebuggerStepThrough]
            set { this.isFeatured = value; }
            [DebuggerStepThrough]
            get { return this.isFeatured; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += this.Page_Load;
            this.EventsCalendarDisplay.AppointmentCreated += this.EventsCalendarDisplay_AppointmentCreated;
            this.EventsCalendarDisplay.AppointmentDataBound += this.EventsCalendarDisplay_AppointmentDataBound;
            this.EventsCalendarToolTipManager.AjaxUpdate += this.EventsCalendarToolTipManager_AjaxUpdate;
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
                this.BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the AppointmentCreated event of the EventsCalendarDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.AppointmentCreatedEventArgs"/> instance containing the event data.</param>
        private void EventsCalendarDisplay_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
        {
            if (e.Appointment.Visible && !this.IsAppointmentRegisteredForTooltip(e.Appointment))
            {
                this.EventsCalendarToolTipManager.TargetControls.Add(e.Appointment.ClientID, e.Appointment.ID.ToString(), true);
            }
        }

        /// <summary>
        /// Handles the AppointmentDataBound event of the EventsCalendarDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.SchedulerEventArgs"/> instance containing the event data.</param>
        private void EventsCalendarDisplay_AppointmentDataBound(object sender, SchedulerEventArgs e)
        {
            this.EventsCalendarToolTipManager.TargetControls.Clear();
            ScriptManager.RegisterStartupScript(this, typeof(EventCalendar), "HideToolTip", "hideActiveToolTip();", true);
        }

        /// <summary>
        /// Handles the AjaxUpdate event of the EventsCalendarToolTipManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.ToolTipUpdateEventArgs"/> instance containing the event data.</param>
        private void EventsCalendarToolTipManager_AjaxUpdate(object sender, ToolTipUpdateEventArgs e)
        {
            int eventId;
            if (int.TryParse(e.Value.Split('_')[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out eventId))
            {
                Event ev = Event.Load(eventId);
                EventToolTip toolTip = (EventToolTip)this.LoadControl("EventToolTip.ascx");

                toolTip.ModuleConfiguration = this.ModuleConfiguration;
                toolTip.EventStartDate = ev.EventStart.ToShortTimeString();
                toolTip.Overview = ev.Overview;
                toolTip.Title = ev.Title;
                toolTip.EventEndDate = ev.EventEnd.ToShortTimeString();
                toolTip.SetEventId(ev.Id);
                e.UpdatePanel.ContentTemplateContainer.Controls.Add(toolTip);
            }
        }

        /// <summary>
        /// Determines whether the specified appointment is registered with the tooltip manager.
        /// </summary>
        /// <param name="apt">The appointment</param>
        /// <returns>
        /// <c>true</c> if the specified appointment is registered with the tooltip manager; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAppointmentRegisteredForTooltip(Appointment apt)
        {
            foreach (ToolTipTargetControl targetControl in this.EventsCalendarToolTipManager.TargetControls)
            {
                if (targetControl.TargetControlID == apt.ClientID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            this.EventsCalendarDisplay.DataSource = EventCollection.Load(this.PortalId, ListingMode.All, false, this.isFeatured);
            this.EventsCalendarDisplay.DataEndField = "EventEnd";
            this.EventsCalendarDisplay.DataKeyField = "Id";
            this.EventsCalendarDisplay.DataRecurrenceField = "RecurrenceRule";
            this.EventsCalendarDisplay.DataRecurrenceParentKeyField = "RecurrenceParentId";
            this.EventsCalendarDisplay.DataStartField = "EventStart";
            this.EventsCalendarDisplay.DataSubjectField = "Title";
            this.EventsCalendarDisplay.DataBind();

            if (Utility.GetStringSetting(this.Settings, Setting.SkinSelection.PropertyName) != null)
            {
                this.EventsCalendarDisplay.Skin = this.EventsCalendarToolTipManager.Skin = Utility.GetStringSetting(this.Settings, Setting.SkinSelection.PropertyName);
            }
        }
    }
}
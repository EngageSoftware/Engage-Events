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
    using System.Globalization;
    using System.Web.UI;
    using DotNetNuke.Services.Exceptions;
    using Engage.Events;
    using Telerik.Web.UI;
    using Setting=Setting;

    /// <summary>
    /// Control to display the events calendar view
    /// </summary>
    public partial class EventCalendar : ModuleBase
    {

        private bool isFeatured;

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
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
        /// Handles the AppointmentInsert event of the EventsCalendarDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.SchedulerCancelEventArgs"/> instance containing the event data.</param>
        protected void EventsCalendarDisplay_AppointmentInsert(object sender, SchedulerCancelEventArgs e)
        {
            Event ev = Event.Create(this.PortalId, this.ModuleId, this.UserInfo.Email, e.Appointment.Subject, "", e.Appointment.Start);
            ev.EventEnd = e.Appointment.Start.Add(e.Appointment.Duration);
            ev.Save(this.UserId);
        }

        /// <summary>
        /// Handles the AppointmentDelete event of the EventsCalendarDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.SchedulerCancelEventArgs"/> instance containing the event data.</param>
        protected void EventsCalendarDisplay_AppointmentDelete(object sender, SchedulerCancelEventArgs e)
        {
            Event.Delete(Convert.ToInt32(e.Appointment.ID));
        }

        /// <summary>
        /// Handles the AppointmentUpdate event of the EventsCalendarDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.AppointmentUpdateEventArgs"/> instance containing the event data.</param>
        protected void EventsCalendarDisplay_AppointmentUpdate(object sender, AppointmentUpdateEventArgs e)
        {
            Event ev = Event.Load(Convert.ToInt32(e.Appointment.ID));
            ev.EventStart = e.Appointment.Start;
            ev.EventEnd = e.Appointment.Start.Add(e.Appointment.Duration);
            ev.Title = e.Appointment.Subject;
            ev.Save(this.UserId);
        }

        /// <summary>
        /// Handles the AppointmentCreated event of the EventsCalendarDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.AppointmentCreatedEventArgs"/> instance containing the event data.</param>
        protected void EventsCalendarDisplay_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
        {
            if (e.Appointment.Visible && !this.IsAppointmentRegisteredForTooltip(e.Appointment))
            {
                this.EventsCalendarToolTipManager.TargetControls.Add(e.Appointment.ClientID, true);
            }
        }

        /// <summary>
        /// Handles the AppointmentDataBound event of the EventsCalendarDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.SchedulerEventArgs"/> instance containing the event data.</param>
        protected void EventsCalendarDisplay_AppointmentDataBound(object sender, SchedulerEventArgs e)
        {
            Event ev = Event.Load(Convert.ToInt32(e.Appointment.ID, CultureInfo.InvariantCulture));
            e.Appointment.Attributes["EventTitle"] = ev.Title;
            e.Appointment.Attributes["EventStart"] = ev.EventStart.ToString();
            e.Appointment.Attributes["Overview"] = ev.Overview;
            e.Appointment.Attributes["EventId"] = ev.Id.ToString(CultureInfo.InvariantCulture);

            if (ev.EventEnd != null)
            {
                e.Appointment.Attributes["EventEnd"] = ev.EventEnd.ToString();
            }

            this.EventsCalendarToolTipManager.TargetControls.Clear();
            ScriptManager.RegisterStartupScript(this, typeof(EventCalendar), "HideToolTip", "hideActiveToolTip();", true);
        }

        /// <summary>
        /// Handles the AjaxUpdate event of the EventsCalendarToolTipManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.ToolTipUpdateEventArgs"/> instance containing the event data.</param>
        protected void EventsCalendarToolTipManager_AjaxUpdate(object sender, ToolTipUpdateEventArgs e)
        {
            int aptId = int.Parse(e.TargetControlID.Split('_')[5]);
            Appointment apt = this.EventsCalendarDisplay.Appointments[aptId];
            EventToolTip toolTip = (EventToolTip)this.LoadControl("EventToolTip.ascx");

            toolTip.ModuleConfiguration = this.ModuleConfiguration;
            toolTip.EventStartDate = apt.Attributes["EventStart"];
            toolTip.Overview = apt.Attributes["Overview"];
            toolTip.Title = apt.Attributes["EventTitle"];
            toolTip.EventEndDate = apt.Attributes["EventEnd"];
            toolTip.SetEventId(Convert.ToInt32(apt.Attributes["EventId"], CultureInfo.InvariantCulture));
            e.UpdatePanel.ContentTemplateContainer.Controls.Add(toolTip);
        }

        /// <summary>
        /// Determines whether [is appointment registered for tooltip] [the specified apt].
        /// </summary>
        /// <param name="apt">The appointment</param>
        /// <returns>
        /// 	<c>true</c> if [is appointment registered for tooltip] [the specified apt]; otherwise, <c>false</c>.
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
            this.EventsCalendarDisplay.DataSource = EventCollection.Load(this.PortalId, ListingMode.All, "EventStart", 0, 0, true, this.isFeatured);
            this.EventsCalendarDisplay.DataBind();

            if (Utility.GetStringSetting(this.Settings, Setting.SkinSelection.PropertyName) != null)
            {
                this.EventsCalendarDisplay.Skin = this.EventsCalendarToolTipManager.Skin = Utility.GetStringSetting(this.Settings, Setting.SkinSelection.PropertyName);
            }
        }

        public bool IsFeatured
        {
            set { this.isFeatured = value; }
            protected get { return this.isFeatured; }
        }
    }
}
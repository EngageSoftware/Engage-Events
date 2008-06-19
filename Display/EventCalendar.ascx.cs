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

using DotNetNuke.Services.Localization;

namespace Engage.Dnn.Events
{
    using System;
    using System.Globalization;
    using System.Web.UI;
    using Display;
    using DotNetNuke.Services.Exceptions;
    using Engage.Events;
    using Telerik.Web.UI;

    /// <summary>
    /// Control to display the events calendar view
    /// </summary>
    public partial class EventCalendar : ModuleBase
    {
        #region Event Handlers

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (DotNetNuke.Framework.AJAX.IsInstalled())
                {
                    DotNetNuke.Framework.AJAX.RegisterScriptManager();
                }

                this.BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the AppointmentInsert event of the RadScheduler1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.SchedulerCancelEventArgs"/> instance containing the event data.</param>
        protected void RadScheduler1_AppointmentInsert(object sender, SchedulerCancelEventArgs e)
        {
            Event ev = Event.Create(PortalId, ModuleId, UserInfo.Email, e.Appointment.Subject, "", e.Appointment.Start);
            ev.EventEnd = e.Appointment.Start.Add(e.Appointment.Duration);
            ev.Save(UserId);
        }

        /// <summary>
        /// Handles the AppointmentDelete event of the RadScheduler1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.SchedulerCancelEventArgs"/> instance containing the event data.</param>
        protected void RadScheduler1_AppointmentDelete(object sender, SchedulerCancelEventArgs e)
        {
            Event.Delete(Convert.ToInt32(e.Appointment.ID));
        }

        /// <summary>
        /// Handles the AppointmentUpdate event of the RadScheduler1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.AppointmentUpdateEventArgs"/> instance containing the event data.</param>
        protected void RadScheduler1_AppointmentUpdate(object sender, AppointmentUpdateEventArgs e)
        {
            Event ev = Event.Load(Convert.ToInt32(e.Appointment.ID));
            ev.EventStart = e.Appointment.Start;
            ev.EventEnd = e.Appointment.Start.Add(e.Appointment.Duration);
            ev.Title = e.Appointment.Subject;
            ev.Save(UserId);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the AppointmentCreated event of the RadScheduler1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.AppointmentCreatedEventArgs"/> instance containing the event data.</param>
        protected void RadScheduler1_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
        {
            if (e.Appointment.Visible && !this.IsAppointmentRegisteredForTooltip(e.Appointment))
            {
                RadToolTipManager1.TargetControls.Add(e.Appointment.ClientID, true);
            }
        }

        /// <summary>
        /// Handles the AppointmentDataBound event of the RadScheduler1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.SchedulerEventArgs"/> instance containing the event data.</param>
        protected void RadScheduler1_AppointmentDataBound(object sender, SchedulerEventArgs e)
        {
            Event ev = Event.Load(Convert.ToInt32(e.Appointment.ID, CultureInfo.InvariantCulture));
            e.Appointment.Attributes["EventTitle"] = ev.Title;
            e.Appointment.Attributes["EventStart"] = ev.EventStart.ToString();
            e.Appointment.Attributes["Overview"] = ev.Overview;

            if (ev.EventEnd != null)
            {
                e.Appointment.Attributes["EventEnd"] = ev.EventEnd.ToString();
            }

            RadToolTipManager1.TargetControls.Clear();
            ScriptManager.RegisterStartupScript(this, typeof(EventCalendar), "HideToolTip", "hideActiveToolTip();", true);
        }

        /// <summary>
        /// Handles the AjaxUpdate event of the RadToolTipManager1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.ToolTipUpdateEventArgs"/> instance containing the event data.</param>
        protected void RadToolTipManager1_AjaxUpdate(object sender, ToolTipUpdateEventArgs e)
        {
            int aptId = int.Parse(e.TargetControlID.Split(Convert.ToChar("_"))[5]);
            Appointment apt = RadScheduler1.Appointments[aptId];
            EventToolTip toolTip = ((EventToolTip)(LoadControl("EventToolTip.ascx")));
            
            toolTip.EventStartDate = apt.Attributes["EventStart"];
            toolTip.Overview = apt.Attributes["Overview"];
            toolTip.Title = apt.Attributes["EventTitle"];
            toolTip.EventEndDate = apt.Attributes["EventEnd"];
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
            foreach (ToolTipTargetControl targetControl in RadToolTipManager1.TargetControls)
            {
                if ((targetControl.TargetControlID == apt.ClientID))
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
            EventCollection events = EventCollection.Load(PortalId, "EventStart", 0, 0, true);
            RadScheduler1.DataSource = events;
            RadScheduler1.DataBind();

            if (Settings[Setting.SkinSelection.PropertyName] != null)
            {
                string SelectedSkin = Settings[Setting.SkinSelection.PropertyName].ToString();
                RadScheduler1.Skin = RadToolTipManager1.Skin = SelectedSkin;
            }
        }

        #endregion
    }
}
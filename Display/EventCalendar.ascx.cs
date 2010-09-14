// <copyright file="EventCalendar.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2010
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Engage.Events;
    using Telerik.Web.UI;

    /// <summary>
    /// Control to display the events calendar view
    /// </summary>
    public partial class EventCalendar : ModuleBase
    {
        /// <summary>
        /// The colors for categories built into the RadScheduler
        /// </summary>
        private static readonly List<string> Colors = new List<string>(10)
            {
                "Blue",
                "Green",
                "Pink",
                "Violet",
                "Red",
                "DarkBlue",
                "DarkGreen",
                "DarkRed",
                "Orange",
                "Yellow"
            };

        /// <summary>
        /// A map between a category ID and a color
        /// </summary>
        private readonly Dictionary<int, string> categoryColors = new Dictionary<int, string>();

        /// <summary>
        /// Gets or sets the ID of the <see cref="Event"/> last displayed in the tool-tip.
        /// </summary>
        /// <value>The ID of the event in the tool-tip.</value>
        private int? ToolTipEventId
        {
            get
            {
                return this.ViewState["ToolTipEventId"] as int?;
            }

            set
            {
                this.ViewState["ToolTipEventId"] = value;
            }
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
                this.AddJQueryReference();
                this.LocalizeCalendar();
                this.BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Localizes the calendar.
        /// </summary>
        private void LocalizeCalendar()
        {
            this.EventsCalendarDisplay.Localization.HeaderToday = Localization.GetString("HeaderToday.Text", this.LocalResourceFile);
            this.EventsCalendarDisplay.Localization.HeaderPrevDay = Localization.GetString("HeaderPrevDay.Text", this.LocalResourceFile);
            this.EventsCalendarDisplay.Localization.HeaderNextDay = Localization.GetString("HeaderNextDay.Text", this.LocalResourceFile);
            this.EventsCalendarDisplay.Localization.HeaderDay = Localization.GetString("HeaderDay.Text", this.LocalResourceFile);
            this.EventsCalendarDisplay.Localization.HeaderWeek = Localization.GetString("HeaderWeek.Text", this.LocalResourceFile);
            this.EventsCalendarDisplay.Localization.HeaderMonth = Localization.GetString("HeaderMonth.Text", this.LocalResourceFile);

            this.EventsCalendarDisplay.Localization.AllDay = Localization.GetString("AllDay.Text", this.LocalResourceFile);
            this.EventsCalendarDisplay.Localization.Show24Hours = Localization.GetString("Show24Hours.Text", this.LocalResourceFile);
            this.EventsCalendarDisplay.Localization.ShowBusinessHours = Localization.GetString("ShowBusinessHours.Text", this.LocalResourceFile);

            this.EventsCalendarDisplay.Localization.ShowMore = Localization.GetString("ShowMore.Text", this.LocalResourceFile);
        }

        /// <summary>
        /// Handles the AppointmentCreated event of the EventsCalendarDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.AppointmentCreatedEventArgs"/> instance containing the event data.</param>
        private void EventsCalendarDisplay_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
        {
            if (e.Appointment.Visible && !this.IsAppointmentRegisteredForToolTip(e.Appointment))
            {
                foreach (AppointmentControl appointmentControl in e.Appointment.AppointmentControls)
                {
                    this.EventsCalendarToolTipManager.TargetControls.Add(appointmentControl.ClientID, e.Appointment.ID.ToString(), true);                    
                }
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

            var category = ((Event)e.Appointment.DataItem).Category;
            var categoryName = string.IsNullOrEmpty(category.Name) ? this.Localize("DefaultCategory", this.LocalSharedResourceFile) : category.Name;
            
            string color;
            if (!this.categoryColors.TryGetValue(category.Id, out color))
            {
                color = "Default";
            }

            e.Appointment.CssClass = string.Format(
                CultureInfo.InvariantCulture, 
                "cat-{0} rsCategory{1}", 
                Engage.Utility.ConvertToSlug(categoryName),
                color);
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
                this.ToolTipEventId = eventId;
                this.ShowToolTip(eventId, e.UpdatePanel);
            }
        }

        /// <summary>
        /// Sets up the <see cref="EventToolTip"/> control and displays it
        /// </summary>
        /// <param name="eventId">The ID of the <see cref="Event"/> to display within the tool-tip.</param>
        /// <param name="panel">The panel in which the tool-tip is displayed.</param>
        private void ShowToolTip(int eventId, UpdatePanel panel)
        {
            Event ev = Event.Load(eventId);
            if (!this.CanShowEvent(ev))
            {
                return;
            }

            var toolTip = (EventToolTip)(panel.ContentTemplateContainer.FindControl("EventToolTip") ?? this.LoadControl("EventToolTip.ascx"));

            toolTip.ID = "EventToolTip";
            toolTip.ModuleConfiguration = this.ModuleConfiguration;
            toolTip.SetEvent(ev);
            toolTip.ShowEvent();

            if (!panel.ContentTemplateContainer.Controls.Contains(toolTip))
            {
                panel.ContentTemplateContainer.Controls.Add(toolTip);
            }
        }

        /// <summary>
        /// Determines whether the specified appointment is registered with the tool-tip manager.
        /// </summary>
        /// <param name="apt">The appointment</param>
        /// <returns>
        /// <c>true</c> if the specified appointment is registered with the tool-tip manager; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAppointmentRegisteredForToolTip(Appointment apt)
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
            var events = EventCollection.Load(this.PortalId, ListingMode.All, false, this.IsFeatured, this.CategoryIds);
            this.StyleCategories(events);

            this.EventsCalendarDisplay.Culture = CultureInfo.CurrentCulture;
            this.EventsCalendarDisplay.DataSource = events;
            this.EventsCalendarDisplay.DataEndField = "EventEnd";
            this.EventsCalendarDisplay.DataKeyField = "Id";
            this.EventsCalendarDisplay.DataRecurrenceField = "RecurrenceRule";
            this.EventsCalendarDisplay.DataRecurrenceParentKeyField = "RecurrenceParentId";
            this.EventsCalendarDisplay.DataStartField = "EventStart";
            this.EventsCalendarDisplay.DataSubjectField = "Title";
            this.EventsCalendarDisplay.DataBind();

            var skinSetting = ModuleSettings.SkinSelection.GetValueAsEnumFor<TelerikSkin>(this).Value;
            this.EventsCalendarDisplay.Skin = this.EventsCalendarToolTipManager.Skin = skinSetting.ToString();

            this.EventsCalendarDisplay.MonthView.VisibleAppointmentsPerDay = ModuleSettings.EventsPerDay.GetValueAsInt32For(this).Value;

            if (this.ToolTipEventId.HasValue)
            {
                this.ShowToolTip(this.ToolTipEventId.Value, this.EventsCalendarToolTipManager.UpdatePanel);
                this.ToolTipEventId = null;
            }
        }

        /// <summary>
        /// Sets the <see cref="EventsCalendarDisplay"/> to style each event by category.
        /// </summary>
        /// <param name="events">The events.</param>
        private void StyleCategories(IEnumerable<Event> events)
        {
            var categoryIds = events.Select(e => e.CategoryId).Distinct();

            var i = 0;
            foreach (var categoryId in categoryIds)
            {
                this.categoryColors.Add(categoryId, Colors[i++]);

                if (i == Colors.Count)
                {
                    break;
                }
            }
        }
    }
}
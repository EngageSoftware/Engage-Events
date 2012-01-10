// <copyright file="EventCalendar.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
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
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using DotNetNuke.Services.Exceptions;

    using Engage.Events;

    using Telerik.Web.UI;

    /// <summary>
    /// Control to display the events calendar view
    /// </summary>
    public partial class EventCalendar : ModuleBase
    {
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
        /// Gets or sets the occurrence date of the <see cref="Event"/> last displayed in the tool-tip.
        /// </summary>
        /// <value>The occurrence date of the event (or <c>null</c> for non-recurring events) in the tool-tip.</value>
        private DateTime? ToolTipEventOccurrenceDate
        {
            get
            {
                return this.ViewState["ToolTipEventOccurrenceDate"] as DateTime?;
            }

            set
            {
                this.ViewState["ToolTipEventOccurrenceDate"] = value;
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
            this.EventsCalendarDisplay.DataBound += this.EventsCalendarDisplay_DataBound;
            this.EventsCalendarDisplay.NavigationCommand += this.EventsCalendarDisplay_NavigationCommand;
            this.EventsCalendarToolTipManager.AjaxUpdate += this.EventsCalendarToolTipManager_AjaxUpdate;
            this.CategoryFilterAction.CategoryChanged += this.CategoryFilterAction_CategoryChanged;
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
            this.EventsCalendarDisplay.Culture = CultureInfo.CurrentCulture;
            this.EventsCalendarDisplay.FirstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var lastDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - 1;
            this.EventsCalendarDisplay.LastDayOfWeek = lastDayOfWeek < DayOfWeek.Sunday ? DayOfWeek.Saturday : lastDayOfWeek;

            this.EventsCalendarDisplay.Localization.HeaderToday = this.Localize("HeaderToday.Text");
            this.EventsCalendarDisplay.Localization.HeaderPrevDay = this.Localize("HeaderPrevDay.Text");
            this.EventsCalendarDisplay.Localization.HeaderNextDay = this.Localize("HeaderNextDay.Text");
            this.EventsCalendarDisplay.Localization.HeaderDay = this.Localize("HeaderDay.Text");
            this.EventsCalendarDisplay.Localization.HeaderWeek = this.Localize("HeaderWeek.Text");
            this.EventsCalendarDisplay.Localization.HeaderMonth = this.Localize("HeaderMonth.Text");

            this.EventsCalendarDisplay.Localization.AllDay = this.Localize("AllDay.Text");
            this.EventsCalendarDisplay.Localization.Show24Hours = this.Localize("Show24Hours.Text");
            this.EventsCalendarDisplay.Localization.ShowBusinessHours = this.Localize("ShowBusinessHours.Text");

            this.EventsCalendarDisplay.Localization.ShowMore = this.Localize("ShowMore.Text");
        }

        /// <summary>
        /// Handles the AppointmentCreated event of the EventsCalendarDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.AppointmentCreatedEventArgs"/> instance containing the event data.</param>
        private void EventsCalendarDisplay_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
        {
            if (!e.Appointment.Visible || this.IsAppointmentRegisteredForToolTip(e.Appointment))
            {
                return;
            }

            var appointmentId = e.Appointment.ID.ToString();
            foreach (var domElementId in e.Appointment.DomElements)
            {
                this.EventsCalendarToolTipManager.TargetControls.Add(domElementId, appointmentId, true);
            }
        }

        /// <summary>
        /// Handles the AppointmentDataBound event of the EventsCalendarDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.SchedulerEventArgs"/> instance containing the event data.</param>
        private void EventsCalendarDisplay_AppointmentDataBound(object sender, SchedulerEventArgs e)
        {
            var @event = (Event)e.Appointment.DataItem;
            var category = @event.Category;
            var categoryName = string.IsNullOrEmpty(category.Name) ? this.Localize("DefaultCategory", this.LocalSharedResourceFile) : category.Name;
            var color = category.Color ?? "Default";

            e.Appointment.CssClass = string.Format(
                CultureInfo.InvariantCulture, 
                "cat-{0} rsCategory{1} {2} {3} {4}", 
                Engage.Utility.ConvertToSlug(categoryName),
                Engage.Utility.ConvertToSlug(color),
                @event.IsFeatured ? "featured" : string.Empty,
                @event.IsRecurring ? "recurring" : string.Empty,
                @event.IsFull ? "seats-full" : @event.Capacity.HasValue ? "seats-available" : "no-seat-limit");
        }

        /// <summary>
        /// Handles the <see cref="BaseDataBoundControl.DataBound"/> event of the <see cref="EventsCalendarDisplay"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void EventsCalendarDisplay_DataBound(object sender, EventArgs e)
        {
            ////this.ToolTipEventId = null;
            this.EventsCalendarToolTipManager.TargetControls.Clear();
            ScriptManager.RegisterStartupScript(this, typeof(EventCalendar), "HideToolTip", "hideActiveToolTip();", true);
        }

        /// <summary>
        /// Handles the <see cref="RadScheduler.NavigationCommand"/> event of the <see cref="EventsCalendarDisplay"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SchedulerNavigationCommandEventArgs"/> instance containing the event data.</param>
        private void EventsCalendarDisplay_NavigationCommand(object sender, SchedulerNavigationCommandEventArgs e)
        {
            this.ToolTipEventId = null;
        }

        /// <summary>
        /// Handles the AjaxUpdate event of the EventsCalendarToolTipManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.ToolTipUpdateEventArgs"/> instance containing the event data.</param>
        private void EventsCalendarToolTipManager_AjaxUpdate(object sender, ToolTipUpdateEventArgs e)
        {
            // Value is ID_# when when the appointment is a recurrence, but just ID when it's not
            int eventId;
            DateTime? occurrenceDate = null;
            if (!int.TryParse(e.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out eventId))
            {
                var appointment = this.EventsCalendarDisplay.Appointments.FindByID(e.Value);
                eventId = (int)appointment.RecurrenceParentID;
                occurrenceDate = appointment.Start;
            }

            var @event = Event.Load(eventId);
            this.ToolTipEventId = eventId;
            this.ToolTipEventOccurrenceDate = occurrenceDate;
            this.ShowToolTip(@event, occurrenceDate, e.UpdatePanel);
        }

        /// <summary>
        /// Handles the <see cref="Events.CategoryFilterAction.CategoryChanged"/> event of the <see cref="CategoryFilterAction"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CategoryFilterAction_CategoryChanged(object sender, EventArgs e)
        {
            ////this.BindData();
            // NOTE: temporarly reload the page instead of using postback since the multiplecategoriesfilter viewstate doesn't seem to sync properly because of all the client javascript.
            this.Response.Redirect(BuildLinkUrl(this.TabId));
        }

        /// <summary>
        /// Sets up the <see cref="EventToolTip"/> control and displays it
        /// </summary>
        /// <param name="event">The event being displayed</param>
        /// <param name="occurrenceDate">The occurrence date of the event, or <c>null</c> if it's not a recurring event.</param>
        /// <param name="panel">The panel in which the tool-tip is displayed.</param>
        private void ShowToolTip(Event @event, DateTime? occurrenceDate, UpdatePanel panel)
        {
            if (!this.CanShowEvent(@event))
            {
                return;
            }

            Debug.Assert(occurrenceDate.HasValue == @event.IsRecurring, "Recurring events need occurrence dates");
            if (occurrenceDate != null)
            {
                @event = @event.CreateOccurrence(DateTime.SpecifyKind(occurrenceDate.Value, DateTimeKind.Unspecified));
            }

            var toolTip = (EventToolTip)(panel.ContentTemplateContainer.FindControl("EventToolTip") ?? this.LoadControl("EventToolTip.ascx"));

            toolTip.ID = "EventToolTip";
            toolTip.ModuleConfiguration = this.ModuleConfiguration;
            toolTip.SetEvent(@event);
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
            return this.EventsCalendarToolTipManager.TargetControls.Cast<ToolTipTargetControl>().Any(targetControl => apt.DomElements.Contains(targetControl.TargetControlID));
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            this.EventsCalendarDisplay.DataEndField = "EventEnd";
            this.EventsCalendarDisplay.DataKeyField = "Id";
            this.EventsCalendarDisplay.DataRecurrenceField = "RecurrenceRule";
            this.EventsCalendarDisplay.DataRecurrenceParentKeyField = "RecurrenceParentId";
            this.EventsCalendarDisplay.DataStartField = "EventStart";
            this.EventsCalendarDisplay.DataSubjectField = "Title";

            var selectedCategoryId = this.CategoryFilterAction.SelectedCategoryIds;
            this.EventsCalendarDisplay.DataSource = EventCollection.Load(
                this.PortalId,
                null,
                null,
                false,
                this.IsFeatured,
                this.HideFullEvents,
                IsLoggedIn ? this.UserInfo.Email : null,
                selectedCategoryId ?? this.CategoryIds);
            this.EventsCalendarDisplay.DataBind();

            var skinSetting = ModuleSettings.SkinSelection.GetValueAsEnumFor<TelerikSkin>(this).Value;
            this.EventsCalendarDisplay.Skin = this.EventsCalendarToolTipManager.Skin = skinSetting.ToString();

            this.EventsCalendarDisplay.MonthView.VisibleAppointmentsPerDay = ModuleSettings.EventsPerDay.GetValueAsInt32For(this).Value;

            if (this.ToolTipEventId.HasValue)
            {
                this.ShowToolTip(Event.Load(this.ToolTipEventId.Value), this.ToolTipEventOccurrenceDate, this.EventsCalendarToolTipManager.UpdatePanel);
                this.ToolTipEventId = null;
            }
        }
    }
}

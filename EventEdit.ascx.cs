// <copyright file="EventEdit.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2010
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
    using System.Linq;
    using System.Web.UI.WebControls;
    using DotNetNuke.Common;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.UserControls;

    using Engage.Events;

    /// <summary>
    /// This class contains a collection of methods for adding or editing an Event.
    /// </summary>
    public partial class EventEdit : ModuleBase
    {
        /// <summary>
        /// Gets the URL to navigate to in order to add a new event.
        /// </summary>
        /// <value>The URL to navigate to in order to add a new event</value>
        private string AddEventUrl
        {
            get { return this.BuildLinkUrl(this.ModuleId, "EventEdit"); }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AJAX.RegisterPostBackControl(this.SaveEventButton);
            AJAX.RegisterPostBackControl(this.SaveAndCreateNewEventButton);

            this.Load += this.Page_Load;
            this.SaveEventButton.Click += this.SaveEventButton_OnClick;
            this.SaveAndCreateNewEventButton.Click += this.SaveAndCreateNewEventButton_OnClick;
            this.RecurringCheckBox.CheckedChanged += this.RecurringCheckbox_CheckedChanged;
            this.AllowRegistrationsCheckBox.CheckedChanged += this.AllowRegistrationsCheckBox_CheckedChanged;
            this.LimitRegistrationsCheckBox.CheckedChanged += this.LimitRegistrationsCheckBox_CheckedChanged;
            this.CapacityMetMessageRadioButtonList.SelectedIndexChanged += this.CapacityMetMessageRadioButtonList_SelectedIndexChanged;
            this.RecurrenceEditorValidator.ServerValidate += this.RecurrenceEditorValidator_ServerValidate;
            this.EventDescriptionTextEditorValidator.ServerValidate += this.EventDescriptionTextEditorValidator_ServerValidate;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the AllowRegistrationsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AllowRegistrationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.LimitRegistrationsPanel.Visible = this.AllowRegistrationsCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the LimitRegistrationsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LimitRegistrationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.RegistrationLimitPanel.Visible = this.LimitRegistrationsCheckBox.Checked;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the CapacityMetMessageRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CapacityMetMessageRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CustomCapacityMetMessagePanel.Visible = bool.Parse(this.CapacityMetMessageRadioButtonList.SelectedValue);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.FillLists();

                    if (this.EventId.HasValue)
                    {
                        this.BindData();
                    }
                }

                this.SetButtonLinks();
                this.LocalizeControl();
                this.SuccessModuleMessage.Visible = false;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the OnClick event of the SaveEventButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SaveEventButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (this.Page.IsValid)
                {
                    this.Save();
                    this.DisplayFinalSuccess();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the OnClick event of the SaveAndCreateNewEventButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SaveAndCreateNewEventButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (this.Page.IsValid)
                {
                    this.Save();
                    this.Response.Redirect(this.AddEventUrl, true);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the ServerValidate event of the EventDescriptionTextEditorValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        private void EventDescriptionTextEditorValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.TextEditorHasValue(this.EventDescriptionTextEditor);
        }

        /// <summary>
        /// Handles the ServerValidate event of the RecurrenceEditorValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        private void RecurrenceEditorValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.RecurrenceEditor.IsValid;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the RecurringCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RecurringCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.RecurrenceEditor.Visible = this.RecurringCheckBox.Checked;
        }

        /// <summary>
        /// Determines whether the given <paramref name="textEditor"/> has a meaningful value, checking for empty HTML (based on the "DefaultEmptyText" resource key).
        /// </summary>
        /// <param name="textEditor">The text editor to check.</param>
        /// <returns><c>true</c> if the given <paramref name="textEditor"/> has any meaningful value, otherwise <c>false</c></returns>
        private bool TextEditorHasValue(TextEditor textEditor)
        {
            string defaultEmptyText = Localization.GetString("DefaultEmptyText.Text", this.LocalResourceFile);
#if DEBUG
            defaultEmptyText = defaultEmptyText.Replace("[L]", string.Empty);
#endif
            return Engage.Utility.HasValue(textEditor.Text) && !textEditor.Text.Equals(defaultEmptyText, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the custom capacity met message entered for this event.
        /// </summary>
        /// <returns>The capacity met message to use for this event, or <c>null</c> if the default message is to be used</returns>
        private string GetCustomCapacityMetMessage()
        {
            return this.CapacityMetMessageRadioButtonList.Visible && bool.Parse(this.CapacityMetMessageRadioButtonList.SelectedValue) ? this.CustomCapacityMetMessageTextEditor.Text : null;
        }

        /// <summary>
        /// Gets the ID of the selected category (create a new category if no existing category was selected).
        /// </summary>
        /// <returns>The ID of the category</returns>
        private int GetSelectedCategoryId()
        {
            int categoryId;
            if (int.TryParse(this.CategoryComboBox.SelectedValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out categoryId))
            {
                return categoryId;
            }

            var category = Category.Create(this.PortalId, this.CategoryComboBox.Text);
            category.Save(this.UserId);
            return category.Id;
        }

        /// <summary>
        /// Gets the capacity for this event.
        /// </summary>
        /// <returns>This event's capacity</returns>
        private int? GetEventCapacity()
        {
            return this.LimitRegistrationsCheckBox.Checked && this.LimitRegistrationsCheckBox.Visible ? (int?)this.RegistrationLimitTextBox.Value : null;
        }

        /// <summary>
        /// Fills the <see cref="TimeZoneDropDownList"/> and <see cref="CategoryComboBox"/>.
        /// </summary>
        private void FillLists()
        {
            // TODO: Now that we support .NET 3.5, replace this with TimeZoneInfo.GetSystemTimeZones
            Localization.LoadTimeZoneDropDownList(this.TimeZoneDropDownList, CultureInfo.CurrentCulture.Name, ((int)Dnn.Utility.GetUserTimeZoneOffset(this.UserInfo, this.PortalSettings).TotalMinutes).ToString(CultureInfo.InvariantCulture));

            this.CategoryComboBox.DataTextField = "Name";
            this.CategoryComboBox.DataValueField = "Id";
            this.CategoryComboBox.DataSource = from category in CategoryCollection.Load(this.PortalId)
                                               select new
                                                   {
                                                       Name = string.IsNullOrEmpty(category.Name)
                                                                ? this.Localize("DefaultCategory.Text", this.LocalSharedResourceFile)
                                                                : category.Name,
                                                       Id = category.Id.ToString(CultureInfo.InvariantCulture)
                                                   };
            this.CategoryComboBox.DataBind();
        }

        /// <summary>
        /// Displays the final success.
        /// </summary>
        private void DisplayFinalSuccess()
        {
            this.SuccessModuleMessage.Visible = true;
            this.AddNewEvent.Visible = false;
            this.FooterMultiview.SetActiveView(this.FinalFooterView);
        }

        /// <summary>
        /// Sets the <c>NavigateUrl</c> property for the button links.
        /// </summary>
        private void SetButtonLinks()
        {
            this.CancelGoHomeLink.NavigateUrl = this.CancelEventLink.NavigateUrl = Globals.NavigateURL();
            this.CreateAnotherEventLink.NavigateUrl = this.AddEventUrl;
        }

        /// <summary>
        /// This method will update the form with any localized values that cannot be localized by using the ResourceKey attribute in the control's markup.
        /// </summary>
        private void LocalizeControl()
        {
            string addEditResourceKey = this.EventId.HasValue ? "EditEvent.Text" : "AddNewEvent.Text";
            this.AddEditEventLabel.Text = Localization.GetString(addEditResourceKey, this.LocalResourceFile);
            this.SaveAndCreateNewEventButton.AlternateText = Localization.GetString("SaveAndCreateNew.Alt", LocalResourceFile);
            this.SaveEventButton.AlternateText = Localization.GetString("Save.Alt", LocalResourceFile);
        }

        /// <summary>
        /// This method will either update or create an event based on the current context of EventId
        /// </summary>
        private void Save()
        {
            if (this.EventId.HasValue)
            {
                this.Update();
            }
            else
            {
                this.Insert();
            }
        }

        /// <summary>
        /// This method is responsible for updating an event which already exists in the data store.
        /// </summary>
        private void Update()
        {
            int timeZoneOffsetMinutes;
            int? eventId = this.EventId;
            if (eventId.HasValue
                && int.TryParse(this.TimeZoneDropDownList.SelectedValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out timeZoneOffsetMinutes))
            {
                var timeZoneOffset = new TimeSpan(0, timeZoneOffsetMinutes, 0);
                if (this.InDaylightTimeCheckBox.Checked)
                {
                    timeZoneOffset = timeZoneOffset.Add(new TimeSpan(1, 0, 0));
                }

                Event e = Event.Load(eventId.Value);
                e.EventStart = this.StartDateTimePicker.SelectedDate.Value;
                e.EventEnd = this.EndDateTimePicker.SelectedDate.Value;
                e.TimeZoneOffset = timeZoneOffset;
                e.InDaylightTime = this.InDaylightTimeCheckBox.Checked;
                e.Location = this.EventLocationTextBox.Text;
                e.Title = this.EventTitleTextBox.Text;
                e.Overview = this.EventOverviewTextEditor.Text;
                e.Description = this.EventDescriptionTextEditor.Text;
                e.IsFeatured = this.FeaturedCheckBox.Checked;
                e.AllowRegistrations = this.AllowRegistrationsCheckBox.Checked;
                e.Capacity = this.GetEventCapacity();
                e.RecurrenceRule = this.RecurrenceEditor.GetRecurrenceRule(e.EventStart, e.EventEnd);
                e.CapacityMetMessage = this.GetCustomCapacityMetMessage();
                e.CategoryId = this.GetSelectedCategoryId();
                e.Save(this.UserId);
            }
        }

        /// <summary>
        /// Based on the values entered by the user for the event, this method will populate an event object and call the Event object's save method.
        /// </summary>
        private void Insert()
        {
            int timeZoneOffsetMinutes;
            if (int.TryParse(this.TimeZoneDropDownList.SelectedValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out timeZoneOffsetMinutes))
            {
                var timeZoneOffset = TimeSpan.FromMinutes(timeZoneOffsetMinutes);
                if (this.InDaylightTimeCheckBox.Checked)
                {
                    timeZoneOffset = timeZoneOffset.Add(TimeSpan.FromHours(1));
                }

                DateTime eventStart = this.StartDateTimePicker.SelectedDate.Value;
                DateTime eventEnd = this.EndDateTimePicker.SelectedDate.Value;
                Event e = Event.Create(
                        this.PortalId,
                        this.ModuleId,
                        this.UserInfo.Email,
                        this.EventTitleTextBox.Text,
                        this.EventOverviewTextEditor.Text,
                        this.EventDescriptionTextEditor.Text,
                        eventStart,
                        eventEnd,
                        timeZoneOffset,
                        this.EventLocationTextBox.Text,
                        this.FeaturedCheckBox.Checked,
                        this.AllowRegistrationsCheckBox.Checked,
                        this.RecurrenceEditor.GetRecurrenceRule(eventStart, eventEnd),
                        this.LimitRegistrationsCheckBox.Checked && this.LimitRegistrationsCheckBox.Visible ? (int?)this.RegistrationLimitTextBox.Value : null,
                        this.InDaylightTimeCheckBox.Checked,
                        this.GetCustomCapacityMetMessage(),
                        this.GetSelectedCategoryId());
                
                e.Save(this.UserId);
            }
        }

        /// <summary>
        /// Based on an EventId, this method populates the <see cref="EventEdit"/> user control with the Event's details.
        /// </summary>
        private void BindData()
        {
            int? eventId = this.EventId;
            if (eventId.HasValue)
            {
                Event e = Event.Load(eventId.Value);
                this.EventTitleTextBox.Text = e.Title;
                this.CategoryComboBox.SelectedValue = e.CategoryId.ToString(CultureInfo.InvariantCulture);
                this.EventLocationTextBox.Text = e.Location;
                this.EventOverviewTextEditor.Text = e.Overview;
                this.EventDescriptionTextEditor.Text = e.Description;
                this.StartDateTimePicker.SelectedDate = e.EventStart;
                this.EndDateTimePicker.SelectedDate = e.EventEnd;
                this.FeaturedCheckBox.Checked = e.IsFeatured;
                this.RecurringCheckBox.Checked = e.IsRecurring;
                this.RecurrenceEditor.Visible = this.RecurringCheckBox.Checked;
                this.RecurrenceEditor.SetRecurrenceRule(e.RecurrenceRule);

                this.AllowRegistrationsCheckBox.Checked = this.LimitRegistrationsPanel.Visible = e.AllowRegistrations;
                this.LimitRegistrationsCheckBox.Checked = this.RegistrationLimitPanel.Visible = e.Capacity.HasValue;
                if (e.Capacity.HasValue)
                {
                    this.RegistrationLimitTextBox.Value = e.Capacity.Value;
                }

                bool hasCustomCapacityMetMessage = e.CapacityMetMessage != null;
                this.CustomCapacityMetMessagePanel.Visible = hasCustomCapacityMetMessage;
                this.CapacityMetMessageRadioButtonList.SelectedValue = hasCustomCapacityMetMessage.ToString(CultureInfo.InvariantCulture);
                this.CustomCapacityMetMessageTextEditor.Text = e.CapacityMetMessage;

                this.InDaylightTimeCheckBox.Checked = e.InDaylightTime;
                TimeSpan timeZoneOffset = e.TimeZoneOffset;
                if (e.InDaylightTime)
                {
                    timeZoneOffset = timeZoneOffset.Subtract(new TimeSpan(1, 0, 0));
                }

                this.TimeZoneDropDownList.SelectedValue = ((int)timeZoneOffset.TotalMinutes).ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
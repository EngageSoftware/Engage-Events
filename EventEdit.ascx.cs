 // <copyright file="EventEdit.ascx.cs" company="Engage Software">
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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI.WebControls;

    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;

    using Engage.Events;

    using Telerik.Web.UI;

    using Globals = DotNetNuke.Common.Globals;

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
            if (!this.PermissionsService.CanManageEvents)
            {
                this.DenyAccess();
                return;
            }

            base.OnInit(e);
            AJAX.RegisterPostBackControl(this.SaveEventButton);
            AJAX.RegisterPostBackControl(this.SaveAndCreateNewEventButton);

            this.Load += this.Page_Load;
            this.SaveEventButton.Click += this.SaveEventButton_Click;
            this.SaveAndCreateNewEventButton.Click += this.SaveAndCreateNewEventButton_Click;
            this.DeleteAction.Delete += this.DeleteAction_Delete;
            this.CreateAnotherEventLink.Click += this.CreateAnotherEventLink_Click;
            this.RecurringCheckBox.CheckedChanged += this.RecurringCheckbox_CheckedChanged;
            this.AllowRegistrationsCheckBox.CheckedChanged += this.AllowRegistrationsCheckBox_CheckedChanged;
            this.LimitRegistrationsCheckBox.CheckedChanged += this.LimitRegistrationsCheckBox_CheckedChanged;
            this.CapacityMetMessageRadioButtonList.SelectedIndexChanged += this.CapacityMetMessageRadioButtonList_SelectedIndexChanged;
            this.RecurrenceEditorValidator.ServerValidate += this.RecurrenceEditorValidator_ServerValidate;
            this.UniqueCategoryNameValidator.ServerValidate += this.UniqueCategoryNameValidator_ServerValidate;
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
                    this.DeleteAction.Visible = this.EventId.HasValue;
                    this.AllowRegistrationsCheckBox.Checked = ModuleSettings.AllowRegistrationsByDefault.GetValueAsBooleanFor(this).Value;

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
        private void SaveEventButton_Click(object sender, EventArgs e)
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
        /// Handles the <see cref="Button.Click"/> event of the <see cref="SaveAndCreateNewEventButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SaveAndCreateNewEventButton_Click(object sender, EventArgs e)
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
        /// Handles the <see cref="Events.DeleteAction.Delete"/> event of the <see cref="DeleteAction"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DeleteAction_Delete(object sender, EventArgs e)
        {
            this.Response.Redirect(Globals.NavigateURL());
        }

        /// <summary>
        /// Handles the <see cref="Button.Click"/> event of the <see cref="CreateAnotherEventLink"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CreateAnotherEventLink_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.AddEventUrl, true);
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
        /// Handles the <see cref="CustomValidator.ServerValidate"/> event of the <see cref="UniqueCategoryNameValidator"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="ServerValidateEventArgs"/> instance containing the event data.</param>
        private void UniqueCategoryNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // they picked an existing category, or the new category name doesn't exist in the list of this portal's categories
            args.IsValid = this.CategoryComboBox.SelectedItem != null ||
                           !CategoryCollection.Load(this.PortalId).Any(category => category.Name.Equals(args.Value, StringComparison.CurrentCultureIgnoreCase));
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

            var category = Category.Create(this.PortalId, null, this.CategoryComboBox.Text, null);
            category.Save(this.UserId);

            // if this is a new category and the categories are restricted in the settings, make sure that this category can show up in this module
            if (this.CategoryIds.Any())
            {
                ModuleSettings.Categories.Set(this, ModuleSettings.Categories.GetValueAsStringFor(this) + "," + category.Id.ToString(CultureInfo.InvariantCulture));
            }

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
        /// Convert to the hierarchical categories.
        /// </summary>
        /// <param name="hierarchicalList">The hierarchical list.</param>
        /// <param name="categories">The categories.</param>
        /// <param name="parentCategory">The parent category.</param>
        /// <param name="level">The level.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Telerik.Web.UI.RadComboBoxItem.#ctor(System.String,System.String)", Justification = "Don't need to localize space")]
        private void ToIndentedList(List<RadComboBoxItem> hierarchicalList, List<Category> categories, Category parentCategory, int level)
        {
            foreach (var category in categories.Where(c => parentCategory == null ? c.ParentId == null : c.ParentId == parentCategory.Id))
            {
                var listItemText = string.IsNullOrEmpty(category.Name)
                                       ? this.Localize("DefaultCategory.Text", this.LocalSharedResourceFile)
                                       : level > 0 ? " " + category.Name : category.Name;
                var listItemValue = category.Id.ToString(CultureInfo.InvariantCulture);

                RadComboBoxItem listItem = null;
                try
                {
                    listItem = new RadComboBoxItem(listItemText, listItemValue);

                    if (level > 0)
                    {
                        var pad = string.IsNullOrEmpty(this.Localize("Indent.Text")) ? '>' : this.Localize("Indent.Text")[0];
                        listItem.Text = listItem.Text.PadLeft(listItem.Text.Length + level, pad);
                    }

                    hierarchicalList.Add(listItem);
                }
                catch
                {
                    if (listItem != null)
                    {
                        listItem.Dispose();
                    }

                    throw;
                }

                var categoryId = category.Id;
                if (categories.ToList().Any(c => c.ParentId == categoryId))
                {
                    this.ToIndentedList(hierarchicalList, categories, category, level + 1);
                }
            }
        }

        /// <summary>
        /// Fills the <see cref="TimeZoneDropDownList"/> and <see cref="CategoryComboBox"/>.
        /// </summary>
        private void FillLists()
        {
            this.TimeZoneDropDownList.DataSource = TimeZoneInfo.GetSystemTimeZones();
            this.TimeZoneDropDownList.DataTextField = "DisplayName";
            this.TimeZoneDropDownList.DataValueField = "Id";
            this.TimeZoneDropDownList.DataBind();
            this.TimeZoneDropDownList.SelectedValue = Dnn.Utility.GetUserTimeZone(this.UserInfo, this.PortalSettings).Id;

            var categories = (from category in CategoryCollection.Load(this.PortalId)
                             where !this.CategoryIds.Any() || this.CategoryIds.Contains(category.Id)
                             select category).ToList();

            var indentedList = new List<RadComboBoxItem>();
            this.ToIndentedList(indentedList, categories, null, 0);
            this.CategoryComboBox.AllowCustomText = this.PermissionsService.CanManageCategories;
            this.CategoryComboBox.DataTextField = "Text";
            this.CategoryComboBox.DataValueField = "Value";
            this.CategoryComboBox.DataSource = indentedList;
            this.CategoryComboBox.DataBind();

            // don't show the categories if there's only one option
            this.CategoryPanel.Visible = this.CategoryComboBox.AllowCustomText || categories.Count() > 1;
        }

        /// <summary>
        /// Displays the final success.
        /// </summary>
        private void DisplayFinalSuccess()
        {
            this.SuccessModuleMessage.TextResourceKey = this.EventId.HasValue ? "EditEventSuccess" : "AddEventSuccess";
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
        }

        /// <summary>
        /// This method will update the form with any localized values that cannot be localized by using the ResourceKey attribute in the control's markup.
        /// </summary>
        private void LocalizeControl()
        {
            string addEditResourceKey = this.EventId.HasValue ? "EditEvent.Text" : "AddNewEvent.Text";
            this.AddEditEventLabel.Text = this.Localize(addEditResourceKey);
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
            int? eventId = this.EventId;
            if (!eventId.HasValue)
            {
                return;
            }

            Event e = Event.Load(eventId.Value);
            if (!this.CanShowEvent(e))
            {
                return;
            }

            e.EventStart = this.StartDateTimePicker.SelectedDate.Value;
            e.EventEnd = this.EndDateTimePicker.SelectedDate.Value;
            e.TimeZoneId = this.TimeZoneDropDownList.SelectedValue;
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

        /// <summary>
        /// Based on the values entered by the user for the event, this method will populate an event object and call the Event object's save method.
        /// </summary>
        private void Insert()
        {
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
                this.TimeZoneDropDownList.SelectedValue,
                this.EventLocationTextBox.Text,
                this.FeaturedCheckBox.Checked,
                this.AllowRegistrationsCheckBox.Checked,
                this.RecurrenceEditor.GetRecurrenceRule(eventStart, eventEnd),
                this.LimitRegistrationsCheckBox.Checked && this.LimitRegistrationsCheckBox.Visible ? (int?)this.RegistrationLimitTextBox.Value : null,
                this.GetCustomCapacityMetMessage(),
                this.GetSelectedCategoryId());
                
            e.Save(this.UserId);
        }

        /// <summary>
        /// Based on an EventId, this method populates the <see cref="EventEdit"/> user control with the Event's details.
        /// </summary>
        private void BindData()
        {
            int? eventId = this.EventId;
            if (!eventId.HasValue)
            {
                return;
            }

            Event e = Event.Load(eventId.Value);
            if (!this.CanShowEvent(e))
            {
                return;
            }

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
            this.DeleteAction.CurrentEvent = e;

            this.AllowRegistrationsCheckBox.Checked = this.LimitRegistrationsPanel.Visible = e.AllowRegistrations;
            this.LimitRegistrationsCheckBox.Checked = this.RegistrationLimitPanel.Visible = e.Capacity.HasValue;
            this.RegistrationCountLabel.Text = string.Format(
                CultureInfo.CurrentCulture,
                this.Localize(e.AttendeeCount == 1 ? "Attendee Count (Singular).Format" : "Attendee Count.Format"),
                e.AttendeeCount);
            this.RegistrationLimitValidator.ValueToCompare = e.AttendeeCount.ToString(CultureInfo.CurrentCulture);
            if (e.Capacity.HasValue)
            {
                this.RegistrationLimitTextBox.Value = e.Capacity.Value;
            }

            var hasCustomCapacityMetMessage = e.CapacityMetMessage != null;
            this.CustomCapacityMetMessagePanel.Visible = hasCustomCapacityMetMessage;
            this.CapacityMetMessageRadioButtonList.SelectedValue = hasCustomCapacityMetMessage.ToString(CultureInfo.InvariantCulture);
            this.CustomCapacityMetMessageTextEditor.Text = e.CapacityMetMessage;

            this.TimeZoneDropDownList.SelectedValue = e.TimeZoneId;
        }
    }
}
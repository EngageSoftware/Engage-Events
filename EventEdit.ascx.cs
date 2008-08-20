// <copyright file="EventEdit.ascx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
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
    using System.Web.UI.WebControls;
    using DotNetNuke.Common;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
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
            AJAX.RegisterPostBackControl(this.SaveAndCreateNewEventButton);
            AJAX.RegisterPostBackControl(this.SaveEventButton);
            AJAX.RegisterPostBackControl(this.EventOverviewTextEditor);
            AJAX.RegisterPostBackControl(this.EventDescriptionTextEditor);

            this.Load += this.Page_Load;
            this.EventDescriptionTextEditorValidator.ServerValidate += this.EventDescriptionTextEditorValidator_ServerValidate;
            this.SaveEventButton.Click += this.SaveEventButton_OnClick;
            this.SaveAndCreateNewEventButton.Click += this.SaveAndCreateNewEventButton_OnClick;
            this.RecurringCheckBox.CheckedChanged += this.RecurringCheckbox_CheckedChanged;
            this.RecurrenceEditorValidator.ServerValidate += this.RecurrenceEditorValidator_ServerValidate;
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
                if (!this.IsPostBack && EventId > 0)
                {
                    this.BindData();
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
                    this.Response.Redirect(this.AddEventUrl);
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
            args.IsValid = Engage.Utility.HasValue(this.EventDescriptionTextEditor.Text);
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
        /// This method will update the form with any localized values that cannot be localized by using the DotNetNuke ResourceKey attribute in the control's markup.
        /// </summary>
        private void LocalizeControl()
        {
            string addEditResourceKey = EventId > 0 ? "EditEvent.Text" : "AddNewEvent.Text";
            this.AddEditEventLabel.Text = Localization.GetString(addEditResourceKey, this.LocalResourceFile);
            this.SaveAndCreateNewEventButton.AlternateText = Localization.GetString("SaveAndCreateNew.Alt", LocalResourceFile);
            this.SaveEventButton.AlternateText = Localization.GetString("Save.Alt", LocalResourceFile);
        }

        /// <summary>
        /// This method will either update or create an event based on the current context of EventId
        /// </summary>
        private void Save()
        {
            if (EventId > 0)
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
            Event e = Event.Load(EventId);
            e.EventStart = this.StartDateTimePicker.SelectedDate.Value;
            e.EventEnd = this.EndDateTimePicker.SelectedDate.Value;
            e.Location = this.EventLocationTextBox.Text;
            e.Title = this.EventTitleTextBox.Text;
            e.Overview = this.EventOverviewTextEditor.Text;
            e.Description = this.EventDescriptionTextEditor.Text;
            e.IsFeatured = this.FeaturedCheckBox.Checked;
            e.AllowRegistrations = this.AllowRegistrationsCheckBox.Checked;
            e.RecurrenceRule = this.RecurrenceEditor.GetRecurrenceRule(e.EventStart, e.EventEnd);
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
                this.EventLocationTextBox.Text, 
                this.FeaturedCheckBox.Checked, 
                this.AllowRegistrationsCheckBox.Checked,
                this.RecurrenceEditor.GetRecurrenceRule(eventStart, eventEnd));

            e.Save(this.UserId);
        }

        /// <summary>
        /// Based on an EventId, this method populates the EventEdit user control with the Event's details.
        /// </summary>
        private void BindData()
        {
            Event e = Event.Load(EventId);
            this.EventTitleTextBox.Text = e.Title;
            this.EventLocationTextBox.Text = e.Location;
            this.EventOverviewTextEditor.Text = e.Overview;
            this.EventDescriptionTextEditor.Text = e.Description;
            this.StartDateTimePicker.SelectedDate = e.EventStart;
            this.EndDateTimePicker.SelectedDate = e.EventEnd;
            this.FeaturedCheckBox.Checked = e.IsFeatured;
            this.AllowRegistrationsCheckBox.Checked = e.AllowRegistrations;
            this.RecurringCheckBox.Checked = e.IsRecurring;
            this.RecurrenceEditor.Visible = this.RecurringCheckBox.Checked;
            this.RecurrenceEditor.SetRecurrenceRule(e.RecurrenceRule);
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
    }
}
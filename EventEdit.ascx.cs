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
    using DotNetNuke.Common;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Engage.Events;

    /// <summary>
    /// This class contains a collection of methods for adding or editing an Event.
    /// </summary>
    public partial class EventEdit : ModuleBase
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
                if (!Page.IsPostBack && EventId > 0)
                {
                        this.BindData();
                }

                this.LocalizeControl();
                SuccessModuleMessage.Visible = false;
                FinalButtons.Visible = false;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the OnClick event of the CancelEventButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CancelEventButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.NavigateURL(), true);
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
        protected void SaveEventButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
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
        protected void SaveAndCreateNewEventButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    this.Save();
                    this.DisplaySuccessWithCleanForm();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Click event of the CreateAnotherEventButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CreateAnotherEventButton_Click(object sender, EventArgs e)
        {
            this.CleanForm();
            SuccessModuleMessage.Visible = false;
            AddNewEvent.Visible = true;
            AddEventFooterButtons.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the ExitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ExitButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(), true);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Displays the success with clean form so that user may enter another event.
        /// </summary>
        private void DisplaySuccessWithCleanForm()
        {
            SuccessModuleMessage.Visible = true;
            this.CleanForm();
        }

        /// <summary>
        /// Cleans the form.
        /// </summary>
        private void CleanForm()
        {
            StartDateTimePicker.SelectedDate = null;
            EndDateTimePicker.SelectedDate = null;
            EventLocationTextBox.Text = String.Empty;
            EventTitleTextBox.Text = String.Empty;
            EventDescriptionTextEditor.Text = null;
        }

        /// <summary>
        /// Displays the final success.
        /// </summary>
        private void DisplayFinalSuccess()
        {
            SuccessModuleMessage.Visible = true;
            AddNewEvent.Visible = false;
            AddEventFooterButtons.Visible = false;
            FinalButtons.Visible = true;
        }

        /// <summary>
        /// This method will update the form with any localized values that cannot be localized by using the DotNetNuke ResourceKey attribute in the control's markup.
        /// </summary>
        private void LocalizeControl()
        {
            if (EventId > 0)
            {
                AddEditEventLabel.Text = Localization.GetString("EditEvent.Text", LocalResourceFile);
            }
            else
            {
                AddEditEventLabel.Text = Localization.GetString("AddNewEvent.Text", LocalResourceFile);
            }
        }

        /// <summary>
        /// This method will either update or create an event based on the current context of EventId>
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
            e.EventStart = Convert.ToDateTime(StartDateTimePicker.SelectedDate);
            e.EventEnd = EndDateTimePicker.SelectedDate;
            e.Location = EventLocationTextBox.Text;
            e.Title = EventTitleTextBox.Text;
            e.Overview = EventDescriptionTextEditor.Text;
            e.Save(UserId);
        }

        /// <summary>
        /// Based on the values entered by the user for the event, this method will populate an event object and call the Event object's save method.
        /// </summary>
        private void Insert()
        {
            Event e = Event.Create(PortalId, ModuleId, UserInfo.Email, EventTitleTextBox.Text, EventDescriptionTextEditor.Text, Convert.ToDateTime(StartDateTimePicker.SelectedDate));
            e.Location = EventLocationTextBox.Text;
            e.EventEnd = EndDateTimePicker.SelectedDate;
            e.Save(UserId);
        }

        /// <summary>
        /// Based on an EventId, this method populates the EventEdit user control with the Event's details.
        /// </summary>
        private void BindData()
        {
            Event e = Event.Load(EventId);
            EventTitleTextBox.Text = e.Title;
            EventLocationTextBox.Text = e.Location;
            EventDescriptionTextEditor.Text = e.Overview;
            StartDateTimePicker.DbSelectedDate = e.EventStart;
            EndDateTimePicker.DbSelectedDate = e.EventEnd;
        }

        #endregion
    }
}
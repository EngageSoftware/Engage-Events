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
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Engage.Events;
    using Utility = Engage.Utility;

    /// <summary>
    /// This class contains a collection of methods for adding or editing an Event.
    /// </summary>
    public partial class EventEdit : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
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
        protected void SaveAndCreateNewEventButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (this.Page.IsValid)
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
            this.SuccessModuleMessage.Visible = false;
            this.AddNewEvent.Visible = true;
            this.AddEventFooterButtons.Visible = true;
        }

        /// <summary>
        /// Handles the ServerValidate event of the EventDescriptionTextEditorValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        protected void EventDescriptionTextEditorValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Utility.HasValue(this.EventDescriptionTextEditor.Text);
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
                if (!this.Page.IsPostBack && this.EventId > 0)
                {
                    this.BindData();
                }

                this.SetButtonLinks();
                this.LocalizeControl();
                this.SuccessModuleMessage.Visible = false;
                this.FinalButtons.Visible = false;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Displays the success with clean form so that user may enter another event.
        /// </summary>
        private void DisplaySuccessWithCleanForm()
        {
            this.SuccessModuleMessage.Visible = true;
            this.CleanForm();
        }

        /// <summary>
        /// Cleans the form.
        /// </summary>
        private void CleanForm()
        {
            this.StartDateTimePicker.SelectedDate = null;
            this.EndDateTimePicker.SelectedDate = null;
            this.EventLocationTextBox.Text = String.Empty;
            this.EventTitleTextBox.Text = String.Empty;
            this.EventDescriptionTextEditor.Text = null;
        }

        /// <summary>
        /// Displays the final success.
        /// </summary>
        private void DisplayFinalSuccess()
        {
            this.SuccessModuleMessage.Visible = true;
            this.AddNewEvent.Visible = false;
            this.AddEventFooterButtons.Visible = false;
            this.FinalButtons.Visible = true;
        }

        /// <summary>
        /// Sets the <c>NavigateUrl</c> property for the button links.
        /// </summary>
        private void SetButtonLinks()
        {
            this.ExitLink.NavigateUrl = this.CancelEventLink.NavigateUrl = Globals.NavigateURL();
        }

        /// <summary>
        /// This method will update the form with any localized values that cannot be localized by using the DotNetNuke ResourceKey attribute in the control's markup.
        /// </summary>
        private void LocalizeControl()
        {
            if (this.EventId > 0)
            {
                this.AddEditEventLabel.Text = Localization.GetString("EditEvent.Text", this.LocalResourceFile);
            }
            else
            {
                this.AddEditEventLabel.Text = Localization.GetString("AddNewEvent.Text", this.LocalResourceFile);
            }
            this.CreateAnotherEventButton.AlternateText = Localization.GetString("CreateAnother.Alt", LocalResourceFile);
            this.SaveAndCreateNewEventButton.AlternateText = Localization.GetString("SaveAndCreateNew.Alt", LocalResourceFile);
            this.SaveEventButton.AlternateText = Localization.GetString("Save.Alt", LocalResourceFile);
            this.ExitLink.Text = Localization.GetString("Exit.Alt", LocalResourceFile);
            this.CancelEventLink.Text = Localization.GetString("Cancel.Alt", LocalResourceFile);
        }

        /// <summary>
        /// This method will either update or create an event based on the current context of EventId
        /// </summary>
        private void Save()
        {
            if (this.EventId > 0)
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
            Event e = Event.Load(this.EventId);
            e.EventStart = this.StartDateTimePicker.SelectedDate.Value;
            e.EventEnd = this.EndDateTimePicker.SelectedDate;
            e.Location = this.EventLocationTextBox.Text;
            e.Title = this.EventTitleTextBox.Text;
            e.Overview = this.EventDescriptionTextEditor.Text;
            e.Save(this.UserId);
        }

        /// <summary>
        /// Based on the values entered by the user for the event, this method will populate an event object and call the Event object's save method.
        /// </summary>
        private void Insert()
        {
            Event e = Event.Create(
                this.PortalId,
                this.ModuleId,
                this.UserInfo.Email,
                this.EventTitleTextBox.Text,
                this.EventDescriptionTextEditor.Text,
                this.StartDateTimePicker.SelectedDate.Value);
            e.Location = this.EventLocationTextBox.Text;
            e.EventEnd = this.EndDateTimePicker.SelectedDate;
            e.Save(this.UserId);
        }

        /// <summary>
        /// Based on an EventId, this method populates the EventEdit user control with the Event's details.
        /// </summary>
        private void BindData()
        {
            Event e = Event.Load(this.EventId);
            this.EventTitleTextBox.Text = e.Title;
            this.EventLocationTextBox.Text = e.Location;
            this.EventDescriptionTextEditor.Text = e.Overview;
            this.StartDateTimePicker.SelectedDate = e.EventStart;
            this.EndDateTimePicker.SelectedDate = e.EventEnd;
        }
    }
}
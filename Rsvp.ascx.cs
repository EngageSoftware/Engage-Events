// <copyright file="Rsvp.ascx.cs" company="Engage Sooftware">
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
    using System.Globalization;
    using System.Web;
    using System.Web.UI.WebControls;
    using DotNetNuke.Common;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Engage.Events;

    /// <summary>
    /// Code-behind for a control (Rsvp.ascx) that allows users to register their attendance for an event.
    /// </summary>
    public abstract partial class Rsvp : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.SubmitButton.Click += this.SubmitButton_Click;
            this.AddToCalendarButton.Click += this.AddToCalendarButton_Click;

            AJAX.RegisterPostBackControl(this.AddToCalendarButton);
        }

        /// <summary>
        /// Handles the Click event of the BackToEventsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BackToEventsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL());
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
                if (UserInfo.UserID < 0)
                {
                    Response.Redirect(RegisterUrl, true);
                }

                if (!IsPostBack)
                {
                    this.BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Click event of the SubmitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                Engage.Events.Rsvp rsvp = Engage.Events.Rsvp.Load(EventId, UserInfo.Email);
                if (rsvp == null)
                {
                    rsvp = Engage.Events.Rsvp.Create(EventId, UserInfo.FirstName, UserInfo.LastName, UserInfo.Email);
                }

                rsvp.Status = (RsvpStatus)Enum.Parse(typeof(RsvpStatus), this.RsvpStatusRadioButtons.SelectedValue);
                rsvp.Save(UserId);

                this.RsvpMultiView.ActiveViewIndex = 1;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Click event of the AddToCalendarButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AddToCalendarButton_Click(object sender, EventArgs e)
        {
                Event evnt = Event.Load(this.EventId);
                SendICalendarToClient(HttpContext.Current.Response, evnt.ToICal(UserInfo.Email), evnt.Title);
        }

        /// <summary>
        /// Binds the data for this control.  Sets up values for the specific <see cref="Event"/> that this <see cref="Rsvp"/> is for.
        /// </summary>
        private void BindData()
        {
            Event e = Event.Load(EventId);

            this.EventNameLabel.Text = string.Format(CultureInfo.CurrentCulture, Localization.GetString("EventNameLabel.Text", LocalResourceFile), e.Title);
            this.AddToCalendarButton.Enabled = true;

            this.RsvpStatusRadioButtons.Items.Clear();
            this.RsvpStatusRadioButtons.Items.Add(new ListItem(Localization.GetString(RsvpStatus.Attending.ToString(), LocalResourceFile), RsvpStatus.Attending.ToString()));
            this.RsvpStatusRadioButtons.Items.Add(new ListItem(Localization.GetString(RsvpStatus.NotAttending.ToString(), LocalResourceFile), RsvpStatus.NotAttending.ToString()));
            this.RsvpStatusRadioButtons.Items[0].Selected = true;
        }
    }
}
// <copyright file="Respond.ascx.cs" company="Engage Sooftware">
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
    /// Code-behind for a control (Respond.ascx) that allows users to register their attendance for an event.
    /// </summary>
    public abstract partial class Respond : ModuleBase
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
            this.BackToEventsButton.Click += this.BackToEventsButton_Click;

            AJAX.RegisterPostBackControl(this.AddToCalendarButton);
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
                int? eventId = this.EventId;
                if (eventId.HasValue)
                {
                    Response response = Engage.Events.Response.Load(eventId.Value, this.EventStart, this.UserInfo.Email)
                                        ??
                                        Engage.Events.Response.Create(eventId.Value, this.EventStart, this.UserInfo.FirstName, this.UserInfo.LastName, this.UserInfo.Email);

                    response.Status = (ResponseStatus)Enum.Parse(typeof(ResponseStatus), this.ResponseStatusRadioButtons.SelectedValue);
                    response.Save(UserId);

                    this.ResponseMultiview.ActiveViewIndex = 1;
                }
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
            int? eventId = this.EventId;
            if (eventId.HasValue)
            {
                Event evnt = Event.Load(eventId.Value);
                SendICalendarToClient(HttpContext.Current.Response, evnt.ToICal(this.UserInfo.Email, Dnn.Utility.GetUserTimeZoneOffset(this.UserInfo, this.PortalSettings)), evnt.Title);
            }
        }

        /// <summary>
        /// Handles the Click event of the BackToEventsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BackToEventsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL());
        }

        /// <summary>
        /// Binds the data for this control.  Sets up values for the specific <see cref="Event"/> that this <see cref="Respond"/> is for.
        /// </summary>
        private void BindData()
        {
            int? eventId = this.EventId;
            if (eventId.HasValue)
            {
                Event e = Event.Load(eventId.Value);

                this.EventNameLabel.Text = string.Format(CultureInfo.CurrentCulture, Localization.GetString("EventNameLabel.Text", LocalResourceFile), e.Title);
                this.AddToCalendarButton.Enabled = true;

                this.ResponseStatusRadioButtons.Items.Clear();
                this.ResponseStatusRadioButtons.Items.Add(new ListItem(Localization.GetString(ResponseStatus.Attending.ToString(), LocalResourceFile), ResponseStatus.Attending.ToString()));
                this.ResponseStatusRadioButtons.Items.Add(new ListItem(Localization.GetString(ResponseStatus.NotAttending.ToString(), LocalResourceFile), ResponseStatus.NotAttending.ToString()));
                this.ResponseStatusRadioButtons.Items[0].Selected = true;
            }
        }
    }
}
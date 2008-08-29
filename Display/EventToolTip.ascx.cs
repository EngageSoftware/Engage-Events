// <copyright file="EventToolTip.ascx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
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
    using System.IO;
    using System.Text;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Localization;
    using Engage.Events;
    using ModuleBase = Events.ModuleBase;
    using Utility = Dnn.Utility;

    /// <summary>
    /// Used to display a "tool tip" for an appointment.
    /// </summary>
    public partial class EventToolTip : ModuleBase
    {
        /// <summary>
        /// The backing field for <see cref="SetEvent"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Event currentEvent;

        /// <summary>
        /// Sets the event to be displayed in the event tooltip.
        /// </summary>
        /// <param name="tooltipEvent">The event to display in the tooltip.</param>
        public void SetEvent(Event tooltipEvent)
        {
            this.currentEvent = tooltipEvent;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += this.Page_PreRender;
            this.RegisterButton.Click += this.RegisterButton_Click;
            this.AddToCalendarButton.Click += this.AddToCalendarButton_Click;
            this.EditButton.Click += this.EditButton_Click;

            AJAX.RegisterPostBackControl(this.AddToCalendarButton);
            this.LocalResourceFile = this.AppRelativeTemplateSourceDirectory + Localization.LocalResourceDirectory + "/" + Path.GetFileNameWithoutExtension(this.TemplateControl.AppRelativeVirtualPath);
        }

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_PreRender(object sender, EventArgs e)
        {
            this.EventDate.Text = Dnn.Events.Utility.GetFormattedEventDate(this.currentEvent.EventStart, this.currentEvent.EventEnd);
            this.EventOverview.Text = this.currentEvent.Overview;
            this.EventTitle.Text = this.currentEvent.Title;

            ////this.AddToCalendarButton.Visible = Engage.Utility.IsLoggedIn;
            this.RegisterButton.Visible = this.currentEvent.AllowRegistrations;
            this.EditButton.Visible = this.IsAdmin;
        }

        /// <summary>
        /// Handles the Click event of the RegisterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.BuildLinkUrl(this.ModuleId, "Register", Dnn.Events.Utility.GetEventParameters(this.currentEvent)));
        }

        /// <summary>
        /// Handles the Click event of the AddToCalendarButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AddToCalendarButton_Click(object sender, EventArgs e)
        {
            SendICalendarToClient(this.Response, this.currentEvent.ToICal(this.UserInfo.Email, Utility.GetUserTimeZoneOffset(this.UserInfo, this.PortalSettings)), this.currentEvent.Title);
        }

        /// <summary>
        /// Handles the Click event of the EditButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EditButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.BuildLinkUrl(this.ModuleId, "EventEdit", Dnn.Events.Utility.GetEventParameters(this.currentEvent)), true);
        }
    }
}
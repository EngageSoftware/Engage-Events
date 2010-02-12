// <copyright file="responseDisplay.ascx.cs" company="Engage Software">
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
    using System.Diagnostics;
    using System.Globalization;
    using DotNetNuke.Services.Exceptions;
    using Engage.Events;

    /// <summary>
    /// Displays a summary of who has and hasn't responded for events.
    /// </summary>
    public partial class ResponseDisplay : ModuleBase
    {
        /// <summary>
        /// Backing field for <see cref="ResponseSummaryDisplay"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ResponseSummary responseSummary;

        /// <summary>
        /// Sets the <see cref="ResponseSummary"/> to display.
        /// </summary>
        /// <param name="value">The <see cref="ResponseSummary"/> to display.</param>
        public void SetResponseSummary(ResponseSummary value)
        {
            this.responseSummary = value;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += this.Page_PreRender;
        }

        /// <summary>
        /// Gets the URL for the Response detail page of a given Response instance.
        /// </summary>
        /// <param name="eventId">The event ID.</param>
        /// <param name="eventStart">The date for which this response was made.</param>
        /// <param name="status">The status.</param>
        /// <param name="count">The number of Responses for this event and status.</param>
        /// <returns>
        /// A URL for the Response detail page of a given Response instance
        /// </returns>
        private string GetDetailUrl(int eventId, DateTime eventStart, ResponseStatus status, int count)
        {
            return count > 0
                       ? this.BuildLinkUrl(
                             this.ModuleId,
                             "ResponseDetail",
                             Utility.GetEventParameters(eventId, eventStart, "status=" + status))
                       : string.Empty;
        }

        /// <summary>
        /// Handles the <c>PreRender</c> event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (this.responseSummary != null)
                {
                    this.TitleLabel.Text = this.responseSummary.Title;
                    this.NotAttendingLink.NavigateUrl = this.GetDetailUrl(this.responseSummary.EventId, this.responseSummary.EventStart, ResponseStatus.NotAttending, this.responseSummary.NotAttending);
                    this.NotAttendingLink.Text = this.responseSummary.NotAttending.ToString(CultureInfo.CurrentCulture);
                    this.AttendingLink.NavigateUrl = this.GetDetailUrl(this.responseSummary.EventId, this.responseSummary.EventStart, ResponseStatus.Attending, this.responseSummary.Attending);
                    this.AttendingLink.Text = this.responseSummary.Attending.ToString(CultureInfo.CurrentCulture);

                    this.DateLabel.Text = Utility.GetFormattedEventDate(this.responseSummary.EventStart, this.responseSummary.EventEnd);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}
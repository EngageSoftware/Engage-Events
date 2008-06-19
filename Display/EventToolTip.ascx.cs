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
    using System.Web.UI;

    /// <summary>
    /// Used to display a "tool tip" for an appointment.
    /// </summary>
    public partial class EventToolTip : UserControl
    {
        /// <summary>
        /// The backing field for EventStartDate.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string eventStartDate;

        /// <summary>
        /// The backing field for EventStartDate.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string eventEndDate;

        /// <summary>
        /// The backing field for Overview.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string overview;

        /// <summary>
        /// The backing field for Overview.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string title;

        /// <summary>
        /// Gets or sets the event start date to be displayed in the event tooltip.
        /// </summary>
        /// <value>The text resource key.</value>
        public string EventStartDate
        {
            [DebuggerStepThrough]
            get { return this.eventStartDate; }
            [DebuggerStepThrough]
            set { this.eventStartDate = value; }
        }

        /// <summary>
        /// Gets or sets the event start date to be displayed in the event tooltip.
        /// </summary>
        /// <value>The text resource key.</value>
        public string EventEndDate
        {
            [DebuggerStepThrough]
            get { return this.eventEndDate; }
            [DebuggerStepThrough]
            set { this.eventEndDate = value; }
        }

        /// <summary>
        /// Gets or sets the overview to be displayed in the event tooltip.
        /// </summary>
        /// <value>The text resource key.</value>
        public string Overview
        {
            [DebuggerStepThrough]
            get { return this.overview; }
            [DebuggerStepThrough]
            set { this.overview = value; }
        }

        /// <summary>
        /// Gets or sets the title to be displayed in the event tooltip.
        /// </summary>
        /// <value>The text resource key.</value>
        public string Title
        {
            [DebuggerStepThrough]
            get { return this.title; }
            [DebuggerStepThrough]
            set { this.title = value; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            string date = this.EventStartDate;
            if (this.EventEndDate != null)
            {
                date += " - " + this.EventEndDate;
            }

            EventDate.Text = date;
            EventOverview.Text = this.Overview;
            EventTitle.Text = this.Title;
        }
    }
}
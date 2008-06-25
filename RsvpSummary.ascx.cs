// <copyright file="RsvpSummary.ascx.cs" company="Engage Software">
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
    using DotNetNuke.Common;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Engage.Events;

    /// <summary>
    /// Displays a summary of who has and hasn't RSVP'd for events.
    /// </summary>
    public partial class RsvpSummary : ModuleBase
    {
        /// <summary>
        /// Gets the index of the current page from the querystring.
        /// </summary>
        /// <value>The index of the current page.</value>
        private int CurrentPageIndex
        {
            get
            {
                int index = 1;
                if (this.Request.QueryString["currentpage"] != null)
                {
                    index = Convert.ToInt32(this.Request.QueryString["currentpage"]);
                }

                return index;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    this.SetupControl();
                    this.BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the SortRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void SortRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData(this.SortRadioButtonList.SelectedValue);
        }

        /// <summary>
        /// Gets the URL for the RSVP detail page of a given RSVP instance.
        /// </summary>
        /// <param name="o">The eventId.</param>
        /// <param name="status">The status.</param>
        /// <param name="c">The number of RSVP's for this event.</param>
        /// <returns>A URL for the RSVP detail page of a given RSVP instance</returns>
        protected string GetDetailUrl(object o, string status, object c)
        {
            int eventId = Convert.ToInt32(o);
            int count = Convert.ToInt32(c);

            return count > 0 ? this.BuildLinkUrl(
                                   "&modId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=RsvpDetail&eventid="
                                   + eventId.ToString(CultureInfo.InvariantCulture) + "&status=" + status) : string.Empty;
        }

        /// <summary>
        /// Sets up this control.  Sets localization and sets the <c>NavigateUrl</c> for the cancel and go home button.
        /// </summary>
        private void SetupControl()
        {
            this.CancelGoHomeLink.Text = Localization.GetString("CancelGoHomeLink.Alt", LocalResourceFile);
            this.CancelGoHomeLink.NavigateUrl = Globals.NavigateURL();
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            this.BindData("EventStart");
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        private void BindData(string sortColumn)
        {
            RsvpSummaryCollection rsvps = RsvpSummaryCollection.Load(this.PortalId, sortColumn, this.CurrentPageIndex - 1, 10);
            this.SummaryRepeater.DataSource = rsvps;
            this.SummaryRepeater.DataBind();

            this.NoRsvpsMessageLabel.Visible = rsvps.TotalRecords == 0;

            this.SummaryPager.Visible = !this.NoRsvpsMessageLabel.Visible;
            this.SummaryPager.TotalRecords = rsvps.TotalRecords;
            this.SummaryPager.PageSize = 10;
            this.SummaryPager.CurrentPage = this.CurrentPageIndex;
            this.SummaryPager.TabID = this.TabId;
        }
    }
}
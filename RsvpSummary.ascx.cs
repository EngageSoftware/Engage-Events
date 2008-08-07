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
    using System.Web.UI.WebControls;
    using Display;
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
        /// Gets the formatted date string for this event.
        /// </summary>
        /// <param name="startDate">The event's start date.</param>
        /// <param name="endDate">The event's end date.</param>
        /// <returns>A formatted string representing the timespan over which this event occurs.</returns>
        protected string GetDateString(object startDate, object endDate)
        {
            return string.Format(CultureInfo.CurrentCulture, Localization.GetString("Timespan.Text", this.LocalResourceFile), startDate, endDate);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.SortRadioButtonList.SelectedIndexChanged += this.SortRadioButtonList_SelectedIndexChanged;
            this.SummaryRepeater.ItemDataBound += this.SummaryRepeater_ItemDataBound;
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
        /// Handles the ItemDataBound event of the SummaryRepeater control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        private void SummaryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RsvpDisplay rsvpDisplay = (RsvpDisplay)e.Item.FindControl("RsvpDisplay");
                rsvpDisplay.SetRsvpSummary(Engage.Events.RsvpSummary.Load(((Engage.Events.RsvpSummary)e.Item.DataItem).EventId));
                rsvpDisplay.ModuleConfiguration = this.ModuleConfiguration;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the SortRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SortRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData(this.SortRadioButtonList.SelectedValue);
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
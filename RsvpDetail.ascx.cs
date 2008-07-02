// <copyright file="RsvpDetail.ascx.cs" company="Engage Software">
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
    /// Lists details for user event registrations
    /// </summary>
    public partial class RsvpDetail : ModuleBase
    {
        #region Properties
        
        /// <summary>
        /// Gets the index of the current page.
        /// </summary>
        /// <value>The index of the current page.</value>
        private int CurrentPageIndex
        {
            get
            {
                int index = 1;

                // Get the currentpage index from the url parameter
                if (Request.QueryString["currentpage"] != null)
                {
                    index = Convert.ToInt32(Request.QueryString["currentpage"]);
                }

                return index;
            }
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        private string Status
        {
            get
            {
                string status = string.Empty;

                // Get the currentpage index from the url parameter
                if (Request.QueryString["status"] != null)
                {
                    status = Request.QueryString["status"].ToString();
                }

                return status;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets the status icon.
        /// </summary>
        /// <param name="o">An object representing the registration status</param>
        /// <returns>the url to the icon for the corresponding registration status</returns>
        protected static string GetStatusIcon(object o)
        {
            string url = string.Empty;
            RsvpStatus status = (RsvpStatus)Enum.Parse(typeof(RsvpStatus), o.ToString());

            switch (status)
            {
                case RsvpStatus.Attending:
                    return "~/DesktopModules/EngageEvents/Images/yes.gif";
                case RsvpStatus.NotAttending:
                    return "~/DesktopModules/EngageEvents/Images/no.gif";
                case RsvpStatus.NoResponse:
                    return "~/DesktopModules/EngageEvents/Images/noresponse.gif";
            }

            return url;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.SortRadioButtonList.SelectedIndexChanged += this.SortRadioButtonList_SelectedIndexChanged;
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
                if (!Page.IsPostBack)
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
        /// Handles the SelectedIndexChanged event of the RbSort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SortRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData(this.SortRadioButtonList.SelectedValue);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets up this control.  Sets localization and sets the NavigateUrl for the Cancel and Go Home button.
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
            this.BindData("CreationDate");
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        private void BindData(string sortColumn)
        {
            RsvpCollection rsvps = RsvpCollection.Load(EventId, this.Status, sortColumn, this.CurrentPageIndex - 1, grdRsvpDetail.PageSize);
            grdRsvpDetail.DataSource = rsvps;
            grdRsvpDetail.DataBind();

            pager.TotalRecords = rsvps.TotalRecords;
            pager.PageSize = grdRsvpDetail.PageSize;
            pager.CurrentPage = this.CurrentPageIndex;
            pager.TabID = TabId;
            pager.QuerystringParams = "&modId=" + ModuleId.ToString() + "&key=rsvpDetail&status=" + this.Status + "&eventid=" + EventId;
            grdRsvpDetail.Attributes.Add("SortColumn", sortColumn);

            this.RsvpDisplay.SetRsvpSummary(Engage.Events.RsvpSummary.Load(EventId));
            this.RsvpDisplay.ModuleConfiguration = this.ModuleConfiguration;
        }

        #endregion
    }
}
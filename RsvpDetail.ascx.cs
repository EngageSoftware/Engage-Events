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

    ///<summary>
    ///
    ///</summary>
    public partial class RsvpDetail : ModuleBase
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
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
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
        /// Sets up this control.  Sets localization and sets the <c>NavigateUrl</c> for the exit button.
        /// </summary>
        private void SetupControl()
        {
            this.ExitLink.Text = Localization.GetString("Exit.Alt", LocalResourceFile);
            this.ExitLink.NavigateUrl = Globals.NavigateURL();
        }

        private void BindData()
        {
            BindData("CreationDate");
        }

        private void BindData(string sortColumn)
        {
            RsvpCollection rsvps = RsvpCollection.Load(EventId, Status, sortColumn, CurrentPageIndex - 1, grdRsvpDetail.PageSize);
            grdRsvpDetail.DataSource = rsvps;
            grdRsvpDetail.DataBind();

            pager.TotalRecords = rsvps.TotalRecords;
            pager.PageSize = grdRsvpDetail.PageSize;
            pager.CurrentPage = CurrentPageIndex;
            pager.TabID = TabId;
            pager.QuerystringParams = "&modId=" + ModuleId.ToString() + "&key=rsvpDetail&status=" + Status + "&eventid=" + EventId;
            grdRsvpDetail.Attributes.Add("SortColumn", sortColumn);

            Event e = Event.Load(EventId);
            lblDate.Text= e.EventStartLongFormatted;
            lblName.Text= e.Title;
        }

        protected static string GetStatusIcon(object o)
        {

            string url = string.Empty;
            RsvpStatus status = (RsvpStatus)Enum.Parse(typeof(RsvpStatus), o.ToString());

            switch (status)
            {
                case RsvpStatus.Attending:
                    return "~/desktopmodules/engageevents/Images/yes.gif";
                case RsvpStatus.NotAttending:
                    return "~/desktopmodules/engageevents/Images/no.gif";
                case RsvpStatus.NoResponse:
                    return "~/desktopmodules/engageevents/Images/noresponse.gif";
            }

            return url;
        }

        private int CurrentPageIndex
        {
            get
            {
                int index = 1;
                //Get the currentpage index from the url parameter
                if (Request.QueryString["currentpage"] != null)
                {
                    index = Convert.ToInt32(Request.QueryString["currentpage"]);
                }

                return index;
            }
        }

        private string Status
        {
            get
            {
                string status = string.Empty;
                //Get the currentpage index from the url parameter
                if (Request.QueryString["status"] != null)
                {
                    status = Request.QueryString["status"].ToString();
                }

                return status;
            }

        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the RbSort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void RbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData(rbSort.SelectedValue);
        }
    }
}